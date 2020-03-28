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
    /// Interaction logic for SiteStats.xaml
    /// </summary>
    public partial class SiteStats : BaseControl
    {
        private DoormatCore.Sites.SiteStats stats;

        public DoormatCore.Sites.SiteStats Stats
        {
            get { return stats; }
            set { stats = value; OnPropertyChanged(nameof(Stats)); }
        }

        public SiteStats()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void RefreshStats()
        {
            OnPropertyChanged(nameof(Stats));
        }
    }
}
