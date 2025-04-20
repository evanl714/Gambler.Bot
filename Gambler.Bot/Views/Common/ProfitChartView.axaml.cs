using Avalonia.Controls;
using System;

namespace Gambler.Bot.Views.Common
{
    public partial class ProfitChartView : UserControl
    {
        public ProfitChartView()
        {
            InitializeComponent();
        }
        bool showAnimations = true;
        Func<float, float> easingfunc = null;
        public bool ShowAnimations 
        { 
            get => showAnimations; 
            set
            {
                showAnimations= value;
                if (showAnimations)
                {
                    if (easingfunc != null && CartessianChart.EasingFunction == null)
                    {
                        CartessianChart.EasingFunction = easingfunc;
                    }
                }
                else 
                {
                    if (CartessianChart.EasingFunction!=null)
                    {
                        easingfunc= CartessianChart.EasingFunction;
                    }
                    CartessianChart.EasingFunction = null;
                }

            }

        } 

        private void CartesianChart_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
        }
    }
}
