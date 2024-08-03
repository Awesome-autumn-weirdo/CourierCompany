using CourierCompany;
using System.Collections.Generic;
using System.Linq;

public class Simulation : ISimulation
{
    private readonly List<Courier> couriers;
    private readonly List<Order> orders;

    private readonly int numCouriers;
    private readonly double simulationTime;
    private readonly double averageCourierSpeed;
    private readonly double meanTimeBetweenOrders;
    private readonly double deltaT;
    private readonly double maxDistance;

    private int totalOrders = 0;
    private int completedOrders = 0;
    private int failedOrders = 0;
    private double totalWaitingTime = 0;

    public Simulation(int numCouriers, double simulationTime, double averageCourierSpeed, double meanTimeBetweenOrders, double deltaT, double maxDistance)
    {
        this.numCouriers = numCouriers;
        this.simulationTime = simulationTime;
        this.averageCourierSpeed = averageCourierSpeed;
        this.meanTimeBetweenOrders = meanTimeBetweenOrders;
        this.deltaT = deltaT;
        this.maxDistance = maxDistance;

        couriers = new List<Courier>(numCouriers);
        orders = new List<Order>();
    }

    public void Run()
    {
        InitializeCouriers();
        SimulateDeliveries();
    }

    public SimulationResult GetResult()
    {
        double averageWaitingTime = StatisticsUtils.CalculateAverageWaitingTime(orders, completedOrders);
        double averageIdleTime = StatisticsUtils.CalculateAverageIdleTime(couriers);
        double averageLoad = StatisticsUtils.CalculateAverageCourierLoad(couriers);
        double averageDeliveryTime = orders.Any() ? orders.Average(o => o.DeliveryTime - o.ArrivalTime) : 0;
        double averageDistance = StatisticsUtils.CalculateAverageDistance(orders);

        return new SimulationResult
        {
            TotalOrders = totalOrders,
            CompletedOrders = completedOrders,
            FailedOrders = failedOrders,
            AverageWaitingTime = averageWaitingTime,
            AverageIdleTime = averageIdleTime,
            AverageCourierLoad = averageLoad,
            AverageDeliveryTime = averageDeliveryTime,
            AverageDistance = averageDistance
        };
    }

    private void InitializeCouriers()
    {
        couriers.Clear();
        for (int i = 0; i < numCouriers; i++)
        {
            couriers.Add(new Courier());
        }
    }

    private void SimulateDeliveries()
    {
        double currentTime = 0;
        double timeToNextOrder = RandomUtils.GenerateExponentialRandom(1.0 / (meanTimeBetweenOrders / 60.0));

        while (currentTime < simulationTime)
        {
            foreach (var courier in couriers)
            {
                for (int i = courier.AssignedOrders.Count - 1; i >= 0; i--)
                {
                    var order = courier.AssignedOrders[i];
                    if (order.DeliveryTime <= currentTime)
                    {
                        completedOrders++;
                        double waitingTime = order.DeliveryTime - order.ArrivalTime;
                        totalWaitingTime += waitingTime;
                        courier.CompleteOrder(order, currentTime);
                    }
                }
            }

            if (timeToNextOrder <= currentTime)
            {
                double distance = RandomUtils.GenerateUniformRandom(1.0, maxDistance);
                Order newOrder = new Order(currentTime, distance);
                orders.Add(newOrder);
                totalOrders++;

                AssignOrderToCourier(newOrder);

                timeToNextOrder += RandomUtils.GenerateExponentialRandom(1.0 / (meanTimeBetweenOrders / 60.0));
            }

            currentTime += deltaT;
        }

        foreach (var courier in couriers)
        {
            failedOrders += courier.AssignedOrders.Count;
        }
    }

    private void AssignOrderToCourier(Order newOrder)
    {
        Courier leastLoadedCourier = couriers.OrderBy(c => c.CurrentLoad).FirstOrDefault();
        if (leastLoadedCourier == null)
        {
            failedOrders++;
            return;
        }

        leastLoadedCourier.AssignOrder(newOrder);

        double deliveryTime = newOrder.Distance / averageCourierSpeed * 60.0;
        newOrder.SetDeliveryTime(newOrder.ArrivalTime + deliveryTime);
    }

    // Добавляем свойства с сеттерами для доступа к спискам заказов и курьеров
    public List<Courier> Couriers
    {
        get => couriers;
        set
        {
            // Заменить текущий список курьеров новыми данными
            couriers.Clear();
            if (value != null)
            {
                couriers.AddRange(value);
            }
        }
    }

    public List<Order> Orders
    {
        get => orders;
        set
        {
            // Заменить текущий список заказов новыми данными
            orders.Clear();
            if (value != null)
            {
                orders.AddRange(value);
            }
        }
    }
}
