using StartProcessTime.Data.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartProcessTime.Data
{
    public class ProcessTable
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Description { get; set; }
        public ProcessInformation ProcessInformation {get;set;}
        public ICollection<ProcessLoadCPU> ProcessLoadCPUs { get; set; }
        public ICollection<ProcessPrivateByte> ProcessPrivateBytes { get; set; }
        public ICollection<ProcessWorkingSet> ProcessWorkingSets { get; set; }
        public ICollection<ProcessHandleCount> ProcessHandleCounts { get; set; }
    }
}
