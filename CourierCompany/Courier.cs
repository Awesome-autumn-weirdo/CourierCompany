using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierCompany
{
    public class Courier : Delivery
    {
        public List<Order> AssignedOrders { get; private set; } = new List<Order>();
        public double IdleTime { get; set; } = 0;
        public double lastActiveTime = 0;
        public int TotalOrdersAssigned { get; set; } = 0;

        public double CurrentLoad
        {
            get
            {
                return AssignedOrders.Count;
            }
        }

        public void AssignOrder(Order order)
        {
            if (AssignedOrders.Count > 0)
            {
                if (order.ArrivalTime > lastActiveTime)
                {
                    IdleTime += (order.ArrivalTime - lastActiveTime);
                }
            }
            AssignedOrders.Add(order);
            TotalOrdersAssigned++;
            lastActiveTime = order.ArrivalTime;
        }

        public void CompleteOrder(Order order, double currentTime)
        {
            AssignedOrders.Remove(order);
            if (AssignedOrders.Count > 0)
            {
                lastActiveTime = currentTime;
            }
        }
    }
}
