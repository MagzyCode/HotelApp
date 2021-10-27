using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Essence
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; } //
        public string Name { get; set; } //
        public string SecondName { get; set; } //
        public string Patronymic { get; set; } //
        public DateTime DayOfBorn { get; set; } //
        public string Gender { get; set; }
        public string PersonalMail { get; set; }
        public string Passport { get; set; } //
        public string Password { get; set; } //
        public virtual ICollection<ServiceOrder> ServiceOrders { get; set; }
        public virtual ICollection<RoomOrder> RoomOrders { get; set; }

        public Client()
        {
            ServiceOrders = new List<ServiceOrder>();
            RoomOrders = new List<RoomOrder>();
        }
    }
}
