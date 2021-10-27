using Hotel.Content;
using Hotel.Essence;
using Hotel.ExceptionСlasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hotel.Windows
{
    /// <summary>
    /// Логика взаимодействия для ServiceWindow.xaml
    /// </summary>
    public partial class ServiceWindow : Window
    {
        public Client Recipient { private get; set; }
        public string ServiceName { private get; set; }
        public int StartOfWorkDay { private get; set; } 

        private const int END_OF_WORK_DAY = 21;

        public ServiceWindow(Client client)
        {
            InitializeComponent();
            Recipient = client;
        }

        private void SettingPossibleDates()
        {
            // устанавливаю возмозжность заказывать услугу только на месяц вперёд
            massageDate.DisplayDateStart = otherDate.DisplayDateStart =
                foodDate.DisplayDateStart = DateTime.Now;
            travelDate.DisplayDateStart = DateTime.Now.Hour >= 17 ? DateTime.Now.AddDays(1) : DateTime.Now;
            massageDate.DisplayDateEnd = travelDate.DisplayDateEnd = otherDate.DisplayDateEnd =
                foodDate.DisplayDateEnd = DateTime.Now.AddMonths(1);
        }

        private void ShowCost(object sender, SelectionChangedEventArgs e)
        {

            ComboBox allServices = sender as ComboBox; // какую из 4 видов услуг выбрали
            string service = (allServices.SelectedItem as TextBlock).Text; // название услуги
            ServiceName = service;
            SettingPossibleDates();
            // стоимость услуги
            int cost = Controller.Context.Services.ToList().
                Where(i => i.ServiceName.Equals(service)).
                Select(i => i.ServiceCost).
                Sum();

            switch (allServices.Name)
            {
                case "chooseMassage":
                    {
                        massageCost.Text = "Стоимость: " + cost.ToString() + "$";
                        massageOrder.Visibility = Visibility.Visible;
                        break;
                    }
                case "chooseTravel":
                    {
                        travelCost.Text = "Стоимость: " + cost.ToString() + "$";
                        travelOrder.Visibility = Visibility.Visible;
                        break;
                    }
                case "chooseOther":
                    {
                        otherCost.Text = "Стоимость: " + cost.ToString() + "$";
                        otherOrder.Visibility = Visibility.Visible;
                        break;
                    }
                case "chooseFood":
                    {
                        foodCost.Text = "Стоимость: " + cost.ToString() + "$";
                        foodOrder.Visibility = Visibility.Visible;
                        break;
                    }
            }

        }

        private (DateTime begin, DateTime finish) CheckOrder(DateTime date, TextBlock time, int minut)
        {
            int hour = Convert.ToInt32(time.Text.Split(':').First());
            DateTime start = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0);
            DateTime end = start.AddMinutes(minut);
            return (start, end); 
        }

        private (bool anyRoomOrder, bool isDateChoose) CanClientMakeOrder(DatePicker date)
        {
            if (date.SelectedDate.HasValue)
            {
                return (Controller.Context.RoomOrders.ToList().
                    Where(i => i.SettlementDate <= date.SelectedDate.Value &&
                    i.DepartureDate > date.SelectedDate.Value &&
                    i.Client == Recipient).Any(), true);
            }
            return (false, false);
        }

        private (bool result, string message) CheckRoomsAndDate(DatePicker date)
        {
            bool anyRoomOrder;
            bool isDateChoose;
            (anyRoomOrder, isDateChoose) = CanClientMakeOrder(date);
            if (!isDateChoose)
            {
                return (false, "Дата не выбрана");
            }
            else if (!anyRoomOrder)
            {
                return (false, "Невозможно заказать услугу на дату, когда клиент не проживает в гостинице");
            }
            return (true, null);
        }

        private void BuyService(object sender, RoutedEventArgs e)
        {
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            bool check;
            string message;
            try
            {
                
                string buy = (sender as Button).Name;
                switch (buy)
                {
                    case "orderMassage":
                        {
                            (check, message) = CheckRoomsAndDate(massageDate);
                            if (!check)
                            {
                                throw new CanMakeServiceOrderException(message);
                            }
                            (start, end) = CheckOrder(massageDate.SelectedDate.Value, massageTime, 60);
                            break;
                        }
                    case "orderTravel":
                        {
                            (check, message) = CheckRoomsAndDate(travelDate);
                            if (!check)
                            {
                                throw new CanMakeServiceOrderException(message);
                            }
                            (start, end) = CheckOrder(travelDate.SelectedDate.Value, travelTime, 420);
                            break;
                        }
                    case "orderOther":
                        {
                            (check, message) = CheckRoomsAndDate(otherDate);
                            if (!check)
                            {
                                throw new CanMakeServiceOrderException(message);
                            }
                            (start, end) = CheckOrder(otherDate.SelectedDate.Value, otherTime, 15);
                            break;
                        }
                    case "orderFood":
                        {
                            (check, message) = CheckRoomsAndDate(massageDate);
                            if (!check)
                            {
                                throw new CanMakeServiceOrderException(message);
                            }
                            (start, end) = CheckOrder(foodDate.SelectedDate.Value, foodTime, 15);
                            break;
                        }
                }
                OrderСonfirmation order = new OrderСonfirmation();
                order.ShowDialog();
                if (order.AcceptOrder.Equals(true))
                {
                    AddServiceOrderInDatabase(start, end);
                    Close();
                }
            }
            catch (CanMakeServiceOrderException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void AddServiceOrderInDatabase(DateTime start, DateTime end)
        {          
            string post = Controller.Context.Services.ToList().
                Where(i => i.ServiceName.Equals(ServiceName)).First().Fulfilling;
            var employeesInNeedPost = Controller.Context.Employees.ToList().
                Where(i => i.Post.Equals(post));

            var empId = employeesInNeedPost.
                Where(i => i.ServiceOrders.Count.Equals(employeesInNeedPost.
                Select(a => a.ServiceOrders.Count).Min())).
                First().EmployeeId.Value;
            var service = Controller.Context.Services.ToList().
                Where(i => i.ServiceName.Equals(ServiceName)).First();
            var serviceCost = service.ServiceCost;

            ServiceOrder serviceOrder = new ServiceOrder()
            {
                EmployeeId = empId,
                StartTime = start,
                EndTime = end,
                TotalPrice = serviceCost,
                Client = Recipient,
                Service = service,
                TimeOfOrder = DateTime.Now            
            };
            Controller.Context.ServiceOrders.Add(serviceOrder);
            Controller.Context.SaveChanges();
            Close();
        }

        private void AddCorrectTime(TextBlock time, int endOfWorkDay, int addedMinutes)
        {
            int hours = Convert.ToInt32(time.Text.Split(':')[0]);
            int minutes = Convert.ToInt32(time.Text.Split(':')[1]);
            DateTime dateTime = new DateTime(1, 1, 1, hours, minutes, 0);
            if (hours < endOfWorkDay && !(dateTime.AddMinutes(addedMinutes).Hour >= endOfWorkDay))
            {
                dateTime = dateTime.AddMinutes(addedMinutes);
            }
            time.Text = dateTime.Hour.ToString() + ":" +
                (dateTime.Minute >= 0 && dateTime.Minute <= 9 ? dateTime.Minute + "0" : dateTime.Minute.ToString());
        }

        private void SubCorrectTime(TextBlock time, int startOfWorkDay, int takenMinutes)
        {
            int hours = Convert.ToInt32(time.Text.Split(':')[0]);
            int minutes = Convert.ToInt32(time.Text.Split(':')[1]);
            DateTime dateTime = new DateTime(1, 1, 1, hours, minutes, 0);
            if (hours >= startOfWorkDay && !(dateTime.AddMinutes(-takenMinutes).Hour < startOfWorkDay))
            {
                dateTime = dateTime.AddMinutes(-takenMinutes);
            }
            time.Text = dateTime.Hour.ToString() + ":" +
                (dateTime.Minute >= 0 && dateTime.Minute <= 9 ? dateTime.Minute + "0" : dateTime.Minute.ToString());
        }

        private void AddTime(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case "addMassage":
                    {
                        AddCorrectTime(massageTime, END_OF_WORK_DAY, 60);
                        break;
                    }
                case "addTravel":
                    {
                        AddCorrectTime(travelTime, 17, 420);
                        break;
                    }
                case "addOther":
                    {
                        AddCorrectTime(otherTime, END_OF_WORK_DAY, 15);
                        break;
                    }
                case "addFood":
                    {
                        AddCorrectTime(foodTime, END_OF_WORK_DAY, 15);
                        break;
                    }
            }
        }

        private void SubTime(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case "subMassage":
                    {
                        SubCorrectTime(massageTime, StartOfWorkDay, 60);
                        break;
                    }
                case "subTravel":
                    {
                        SubCorrectTime(travelTime, StartOfWorkDay, 420);
                        break;
                    }
                case "subOther":
                    {
                        SubCorrectTime(otherTime, StartOfWorkDay, 15);
                        break;
                    }
                case "subFood":
                    {
                        SubCorrectTime(foodTime, StartOfWorkDay, 15);
                        break;
                    }
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientMenu client = new ClientMenu(Recipient);
            client.Show();
        }

        private void ShowCorrectTime(TextBlock time, DateTime selectedDate, StackPanel block)
        {
            if (selectedDate.Year.Equals(DateTime.Now.Year) && selectedDate.Month.Equals(DateTime.Now.Month)
                            && selectedDate.Day.Equals(DateTime.Now.Day) && DateTime.Now.Hour < 18
                            && DateTime.Now.Hour >= 9)
            {
                StartOfWorkDay = DateTime.Now.Hour + 1;
            }
            else
            {
                StartOfWorkDay = 9;
            }
            time.Text = StartOfWorkDay + ":00";
            block.Visibility = Visibility.Visible; 
        }

        private void FindTimeForClient(object sender, SelectionChangedEventArgs e)
        {
            DateTime find = (sender as DatePicker).SelectedDate.Value;
            switch((sender as DatePicker).Name)
            {
                case "massageDate":
                    {
                        ShowCorrectTime(massageTime, find, chooseMassageTime);
                        break;
                    }
                case "travelDate":
                    {
                        ShowCorrectTime(travelTime, find, chooseTravelTime);
                        break;
                    }
                case "otherDate":
                    {
                        ShowCorrectTime(otherTime, find, chooseOtherTime);
                        break;
                    }
                case "foodDate":
                    {
                        ShowCorrectTime(foodTime, find, chooseFoodTime);
                        break;
                    }
            }
        }
    }
}
