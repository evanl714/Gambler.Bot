using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KryGamesBotControls.Strategies.PresetList
{
    /// <summary>
    /// Interaction logic for LabouchereDice.xaml
    /// </summary>
    public partial class PresetDice : UserControl
    {
        public decimal Edge { get; set; }
        public PresetDice()
        {
            InitializeComponent();
        }

        private void seChance_LostFocus(object sender, RoutedEventArgs e)
        {
            Calculate(nameof(seChance));
        }

        private void sePayout_LostFocus(object sender, RoutedEventArgs e)
        {
            Calculate(nameof(sePayout));
        }

        private void btnLow_Checked(object sender, RoutedEventArgs e)
        {
            btnLow.IsChecked = true;
            btnHigh.IsChecked = false;
        }

        private void btnHigh_Checked(object sender, RoutedEventArgs e)
        {
            btnHigh.IsChecked = true;
            btnLow.IsChecked = false;
        }

        void Calculate(string s)
        {
            switch (s)
            {
                
                case nameof(seChance):
                    if (seChance.Value != 0)
                    {
                        if (sePayout.Value != (100m - Edge) / seChance.Value)
                        {
                            sePayout.EditValue = (100m - Edge) / seChance.Value;
                        }
                    }
                    break;
                case nameof(sePayout):
                    if (seChance.Value != 0)
                    {
                        if (seChance.Value != (100m - Edge) / sePayout.Value)
                        {
                            seChance.EditValue = (100m - Edge) / sePayout.Value;
                        }                        
                    }
                    break;
            }
        }
    }
}
