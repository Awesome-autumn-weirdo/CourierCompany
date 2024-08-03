using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using CourierCompany;

namespace CourierCompany
{
    public partial class Form1 : Form
    {
        private readonly DataManager dataManager;

        public Form1()
        {
            InitializeComponent();
            dataManager = new DataManager(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "simulation_data.xml"));
            //dataManager = new DataManager(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "test.xml"));
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

                // Создаем экземпляр симуляции и запускаем её
                var simulation = new Simulation(numCouriers, simulationTime, averageCourierSpeed, meanTimeBetweenOrders, deltaT, maxDistance);
                simulation.Run();

                // Получаем результаты симуляции
                var result = simulation.GetResult();

                lblResult.Text = $"Всего заказов: {result.TotalOrders}\n" +
                                 $"Выполнено: {result.CompletedOrders}\n" +
                                 $"Не выполнено: {result.FailedOrders}\n" +
                                 $"В процессе: {result.TotalOrders - (result.CompletedOrders + result.FailedOrders)}\n" +
                                 $"Среднее время ожидания (мин): {result.AverageWaitingTime:F2}\n" +
                                 $"Среднее время простоя курьеров (мин): {result.AverageIdleTime:F2}\n" +
                                 $"Средняя загруженность курьеров: {result.AverageCourierLoad:F2}\n" +
                                 $"Среднее время доставки (мин): {result.AverageDeliveryTime:F2}\n" +
                                 $"Среднее расстояние (км): {result.AverageDistance:F2}\n";

                UpdateCharts(simulation);

                // Сохраняем данные
                var simulationData = new SimulationData
                {
                    NumCouriers = numCouriers,
                    SimulationTime = simulationTime,
                    DeliveryTimePerPackage = averageCourierSpeed,
                    MeanTimeBetweenOrders = meanTimeBetweenOrders,
                    DeltaT = deltaT,
                    Radius = maxDistance,
                    TotalOrders = result.TotalOrders,
                    CompletedOrders = result.CompletedOrders,
                    FailedOrders = result.FailedOrders,
                    TotalWaitingTime = result.AverageWaitingTime,
                    Orders = simulation.Orders.ToArray(), 
                    Couriers = simulation.Couriers.ToArray() 
                };

                dataManager.SaveData(simulationData);
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

        private void UpdateCharts(Simulation simulation)
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

            var sortedOrders = simulation.Orders.OrderBy(o => o.ArrivalTime).ToList();
            double currentTime = 0;
            int completedOrdersCount = 0;

            while (currentTime <= double.Parse(txtSimulationTime.Text))
            {
                completedOrdersCount = sortedOrders.Count(o => o.DeliveryTime <= currentTime);
                completedOrdersSeries.Points.AddXY(currentTime, completedOrdersCount);
                currentTime += double.Parse(txtDeltaT.Text);
            }

            chart1.Series.Add(completedOrdersSeries);
            chart1.ChartAreas[0].AxisX.Title = "Время (мин)";
            chart1.ChartAreas[0].AxisY.Title = "Количество выполненных заказов";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private void LoadData()
        {
            try
            {
                var data = dataManager.LoadData();
                if (data != null)
                {
                    txtNumCouriers.Text = data.NumCouriers.ToString();
                    txtSimulationTime.Text = data.SimulationTime.ToString();
                    txtCourierSpeed.Text = data.DeliveryTimePerPackage.ToString();
                    txtOrderRate.Text = data.MeanTimeBetweenOrders.ToString();
                    txtDeltaT.Text = data.DeltaT.ToString();
                    txtRadius.Text = data.Radius.ToString();

                    lblResult.Text = $"Всего заказов: {data.TotalOrders}\n" +
                                     $"Выполнено: {data.CompletedOrders}\n" +
                                     $"Не выполнено: {data.FailedOrders}\n" +
                                     $"В процессе: {data.TotalOrders - (data.CompletedOrders + data.FailedOrders)}\n" +
                                     $"Среднее время ожидания (мин): {data.TotalWaitingTime:F2}\n" +
                                     $"Среднее время простоя курьеров (мин): {StatisticsUtils.CalculateAverageIdleTime(data.Couriers):F2}\n" +
                                     $"Средняя загруженность курьеров: {StatisticsUtils.CalculateAverageCourierLoad(data.Couriers):F2}\n" +
                                     $"Среднее время доставки (мин): {data.Orders.Average(o => o.DeliveryTime - o.ArrivalTime):F2}\n" +
                                     $"Среднее расстояние (км): {StatisticsUtils.CalculateAverageDistance(data.Orders):F2}\n";

                    var simulation = new Simulation(
                        data.NumCouriers,
                        data.SimulationTime,
                        data.DeliveryTimePerPackage,
                        data.MeanTimeBetweenOrders,
                        data.DeltaT,
                        data.Radius
                    )
                    {
                        Orders = data.Orders.ToList(),
                        Couriers = data.Couriers.ToList()
                    };

                    // Обновляем результаты
                    UpdateCharts(simulation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных.\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                var simulationData = new SimulationData
                {
                    // Заполните данные перед сохранением, если нужно
                };

                dataManager.SaveData(simulationData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных.\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
