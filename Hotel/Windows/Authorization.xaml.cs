using Hotel.Classes;
using Hotel.Content;
using Hotel.Essence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    public partial class Authorization : Window
    {
        // для переноса введённых данных в ShowRooms с авторизованным клиентов
        public RoomOrderHelper TemporaryHelper { get; set; } 
        public Authorization()
        {
            InitializeComponent();
        }

        private void ToRegistration(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            Close();
        }

        private void CanSeePassword(object sender, RoutedEventArgs e)
        {
            password.Visibility = Visibility.Collapsed;
            afterCheck.Visibility = Visibility.Visible;
            afterCheck.Text = password.Password;
        }

        private void CanNotSeePassword(object sender, RoutedEventArgs e)
        {
            afterCheck.Visibility = Visibility.Collapsed;
            password.Visibility = Visibility.Visible;
            password.Password = afterCheck.Text;
        }

        private void CheckAccount(object sender, RoutedEventArgs e)
        {
            Client account = Controller.Context.Clients.
                Where(i => i.Password.Equals(this.password.Password) && i.PersonalMail.Equals(this.mail.Text)).
                FirstOrDefault();
            Employee employee = Controller.Context.Employees.
                Where(i => i.Password.Equals(this.password.Password) && i.PersonalMail.Equals(this.mail.Text)).
                FirstOrDefault();
            if (account != null)
            {
                if (TemporaryHelper == null)
                {
                    ClientMenu menu = new ClientMenu(account);
                    menu.Show();
                }
                else
                {
                    ShowRooms showRooms = new ShowRooms(TemporaryHelper, account);
                    showRooms.Show();
                }
                Close();
            }
            else if (employee != null)
            {
                switch (employee.Post)
                {
                    case "Владелец":
                        {
                            OwnerWindow window = new OwnerWindow();
                            window.Show();
                            break;
                        }
                    case "Администратор":
                        {
                            AdminWindow admin = new AdminWindow();
                            admin.Show();
                            break;
                        }
                    default:
                        {
                            EmployeeWindow window = new EmployeeWindow(employee);
                            window.Show();         
                            break;
                        }                      
                }
                Close();
            }
            else
            {
                this.mail.BorderBrush = new SolidColorBrush(Colors.Red);
                this.mail.BorderThickness = new Thickness(2);
                this.password.BorderBrush = new SolidColorBrush(Colors.Red);
                this.password.BorderThickness = new Thickness(2);
                this.infoBlock.Visibility = Visibility.Visible;
            }
        }


        private void BackToMainWindow(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
