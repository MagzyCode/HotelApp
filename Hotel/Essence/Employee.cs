using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Essence
{
    public class Employee
    {
        //[Key]
        public int? EmployeeId { get; set; } //
        public string Name { get; set; } //
        public string SecondName { get; set; } //
        public string Patronymic { get; set; } //
        public DateTime DayOfBorn { get; set; } //
        public string Gender { get; set; }
        public string PersonalMail { get; set; }
        public string Password { get; set; } //
        public string Post { get; set; } //
        public virtual ICollection<ServiceOrder> ServiceOrders { get; set; }
        public virtual ICollection<RoomOrder> RoomOrders { get; set; }

        public Employee()
        {
            ServiceOrders = new List<ServiceOrder>();
            RoomOrders = new List<RoomOrder>();
        }

    }
}
