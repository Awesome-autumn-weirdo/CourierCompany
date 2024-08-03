using CourierCompany;
using System.Collections.Generic;
using System.Linq;
using System;

public class Simulation : ISimulation
{
    private readonly List<Courier> couriers;
    private readonly List<Order> orders;
    private readonly Random random = new Random();

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
        double averageWaitingTime = completedOrders > 0 ? totalWaitingTime / completedOrders : 0;
        double averageIdleTime = CalculateAverageIdleTime();
        double averageLoad = CalculateAverageCourierLoad();
        double averageDeliveryTime = orders.Any() ? orders.Average(o => o.DeliveryTime - o.ArrivalTime) : 0;
        double averageDistance = CalculateAverageDistance();

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
        double timeToNextOrder = GenerateExponentialRandom(1.0 / (meanTimeBetweenOrders / 60.0));

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
                double distance = GenerateUniformRandom(1.0, maxDistance);
                Order newOrder = new Order(currentTime, distance);
                orders.Add(newOrder);
                totalOrders++;

                AssignOrderToCourier(newOrder);

                timeToNextOrder += GenerateExponentialRandom(1.0 / (meanTimeBetweenOrders / 60.0));
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

    private double GenerateUniformRandom(double min, double max) => random.NextDouble() * (max - min) + min;

    private double GenerateExponentialRandom(double lambda)
    {
        double u = random.NextDouble();
        return -Math.Log(1 - u) / lambda;
    }

    private double CalculateAverageIdleTime() => couriers.Any() ? couriers.Average(c => c.IdleTime) : 0;

    private double CalculateAverageCourierLoad() => couriers.Any() ? couriers.Sum(c => c.TotalOrdersAssigned) / (double)couriers.Count : 0;

    private double CalculateAverageDistance() => orders.Any() ? orders.Average(o => o.Distance) : 0;
}

