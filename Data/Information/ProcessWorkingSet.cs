using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartProcessTime.Data
{
    public class ProcessWorkingSet
    {
        public int Id { get; set; }
        //public ProcessInformation ProcessInformation { get; set; }
        public ProcessTable ProcessTable { get; set; }
        public double WorkingSet { get; set; }
        public DateTime Time { get; set; }
    }
}
