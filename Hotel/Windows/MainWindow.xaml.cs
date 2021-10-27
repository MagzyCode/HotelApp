using Hotel.Classes;
using Hotel.Content;
using Hotel.Windows;
using System;
using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;

namespace Hotel
{
    public partial class MainWindow : Window
    {
        private const int MIN_ADULT = 1;
        private const int MIN_CHILD = 0;
        private const int MAX_PEOPLE_IN_ROOM= 7;
        public MainWindow()
        {
            InitializeComponent();
            Initialization();
        }

        public void Initialization() // загрузка начальных дат для календаря и прогрузка БД
        {
            arrivalDate.DisplayDateStart = DateTime.Now;
            departureDate.DisplayDateStart = arrivalDate.DisplayDate.AddDays(1);
            Controller.Context.Clients.Load();
        }

        private void Autorization(object sender, RoutedEventArgs e) // открытие окна авторизации
        {
            Authorization authorization = new Authorization();
            authorization.Show();
            Close();
        }

        private string Operation(string number, string op) // арифметич. операции для блоков отображения детей и взрослых
        {
            switch (op)
            {
                case "+":
                    {
                        number = (Convert.ToInt32(number) + 1).ToString();
                        break;
                    }
                case "-":
                    {
                        number = (Convert.ToInt32(number) - 1).ToString();
                        break;
                    }
            }
            return number;
        }

        private void Add(object sender, RoutedEventArgs e) // добавление людей к одному из блоков
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

        private void Sub(object sender, RoutedEventArgs e) // вычитание кол-ва людей из блоков "взрослые" и "дети"
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

        private void Search(object sender, RoutedEventArgs e) // поиск номеров для клиента с передачей введённых данных
        {
            RoomOrderHelper helper = new RoomOrderHelper()
            {
                NumberOfAdult = this.numberOfAdult.Text,
                NumberOfChild = this.numberOfChild.Text,
                ArrivalDate = this.arrivalDate.SelectedDate,
                DepartureDate = this.departureDate.SelectedDate,
            };
            ShowRooms rooms = new ShowRooms(helper, null);
            try
            {
                if (!CorrectDateForShowRoom(arrivalDate, departureDate))
                {
                    throw new Exception();
                }
                rooms.Show();
                Close();
            }
            catch
            {
                rooms.arrival.SelectedDate = rooms.departure.SelectedDate = null;
                MessageBox.Show("Даты заданы неверно или не заданы вовсе");
            }
        }

        private bool CorrectDateForShowRoom(DatePicker arrival, DatePicker departure)  // проверка дат в поиске
        {
           
            if (arrival.SelectedDate.HasValue && departure.SelectedDate.HasValue &&
                arrival.SelectedDate < departure.SelectedDate &&
                arrival.SelectedDate.Value >= DateTime.Today &&
                departure.SelectedDate.Value > DateTime.Today)
            {
                return true;
            }
            return false;
        }
    }
}
