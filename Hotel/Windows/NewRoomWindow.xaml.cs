using Hotel.Content;
using Hotel.Essence;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Hotel.Windows
{
    public partial class NewRoomWindow : Window
    {
        public NewRoomWindow()
        {
            InitializeComponent();
        }

        private void AddRoom(object sender, RoutedEventArgs e)
        {
            if (type.SelectedIndex.Equals(0))
            {
                MessageBox.Show("Выберите тип комнаты");
            }
            else
            {
                string typeOfRoom = ((TextBlock)type.SelectedItem).Text;
                Room newRoom = new Room()
                {
                    RoomNumber = Controller.Context.Rooms.ToList().Select(i => i.RoomNumber).Max() + 1,
                    RoomType = Controller.Context.RoomTypes.ToList().Where(i => i.Type == typeOfRoom).First()
                };
                Controller.Context.Rooms.Add(newRoom);
                Controller.Context.SaveChanges();
                Close();
            }
        }
    }
}
