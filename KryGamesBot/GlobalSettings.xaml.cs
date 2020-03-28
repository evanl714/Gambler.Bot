using DevExpress.Xpf.Core;
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
using System.Windows.Shapes;

namespace KryGamesBot
{
    /// <summary>
    /// Interaction logic for GlobalSettings.xaml
    /// </summary>
    public partial class GlobalSettings : DXWindow
    {
        public string[] SettingItems { get; set; }
        public GlobalSettings()
        {
            InitializeComponent();
            SettingItems =  new string[]{"Skin","KeePass","Bet Storage","Errors","Notifications","Updates","Live View","Donate","Proxy","Strategy Storage" };
            DataContext = this;
        }

        private void TreeListControl_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {

        }
    }
}
