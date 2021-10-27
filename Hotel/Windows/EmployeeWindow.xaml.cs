using Hotel.Content;
using Hotel.Essence;
using System.Linq;
using System.Windows;

namespace Hotel.Windows
{
    public partial class EmployeeWindow : Window
    {
        public Employee Employee { get; set; }
        public EmployeeWindow(Employee employee)
        {
            InitializeComponent();
            Employee = employee;
            AddSchedule();
        }
        
        public void AddSchedule()
        {
            var orders = Controller.Context.ServiceOrders.ToList().
                Where(i => i.Employee.Equals(Employee));
            if (orders.Count().Equals(0))
            {
                noOrders.Visibility = Visibility.Visible;
                noOrdersAtAll.Visibility = Visibility.Collapsed;
                workSchedule.Visibility = Visibility.Collapsed;
            }
            else
            {
                workSchedule.ItemsSource = orders;
                workSchedule.Visibility = Visibility.Visible;
                noOrders.Visibility = Visibility.Collapsed;
                noOrdersAtAll.Visibility = Visibility.Collapsed;
            }
           
        }

        private void ShowOrdersInNeedDate(object sender, RoutedEventArgs e)
        {

            var dates = calendar.SelectedDates.ToList();
            var orders = Controller.Context.ServiceOrders.ToList().
                Where(i => i.Employee.Equals(Employee) &&
                dates.Any(o => o.Date.Equals(i.StartTime.Date)));
            if (orders.Count().Equals(0))
            {
                noOrders.Visibility = Visibility.Collapsed;
                noOrdersAtAll.Visibility = Visibility.Visible;
                workSchedule.Visibility = Visibility.Collapsed;
            }
            else
            {
                workSchedule.ItemsSource = orders;
                workSchedule.Visibility = Visibility.Visible;
                noOrders.Visibility = Visibility.Collapsed;
                noOrdersAtAll.Visibility = Visibility.Collapsed;
            }
        }
            
        

        private void ShowActualOrders(object sender, RoutedEventArgs e) => AddSchedule();

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
