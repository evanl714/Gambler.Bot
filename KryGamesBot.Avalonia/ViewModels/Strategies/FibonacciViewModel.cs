using DoormatBot.Strategies;
using KryGamesBot.Ava.Classes.BetsPanel;
using KryGamesBot.Ava.Classes.Strategies;
using KryGamesBot.Ava.ViewModels.Games.Dice;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryGamesBot.Ava.ViewModels.Strategies
{
    internal class FibonacciViewModel : ViewModelBase, IStrategy
    {
        private Fibonacci _strategy;

        public Fibonacci Strategy
        {
            get { return _strategy; }
            set { _strategy = value; this.RaisePropertyChanged(); }
        }
        private DoormatCore.Games.Games _game;

        private iPlaceBet _placeBetVM;

        public iPlaceBet PlaceBetVM
        {
            get { return _placeBetVM; }
            set { _placeBetVM = value; this.RaisePropertyChanged(); }
        }


        public DoormatCore.Games.Games Game
        {
            get { return _game; }
            set { _game = value; this.RaisePropertyChanged(); }
        }

        public FibonacciViewModel(Microsoft.Extensions.Logging.ILogger logger) : base(logger)
        {
            
        }
        public void GameChanged(DoormatCore.Games.Games newGame)
        {
            if (PlaceBetVM != null && PlaceBetVM is INotifyPropertyChanged notify)
            {
                notify.PropertyChanged -= Notify2_PropertyChanged;
            }
            Game = newGame;
            switch (Game)
            {
                case DoormatCore.Games.Games.Dice: PlaceBetVM = new DicePlaceBetViewModel(_logger) { ShowToggle = true }; break;
                default: PlaceBetVM = null; break;
            }
            if (PlaceBetVM != null && PlaceBetVM is INotifyPropertyChanged notify2)
            {
                notify2.PropertyChanged += Notify2_PropertyChanged;
            }
        }

        private void Notify2_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.PropertyName))
                return;
            object value = sender.GetType().GetProperty(e.PropertyName).GetValue(sender);
            switch (e.PropertyName)
            {
                case "Amount":
                    Strategy.Amount = (decimal)value;
                    break;
                case "Chance":
                    Strategy.Chance = (decimal)value;
                    break;
            }
        }

        public void SetStrategy(BaseStrategy Strategy)
        {
            if (Strategy == null)
                throw new ArgumentNullException();
            if (!(Strategy is Fibonacci mart))
                throw new ArgumentException("Must be martingale to use thise viewmodel");

            this.Strategy = mart;
            if (PlaceBetVM is DicePlaceBetViewModel dice)
            {
                dice.Amount = mart.Amount;
                dice.Chance = mart.Chance;
                dice.ShowAmount = false;
            }
        }
        public void Saving()
        {

        }
        public bool TopAlign()
        {
            return true;
        }
        public void Dispose()
        {
            _placeBetVM = null;
        }
    }
}
