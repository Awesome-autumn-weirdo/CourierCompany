using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierCompany
{
    public class Order : Delivery
    {
        public double DeliveryTime { get; set; }
        public double WaitingTime { get; set; }

        public Order() { }

        public Order(double arrivalTime, double distance)
        {
            ArrivalTime = arrivalTime;
            Distance = distance;
        }

        public void SetDeliveryTime(double deliveryTime)
        {
            DeliveryTime = deliveryTime;
        }

        public void SetWaitingTime(double waitingTime)
        {
            WaitingTime = waitingTime;
        }
    }
}
