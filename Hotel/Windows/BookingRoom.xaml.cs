using Hotel.Content;
using Hotel.Essence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
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
    /// Логика взаимодействия для BookingRoom.xaml
    /// </summary>
    public partial class BookingRoom : Window
    {
        private const int MIN_ADULT = 1;
        private const int MIN_CHILD = 0;
        public int MaxPeopleInRoom { get; set; }
        public Client Client { get; set; }
        public RoomType Room { get; set; }

        public BookingRoom(Client client, RoomType room)
        {
            InitializeComponent();
            Client = client;
            Room = room;
            MaxPeopleInRoom = Room.Capacity;
            arrival.DisplayDateStart = DateTime.Now;
            departure.DisplayDateStart = DateTime.Now.AddDays(1);
        }

        private string Operation(string number, string op)
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

        private void Add(object sender, RoutedEventArgs e)
        {
            int numberOfPeople = Convert.ToInt32(numberOfAdult.Text) +
                Convert.ToInt32(numberOfChild.Text);
            switch ((sender as Button).Name)
            {
                case "addAdult":
                    {
                        if (numberOfPeople < MaxPeopleInRoom)
                        {
                            numberOfAdult.Text = Operation(numberOfAdult.Text, "+");
                        }
                        break;
                    }
                case "addChild":
                    {
                        if (numberOfPeople < MaxPeopleInRoom)
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

        private bool CheckFields()
        {
            if (arrival.SelectedDate.HasValue && departure.SelectedDate.HasValue &&
                arrival.SelectedDate.Value >= DateTime.Today && departure.SelectedDate.Value > DateTime.Today &&
                arrival.SelectedDate.Value < departure.SelectedDate.Value)
            {
                return true;
            }
            return false;
        }

        public bool IsAnyRoomInHotel(List<Room> needRooms)
        {
            if (arrival.SelectedDate != null && departure.SelectedDate != null)
            {
                bool numberOfFreeRoom = !needRooms.
                    Select(i => i.RoomOrders).
                    Where(i => i.Count == 0 ||
                    i.Any(o => (o.SettlementDate > arrival.SelectedDate && o.SettlementDate > departure.SelectedDate) ||
                    (o.DepartureDate < arrival.SelectedDate && o.DepartureDate < departure.SelectedDate))).
                    Count().Equals(0);
                return numberOfFreeRoom;
            }
            return false;
        }

        private void BuyRoom(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!CheckFields() || !IsAnyRoomInHotel(Room.Rooms.ToList()))
                {
                    throw new Exception();
                }
                var employees = Controller.Context.Employees.ToList().Where(i => i.Post.Equals("Консьерж")).ToList();
                // employeeId
                int leastLoadedEmployee = employees.
                        Where(i => i.ServiceOrders.Count.Equals(employees.Select(a => a.ServiceOrders.Count).Min())).
                        First().EmployeeId.Value;
                // clientId
                int clientId = Client.ClientId;
                var room = Room.Rooms.ToList().
                    Where(i => i.RoomOrders.Count == 0 ||
                    i.RoomOrders.All(o => (o.SettlementDate > arrival.SelectedDate &&
                    o.SettlementDate > departure.SelectedDate) ||
                    (o.DepartureDate < arrival.SelectedDate &&
                    o.DepartureDate < departure.SelectedDate))).
                    First();
                int numberOfFreeRoom = room.RoomNumber;
                long daysInHotel = departure.SelectedDate.Value.Ticks / TimeSpan.TicksPerDay
                    - arrival.SelectedDate.Value.Ticks / TimeSpan.TicksPerDay;
                int cost = (int)daysInHotel * Room.Cost;

                DateTime arrivalDate = new DateTime(arrival.SelectedDate.Value.Year, 
                    arrival.SelectedDate.Value.Month, arrival.SelectedDate.Value.Day, 0, 0, 0);
                DateTime departureDate = new DateTime(departure.SelectedDate.Value.Year,
                    departure.SelectedDate.Value.Month, departure.SelectedDate.Value.Day, 0, 0, 0);

                RoomOrder newOrder = new RoomOrder()
                {
                    ClientId = clientId,
                    RoomNumberInOrder = numberOfFreeRoom,
                    SettlementDate = arrivalDate,
                    DepartureDate = departureDate,
                    EmployeeId = leastLoadedEmployee,
                    TotalPrice = cost,
                    TimeOfRoomOrder = DateTime.Now,
                    Room = room
                };

                Controller.Context.RoomOrders.Add(newOrder);
                Controller.Context.SaveChanges();
                Close();

            }
            catch
            {
                MessageBox.Show("Даты не корректны");
            }
        }

        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CheckFields())
            {
                long daysInHotel = departure.SelectedDate.Value.Ticks / TimeSpan.TicksPerDay
                    - arrival.SelectedDate.Value.Ticks / TimeSpan.TicksPerDay;
                finalOrderCost.Text = "К оплате: " + (daysInHotel * Room.Cost).ToString() + " ($)";
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = true;
        }
    }
}
