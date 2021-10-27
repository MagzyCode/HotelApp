using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Essence
{
    public class RoomType
    {
        [Key]
        public string Type { get; set; }
        public int Cost { get; set; }
        public string Description { get; set; }
        public int Area { get; set; }
        public int Capacity { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }

        public RoomType()
        {
            Rooms = new List<Room>();
        }
    }
}
