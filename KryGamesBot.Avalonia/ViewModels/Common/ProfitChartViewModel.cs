using Avalonia.Threading;
using KryGamesBot.Ava.Classes;
using KryGamesBot.Ava.Classes.Charts;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryGamesBot.Ava.ViewModels.Common
{
    public class ProfitChartViewModel:ViewModelBase
    {
        public int MaxItems { get; set; } = -1;
        RealTimeDataCollection DataPoints = new RealTimeDataCollection();

        public ISeries[] Series { get; set; } 

        public LabelVisual Title { get; set; } =
        new LabelVisual
        {
            Text = "My chart title",
            TextSize = 25,
            Padding = new LiveChartsCore.Drawing.Padding(15),
            Paint = new SolidColorPaint(SKColors.DarkSlateGray)
        };

        int ChartItems = 0;
        decimal ChartProfit = 0;
        delegate void dAddPoint(decimal profit);
        public bool Enabled { get; set; } = true;
        private string _freezeText;

        public string FreezeText
        {
            get { return _freezeText; }
            set { _freezeText = value; this.RaisePropertyChanged(); }
        }

        public ProfitChartViewModel()
        {
            FreezeText = "Freeze Chart";
            Series = new ISeries[] {
               new LineSeries<SimpleDataPoint>
               {
                   Values = DataPoints,
                   Fill = null,
                   Mapping = (sample, index) => new(sample.Argument, sample.Value)
               } };
            
        }

        public void AddPoint(decimal BetProfit)
        {
            if (Enabled)
            {
                if (!Dispatcher.UIThread.CheckAccess())
                    Dispatcher.UIThread.Invoke(()=> { AddPoint(BetProfit); });
                else
                {
                    ChartProfit += BetProfit;
                    DataPoints.Add(new SimpleDataPoint(ChartItems++, (double)ChartProfit));
                    //profitChart.AddPoint(ChartItems++, (double)Profit);
                    while (DataPoints.Count > (MaxItems > 0 ? MaxItems : UISettings.Settings.ChartBets))
                    {
                        DataPoints.RemoveRangeAt(0, (DataPoints.Count - (MaxItems > 0 ? MaxItems : UISettings.Settings.ChartBets)) + 1);
                    }
                }
            }
        }
        public void AddRange(List<decimal> points)
        {
            if (!Dispatcher.UIThread.CheckAccess())
                Dispatcher.UIThread.Invoke(() => { AddRange(points); });
            else
            {
                DataPoints.AddRange(points.Select(m => new SimpleDataPoint(ChartItems++, (double)(ChartProfit += m))).ToList());
                while (DataPoints.Count > (MaxItems > 0 ? MaxItems : UISettings.Settings.ChartBets))
                {
                    DataPoints.RemoveRangeAt(0, (DataPoints.Count - (MaxItems > 0 ? MaxItems : UISettings.Settings.ChartBets)) + 1);
                }
            }
        }
        
        public void Reset()
        {
            bool OriginalEnabled = Enabled;
            Enabled = false;
            ChartProfit = 0;
            ChartItems = 0;
            DataPoints.Clear();
            Enabled = OriginalEnabled;
        }

        

        private void ToggleFreeze()
        {
            Enabled = !Enabled;
            if (Enabled)
                FreezeText = "Freeze Chart";
            else
                FreezeText = "Resume Chart";
        }
    }
}
