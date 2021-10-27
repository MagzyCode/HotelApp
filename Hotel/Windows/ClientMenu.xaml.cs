using Hotel.Classes;
using Hotel.Content;
using Hotel.Essence;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Hotel.Windows
{
    public partial class ClientMenu : Window
    {
        public Client Guest { get; set; }
        public RoomOrder RoomOrder { get; set; }
        public ServiceOrder ServiceOrder { get; set; }

        public ClientMenu(Client client)
        {
            InitializeComponent();
            Guest = client;
            name.Text = client.Name;
        }

        public void NullificationOfDeleteOrder()
        {
            cancelOrder.Visibility = Visibility.Collapsed;
            RoomOrder = null;
            ServiceOrder = null;
        }


        private void ChooseService(object sender, RoutedEventArgs e)
        {
            NullificationOfDeleteOrder();
            if (Controller.Context.RoomOrders.ToList().
                Where(i => i.Client.Equals(Guest)).
                Where(i => i.SettlementDate <= DateTime.Now && i.DepartureDate >= DateTime.Now).Any())
            {
                ServiceWindow window = new ServiceWindow(Guest);
                window.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Вы не проживаете отеле и не можете заказать услугу \n Закажите номер, для оформления услуг");
            }

        }

        private void ChooseRoom(object sender, RoutedEventArgs e)
        {
            NullificationOfDeleteOrder();
            ShowRooms rooms = new ShowRooms(new RoomOrderHelper(), Guest);
            rooms.Show();
            Close();
        }

        private void ShowClientMenu(object sender, RoutedEventArgs e)
        {
            actualOrders.Visibility = Visibility.Collapsed;
            privateOrders.Visibility = Visibility.Collapsed;
            cabinet.Visibility = Visibility.Visible;
            NullificationOfDeleteOrder();
        }

        private void ShowSelectedOrders(object sender, RoutedEventArgs e)
        {
            NullificationOfDeleteOrder();
            cabinet.Visibility = Visibility.Collapsed;
            clientsRoomOrder.Visibility = Visibility.Collapsed;
            clientsPrivateRoomOrder.Visibility = Visibility.Collapsed;

            actualOrders.Visibility = Visibility.Visible;
            clientsServices.Visibility = Visibility.Visible;
            titleOfOrders.Text = "Заказанные услуги";

            privateOrders.Visibility = Visibility.Visible;
            clientsPrivateServices.Visibility = Visibility.Visible;
            titleOfPrivateOrder.Text = "Закрытые заказы";

            clientsServices.ItemsSource = Controller.Context.ServiceOrders.ToList().
                Where(i => i.Client.Equals(Guest) && i.EndTime > DateTime.Now).
                OrderBy(i => i.StartTime);
            clientsPrivateServices.ItemsSource = Controller.Context.ServiceOrders.ToList().
                Where(i => i.Client.Equals(Guest) && i.EndTime < DateTime.Now).
                OrderByDescending(i => i.StartTime);
        }

        private void ShowRoomOrders(object sender, RoutedEventArgs e)
        {
            NullificationOfDeleteOrder();

            cabinet.Visibility = Visibility.Collapsed;
            clientsServices.Visibility = Visibility.Collapsed;
            clientsPrivateServices.Visibility = Visibility.Collapsed;

            actualOrders.Visibility = Visibility.Visible;
            clientsRoomOrder.Visibility = Visibility.Visible;
            titleOfOrders.Text = "Заказанные номера";

            privateOrders.Visibility = Visibility.Visible;
            clientsPrivateRoomOrder.Visibility = Visibility.Visible;
            titleOfPrivateOrder.Text = "Прошлые номера";

            clientsRoomOrder.ItemsSource = Controller.Context.RoomOrders.ToList().
                Where(i => i.Client.Equals(Guest) && i.DepartureDate > DateTime.Now).
                OrderBy(i => i.SettlementDate);
            clientsPrivateRoomOrder.ItemsSource = Controller.Context.RoomOrders.ToList().
                Where(i => i.Client.Equals(Guest) && i.DepartureDate < DateTime.Now).
                OrderBy(i => i.SettlementDate);
        }

        private void ChangeSomeInfo(object sender, RoutedEventArgs e)
        {
            NullificationOfDeleteOrder();
            ClientEditWindow clientEdit = new ClientEditWindow(Guest);
            clientEdit.ShowDialog();
            Guest.Name = clientEdit.name.Text;
            Guest.SecondName = clientEdit.secondName.Text;
            Guest.Patronymic = clientEdit.patronymic.Text;
            Guest.DayOfBorn = clientEdit.dayOfBorn.SelectedDate.Value;
            Guest.PersonalMail = clientEdit.mail.Text;
            Guest.Password = clientEdit.password.Text;
            Guest.Gender = clientEdit.male.IsChecked.Value == true ? "мужской" : "женский";
            Guest.Passport = clientEdit.passport.Text;
            name.Text = Guest.Name;
            Controller.Context.SaveChanges();
        }

        private void WindowClosing(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void SelectOrder(object sender, MouseButtonEventArgs e)
        {

            if (clientsServices.Visibility.Equals(Visibility.Visible) && clientsServices.SelectedItem != null
                && clientsServices.SelectedItems.Count == 1)
            {
                ServiceOrder = clientsServices.SelectedItem as ServiceOrder;
                cancelOrder.Visibility = Visibility.Visible;
                RoomOrder = null;
            }
            else if (clientsRoomOrder.Visibility.Equals(Visibility.Visible) && clientsRoomOrder.SelectedItem != null
                && clientsRoomOrder.SelectedItems.Count == 1)
            {
                ServiceOrder = null;
                cancelOrder.Visibility = Visibility.Visible;
                RoomOrder = clientsRoomOrder.SelectedItem as RoomOrder;
            }
            else
            {
                NullificationOfDeleteOrder();
            }


        }

        private void DeleteOrder(object sender, RoutedEventArgs e)
        {
            if (RoomOrder != null)
            {
                Controller.Context.RoomOrders.Remove(RoomOrder);
                Controller.Context.SaveChanges();
                cancelOrder.Visibility = Visibility.Hidden;
                clientsRoomOrder.ItemsSource = Controller.Context.RoomOrders.ToList().
                    Where(i => i.Client.Equals(Guest) && i.DepartureDate > DateTime.Now).
                    OrderBy(i => i.SettlementDate);

            }
            else if (ServiceOrder != null)
            {
                Controller.Context.ServiceOrders.Remove(ServiceOrder);
                Controller.Context.SaveChanges();
                cancelOrder.Visibility = Visibility.Hidden;
                clientsServices.ItemsSource = Controller.Context.ServiceOrders.ToList().
                       Where(i => i.Client.Equals(Guest) && i.EndTime > DateTime.Now).
                       OrderBy(i => i.StartTime);
            }
            else
            {
                MessageBox.Show("Заказ для удаления не выбран или выбрано несколько заказов(выберите один)");
            }
            NullificationOfDeleteOrder();
        }

    }

}
