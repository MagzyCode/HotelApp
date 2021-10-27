using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Essence
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int ServiceCost { get; set; }
        public string Fulfilling { get; set; } // кто должно выполнить (должность)
        public virtual ICollection<ServiceOrder> ServiceOrders { get; set; }
        
        public Service()
        {
            ServiceOrders = new List<ServiceOrder>();
        }

    }
}
