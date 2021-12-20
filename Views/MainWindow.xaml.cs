using Microsoft.EntityFrameworkCore;
using StartProcessTime.Data;
using StartProcessTime.ProcessD;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StartProcessTime.Views
{
    /// <summary>
    /// Install-Package LiveChartsCore.SkiaSharpView.WPF -Version 2.0.0-beta.90
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ProcessTimeContext Context;

        public MainWindow()
        {
            InitializeComponent();

            DbContextOptionsBuilder<ProcessTimeContext> optionsBuilder = new();
            optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB; Database=ProcessTime; Integrated Security=True").EnableSensitiveDataLogging();
            Context =  new ProcessTimeContext(optionsBuilder.Options);
        }

        /// <summary>
        /// НАЧАТЬ ЗАХВАТ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBegin_Click(object sender, RoutedEventArgs e)
        {
            ProcessExplorer taskExplorer = new ProcessExplorer(Context);
            taskExplorer.ShowDialog();
            //OpenProcessFile openProcessFile = new OpenProcessFile(Context);
            //openProcessFile.Show();
        }

        /// <summary>
        /// ВЫХОД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// ГРАФИКИ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonChart_Click(object sender, RoutedEventArgs e)
        {
            ChartArchiveProcess chartArchiveProcess = new ChartArchiveProcess(Context);
            chartArchiveProcess.Show();
        }

        /// <summary>
        /// ДОБАВИТЬ ПРОЦЕСС В СПИСОК
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddProcess_Click(object sender, RoutedEventArgs e)
        {
            OpenProcessFile openProcessFile = new OpenProcessFile(Context);
            openProcessFile.Show();
        }
    }
}
