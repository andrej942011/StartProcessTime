using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartProcessTime.Data.Information
{
    public class ProcessPrivateByte
    {
        public int Id { get; set; }
        //public ProcessInformation ProcessInformation { get; set; }
        public ProcessTable ProcessTable { get; set; }
        public double PrivateBytes { get; set; }
        public DateTime Time { get; set; }
    }
}
