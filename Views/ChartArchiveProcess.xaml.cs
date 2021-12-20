using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.WPF;
using StartProcessTime.Data;
using StartProcessTime.Views.LiveChartT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;

namespace StartProcessTime.Views
{
    /// <summary>
    /// Логика взаимодействия для ChartArchiveProcess.xaml
    /// </summary>
    public partial class ChartArchiveProcess : Window
    {
        ViewModel viewModel;
        ProcessTimeContext context;
        //https://lvcharts.net/App/examples/v1/Wpf/Events
        public ChartArchiveProcess(ProcessTimeContext _сontext)
        {
            InitializeComponent();
            context = _сontext;

            //Заполним DataGrid
            DataGridProcessTable.ItemsSource = context.ProcessTables.ToList();

            //ChartInitialize();
        }

        /// <summary>
        /// Работает через Context(выводит все)
        /// </summary>
        
        private async void BTShowGraph_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ProcessTable? process = DataGridProcessTable.SelectedItem as ProcessTable;

            if (process != null)
            {
                var LoadCPUs = await (from p in context.ProcessLoadCPUs
                    where p.ProcessTable.Id == process.Id
                    select p).ToListAsync();

                var HandleCounts = await (from p in context.ProcessHandleCounts
                    where p.ProcessTable.Id == process.Id
                    select p).ToListAsync();

                var ProcessPrivateBytes = await (from p in context.ProcessPrivateBytes
                    where p.ProcessTable.Id == process.Id
                    select p).ToListAsync();

                var ProcessWorkingSets = await (from p in context.ProcessWorkingSets
                    where p.ProcessTable.Id == process.Id
                    select p).ToListAsync();

                viewModel = new ViewModel(LoadCPUs, HandleCounts, ProcessPrivateBytes, ProcessWorkingSets);
                Chart1.Series = viewModel.Series;
                Chart1.XAxes = viewModel.XAxes;
                Chart1.YAxes = viewModel.YAxes;
            }
            else
            {
                MessageBox.Show("Мне не чего показывать!");
            }

            stopWatch.Stop();
            MessageBox.Show(stopWatch.Elapsed.ToString("g"));
        }

        
    }
}
