using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierCompany
{
        [Serializable]
        public class SimulationData
        {
            public int NumCouriers { get; set; }
            public double SimulationTime { get; set; }
            public double DeliveryTimePerPackage { get; set; }
            public double MeanTimeBetweenOrders { get; set; }
            public double DeltaT { get; set; }
            public double Radius { get; set; }
            public int TotalOrders { get; set; }
            public int CompletedOrders { get; set; }
            public int FailedOrders { get; set; }
            public double TotalWaitingTime { get; set; }
            public Order[] Orders { get; set; }
            public Courier[] Couriers { get; set; }
        }
 
}
