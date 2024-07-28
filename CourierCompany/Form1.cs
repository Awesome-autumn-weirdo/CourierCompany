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
        //private string dataFilePath = "simulation_data.xml";
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
                int numCouriers;
                double simulationTime;
                double averageCourierSpeed;
                double meanTimeBetweenOrders;
                double deltaT;

                // Проверка и парсинг количества курьеров
                if (!int.TryParse(txtNumCouriers.Text, out numCouriers))
                {
                    MessageBox.Show("Количество курьеров должно быть целым числом.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (numCouriers <= 0)
                {
                    MessageBox.Show("Количество курьеров должно быть больше нуля.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Проверка и парсинг времени симуляции
                if (txtSimulationTime.Text.Contains("."))
                {
                    MessageBox.Show("Для записи дробных чисел используйте запятые.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(txtSimulationTime.Text, out simulationTime))
                {
                    MessageBox.Show("Время симуляции должно быть числом.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (simulationTime <= 0)
                {
                    MessageBox.Show("Время симуляции должно быть больше нуля.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Проверка и парсинг средней скорости курьеров
                if (txtCourierSpeed.Text.Contains("."))
                {
                    MessageBox.Show("Для записи дробных чисел используйте запятые.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(txtCourierSpeed.Text, out averageCourierSpeed))
                {
                    MessageBox.Show("Средняя скорость курьера должна быть числом.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (averageCourierSpeed <= 0)
                {
                    MessageBox.Show("Средняя скорость курьера должна быть больше нуля.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Проверка и парсинг среднего времени между заказами
                if (txtOrderRate.Text.Contains("."))
                {
                    MessageBox.Show("Для записи дробных чисел используйте запятые.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(txtOrderRate.Text, out meanTimeBetweenOrders))
                {
                    MessageBox.Show("Интенсивность потока должна быть числом.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (meanTimeBetweenOrders <= 0)
                {
                    MessageBox.Show("Интенсивность потока должна быть больше нуля.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Проверка и парсинг дельта времени
                if (txtDeltaT.Text.Contains("."))
                {
                    MessageBox.Show("Для записи дробных чисел используйте запятые.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(txtDeltaT.Text, out deltaT))
                {
                    MessageBox.Show("Шаг времени должен быть числом.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (deltaT <= 0)
                {
                    MessageBox.Show("Шаг времени должен быть больше нуля.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Если все проверки пройдены, выполняем симуляцию
                InitializeCouriers(numCouriers);
                SimulateDeliveries(simulationTime, averageCourierSpeed, meanTimeBetweenOrders, deltaT);

                double averageWaitingTime = completedOrders > 0 ? totalWaitingTime / completedOrders : 0;
                double averageIdleTime = CalculateAverageIdleTime();
                double averageLoad = CalculateAverageCourierLoad();
                double averageDeliveryTime = orders.Count > 0 ? orders.Average(o => o.DeliveryTime - o.ArrivalTime) : 0;
                double averageDistance = CalculateAverageDistance(); // среднее расстояние

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
            catch (FormatException)
            {
                MessageBox.Show("Неправильный формат числового значения.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при обработке данных.\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void SimulateDeliveries(double simulationTime, double averageCourierSpeed, double meanTimeBetweenOrders, double deltaT)
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
                    double distance = GenerateUniformRandom(1.0, 20.0);
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
                courier.IdleTime += (simulationTime - courier.lastActiveTime);
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
            double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
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

    public class Order
    {
        public double ArrivalTime { get; set; }
        public double DeliveryTime { get; set; }
        public double WaitingTime { get; set; }
        public double Distance { get; set; }

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

    public class Courier
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

    [Serializable]
    public class SimulationData
    {
        public int NumCouriers { get; set; }
        public double SimulationTime { get; set; }
        public double DeliveryTimePerPackage { get; set; }
        public double MeanTimeBetweenOrders { get; set; }
        public double DeltaT { get; set; }
        public int TotalOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int FailedOrders { get; set; }
        public double TotalWaitingTime { get; set; }
        public Order[] Orders { get; set; }
        public Courier[] Couriers { get; set; }
    }

}
