using CourierCompany;
using System.IO;
using System.Xml.Serialization;
using System;

namespace CourierCompany
{
    public class DataManager : IDataHandler
    {
        private readonly string dataFilePath;

        public DataManager(string filePath)
        {
            dataFilePath = filePath;
        }

        public void SaveData(SimulationData data)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SimulationData));
                using (FileStream fs = new FileStream(dataFilePath, FileMode.Create))
                {
                    serializer.Serialize(fs, data);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error saving data.", ex);
            }
        }

        public SimulationData LoadData()
        {
            try
            {
                if (File.Exists(dataFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SimulationData));
                    using (FileStream fs = new FileStream(dataFilePath, FileMode.Open))
                    {
                        return (SimulationData)serializer.Deserialize(fs);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error loading data.", ex);
            }
        }
    }
}
