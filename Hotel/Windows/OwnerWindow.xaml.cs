using Hotel.Content;
using Hotel.Essence;
using Hotel.ExceptionСlasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hotel.Windows
{
    /// <summary>
    /// Логика взаимодействия для OwnerWindow.xaml
    /// </summary>
    public partial class OwnerWindow : Window
    {
        private Employee Bye { get; set; }
        public OwnerWindow()
        {
            InitializeComponent();
        }

        private void ShowAllEmployee(object sender, RoutedEventArgs e)
        {
            clientTable.Visibility = Visibility.Hidden;
            statisticTable.Visibility = Visibility.Hidden;
            roomTable.Visibility = Visibility.Hidden;
            employeeTable.Visibility = Visibility.Visible;
            employeeTable.ItemsSource = Controller.Context.Employees.ToList();
        }

        private void ShowAllClient(object sender, RoutedEventArgs e)
        {
            employeeTable.Visibility = Visibility.Hidden;
            statisticTable.Visibility = Visibility.Hidden;
            roomTable.Visibility = Visibility.Hidden;
            clientTable.Visibility = Visibility.Visible;
            clientTable.ItemsSource = Controller.Context.Clients.ToList();
        }

        private void AddEmployee(object sender, RoutedEventArgs e)
        {
            EmployeeRegistration registration = new EmployeeRegistration();
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

        private void ShowResident(object sender, RoutedEventArgs e)
        {
            employeeTable.Visibility = Visibility.Hidden;
            statisticTable.Visibility = Visibility.Hidden;
            roomTable.Visibility = Visibility.Hidden;
            clientTable.Visibility = Visibility.Visible;
            var residents = Controller.Context.RoomOrders.ToList().
                Where(i => i.SettlementDate <= DateTime.Now && i.DepartureDate >= DateTime.Now).
                Select(i => i.Client).ToList();
            clientTable.ItemsSource = residents;
        }

        private void GetStatistic(object sender, RoutedEventArgs e)
        {
            
            var days = calendar.SelectedDates.ToList();
            if (days.Count.Equals(0))
            {
                return;
            }
            int priceInRange = Controller.Context.RoomOrders.ToList().
                Where(i => i.SettlementDate >= days.Min() && i.SettlementDate <= days.Max() 
                && i.SettlementDate <= DateTime.Now).
                Select(i => i.TotalPrice).
                Sum();
            
            int countOfEmployee = Controller.Context.Employees.ToList().Count();
            int countOfClientBase = Controller.Context.Clients.ToList().Count();
            int countOfResidents = Controller.Context.RoomOrders.ToList().
                Where(i => i.SettlementDate <= DateTime.Now && i.DepartureDate >= DateTime.Now).
                Select(i => i.Client).ToList().Count();
            profit.Text = "Прибыль полученая с " + days.Min().ToShortDateString() +
                " до " + days.Max().ToShortDateString() + " составила " + priceInRange.ToString() + "$";
            employeeCount.Text = "Общее количество сотрудников - " + countOfEmployee.ToString();
            clientBaseCount.Text = "Общее количество человек в клиентской базе - " + countOfClientBase.ToString();
            residentCount.Text = "Количество проживающих на сегодняшний день - " + countOfResidents.ToString();
            statisticTable.Visibility = Visibility.Visible;        
        }


        private void WindowClosed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void AddRoom(object sender, RoutedEventArgs e)
        {
            NewRoomWindow window = new NewRoomWindow();
            window.ShowDialog();
            ShowRooms(null, null);
        }

        private void ShowRooms(object sender, RoutedEventArgs e)
        {
            employeeTable.Visibility = Visibility.Hidden;
            statisticTable.Visibility = Visibility.Hidden;
            clientTable.Visibility = Visibility.Hidden;
            roomTable.Visibility = Visibility.Visible;
            roomTable.ItemsSource = Controller.Context.Rooms.ToList();
        }
    }
}
