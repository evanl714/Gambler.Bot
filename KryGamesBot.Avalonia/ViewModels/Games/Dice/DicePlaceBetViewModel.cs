using DoormatCore.Games;
using KryGamesBot.Ava.Classes.BetsPanel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KryGamesBot.Ava.ViewModels.Games.Dice
{
    public class DicePlaceBetViewModel :ViewModelBase, iPlaceBet
    {
        public ICommand BetHighCommand { get; }
        public ICommand BetLowCommand { get; }

        private decimal amount;

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; this.RaisePropertyChanged(nameof(Amount)); Calculate(nameof(Amount)); }
        }

        private decimal chance;

        public decimal Chance
        {
            get { return chance; }
            set { chance = value; this.RaisePropertyChanged(nameof(Chance)); Calculate(nameof(Chance)); }
        }

        private decimal payout;

        public decimal Payout
        {
            get { return payout; }
            set { payout = value; this.RaisePropertyChanged(nameof(Payout)); Calculate(nameof(Payout)); }
        }

        private decimal profit;

        public decimal Profit
        {
            get { return profit; }
            set { profit = value; this.RaisePropertyChanged(nameof(Profit)); Calculate(nameof(Profit)); }
        }

        public decimal Edge { get; set; } = 1;


        public DicePlaceBetViewModel()
        {
            BetHighCommand = ReactiveCommand.Create(BetHigh);
            BetLowCommand = ReactiveCommand.Create(BetLow);
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

        private void Bet(bool High)
        {
            PlaceBet?.Invoke(this, new PlaceBetEventArgs(new PlaceDiceBet(Amount, High, Chance)));
        }

        private void BetHigh()
        {
            Bet(true);
        }
        private void BetLow()
        {
            Bet(false);
        }
    }
}
