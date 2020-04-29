using DoormatBot.Strategies;
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
    /// Interaction logic for Martingale.xaml
    /// </summary>
    public partial class MartingaleControl : BaseControl, iStrategy
    {
        private DoormatBot.Strategies.Martingale currentStrat = null;
        public DoormatBot.Strategies.Martingale Strategy { 
            get { 
                return currentStrat; } 
            set { 
                currentStrat = value;
                OnPropertyChanged(nameof(Strategy)); 
            }
        }

        public UserControl StartControl { get; set; } = new MartingaleDice();

        public MartingaleControl()
        {
            InitializeComponent();
            
            DataContext = this;            
        }

        public event EventHandler StartBetting;

        public void GameChanged(DoormatCore.Games.Games newGame)
        {
            switch(newGame)
            {
                case DoormatCore.Games.Games.Dice:
                default: StartControl = new MartingaleDice();break;

            }
            StartControl.DataContext = Strategy;
            OnPropertyChanged("StartControl");
        }

        public void SetStrategy(BaseStrategy Strategy)
        {
            if (Strategy is DoormatBot.Strategies.Martingale)
                this.Strategy = (Strategy as DoormatBot.Strategies.Martingale);
            StartControl.DataContext = Strategy;
            OnPropertyChanged(nameof(Strategy));
        }

        private void ComboBoxEdit_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            switch(cbeMultiplierMode.SelectedIndex)
            {
                case 0: lciDivideCounter.Visibility = lciDivider.Visibility = lciMaxMultiplies.Visibility = Visibility.Collapsed;  break;
                case 1: lciDivideCounter.Visibility = lciDivider.Visibility =  Visibility.Collapsed; lciMaxMultiplies.Visibility = Visibility.Visible; break;
                case 2: 
                case 3: lciDivideCounter.Visibility = lciDivider.Visibility = Visibility.Visible; lciMaxMultiplies.Visibility = Visibility.Collapsed; break;
            }
        }

        private void cbeMultiplierModeWin_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            switch (cbeMultiplierModeWin.SelectedIndex)
            {
                case 0: lciWinDivideCounter.Visibility = lciWinDivider.Visibility = lciWinMaxMultiplies.Visibility = Visibility.Collapsed; break;
                case 1: lciWinDivideCounter.Visibility = lciWinDivider.Visibility = Visibility.Collapsed; lciWinMaxMultiplies.Visibility = Visibility.Visible; break;
                case 2:
                case 3: lciWinDivideCounter.Visibility = lciWinDivider.Visibility = Visibility.Visible; lciWinMaxMultiplies.Visibility = Visibility.Collapsed; break;
            }
        }
        public bool TopAlign()
        {
            return true;
        }

        public void Saving()
        {
            
        }
    }
}
