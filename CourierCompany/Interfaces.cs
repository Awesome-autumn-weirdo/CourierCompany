using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierCompany
{
    public interface IDataHandler
    {
        void SaveData(SimulationData data);
        SimulationData LoadData();
    }

    public interface ISimulation
    {
        void Run();
        SimulationResult GetResult();
    }

}
