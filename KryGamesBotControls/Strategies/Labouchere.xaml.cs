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

namespace KryGamesBotControls.Strategies
{
    /// <summary>
    /// Interaction logic for Labouchere.xaml
    /// </summary>
    public partial class Labouchere : BaseControl, iStrategy
    {
        private DoormatBot.Strategies.Labouchere strategy;

        public DoormatBot.Strategies.Labouchere Strategy
        {
            get { return strategy; }
            set { strategy = value; OnPropertyChanged(nameof(Strategy)); }
        }


        public Labouchere()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event EventHandler StartBetting;

        public void GameChanged(DoormatCore.Games.Games newGame)
        {
            throw new NotImplementedException();
        }

        public void SetStrategy(BaseStrategy Strategy)
        {
            if (Strategy is DoormatBot.Strategies.Labouchere)
            {
                this.Strategy = Strategy as DoormatBot.Strategies.Labouchere;
            }
        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            //show stuff and shit like whatever
            //ok but really, show a browse button to import a list of bets - csv, text, excel??
        }
        public bool TopAlign()
        {
            return true;
        }
    }
}
