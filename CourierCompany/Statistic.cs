using CourierCompany;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CourierCompany
{
    public static class StatisticsUtils
    {
        public static double CalculateAverageWaitingTime(IEnumerable<Order> orders, int completedOrders)
        {
            if (completedOrders > 0)
            {
                var completedOrdersList = orders.Where(o => o.DeliveryTime > o.ArrivalTime);
                return completedOrdersList.Average(o => o.DeliveryTime - o.ArrivalTime);
            }
            else
            {
                return 0;
            }
        }

        public static double CalculateAverageIdleTime(IEnumerable<Courier> couriers)
        {
            if (couriers.Any())
            {
                return couriers.Average(c => c.IdleTime);
            }
            else
            {
                return 0;
            }
        }

        public static double CalculateAverageCourierLoad(IEnumerable<Courier> couriers)
        {
            if (couriers.Any())
            {
                return couriers.Average(c => c.TotalOrdersAssigned);
            }
            else
            {
                return 0;
            }
        }

        public static double CalculateAverageDistance(IEnumerable<Order> orders)
        {
            if (orders.Any())
            {
                return orders.Average(o => o.Distance);
            }
            else
            {
                return 0;
            }
        }

    }
}


