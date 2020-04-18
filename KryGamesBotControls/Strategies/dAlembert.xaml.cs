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
    /// Interaction logic for dAlembert.xaml
    /// </summary>
    public partial class dAlembert : BaseControl, iStrategy
    {
        private DoormatBot.Strategies.DAlembert strategy;

        public event EventHandler StartBetting;

        public DoormatBot.Strategies.DAlembert Strategy
        {
            get { return strategy; }
            set { strategy = value; OnPropertyChanged(nameof(Strategy)); }
        }


        public dAlembert()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public void GameChanged(DoormatCore.Games.Games newGame)
        {
            
        }

        public void SetStrategy(BaseStrategy Strategy)
        {
            if (Strategy is DoormatBot.Strategies.DAlembert)
                this.Strategy = Strategy as DoormatBot.Strategies.DAlembert;
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
