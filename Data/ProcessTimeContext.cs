using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StartProcessTime.Data.Information;

namespace StartProcessTime.Data
{
    public class ProcessTimeContext:DbContext
    {
        /// <summary>
        /// Статистика о работе процессов 
        /// </summary>
        public DbSet<ProcessLoadCPU> ProcessLoadCPUs { get; set; }
        public DbSet<ProcessPrivateByte> ProcessPrivateBytes { get; set; }
        public DbSet<ProcessWorkingSet> ProcessWorkingSets { get; set; }
        public DbSet<ProcessHandleCount> ProcessHandleCounts { get; set; }
       
        /// <summary>
        /// Доступные процессы к захвату
        /// </summary>
        public DbSet<ProcessInformation> ProcessInformations { get; set; }

        /// <summary>
        /// Общий список запусков
        /// </summary>
        public DbSet<ProcessTable> ProcessTables { get; set; }


        public ProcessTimeContext(DbContextOptions<ProcessTimeContext> options) :base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProcessTimeContext).Assembly);
        }

        public void CreateDatabase()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();

            this.SaveChanges();
        }
    }
}
