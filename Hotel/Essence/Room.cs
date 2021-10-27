using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Hotel.Essence
{
    public class Room
    {
        [Key]
        public int RoomNumber { get; set; }
        public virtual ICollection<RoomOrder> RoomOrders { get; set; }
        public virtual RoomType RoomType { get; set; }

        public Room()
        {
            RoomOrders = new List<RoomOrder>();
        }

    }
}
