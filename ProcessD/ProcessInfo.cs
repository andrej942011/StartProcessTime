using StartProcessTime.Data;
using StartProcessTime.Data.Information;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace StartProcessTime.ProcessD
{
    public class ProcessInfo
    {
        //public string ProcessName { get; set; }
        Process myProcess;

        private CancellationToken token;
        private CancellationTokenSource cts;
        private ProcessTimeContext context;
        ProcessTable processTable;
        static object locker = new object();

        public ProcessInfo(ProcessTimeContext _сontext, ProcessTable _processTable)
        {
            processTable = _processTable;
            context = _сontext;
        }

        public void StartMonitoring(Process _myProcess) //string processName
        {
            myProcess = _myProcess;

            //Получение информации о процессе
            cts = new CancellationTokenSource();
            token = cts.Token;

            LoadPrecessAsync(); //Task1
            WorkingSetProcessAsync(); //Task2
            PrivateBytesProcessAsync(); //Task3
            HandleCountAsync(); //Task4
        }

        public void StopMonitoring()
        {
            cts.Cancel();
            Debug.WriteLine("Stop");
        }

        
        void LoadPrecess(CancellationToken token)
        {
            PerformanceCounter cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Process";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total"; //_Total

            PerformanceCounter processCounter = new PerformanceCounter();
            processCounter.CategoryName = "Process";
            processCounter.CounterName = "% Processor Time";
            processCounter.InstanceName = myProcess.ProcessName;

            while (true) //working
            {
                if (token.IsCancellationRequested)
                {
                    Debug.WriteLine("Операция отменена токеном 1");
                    return;
                }

                try
                {
                    //Первое значение всегда возвращает 0
                    var unused = cpuCounter.NextValue();
                    var unusedP = processCounter.NextValue();
                    Thread.Sleep(500);//Task.Delay(1000);

                    var counter = (processCounter.NextValue() / (cpuCounter.NextValue())) * 100;
                    Debug.WriteLine(counter.ToString("F2") + "%");

                    //Запись в бд
                    ProcessLoadCPU loadCPU = new ProcessLoadCPU();
                    loadCPU.ProcessTable = processTable;
                    loadCPU.Time = DateTime.Now;
                    loadCPU.LoadCPU = counter;

                    lock (locker)
                    {
                        context.ProcessLoadCPUs.Add(loadCPU);
                        context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"LoadPrecessAsync: {e.Message}");
                    Thread.Sleep(1000);
                }
            }
        }
        void WorkingSetProcess(CancellationToken token)
        {
            PerformanceCounter processCounter = new PerformanceCounter();
            processCounter.CategoryName = "Process";
            processCounter.CounterName = "Working Set"; //Private Bytes
            processCounter.InstanceName = myProcess.ProcessName;//"7zFM";//"notepad";

            while (true)//working
            {
                if (token.IsCancellationRequested)
                {
                    Debug.WriteLine("Операция отменена токеном 2");
                    return;
                }

                try
                {
                    //First value always returns a 0 (Первое значение всегда возвращает 0)
                    var unusedP = processCounter.NextValue();

                    Thread.Sleep(1000);

                    var workingSet = processCounter.NextValue();
                    Debug.WriteLine("WorkingSet: " + workingSet);

                    //Запись в бд
                    ProcessWorkingSet processWorkingSet = new ProcessWorkingSet();
                    processWorkingSet.ProcessTable = processTable;
                    processWorkingSet.Time = DateTime.Now;
                    processWorkingSet.WorkingSet = workingSet;

                    lock (locker)
                    {
                        context.ProcessWorkingSets.Add(processWorkingSet);
                        context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"WorkingSet: {e.Message}");
                    Thread.Sleep(1000);
                }
            }
        }
        void PrivateBytesProcess(CancellationToken token)
        {
            PerformanceCounter processCounter = new PerformanceCounter();
            processCounter.CategoryName = "Process";
            processCounter.CounterName = "Private Bytes"; //Private Bytes
            processCounter.InstanceName = myProcess.ProcessName;//"7zFM";//"notepad";

            while (true)//working
            {
                if (token.IsCancellationRequested)
                {
                    Debug.WriteLine("Операция отменена токеном 3");
                    return;
                }

                try
                {
                    //First value always returns a 0 (Первое значение всегда возвращает 0)
                    var unusedP = processCounter.NextValue();

                    Thread.Sleep(1000);

                    var privateBytes = processCounter.NextValue();
                    Debug.WriteLine("PrivateBytes: " + privateBytes);

                    //Запись в БД
                    ProcessPrivateByte processPrivateByte = new ProcessPrivateByte();
                    processPrivateByte.ProcessTable = processTable;
                    //processPrivateByte.ProcessInformation = ProcessInformation;
                    processPrivateByte.Time = DateTime.Now;
                    processPrivateByte.PrivateBytes = privateBytes;

                    lock (locker)
                    {
                        context.ProcessPrivateBytes.Add(processPrivateByte);
                        context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"PrivateBytes: {e.Message}");
                    Thread.Sleep(1000);
                }
            }
        }
        void HandleCount(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    Debug.WriteLine("Операция отменена токеном 4");
                    return;
                }

                try
                {
                    ProcessHandleCount process = new ProcessHandleCount();
                    process.ProcessTable = processTable;
                    process.Time = DateTime.Now;
                    process.HandleCount = myProcess.HandleCount;

                    lock (locker)
                    {
                        context.ProcessHandleCounts.Add(process);
                        context.SaveChanges();
                    }

                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"LoadPrecessAsync: {e.Message}");
                    Thread.Sleep(1000);
                }
            }
        }

        async void LoadPrecessAsync()
        {
            await Task.Run(() => LoadPrecess(token));
        }
        async void WorkingSetProcessAsync()
        {
            await Task.Run(() => WorkingSetProcess(token));

        }
        async void PrivateBytesProcessAsync()
        {
            await Task.Run(() => PrivateBytesProcess(token));
        }
        async void HandleCountAsync()
        {
            await Task.Run(() => HandleCount(token));
        }
    }
}
