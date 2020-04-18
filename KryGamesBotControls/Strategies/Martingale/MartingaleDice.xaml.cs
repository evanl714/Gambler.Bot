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

namespace KryGamesBotControls.Strategies.Martingale
{
    /// <summary>
    /// Interaction logic for MartingaleDice.xaml
    /// </summary>
    public partial class MartingaleDice : BaseControl
    {
        public decimal Edge { get; set; }
        public MartingaleDice()
        {
            InitializeComponent();
        }
        void Calculate(string s)
        {
            switch (s)
            {
                case nameof(seAmount):
                    if (seProfit.Value != (seAmount.Value * sePayout.Value) - seAmount.Value)
                    {
                        seProfit.EditValue = (seAmount.Value * sePayout.Value) - seAmount.Value;
                    }
                    break;
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
                        if (seProfit.Value != seAmount.Value * sePayout.Value - seAmount.Value)
                            seProfit.EditValue = seAmount.Value * sePayout.Value - seAmount.Value;
                    }
                    break;
            }
        }
        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void seAmount_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            
        }

        private void seChance_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            
        }

        private void sePayout_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            
        }

        private void btnHigh_Checked(object sender, RoutedEventArgs e)
        {
            btnHigh.IsChecked = true;
            btnLow.IsChecked = false;
        }

        private void btnLow_Checked(object sender, RoutedEventArgs e)
        {
            btnLow.IsChecked = true;
            btnHigh.IsChecked = false;
        }

        private void seAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            Calculate(nameof(seAmount));
        }

        private void seChance_LostFocus(object sender, RoutedEventArgs e)
        {
            Calculate(nameof(seChance));
        }

        private void sePayout_LostFocus(object sender, RoutedEventArgs e)
        {
            Calculate(nameof(sePayout));
        }
    }
}
