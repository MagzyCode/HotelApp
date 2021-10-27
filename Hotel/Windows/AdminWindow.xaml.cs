using Hotel.Content;
using Hotel.Essence;
using Hotel.ExceptionСlasses;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private Employee Bye { get; set; }
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void ShowAllEmployee(object sender, RoutedEventArgs e)
        {
            clientTable.Visibility = Visibility.Hidden;
            ordersTable.Visibility = Visibility.Hidden;
            roomTable.Visibility = Visibility.Hidden;
            allRoomsTable.Visibility = Visibility.Hidden;
            employeeTable.Visibility = Visibility.Visible;
            employeeTable.ItemsSource = Controller.Context.Employees.ToList()
                .Where(i => i.Post != "Владелец" && i.Post != "Администратор");
        }

        private void ShowAllClient(object sender, RoutedEventArgs e)
        {
            allRoomsTable.Visibility = Visibility.Hidden;
            employeeTable.Visibility = Visibility.Hidden;
            ordersTable.Visibility = Visibility.Hidden;
            roomTable.Visibility = Visibility.Hidden;
            clientTable.Visibility = Visibility.Visible;
            clientTable.ItemsSource = Controller.Context.Clients.ToList();
        }

        private void AddEmployee(object sender, RoutedEventArgs e)
        {
            EmployeeRegistration registration = new EmployeeRegistration();
            registration.administrator.Visibility = Visibility.Collapsed;
            registration.ShowDialog();
            if (registration.name.Text.Equals(""))
            {
                return;
            }
            string text = ((TextBlock)registration.post.SelectedItem).Text;
            // int? employeeNumber = Controller.Context.Employees.Select(i => i.EmployeeId).Max() + 1;
            Employee newEmployee = new Employee()
            {
                Name = registration.name.Text,
                SecondName = registration.secondName.Text,
                Patronymic = registration.patronymic.Text,
                DayOfBorn = registration.dayOfBorn.SelectedDate.Value,
                PersonalMail = registration.mail.Text,
                Password = registration.password.Text,
                Gender = registration.female.IsChecked.Value ? "женский" : "мужской",
                Post = text,
                // EmployeeId = employeeNumber
            };
            Controller.Context.Employees.Add(newEmployee);
            Controller.Context.SaveChanges();
        }

        private void SelectEmployee(object sender, MouseButtonEventArgs e)
        {
            Bye = employeeTable.SelectedItem as Employee;
        }

        private void Firing(object sender, RoutedEventArgs e) // удаление работника
        {
            try
            {
                if (Bye == null || employeeTable.SelectedItems.Count > 1)
                {
                    throw new IsCorrectChooseException("Вы не выбрали сотрудника или выбрали несколько");
                }
                else if (Controller.Context.Employees.ToList().Where(i => i.Post == Bye.Post).Count() == 1)
                {
                    throw new IsCorrectChooseException("Нельзя удалить сотрудника, если нет больше сотрудников данной дожнности");
                }
                var allServices = Controller.Context.ServiceOrders.ToList();
                var services = allServices.Where(i => i.EmployeeId.Equals(Bye.EmployeeId)).ToList();
                List<Employee> employees = Controller.Context.Employees.
                    Where(i => i.Post.Equals(Bye.Post) && i.EmployeeId != Bye.EmployeeId).ToList();
                foreach (var item in services)
                {
                    // ищем минимально загруженного человека той же специальности
                    Employee min = employees.
                        Where(i => i.ServiceOrders.Count.
                        Equals(employees.Select(a => a.ServiceOrders.Count).Min())).
                        First();
                    var serviceOrder = item;
                    serviceOrder.EmployeeId = min.EmployeeId.Value;
                    serviceOrder.Employee = min;
                }
                Controller.Context.Employees.Remove(Bye);
                Controller.Context.SaveChanges();
                Bye = null;
            }
            catch (IsCorrectChooseException ex)
            {
                Bye = null;
                MessageBox.Show(ex.Message);
            }
        }

        private void СhangeEmployeeInfo(object sender, RoutedEventArgs e)
        {
            if (Bye != null && Bye.Post != "Владелец" && employeeTable.SelectedItems.Count == 1)
            {
                OwnewChangeEmployeeWindow window = new OwnewChangeEmployeeWindow();
                window.name.Text = Bye.Name;
                window.secondName.Text = Bye.SecondName;
                window.patronymic.Text = Bye.Patronymic;
                window.dayOfBorn.SelectedDate = Bye.DayOfBorn;
                window.mail.Text = Bye.PersonalMail;
                window.password.Text = Bye.Password;
                window.male.IsChecked = Bye.Gender == "мужской" ? true : false;
                window.female.IsChecked = Bye.Gender == "женский" ? true : false;
                window.post.Text = Bye.Post;
                window.ShowDialog();
                if (!window.name.Text.Equals(""))
                {
                    Bye.Name = window.name.Text;
                    Bye.SecondName = window.secondName.Text;
                    Bye.Patronymic = window.patronymic.Text;
                    Bye.DayOfBorn = window.dayOfBorn.SelectedDate.Value;
                    Bye.PersonalMail = window.mail.Text;
                    Bye.Password = window.password.Text;
                    Bye.Gender = window.male.IsChecked.Value == true ? "мужской" : "женский";
                    Bye.Post = (window.post.SelectedItem as TextBlock).Text;
                    Controller.Context.SaveChanges();
                }
            }
            else
            {
                Bye = null;
                MessageBox.Show("Вы не выбрали сотрудника или выбрали несколько");
            }
        }

        private void ShowResident(object sender, RoutedEventArgs e)
        {
            allRoomsTable.Visibility = Visibility.Hidden;
            employeeTable.Visibility = Visibility.Hidden;
            ordersTable.Visibility = Visibility.Hidden;
            clientTable.Visibility = Visibility.Visible;
            var residents = Controller.Context.RoomOrders.ToList().
                Where(i => i.SettlementDate <= DateTime.Now && i.DepartureDate >= DateTime.Now).
                Select(i => i.Client).ToList();
            clientTable.ItemsSource = residents;
        }

        private void WindowClosed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void ShowAllOrders(object sender, RoutedEventArgs e)
        {
            employeeTable.Visibility = Visibility.Hidden;
            roomTable.Visibility = Visibility.Hidden;
            allRoomsTable.Visibility = Visibility.Hidden;
            ordersTable.Visibility = Visibility.Visible;
            clientTable.Visibility = Visibility.Hidden;
            ordersTable.ItemsSource = Controller.Context.ServiceOrders.ToList().
                Where(i => i.StartTime > DateTime.Now);
        }

        private void ShowAllRooms(object sender, RoutedEventArgs e)
        {
            roomTable.ItemsSource = Controller.Context.RoomOrders.ToList().
                Where(i => i.DepartureDate > DateTime.Now);
            employeeTable.Visibility = Visibility.Hidden;
            ordersTable.Visibility = Visibility.Hidden;
            clientTable.Visibility = Visibility.Hidden;
            allRoomsTable.Visibility = Visibility.Hidden;
            roomTable.Visibility = Visibility.Visible;
        }


        private void ShowRooms(object sender, RoutedEventArgs e)
        {
            employeeTable.Visibility = Visibility.Hidden;
            clientTable.Visibility = Visibility.Hidden;
            roomTable.Visibility = Visibility.Hidden;
            allRoomsTable.Visibility = Visibility.Visible;
            allRoomsTable.ItemsSource = Controller.Context.Rooms.ToList();
        }
    }
}
