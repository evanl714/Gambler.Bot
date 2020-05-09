using KryGamesBotControls.Helpers;
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

namespace KryGamesBotControls.Common
{
    /// <summary>
    /// Interaction logic for LiveBetSettings.xaml
    /// </summary>
    public partial class LiveBetSettings : UserControl
    {
        public LiveBetSettings()
        {
            InitializeComponent();
            seLiveBets.EditValue = UISettings.Settings.LiveBets;
            seProfitChart.EditValue = UISettings.Settings.ChartBets;
        }

        private void seLiveBets_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                UISettings.Settings.LiveBets=(int)seLiveBets.EditValue;
            }
            catch
            {

            }
        }

        private void seProfitChart_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                UISettings.Settings.ChartBets = (int)seProfitChart.EditValue;
            }
            catch
            {

            }
        }
    }
}
