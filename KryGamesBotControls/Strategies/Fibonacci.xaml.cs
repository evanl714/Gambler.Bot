using Gambler.Bot.AutoBet.Strategies;
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
        private Gambler.Bot.AutoBet.Strategies.Fibonacci strategy;

        public Gambler.Bot.AutoBet.Strategies.Fibonacci Strategy
        {
            get { return strategy; }
            set { strategy = value; OnPropertyChanged(nameof(Strategy)); }
        }


        public Fibonacci()
        {
            InitializeComponent();
        }

        public event EventHandler StartBetting;

        public void GameChanged(Gambler.Bot.Core.Games.Games newGame)
        {
            throw new NotImplementedException();
        }

        public void SetStrategy(BaseStrategy Strategy)
        {
            if (Strategy is Gambler.Bot.AutoBet.Strategies.Fibonacci)
                this.Strategy = Strategy as Gambler.Bot.AutoBet.Strategies.Fibonacci;

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
