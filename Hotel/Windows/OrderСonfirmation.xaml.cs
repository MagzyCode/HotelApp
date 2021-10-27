using System.Windows;
using System.Windows.Controls;

namespace Hotel.Windows
{

    public partial class OrderСonfirmation : Window
    {
        public bool AcceptOrder { get; private set; }
        public OrderСonfirmation()
        {
            InitializeComponent();
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Name.Equals("accept"))
            {
                AcceptOrder = true;
            }
            else AcceptOrder = false;
            Close();
        }
    }
}
