using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Serialization;

namespace CourierCompany
{
    public partial class Form1 : Form
    {
        private Random random = new Random();
        private List<Courier> couriers = new List<Courier>();
        private List<Order> orders = new List<Order>();
        private int totalOrders = 0;
        private int completedOrders = 0;
        private int failedOrders = 0;
        private double totalWaitingTime = 0;
        private string dataFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "simulation_data.xml");

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        private void btnSimulate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateAndParseInput(txtNumCouriers.Text, out int numCouriers, "Количество курьеров") ||
                    !ValidateAndParseInput(txtSimulationTime.Text, out double simulationTime, "Время симуляции") ||
                    !ValidateAndParseInput(txtCourierSpeed.Text, out double averageCourierSpeed, "Средняя скорость курьера") ||
                    !ValidateAndParseInput(txtOrderRate.Text, out double meanTimeBetweenOrders, "Интенсивность потока") ||
                    !ValidateAndParseInput(txtDeltaT.Text, out double deltaT, "Шаг времени") ||
                    !ValidateAndParseInput(txtRadius.Text, out double maxDistance, "Максимальное расстояние"))
                {
                    return;
                }

                InitializeCouriers(numCouriers);
                SimulateDeliveries(simulationTime, averageCourierSpeed, meanTimeBetweenOrders, deltaT, maxDistance);

                double averageWaitingTime = completedOrders > 0 ? totalWaitingTime / completedOrders : 0;
                double averageIdleTime = CalculateAverageIdleTime();
                double averageLoad = CalculateAverageCourierLoad();
                double averageDeliveryTime = orders.Count > 0 ? orders.Average(o => o.DeliveryTime - o.ArrivalTime) : 0;
                double averageDistance = CalculateAverageDistance();

                lblResult.Text = $"Всего заказов: {totalOrders}\n" +
                                 $"Выполнено: {completedOrders}\n" +
                                 $"Не выполнено: {failedOrders}\n" +
                                 $"В процессе: {totalOrders - (completedOrders + failedOrders)}\n" +
                                 $"Среднее время ожидания (мин): {averageWaitingTime:F2}\n" +
                                 $"Среднее время простоя курьеров (мин): {averageIdleTime:F2}\n" +
                                 $"Средняя загруженность курьеров: {averageLoad:F2}\n" +
                                 $"Среднее время доставки (мин): {averageDeliveryTime:F2}\n" +
                                 $"Среднее расстояние (км): {averageDistance:F2}\n";

                UpdateCharts(deltaT);
                SaveData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при обработке данных.\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateAndParseInput(string input, out int result, string fieldName)
        {
            if (input.Contains("."))
            {
                MessageBox.Show($"Для записи дробных чисел в поле \"{fieldName}\" используйте запятые.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = default;
                return false;
            }

            if (!int.TryParse(input, out result) || result <= 0)
            {
                MessageBox.Show($"{fieldName} должно быть положительным целым числом.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool ValidateAndParseInput(string input, out double result, string fieldName)
        {
            if (input.Contains("."))
            {
                MessageBox.Show($"Для записи дробных чисел в поле \"{fieldName}\" используйте запятые.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = default;
                return false;
            }

            if (!double.TryParse(input, out result) || result <= 0)
            {
                MessageBox.Show($"{fieldName} должно быть положительным числом.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private double CalculateAverageDistance()
        {
            return orders.Count > 0 ? orders.Average(o => o.Distance) : 0;
        }

        private void InitializeCouriers(int numCouriers)
        {
            couriers.Clear();
            for (int i = 0; i < numCouriers; i++)
            {
                couriers.Add(new Courier());
            }
        }

        private double CalculateAverageCourierLoad()
        {
            return couriers.Count > 0 ? (double)couriers.Sum(c => c.TotalOrdersAssigned) / couriers.Count : 0;
        }

        private double CalculateAverageIdleTime()
        {
            return couriers.Count > 0 ? couriers.Average(c => c.IdleTime) : 0;
        }

        private void SimulateDeliveries(double simulationTime, double averageCourierSpeed, double meanTimeBetweenOrders, double deltaT, double maxDistance)
        {
            totalOrders = 0;
            completedOrders = 0;
            failedOrders = 0;
            totalWaitingTime = 0;
            orders.Clear();

            double currentTime = 0;
            double timeToNextOrder = GenerateExponentialRandom(1.0 / (meanTimeBetweenOrders / 60.0)); // Среднее время между заказами в минутах

            while (currentTime < simulationTime)
            {
                // Обработка текущего времени
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

                // Проверка, нужно ли создать новый заказ
                if (timeToNextOrder <= currentTime)
                {
                    double distance = GenerateUniformRandom(1.0, maxDistance);
                    Order newOrder = new Order(currentTime, distance);
                    orders.Add(newOrder);
                    totalOrders++;

                    AssignOrderToCourier(newOrder, averageCourierSpeed);

                    timeToNextOrder += GenerateExponentialRandom(1.0 / (meanTimeBetweenOrders / 60.0));
                }

                // Обновление времени
                currentTime += deltaT;
            }

            // Обработка оставшихся заказов
            foreach (var courier in couriers)
            {
                foreach (var order in courier.AssignedOrders)
                {
                    if (order.DeliveryTime > simulationTime)
                    {
                        failedOrders++;
                    }
                }
            }
        }

        private void AssignOrderToCourier(Order newOrder, double averageCourierSpeed)
        {
            Courier leastLoadedCourier = couriers
                .OrderBy(c => c.CurrentLoad)
                .FirstOrDefault();

            if (leastLoadedCourier == null)
            {
                failedOrders++;
                return;
            }

            leastLoadedCourier.AssignOrder(newOrder);

            double distance = newOrder.Distance;
            double deliveryTime = distance / averageCourierSpeed * 60.0;

            newOrder.SetDeliveryTime(newOrder.ArrivalTime + deliveryTime);
        }

        private double GenerateUniformRandom(double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }

        private void UpdateCharts(double deltaT)
        {
            chart1.Series.Clear();

            Series completedOrdersSeries = new Series("Выполненные заказы")
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.Double,
                YValueType = ChartValueType.Int32,
                BorderWidth = 3,
                Color = Color.DarkGoldenrod
            };

            var sortedOrders = orders.OrderBy(o => o.ArrivalTime).ToList();

            double currentTime = 0;
            int completedOrdersCount = 0;
            while (currentTime <= double.Parse(txtSimulationTime.Text))
            {
                completedOrdersCount = sortedOrders.Count(o => o.DeliveryTime <= currentTime);
                completedOrdersSeries.Points.AddXY(currentTime, completedOrdersCount);
                currentTime += deltaT;
            }

            chart1.Series.Add(completedOrdersSeries);
            chart1.ChartAreas[0].AxisX.Title = "Время (мин)";
            chart1.ChartAreas[0].AxisY.Title = "Количество выполненных заказов";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private double GenerateExponentialRandom(double lambda)
        {
            double u = random.NextDouble();
            return -Math.Log(1 - u) / lambda;
        }

        private double GenerateNormalRandom(double mean, double stdDev)
        {
            double u1 = random.NextDouble();
            double u2 = random.NextDouble();
            double z = Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);
            return mean + stdDev * z;
        }

        private void SaveData()
        {
            try
            {
                SimulationData data = new SimulationData
                {
                    NumCouriers = int.Parse(txtNumCouriers.Text),
                    SimulationTime = double.Parse(txtSimulationTime.Text),
                    DeliveryTimePerPackage = double.Parse(txtCourierSpeed.Text),
                    MeanTimeBetweenOrders = double.Parse(txtOrderRate.Text),
                    DeltaT = double.Parse(txtDeltaT.Text),
                    Radius = double.Parse(txtRadius.Text),
                    TotalOrders = totalOrders,
                    CompletedOrders = completedOrders,
                    FailedOrders = failedOrders,
                    TotalWaitingTime = totalWaitingTime,
                    Orders = orders.ToArray(),
                    Couriers = couriers.ToArray()
                };

                XmlSerializer serializer = new XmlSerializer(typeof(SimulationData));
                using (FileStream fs = new FileStream(dataFilePath, FileMode.Create))
                {
                    serializer.Serialize(fs, data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных.\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            try
            {
                if (File.Exists(dataFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SimulationData));
                    using (FileStream fs = new FileStream(dataFilePath, FileMode.Open))
                    {
                        SimulationData data = (SimulationData)serializer.Deserialize(fs);

                        txtNumCouriers.Text = data.NumCouriers.ToString();
                        txtSimulationTime.Text = data.SimulationTime.ToString();
                        txtCourierSpeed.Text = data.DeliveryTimePerPackage.ToString();
                        txtOrderRate.Text = data.MeanTimeBetweenOrders.ToString();
                        txtDeltaT.Text = data.DeltaT.ToString();
                        txtRadius.Text = data.Radius.ToString();

                        totalOrders = data.TotalOrders;
                        completedOrders = data.CompletedOrders;
                        failedOrders = data.FailedOrders;
                        totalWaitingTime = data.TotalWaitingTime;

                        orders = data.Orders.ToList();
                        couriers = data.Couriers.ToList();

                        double averageWaitingTime = completedOrders > 0 ? totalWaitingTime / completedOrders : 0;
                        double averageIdleTime = couriers.Count > 0 ? couriers.Average(c => c.IdleTime) : 0;
                        double averageDeliveryTime = orders.Count > 0 ? orders.Average(o => o.DeliveryTime - o.ArrivalTime) : 0;
                        double averageLoad = CalculateAverageCourierLoad();
                        double averageDistance = CalculateAverageDistance();

                        lblResult.Text = $"Всего заказов: {totalOrders}\n" +
                                 $"Выполнено: {completedOrders}\n" +
                                 $"Не выполнено: {failedOrders}\n" +
                                 $"В процессе: {totalOrders - (completedOrders + failedOrders)}\n" +
                                 $"Среднее время ожидания (мин): {averageWaitingTime:F2}\n" +
                                 $"Среднее время простоя курьеров (мин): {averageIdleTime:F2}\n" +
                                 $"Средняя загруженность курьеров: {averageLoad:F2}\n" +
                                 $"Среднее время доставки (мин): {averageDeliveryTime:F2}\n" +
                                 $"Среднее расстояние (км): {averageDistance:F2}\n";

                        UpdateCharts(data.DeltaT);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных.\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveData();
        }
    }
}
