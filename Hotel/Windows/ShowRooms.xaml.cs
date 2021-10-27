using Hotel.Classes;
using Hotel.Content;
using Hotel.Essence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для ShowRooms.xaml
    /// </summary>
    public partial class ShowRooms : Window
    {
        private const int MIN_ADULT = 1;
        private const int MIN_CHILD = 0;
        private const int MAX_PEOPLE_IN_ROOM = 7;

        public Client Buyer { get; set; }
        public RoomOrderHelper Helper { get; set; }


        public ShowRooms(RoomOrderHelper helper, Client buyer)
        {
            InitializeComponent();
            Helper = helper;
            Buyer = buyer;
            Initializating();
        }

        private void Initializating()
        {
            arrival.DisplayDateStart = DateTime.Today;
            departure.DisplayDateStart = DateTime.Today.AddDays(1);
            if (Helper.NumberOfAdult != null && Helper.NumberOfChild != null)
            {
                this.numberOfAdult.Text = Helper.NumberOfAdult;
                this.numberOfChild.Text = Helper.NumberOfChild;
            }
            this.arrival.SelectedDate = Helper.ArrivalDate;
            this.departure.SelectedDate = Helper.DepartureDate;
            if (this.Buyer != null)
            {
                inSystem.Visibility = Visibility.Visible;
                userName.Text = Buyer.Name;
                outOfSystem.Visibility = Visibility.Collapsed;
            }
            else
            {
                inSystem.Visibility = Visibility.Collapsed;
                outOfSystem.Visibility = Visibility.Visible;
            }

            if (departure.SelectedDate == null || arrival.SelectedDate == null)
            {
                apartamentCost.Text = "Стоимость: " + GetCostOfRoom("Апартаменты").ToString() + " $/день";
                deluxeCost.Text = "Стоимость: " + GetCostOfRoom("Делюкс").ToString() + " $/день";
                luxeCost.Text = "Стоимость: " + GetCostOfRoom("Люкс").ToString() + " $/день";
                familyRoomCost.Text = "Стоимость: " + GetCostOfRoom("Семейный номер").ToString() + " $/день";
                standartCost.Text = "Стоимость: " + GetCostOfRoom("Стандартный номер").ToString() + " $/день";
                studioCost.Text = "Стоимость: " + GetCostOfRoom("Студийный номер").ToString() + " $/день";
            }
            else
            {
                long daysInHotel = departure.SelectedDate.Value.Ticks / TimeSpan.TicksPerDay
                    - arrival.SelectedDate.Value.Ticks / TimeSpan.TicksPerDay;
                apartamentCost.Text = "Стоимость: " + (GetCostOfRoom("Апартаменты") * daysInHotel).ToString() + " $";
                deluxeCost.Text = "Стоимость: " + (GetCostOfRoom("Делюкс") * daysInHotel).ToString() + " $";
                luxeCost.Text = "Стоимость: " + (GetCostOfRoom("Люкс") * daysInHotel).ToString() + " $";
                familyRoomCost.Text = "Стоимость: " + (GetCostOfRoom("Семейный номер") * daysInHotel).ToString() + " $";
                standartCost.Text = "Стоимость: " + (GetCostOfRoom("Стандартный номер") * daysInHotel).ToString() + " $";
                studioCost.Text = "Стоимость: " + (GetCostOfRoom("Студийный номер") * daysInHotel).ToString() + " $";
            }
            ShowNeedRoomType();
        }

        public int GetCostOfRoom(string type)
        {
            return Controller.Context.RoomTypes.Where(i => i.Type.Equals(type)).First().Cost;
        }

        public int GetCapacityOfRoom(string type)
        {
            return Controller.Context.RoomTypes.Where(i => i.Type.Equals(type)).First().Capacity;
        }

        public (string roomType, List<Room> needRooms) GetRoomsInNeedType(Grid item)
        {
            var panel = item.Children;
            StackPanel stackPanel = panel[2] as StackPanel; // получаем блок со всей нужной инфой
            var info = stackPanel.Children; // получаем все данные
            string type = (info[0] as TextBlock).Text; // тип комнаты
            List<Room> needRoom = Controller.Context.RoomTypes.ToList().
                    Where(i => i.Type.Equals(type)).First().
                    Rooms.ToList(); // подходящие по типу комнаты
            return (type, needRoom);
        }

        public bool IsAnyRoomInHotel(List<Room> needRooms)
        {
            if (arrival.SelectedDate != null && departure.SelectedDate != null)
            {
                bool numberOfFreeRoom = !needRooms.
                    Select(i => i.RoomOrders).
                    Where(i => i.Count == 0 ||
                    i.All(o => (o.SettlementDate > arrival.SelectedDate && o.SettlementDate > departure.SelectedDate) ||
                    (o.DepartureDate < arrival.SelectedDate && o.DepartureDate < departure.SelectedDate))).
                    Count().Equals(0);
                return numberOfFreeRoom;
            }
            return true;
        }

        public GridSplitter SelectSplitter(StackPanel panel, Grid grid)
        {
            GridSplitter splitter = new GridSplitter();
            foreach (var i in panel.Children)
            {
                if (i is GridSplitter && (i as GridSplitter).Name.Contains(grid.Name))
                {
                    splitter = i as GridSplitter;
                    
                }
            }
            return splitter;
        }

        public void ShowNeedRoomType()
        {
            int countOfPeople = Convert.ToInt32(numberOfAdult.Text) +
                Convert.ToInt32(numberOfChild.Text);
            List<Grid> grids = new List<Grid>()  
            {
                apartament, deluxe, luxe, familyRoom, standart, studio
            };
            foreach (var item in grids)
            {
                string title;
                List<Room> needRooms;
                (title, needRooms) = GetRoomsInNeedType(item);
                bool canOrdering = IsAnyRoomInHotel(needRooms);
                int count = GetCapacityOfRoom(title);
                GridSplitter splitter = SelectSplitter(showRooms, item);
                if (!(count >= countOfPeople && canOrdering))
                {
                    item.Visibility = Visibility.Collapsed;
                    splitter.BorderBrush = new SolidColorBrush(Colors.White);
                }
                else
                {
                    item.Visibility = Visibility.Visible;
                    splitter.BorderBrush = new SolidColorBrush(Colors.Black);
                }

            }
            //Initializating();
        }

        private string Operation(string number, string op)
        {
            switch (op)
            {
                case "+":
                    {
                        int newNumber = Convert.ToInt32(number) + 1;
                        number = newNumber.ToString();
                        break;
                    }
                case "-":
                    {
                        int newNumber = Convert.ToInt32(number) - 1;
                        number = newNumber.ToString();
                        break;
                    }
            }
            return number;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            int numberOfPeople = Convert.ToInt32(numberOfAdult.Text) +
                Convert.ToInt32(numberOfChild.Text);
            switch ((sender as Button).Name)
            {
                case "addAdult":
                    {
                        if (numberOfPeople < MAX_PEOPLE_IN_ROOM)
                        {
                            numberOfAdult.Text = Operation(numberOfAdult.Text, "+");
                        }
                        break;
                    }
                case "addChild":
                    {
                        if (numberOfPeople < MAX_PEOPLE_IN_ROOM)
                        {
                            numberOfChild.Text = Operation(numberOfChild.Text, "+");
                        }
                        break;
                    }
            }
        }

        private void Sub(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case "subAdult":
                    {
                        if (Convert.ToInt32(numberOfAdult.Text) > MIN_ADULT)
                        {
                            numberOfAdult.Text = Operation(numberOfAdult.Text, "-");
                        }
                        break;
                    }
                case "subChild":
                    {
                        if (Convert.ToInt32(numberOfChild.Text) > MIN_CHILD)
                        {
                            numberOfChild.Text = Operation(numberOfChild.Text, "-");
                        }
                        break;
                    }
            }
        }

        private void AutorizatingWithOrder(object sender, RoutedEventArgs e)
        {
            enter.Text = ""; // при закрытии окна это влияет куда идти в mainwindow or autorization
            Close();
        }

        private void CheckClientConnection(RoomType type)
        {
            if (Buyer == null)
            {
                MessageBox.Show("Для заказа комнаты нужно авторизироваться");
            }
            else
            {
                BookingRoom bookingRoom = new BookingRoom(Buyer, type);
                bookingRoom.arrival.SelectedDate = this.arrival.SelectedDate;
                bookingRoom.departure.SelectedDate = this.departure.SelectedDate;
                bookingRoom.ShowDialog();
                ShowNeedRoomType();

            }
        }

        private RoomType GetRoomType(string type)
        {
            return Controller.Context.RoomTypes.ToList().Where(i => i.Type == type).First();
        }

        private void BuyRoom(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Content.Equals("Заказать"))
            {
                switch (button.Name)
                {
                    case "buyApartament":
                        {
                            CheckClientConnection(GetRoomType("Апартаменты"));
                            break;
                        }
                    case "buyDeluxe":
                        {
                            CheckClientConnection(GetRoomType("Делюкс"));
                            break;
                        }
                    case "buyLuxe":
                        {
                            CheckClientConnection(GetRoomType("Люкс"));
                            break;
                        }
                    case "buyFamilyRoom":
                        {
                            CheckClientConnection(GetRoomType("Семейный номер"));
                            break;
                        }
                    case "buyStandart":
                        {
                            CheckClientConnection(GetRoomType("Стандартный номер"));
                            break;
                        }
                    case "buyStudio":
                        {
                            CheckClientConnection(GetRoomType("Студийный номер"));
                            break;
                        }
                }
            }
        }

        private void BackToClientMenu(object sender, RoutedEventArgs e) => Close();

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (inSystem.Visibility.Equals(Visibility.Visible))
            {
                ClientMenu clientMenu = new ClientMenu(Buyer);
                clientMenu.Show();
            }
            else if (outOfSystem.Visibility.Equals(Visibility.Visible) && enter.Text.Equals(""))
            {
                Authorization authorization = new Authorization()
                {
                    TemporaryHelper = Helper
                };
                authorization.Show();
            }
            else if (outOfSystem.Visibility.Equals(Visibility.Visible) && enter.Text.Contains("Войти"))
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }

        private void ShowRoomsInSearch(object sender, RoutedEventArgs e)
        {
            ShowNeedRoomType();
            if (departure.SelectedDate == null || arrival.SelectedDate == null)
            {
                apartamentCost.Text = "Стоимость: " + GetCostOfRoom("Апартаменты").ToString() + " $/день";
                deluxeCost.Text = "Стоимость: " + GetCostOfRoom("Делюкс").ToString() + " $/день";
                luxeCost.Text = "Стоимость: " + GetCostOfRoom("Люкс").ToString() + " $/день";
                familyRoomCost.Text = "Стоимость: " + GetCostOfRoom("Семейный номер").ToString() + " $/день";
                standartCost.Text = "Стоимость: " + GetCostOfRoom("Стандартный номер").ToString() + " $/день";
                studioCost.Text = "Стоимость: " + GetCostOfRoom("Студийный номер").ToString() + " $/день";
            }
            else
            {
                long daysInHotel = departure.SelectedDate.Value.Ticks / TimeSpan.TicksPerDay
                    - arrival.SelectedDate.Value.Ticks / TimeSpan.TicksPerDay;
                apartamentCost.Text = "Стоимость: " + (GetCostOfRoom("Апартаменты") * daysInHotel).ToString() + " $";
                deluxeCost.Text = "Стоимость: " + (GetCostOfRoom("Делюкс") * daysInHotel).ToString() + " $";
                luxeCost.Text = "Стоимость: " + (GetCostOfRoom("Люкс") * daysInHotel).ToString() + " $";
                familyRoomCost.Text = "Стоимость: " + (GetCostOfRoom("Семейный номер") * daysInHotel).ToString() + " $";
                standartCost.Text = "Стоимость: " + (GetCostOfRoom("Стандартный номер") * daysInHotel).ToString() + " $";
                studioCost.Text = "Стоимость: " + (GetCostOfRoom("Студийный номер") * daysInHotel).ToString() + " $";
            }
        }
    }
}
