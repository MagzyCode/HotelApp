using Hotel.Essence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Classes
{
    public class RoomOrderHelper
    {
        public string NumberOfAdult { get; set; }
        public string NumberOfChild { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? DepartureDate { get; set; }

        //public enum CostInDay
        //{
        //    ApartamentCost = 150,
        //    DeluxeCost = 100,
        //    LuxeCost = 125,
        //    FamilyRoomCost = 120,
        //    StandartCost = 95,
        //    StudioCost = 200
        //}

        //public enum RoomCapacity
        //{
        //    ApartamentCapacity = 7,
        //    DeluxeCapacity = 6,
        //    LuxeCapacity = 6,
        //    FamilyRoomCapacity = 5,
        //    StandartCapacity = 4,
        //    StudioCapacity = 6
        //}

        //public int ApartamentCostInDay { get; set; }
        //public int DeluxeCostInDay { get; set; }
        //public int LuxeCostInDay { get; set; }
        //public int FamilyRoomCostInDay { get; set; }
        //public int StandartCostInDay { get; set; }
        //public int StudioCostInDay { get; set; }

    }
}
