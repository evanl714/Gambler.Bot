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
    /// Interaction logic for SessionStats.xaml
    /// </summary>
    public partial class SessionStats : BaseControl
    {
        public bool ShowResetButton { get { return btnReset.Visibility == Visibility.Visible; } set { btnReset.Visibility = value ? Visibility.Visible : Visibility.Collapsed; } }
        public event EventHandler ResetStats;
        private Gambler.Bot.AutoBet.Helpers.SessionStats stats;

        public Gambler.Bot.AutoBet.Helpers.SessionStats Stats
        {
            get { return stats; }
            set { stats = value; OnPropertyChanged(nameof(Stats)); }
        }

        public SessionStats()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void RefreshStats()
        {
            OnPropertyChanged(nameof(Stats));
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetStats?.Invoke(this, new EventArgs());
        }
    }
}
