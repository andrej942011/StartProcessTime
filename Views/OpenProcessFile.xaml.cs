using Microsoft.Win32;
using StartProcessTime.Data;
using System;
using System.Collections.Generic;
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

namespace StartProcessTime.Views
{
    /// <summary>
    /// Логика взаимодействия для CreateTask.xaml
    /// </summary>
    public partial class OpenProcessFile : Window
    {
        ProcessTimeContext Context;
        string FilePatch { get; set; }
        public OpenProcessFile(ProcessTimeContext context)
        {
            InitializeComponent();
            Context = context;

            for (int i = 0; i <= 24; i++)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = $"{i} час(ов)";
                CBHour.Items.Add(comboBoxItem);
            }

            for (int i = 0; i <= 60; i++)
            {
                ComboBoxItem comboBoxItemMinute = new ComboBoxItem();
                comboBoxItemMinute.Content = $"{i} минут";
                CBMinute.Items.Add(comboBoxItemMinute);

                ComboBoxItem comboBoxItemSeconds = new ComboBoxItem();
                comboBoxItemSeconds.Content = $"{i} секунд";
                CBSeconds.Items.Add(comboBoxItemSeconds);
            }
        }

        private void BTOpenFileDialog_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePatch = openFileDialog.FileName;
                PathTextBox.Text = FilePatch;
            }
        }

        private void BTAdd_Click(object sender, RoutedEventArgs e)
        {
            if(CBHour.SelectedIndex == -1 || CBMinute.SelectedIndex == -1 || CBSeconds.SelectedIndex == -1)
            {
                MessageBox.Show("Выбирите время интервала запуска!");
            }
            else if(TBDescription.Text == "")
            {
                MessageBox.Show("Впишите описание процесса!");
            }
            else if(PathTextBox.Text == "")
            {
                MessageBox.Show("Выбирите путь до процесса!");
            }
            else
            {
                ProcessInformation processInformation = new ProcessInformation();
                processInformation.HoursInterval = CBHour.SelectedIndex;
                processInformation.MinutesInterval = CBMinute.SelectedIndex;
                processInformation.SecondsInterval = CBSeconds.SelectedIndex;
                processInformation.PathToProcess = FilePatch;
                processInformation.Description = TBDescription.Text;
                processInformation.TimeCreate = DateTime.Now;
                processInformation.SystemProcess = false;

                Context.ProcessInformations.Add(processInformation);
                Context.SaveChanges();
                MessageBox.Show("Процесс успешно добавлен в список");
            }
        }
    }
}
