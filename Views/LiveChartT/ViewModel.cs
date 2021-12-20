using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using StartProcessTime.Data;
using StartProcessTime.Data.Information;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartProcessTime.Views.LiveChartT
{
    public class ViewModel
    {
        public IEnumerable<ISeries> Series { get; set; }
        public IEnumerable<ICartesianAxis> XAxes { get; set; }
        public IEnumerable<ICartesianAxis> YAxes { get; set; }
        ProcessTimeContext Context;

        private List<DateTimePoint> processLoadCPUs;
        private List<DateTimePoint> processPrivateBytes;
        private List<DateTimePoint> processWorkingSets;
        private List<DateTimePoint> processHandleCounts;

        public ViewModel(ProcessTable processTable)
        {
            processLoadCPUs = processTable.ProcessLoadCPUs.Select(cpu => new DateTimePoint(cpu.Time, (double)cpu.LoadCPU)).ToList();
            processPrivateBytes = processTable.ProcessPrivateBytes.Select(x => new DateTimePoint(x.Time, (double)x.PrivateBytes)).ToList();
            processWorkingSets = processTable.ProcessWorkingSets.Select(x => new DateTimePoint(x.Time, (double)x.WorkingSet)).ToList();
            processHandleCounts = processTable.ProcessHandleCounts.Select(c => new DateTimePoint(c.Time, c.HandleCount)).ToList();

            ShowGraphs();
        }
        public ViewModel(List<ProcessLoadCPU> _processLoadCpus, List<ProcessHandleCount> _processHandleCounts, List<ProcessPrivateByte> _processPrivateBytes, List<ProcessWorkingSet> _processWorkingSets)
        {
            processLoadCPUs = _processLoadCpus.Select(cpu => new DateTimePoint(cpu.Time, (double)cpu.LoadCPU)).ToList();
            processPrivateBytes = _processPrivateBytes.Select(x => new DateTimePoint(x.Time, (double)x.PrivateBytes)).ToList();
            processWorkingSets = _processWorkingSets.Select(x => new DateTimePoint(x.Time, (double)x.WorkingSet)).ToList();
            processHandleCounts = _processHandleCounts.Select(c => new DateTimePoint(c.Time, c.HandleCount)).ToList();

            ShowGraphs();
        }
        public ViewModel(ProcessTimeContext сontext)
        {
            Context = сontext;

            processLoadCPUs = Context.ProcessLoadCPUs.Select(cpu => new DateTimePoint(cpu.Time, (double)cpu.LoadCPU)).ToList();
            processPrivateBytes = Context.ProcessPrivateBytes.Select(x => new DateTimePoint(x.Time, (double)x.PrivateBytes)).ToList();
            processWorkingSets = Context.ProcessWorkingSets.Select(x => new DateTimePoint(x.Time, (double)x.WorkingSet)).ToList();
            processHandleCounts = Context.ProcessHandleCounts.Select(c => new DateTimePoint(c.Time, c.HandleCount)).ToList();

            ShowGraphs();
        }

        private void ShowGraphs()
        {
            //Цвета графиков
            var blue = new SKColor(25, 118, 210);
            var red = new SKColor(229, 57, 53);
            var yellow = new SKColor(198, 167, 0);
            var green = new SKColor(0, 128, 0);

            Series = new ObservableCollection<ISeries>
            {
                new LineSeries<DateTimePoint>
                {
                    Name = "LoadCPU",
                    Values = processLoadCPUs,
                    ScalesYAt = 0
                    //Values = new ObservableCollection<DateTimePoint>
                },
                new LineSeries<DateTimePoint>
                {
                    Name = "PrivateBytes",
                    Values = processPrivateBytes,
                    ScalesYAt = 1
                },
                new LineSeries<DateTimePoint>
                {
                    Name = "WorkingSet",
                    Values = processWorkingSets,
                    ScalesYAt = 2
                },
                new LineSeries<DateTimePoint>
                {
                    Name = "HandleCount",
                    Values = processHandleCounts,
                    ScalesYAt = 3
                }
            };

            //Ось X
            XAxes = new List<Axis>
            {
                new Axis
                {
                    Labeler = value => new DateTime((long) value).ToString("ss"),//("dd"),//"MMMM dd"
                    //Labeler = value => new DateTime().ToString("hh"),//"MMMM dd"
                    //LabelsRotation = 15,

                    //in this case we want our columns with a width of 1 day, we can get that number
                    // using the following syntax
                    ///UnitWidth = TimeSpan.FromDays(1).Ticks,
                     //UnitWidth = TimeSpan.FromSeconds(30).Ticks,

                    // The MinStep property forces the separator to be greater than 1 day.
                    ///MinStep = TimeSpan.FromDays(1).Ticks
                     //MinStep = TimeSpan.FromSeconds(10).Ticks

                    // if the difference between our points is in hours then we would:
                    //UnitWidth = TimeSpan.FromHours(1).Ticks,

                    // since all the months and years have a different number of days
                    // we can use the average, it would not cause any visible error in the user interface
                    // Months: TimeSpan.FromDays(30.4375).Ticks
                    // Years: TimeSpan.FromDays(365.25).Ticks
                }
            };

            YAxes = new List<Axis>
            {
                new Axis // the "units" and "tens" series will be scaled on this axis
                {
                    Name = "LoadCPU",
                    LabelsPaint = new SolidColorPaint(blue),
                    ShowSeparatorLines = false,
                    //Position = LiveChartsCore.Measure.AxisPosition.End
                },
                new Axis // the "hundreds" series will be scaled on this axis
                {
                    Name = "PrivateBytes",
                    LabelsPaint = new SolidColorPaint(red),
                    ShowSeparatorLines = false,
                    Position = LiveChartsCore.Measure.AxisPosition.End
                },
                new Axis() // the "thousands" series will be scaled on this axis
                {
                    Name = "WorkingSet",
                    LabelsPaint = new SolidColorPaint(yellow),
                    ShowSeparatorLines = false,
                    Position = LiveChartsCore.Measure.AxisPosition.End
                },
                new Axis // the "units" and "tens" series will be scaled on this axis
                {
                    Name = "HandleCount",
                    LabelsPaint = new SolidColorPaint(green),
                    ShowSeparatorLines = false,
                    //Position = LiveChartsCore.Measure.AxisPosition.End
                },
            };
        }
    }
}
