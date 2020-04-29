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
    /// Interaction logic for Fibonacci.xaml
    /// </summary>
    public partial class Fibonacci : BaseControl, iStrategy
    {
        private DoormatBot.Strategies.Fibonacci strategy;

        public DoormatBot.Strategies.Fibonacci Strategy
        {
            get { return strategy; }
            set { strategy = value; OnPropertyChanged(nameof(Strategy)); }
        }


        public Fibonacci()
        {
            InitializeComponent();
        }

        public event EventHandler StartBetting;

        public void GameChanged(DoormatCore.Games.Games newGame)
        {
            throw new NotImplementedException();
        }

        public void SetStrategy(BaseStrategy Strategy)
        {
            if (Strategy is DoormatBot.Strategies.Fibonacci)
                this.Strategy = Strategy as DoormatBot.Strategies.Fibonacci;

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
