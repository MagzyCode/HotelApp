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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        //private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    Authorization window = new Authorization();
        //    window.Show();
        //}

        private void MaleChecked(object sender, RoutedEventArgs e) => female.IsChecked = false;

        private void FemaleChecked(object sender, RoutedEventArgs e) => male.IsChecked = false;

        private void CheckFields(object sender, RoutedEventArgs e)
        {
            // Regex fioPattern = new Regex(@"^([А-Яа-яЁё]{1})([А-Яа-яЁё]{1,19})$");
            Regex fioPattern = new Regex(@"^([a-zA-ZА-Яа-яЁё]{2,20})$");
            Regex passwordPattern = new Regex(@"^([a-zA-Z0-9]{6,30})$");
            Regex passportPattern = new Regex(@"^HB([0-9]{7})$");
            Regex mailPattern = new Regex(@"^([a-zA-Z0-9]{3,30})(@mail.ru)$");
            DateTime legalAge = new DateTime(DateTime.Now.Year - 18, DateTime.Now.Month, DateTime.Now.Day);

            Thickness thickness = new Thickness(1);
            SolidColorBrush solidColor = new SolidColorBrush(Colors.Black);
            secondName.BorderThickness = name.BorderThickness = patronymic.BorderThickness =
               dayOfBorn.BorderThickness = passport.BorderThickness = mail.BorderThickness =
               password.BorderThickness = thickness;
            secondName.BorderBrush = name.BorderBrush = patronymic.BorderBrush =
                dayOfBorn.BorderBrush = passport.BorderBrush = mail.BorderBrush =
                password.BorderBrush = gender.Foreground = solidColor;
            try
            {
                throw new Exception();
            }
            catch when (!fioPattern.IsMatch(secondName.Text)) // фамилия
            {
                secondName.BorderThickness = new Thickness(2);
                secondName.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            catch when (!fioPattern.IsMatch(name.Text)) // имя
            {
                name.BorderThickness = new Thickness(2);
                name.BorderBrush = new SolidColorBrush(Colors.Red);          
            }
            catch when (!fioPattern.IsMatch(patronymic.Text)) // отчество
            {
                patronymic.BorderThickness = new Thickness(2);
                patronymic.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            catch when (dayOfBorn.SelectedDate.Value > legalAge) // дата рождения
            {
                dayOfBorn.BorderBrush = new SolidColorBrush(Colors.Red);
                dayOfBorn.BorderThickness = new Thickness(2);
            }
            catch when (!passportPattern.IsMatch(passport.Text)) // паспорт
            {
                passport.BorderThickness = new Thickness(2);
                passport.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            catch when (!mailPattern.IsMatch(mail.Text) || 
                 Controller.Context.Clients.ToList().Select(i => i.PersonalMail).Any(i => i == mail.Text)) // почта
            {
                mail.BorderThickness = new Thickness(2);
                mail.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Уже существует аккаунт с такой почтой");
            }
            catch when (!passwordPattern.IsMatch(password.Text)) // пароль
            {
                password.BorderThickness = new Thickness(2);
                password.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            catch when (!male.IsChecked.Value && !female.IsChecked.Value) // пол
            {
                gender.Foreground = new SolidColorBrush(Colors.Red);
            }  
            catch
            {
                Client client = new Client()
                {
                    // ClientId = clientId,
                    Name = name.Text,
                    SecondName = secondName.Text,
                    Patronymic = patronymic.Text,
                    DayOfBorn = dayOfBorn.SelectedDate.Value,
                    Passport = passport.Text,
                    PersonalMail = mail.Text,
                    Password = password.Text,
                    Gender = female.IsChecked.Value ? "женский" : "мужской"
                };
                Controller.Context.Clients.Add(client);
                Controller.Context.SaveChanges();

                ClientMenu menu = new ClientMenu(client);
                menu.Show();
                Close();
                //
            }
        }


        private void WindowClosing(object sender, RoutedEventArgs e)
        {
            Authorization window = new Authorization();
            window.Show();
            Close();
        }
    }
}
