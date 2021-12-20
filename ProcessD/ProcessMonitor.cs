using StartProcessTime.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace StartProcessTime.ProcessD
{
    public class ProcessMonitor
    {
        /// <summary>
        /// Мой запускаемый процесс
        /// </summary>
        public Process myProcess { get; set; }
        System.Timers.Timer Timer { get; set; }

        /// <summary>
        /// Состояние запущенности процесса
        /// </summary>
        private bool startedProcess = false;

        ProcessInformation ProcessInformation;

        ProcessInfo processInfo;
        ProcessTable processTable;
        public ProcessMonitor(ProcessTable _processTable, ProcessTimeContext сontext)
        {
            processTable = _processTable;
            processInfo = new ProcessInfo(сontext, processTable);
            ProcessInformation = processTable.ProcessInformation;
        }

        public void StartProcess()
        {
            //Process.Start("C://Program Files (x86)//Notepad++//notepad++.exe");
            if(ProcessInformation == null)
            {
                MessageBox.Show("ProcessMonitor-ProcessInformation Null!!", "Запуск не возможен!");
            }
            else
            {
                myProcess = new Process();
                myProcess.StartInfo.FileName = ProcessInformation.PathToProcess;
                //myProcess.StartInfo.Verb = "Print";
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.EnableRaisingEvents = true;
                myProcess.Exited += new EventHandler(MyProcessOnExited);
                myProcess.Start();

                //Запустим мониторинг процесса
                processInfo.StartMonitoring(myProcess);

                //myProcess = Process.Start(ProcessInformation.PathToProcess);
                //myProcess.Exited += MyProcessOnExited; //Событие, если процесс завершен

                ////processInfo.StartMonitoring(MyProcess.ProcessName);
                //processInfo.StartMonitoring(myProcess);
                startedProcess = true;
            }
        }

        /// <summary>
        /// Проблемы с закрытием процесса, мешает эвент(myProcess.Exited += new EventHandler(MyProcessOnExited);)
        /// </summary>
        public void StopProcess()
        {
            myProcess.Exited -= new EventHandler(MyProcessOnExited);
            if(Timer !=null)
                Timer.Close();

            processInfo.StopMonitoring();
            myProcess.Kill();
            startedProcess = false;
        }

        private void MyProcessOnExited(object? sender, EventArgs e)
        {
            processInfo.StopMonitoring();
            MessageBox.Show($"Exit time    : {myProcess.ExitTime}\n" +
                            $"Exit code    : {myProcess.ExitCode}\n" +
                            $"Elapsed time : {Math.Round((myProcess.ExitTime - myProcess.StartTime).TotalMilliseconds)}");

            var processInf = processTable.ProcessInformation;
            Timer = new Timer();
            Timer.Interval = new TimeSpan(processInf.HoursInterval, processInf.MinutesInterval, processInf.SecondsInterval).TotalMilliseconds;
            Timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            Timer.Enabled = true;
            //eventHandled.TrySetResult(true);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Timer.Close();
            StartProcess();
        }
    }
}
