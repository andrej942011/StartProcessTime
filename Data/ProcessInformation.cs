using StartProcessTime.Data.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartProcessTime.Data
{
    public class ProcessInformation
    {
        public int Id { get; set; }

        /// <summary>
        /// Короткое имя процесса(подпись-псевданим)
        /// </summary>
        public string Description { get; set; }
        public string PathToProcess { get; set; }
        public DateTime TimeCreate { get; set; }

        /// <summary>
        /// Является ли процесс системным?(короткий путь до процесса)
        /// </summary>
        public bool SystemProcess { get; set; }

        /// <summary>
        /// Интервал запуска в Часах
        /// </summary>
        public int HoursInterval { get; set; }

        /// <summary>
        /// Интервал запуска в Минутах
        /// </summary>
        public int MinutesInterval { get; set; }

        /// <summary>
        /// Интервал запуска в Секундах
        /// </summary>
        public int SecondsInterval { get; set; }

        /// <summary>
        /// Отношение 1:М
        /// </summary>
        public ICollection<ProcessTable> ProcessTables { get; set; }
        //public ICollection<ProcessLoadCPU> ProcessLoadCPUs { get; set; }
        //public ICollection<ProcessPrivateByte> ProcessPrivateBytes { get; set; }
        //public ICollection<ProcessWorkingSet> ProcessWorkingSets { get; set; }
        //public ICollection<ProcessHandleCount> ProcessHandleCounts { get; set; }
    }
}
