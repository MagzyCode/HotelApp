using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Essence
{
    public class RoomOrder
    {
        [Key]
        public int RoomOrderNumber { get; set; } //
        public int ClientId { get; set; }
        public int RoomNumberInOrder { get; set; } 
        public DateTime SettlementDate { get; set; } //
        public DateTime DepartureDate { get; set; } //
        public DateTime TimeOfRoomOrder { get; set; }
        public int EmployeeId { get; set; }
        public int TotalPrice { get; set; } //
        public virtual Room Room { get; set; }
        public virtual Client Client { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
