using DoormatBot.Strategies;
using Microsoft.Scripting.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

namespace KryGamesBotControls.Strategies.Labouchere
{
    /// <summary>
    /// Interaction logic for Labouchere.xaml
    /// </summary>
    public partial class Labouchere : BaseControl, iStrategy
    {
        public UserControl StartControl { get; set; } = new LabouchereDice();
        public class LabBet
        {
            public decimal Amount { get; set; }
        }

        public BindingList<LabBet> Bets { get; set; } = new BindingList<LabBet>();

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
            
            Bets.AddingNew += Bets_AddingNew;
            gcItems.ItemsSource = Bets;
            gcItems.Height = this.Height - 30;
            this.SizeChanged += Labouchere_SizeChanged;
        }

        private void Bets_AddingNew(object sender, AddingNewEventArgs e)
        {
            
        }

        private void Labouchere_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            gcItems.MaxHeight = e.NewSize.Height - 23;
            gcItems.Height = e.NewSize.Height -23;
        }

        public event EventHandler StartBetting;

        public void GameChanged(DoormatCore.Games.Games newGame)
        {
            switch (newGame)
            {
                case DoormatCore.Games.Games.Dice:
                default: StartControl = new LabouchereDice(); break;

            }
            StartControl.DataContext = Strategy;
            OnPropertyChanged("StartControl");
        }

        public void SetStrategy(BaseStrategy Strategy)
        {
            if (Strategy is DoormatBot.Strategies.Labouchere)
            {
                this.Strategy = Strategy as DoormatBot.Strategies.Labouchere;
                Bets.Clear();
                if (this.Strategy!=null && this.Strategy.BetList!=null)
                foreach (decimal x in this.Strategy?.BetList)
                    Bets.Add(new LabBet { Amount= x });
                if (Bets.Count==0)
                {
                    Bets.Add(new LabBet { Amount = 0.00000001m });
                    Bets.Add(new LabBet { Amount = 0.00000002m });
                    Bets.Add(new LabBet { Amount = 0.00000003m });
                    Bets.Add(new LabBet { Amount = 0.00000004m });
                    Bets.Add(new LabBet { Amount = 0.00000005m });
                    Bets.Add(new LabBet { Amount = 0.00000006m });
                    Bets.Add(new LabBet { Amount = 0.00000005m });
                    Bets.Add(new LabBet { Amount = 0.00000004m });
                    Bets.Add(new LabBet { Amount = 0.00000003m });
                    Bets.Add(new LabBet { Amount = 0.00000002m });
                    Bets.Add(new LabBet { Amount = 0.00000001m });
                }
                OnPropertyChanged(nameof(Bets));
                StartControl.DataContext = Strategy;
            }
        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            //show stuff and shit like whatever
            //ok but really, show a browse button to import a list of bets - csv, text, excel??
        }
        public bool TopAlign()
        {
            return false;
        }

        public void Saving()
        {
            Strategy.BetList = Bets.Select(m => (m as LabBet).Amount).ToList();
            
        }

        private void BaseControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }
    }
}
