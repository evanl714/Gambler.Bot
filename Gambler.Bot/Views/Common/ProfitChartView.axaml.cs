using Avalonia.Controls;

namespace Gambler.Bot.Views.Common
{
    public partial class ProfitChartView : UserControl
    {
        public ProfitChartView()
        {
            InitializeComponent();
        }

        private void CartesianChart_UpdateFinished(LiveChartsCore.Kernel.Sketches.IChartView<LiveChartsCore.SkiaSharpView.Drawing.SkiaSharpDrawingContext> chart)
        {
        }

        private void CartesianChart_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
        }
    }
}
