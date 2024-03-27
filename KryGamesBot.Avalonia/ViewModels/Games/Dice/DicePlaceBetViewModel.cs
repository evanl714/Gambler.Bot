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
        private bool _showAmount=true;

        public bool ShowAmount
        {
            get { return _showAmount; }
            set { _showAmount = value; this.RaisePropertyChanged(); }
        }
        private bool _showChance = true;

        public bool ShowChance
        {
            get { return _showChance; }
            set { _showChance = value; this.RaisePropertyChanged(); }
        }

        private bool _highChecked;

        public bool HighChecked
        {
            get { return _highChecked; }
            set { _highChecked = value; this.RaisePropertyChanged(); this.RaisePropertyChanged(nameof(LowChecked)); }
        }

       

        public bool LowChecked
        {
            get { return !HighChecked; }
            set { HighChecked = !value; }
        }

        public ICommand BetHighCommand { get; }
        public ICommand BetLowCommand { get; }

        private decimal amount=0.00000100m;

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; this.RaisePropertyChanged(nameof(Amount)); Calculate(nameof(Amount)); }
        }

        private decimal chance=49.5m;

        public decimal Chance
        {
            get { return chance; }
            set { chance = value; this.RaisePropertyChanged(nameof(Chance)); Calculate(nameof(Chance)); }
        }

        private decimal payout=2;

        public decimal Payout
        {
            get { return payout; }
            set { payout = value; this.RaisePropertyChanged(nameof(Payout)); Calculate(nameof(Payout)); }
        }

        private decimal profit=0.00000100m;

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
            Calculate(nameof(Amount));
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

        private bool showToggle=false;

        public bool ShowToggle
        {
            get { return showToggle; }
            set { showToggle = value; this.RaisePropertyChanged();this.RaisePropertyChanged(nameof(ShowButton)); }
        }

        public bool ShowButton { get=>!ShowToggle; }

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
