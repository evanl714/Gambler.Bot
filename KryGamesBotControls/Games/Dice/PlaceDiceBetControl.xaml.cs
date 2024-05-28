using Gambler.Bot.Core.Games;
using System;
using System.Windows;
using System.Windows.Controls;

namespace KryGamesBotControls.Games.Dice
{
    /// <summary>
    /// Interaction logic for PlaceDiceBetControl.xaml
    /// </summary>
    public partial class PlaceDiceBetControl : BaseControl, iPlaceBet
    {
        private decimal amount;

        public decimal Amount
        {
            get { return amount; }
            set { amount = value;OnPropertyChanged(nameof(Amount)); Calculate(nameof(Amount)); }
        }

        private decimal chance;

        public decimal Chance
        {
            get { return chance; }
            set { chance = value; OnPropertyChanged(nameof(Chance)); Calculate(nameof(Chance)); }
        }

        private decimal payout;

        public decimal Payout
        {
            get { return payout; }
            set { payout = value; OnPropertyChanged(nameof(Payout)); Calculate(nameof(Payout)); }
        }

        private decimal profit;

        public decimal Profit
        {
            get { return profit; }
            set { profit = value; OnPropertyChanged(nameof(Profit)); Calculate(nameof(Profit)); }
        }

        public decimal Edge { get; set; } = 1;

        public PlaceDiceBetControl()
        {
            InitializeComponent();
            DataContext = this;
            Amount = 0.00000100m;
            Chance = 0.00000100m;
            
        }

        void Calculate(string s)
        {
            switch (s)
            {
                case nameof(Amount):
                    if (Profit != (Amount * Payout) - Amount)
                    {
                        Profit = (Amount * Payout) - Amount;
                    }
                    break;
                case nameof(Chance):
                    if (Chance != 0)
                    {
                        if (Payout != (100m - Edge) / Chance)
                        {
                            Payout = (100m - Edge) / Chance;
                        }
                    }
                    break;
                case nameof(Payout):
                    if (Chance != 0)
                    {
                        if (Chance != (100m - Edge) / Payout)
                        {
                            Chance = (100m - Edge) / Payout;
                        }
                        if (Profit != Amount * Payout - Amount)
                            Profit = Amount * Payout - Amount;
                    }
                    break;
            }
        }

        public event EventHandler<PlaceBetEventArgs> PlaceBet;

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            PlaceBet?.Invoke(this, new PlaceBetEventArgs(new PlaceDiceBet(Amount, true, Chance)));
        }

        private void SimpleButton_Click_1(object sender, RoutedEventArgs e)
        {
            PlaceBet?.Invoke(this, new PlaceBetEventArgs(new PlaceDiceBet(Amount, false, Chance)));
        }

        private void seAmount_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            /*if (Profit!=(Amount*Payout)-Amount)
            {
                Profit = (Amount * Payout) - Amount;
            }*/
        }

        private void seChance_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            /*if (Chance != 0)
            {
                if (Payout != (100m - Edge) / Chance)
                {
                    Payout = (100m - Edge) / Chance;
                }
            }*/
        }

        private void sePayout_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            /*if (Chance != 0)
            {
                if (Chance != (100m - Edge) / Payout)
                {
                    Chance = (100m - Edge) / Payout;
                }
                if (Profit != Amount * Payout - Amount)
                    Profit = Amount * Payout - Amount;
            }*/
        }
    }
}
