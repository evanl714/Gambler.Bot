using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using DoormatCore.Games;

namespace KryGamesBotControls.Games.Dice
{
    /// <summary>
    /// Interaction logic for DiceLiveBetsControl.xaml
    /// </summary>
    public partial class DiceLiveBetsControl : UserControl, iLiveBet
    {
        public ListCollectionView Bets = new ListCollectionView(new List<DiceBet>());
        public DiceLiveBetsControl()
        {
            InitializeComponent();
            this.DataContext = this;
            gcDiceBets.ItemsSource = Bets;
            Bets.IsLiveSorting = true;
            Bets.SortDescriptions.Add(new System.ComponentModel.SortDescription("DateValue", System.ComponentModel.ListSortDirection.Descending));
        }
        bool fitted = false;

        public event EventHandler<ViewBetEventArgs> BetClicked;
        delegate void dAddBet(Bet newBet);
        public void AddBet(Bet newBet)
        {
            if (!Dispatcher.CheckAccess())
                Dispatcher.BeginInvoke(new dAddBet(AddBet), newBet);
            else
            {
                //if (Bets.CanRemove)
                    while (Bets.Count > this.NumberOfBets()+1)
                    {

                        //System.Threading.Thread.Sleep(10);
                        //Bets.Remove(Bets.list);
                        Bets.RemoveAt(Bets.Count-1);
                    }
                Bets.AddNewItem(newBet as DiceBet);
                Bets.CommitNew();
                if (!fitted)
                {
                    tvBets.BestFitColumn(colTime);
                    fitted = true;
                }
            }
        }

        public int NumberOfBets()
        {
            return 100;
        }

        private void TableView_RowDoubleClick(object sender, DevExpress.Xpf.Grid.RowDoubleClickEventArgs e)
        {
            DiceBet bet = gcDiceBets.SelectedItem as DiceBet;
            BetClicked?.Invoke(this, new ViewBetEventArgs(bet));
        }
    }
}
