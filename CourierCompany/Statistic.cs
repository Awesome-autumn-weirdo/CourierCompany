using CourierCompany;
using System;
using System.Collections.Generic;
using System.Linq;

public static class StatisticsUtils
{
    public static double CalculateAverageWaitingTime(IEnumerable<Order> orders, int completedOrders)
        => completedOrders > 0 ? orders.Where(o => o.DeliveryTime <= DateTime.Now.Ticks).Average(o => o.DeliveryTime - o.ArrivalTime) : 0;

    public static double CalculateAverageIdleTime(IEnumerable<Courier> couriers)
        => couriers.Any() ? couriers.Average(c => c.IdleTime) : 0;

    public static double CalculateAverageCourierLoad(IEnumerable<Courier> couriers)
        => couriers.Any() ? couriers.Sum(c => c.TotalOrdersAssigned) / (double)couriers.Count() : 0;

    public static double CalculateAverageDistance(IEnumerable<Order> orders)
        => orders.Any() ? orders.Average(o => o.Distance) : 0;
}
