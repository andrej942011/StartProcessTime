using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartProcessTime.Data.Information
{
    public class ProcessLoadCPU
    {
        public int Id { get; set; }
        //public ProcessInformation ProcessInformation { get; set; }
        public ProcessTable ProcessTable { get; set; }
        public double LoadCPU { get; set; }
        public DateTime Time { get; set; }

        //public ProcessStatistic(ProcessInformation process, float loadCpu, float workingSet, float privateBytes)
        //{
        //    ProcessInformation = process;
        //    LoadCPU = loadCpu;
        //    WorkingSet = workingSet;
        //    PrivateBytes = privateBytes;
        //}
    }
}
