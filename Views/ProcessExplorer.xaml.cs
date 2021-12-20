using StartProcessTime.Data;
using StartProcessTime.ProcessD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace StartProcessTime.Views
{
    /// <summary>
    /// Логика взаимодействия для TaskExplorer.xaml
    /// </summary>
    public partial class ProcessExplorer : Window
    {
        private ProcessTimeContext context;
        private ProcessMonitor processMonitor;

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Stopwatch stopWatch = new Stopwatch();
        string currentTime = string.Empty;

        public ProcessExplorer(ProcessTimeContext _context)
        {
            InitializeComponent();
            context = _context;

            processInformationGrid.ItemsSource = context.ProcessInformations.ToList();
        }

        private void BTStartProcess_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Tick += new EventHandler(dt_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            stopWatch.Start();
            dispatcherTimer.Start();



            ProcessInformation? processInformation = processInformationGrid.SelectedItem as ProcessInformation;
            if (processInformation != null)
            {
                ProcessTable processTable = new ProcessTable();
                processTable.CreateTime = DateTime.Now;
                processTable.Description = TBDescription.Text;
                processTable.ProcessInformation = processInformation;

                context.ProcessTables.Add(processTable);
                context.SaveChanges();

                //processPolling = new ProcessPollingTimer(processTable, Context); //Старая реализация
                processMonitor = new ProcessMonitor(processTable, context);
                processMonitor.StartProcess();


                MessageBox.Show($"Процесс {processInformation.Description} запущен");
            }
            else
            {
                MessageBox.Show("Выбирите процесс для запуска");
            }
        }

        private void BTStopProcess_Click(object sender, RoutedEventArgs e)
        {
            if (stopWatch.IsRunning)
            {
                stopWatch.Stop();
            }
            //elapsedtimeitem.Items.Add(currentTime);

            processMonitor.StopProcess();
        }

        void dt_Tick(object sender, EventArgs e)
        {
            if (stopWatch.IsRunning)
            {
                TimeSpan ts = stopWatch.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}:{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                TBTime.Text = currentTime;
            }
        }
    }
}
