using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Hotel.Essence
{
    public class ServiceOrder
    {
        [Key]
        public int ServiceOrderId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime TimeOfOrder { get; set; }
        public int TotalPrice { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Client Client { get; set; }
        public virtual Service Service { get; set; }

    }
}
