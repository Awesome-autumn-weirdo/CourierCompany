using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierCompany
{
    public class Order : Delivery
    {
       // public double ArrivalTime { get; set; }
        public double DeliveryTime { get; set; }
        public double WaitingTime { get; set; }
       // public double Distance { get; set; }

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

        public override void Process()
        {
            // Логика обработки заказа, если нужно
        }
    }
}
