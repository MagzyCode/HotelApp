using Hotel.Content;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace Hotel.Windows
{
    public partial class OwnewChangeEmployeeWindow : Window
    {
        public OwnewChangeEmployeeWindow()
        {
            InitializeComponent();
        }
        private void BackToOwnerWindow(object sender, RoutedEventArgs e)
        {
            name.Text = "";
            Close();
        }

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
               dayOfBorn.BorderThickness = mail.BorderThickness =
               password.BorderThickness = thickness;
            secondName.BorderBrush = name.BorderBrush = patronymic.BorderBrush =
                dayOfBorn.BorderBrush = mail.BorderBrush =
                password.BorderBrush = gender.Foreground = solidColor;
            selectPost.Visibility = Visibility.Hidden;
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
            catch when (!mailPattern.IsMatch(mail.Text) ||
            Controller.Context.Employees.ToList().Select(i => i.PersonalMail).Any(i => i == mail.Text)) // почта
            {
                mail.BorderThickness = new Thickness(2);
                mail.BorderBrush = new SolidColorBrush(Colors.Red);
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
            catch when (post.SelectedIndex.Equals(0))
            {
                selectPost.Visibility = Visibility.Visible;
            }
            catch
            {
                Close();   
            }

        }
    }
}
