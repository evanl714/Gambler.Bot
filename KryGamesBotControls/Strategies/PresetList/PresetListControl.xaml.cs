using DevExpress.XtraRichEdit.Model;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static DoormatBot.Strategies.PresetList;

namespace KryGamesBotControls.Strategies.PresetList
{
    /// <summary>
    /// Interaction logic for PresetListControl.xaml
    /// </summary>
    public partial class PresetListControl : BaseControl, iStrategy
    {
        DoormatCore.Games.Games Game = DoormatCore.Games.Games.Dice;
        public DoormatBot.Strategies.PresetList Strategy { get; set; }
        public UserControl StartControl { get; set; } = new PresetDice();
        public PresetListControl()
        {
            InitializeComponent();
            this.DataContext = this;
            this.SizeChanged += PresetListControl_SizeChanged;
        }

        private void PresetListControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            gcBets.Height = e.NewSize.Height - 24;
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSaveSas_Click(object sender, RoutedEventArgs e)
        {

        }
    

        public event EventHandler StartBetting;

        public void GameChanged(DoormatCore.Games.Games newGame)
        {
            Game = newGame;
            switch (newGame)
            {
                case DoormatCore.Games.Games.Dice:
                    break;
            }
        }

        public void Saving()
        {
            //throw new NotImplementedException();
        }

        public void SetStrategy(BaseStrategy Strategy)
        {
            if (Strategy is DoormatBot.Strategies.PresetList lst)
            {
                this.Strategy = lst;
                
            }
            this.DataContext = this;
            StartControl.DataContext = this.Strategy;
            OnPropertyChanged(nameof(this.Strategy));
            tvBets.BestFitColumns();
        }

        public bool TopAlign()
        {
            return false;
        }

        private void TableView_AddingNewRow(object sender, System.ComponentModel.AddingNewEventArgs e)
        {
            switch (Game)
            {
                case DoormatCore.Games.Games.Dice: e.NewObject = new PresetDiceBet { Amount = 0, Switch=false }; break;
            }
        }
    }
}
