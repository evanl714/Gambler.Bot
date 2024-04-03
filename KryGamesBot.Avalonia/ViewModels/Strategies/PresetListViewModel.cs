using DoormatBot.Strategies;
using KryGamesBot.Ava.Classes.BetsPanel;
using KryGamesBot.Ava.Classes.Strategies;
using KryGamesBot.Ava.ViewModels.Games.Dice;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using static DoormatBot.Strategies.PresetList;

namespace KryGamesBot.Ava.ViewModels.Strategies
{
    public class PresetListViewModel : ViewModelBase, IStrategy
    {
        private PresetList _strategy;

        public PresetList Strategy
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

        private ObservableCollection<PresetDiceBet> _betList;

        public ObservableCollection<PresetDiceBet> BetList
        {
            get { return _betList; }
            set { _betList = value; }
        }

        public DoormatCore.Games.Games Game
        {
            get { return _game; }
            set { _game = value; this.RaisePropertyChanged(); }
        }
        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; }
        }

        public PresetListViewModel()
        {
            AddCommand = ReactiveCommand.Create(Add);
        }

        public ICommand AddCommand { get; set; }
        void Add()
        {
            int insertindex = SelectedIndex + 1;
            if (insertindex >= BetList.Count)
                BetList.Add(new PresetDiceBet { Amount = 0 });
            else
                BetList.Insert(SelectedIndex + 1, new PresetDiceBet { Amount = 0 });
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
                case DoormatCore.Games.Games.Dice: PlaceBetVM = new DicePlaceBetViewModel { ShowToggle = true }; break;
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
            if (!(Strategy is PresetList mart))
                throw new ArgumentException("Must be martingale to use thise viewmodel");

            this.Strategy = mart;
            BetList = new ObservableCollection<PresetDiceBet>(mart.PresetBets);
            if (PlaceBetVM is DicePlaceBetViewModel dice)
            {
                dice.Amount = mart.Amount;
                dice.Chance = mart.Chance;
                dice.ShowAmount = false;
            }
        }
        public void Saving()
        {
            Strategy.PresetBets = new BindingList<PresetDiceBet>(BetList);
        }
        public bool TopAlign()
        {
            return true;
        }
    }
}
