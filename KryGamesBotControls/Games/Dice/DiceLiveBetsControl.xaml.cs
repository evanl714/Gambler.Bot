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
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
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

            /*tvBets.VisibleColumns[0].View       
            Color color = (Color)ColorConverter.ConvertFromString(x.ToString());
            int BrushValue = (int)color.R + (int)color.G + (int)color.B;*/
            bool dark = Helpers.ThemesProviderExtension.IsDark();
            
            /*int BrushValue = (int)syntaxEditor.AutoBackground.Color.R + (int)syntaxEditor.AutoBackground.Color.G + (int)syntaxEditor.AutoBackground.Color.B;
        dark = (double)BrushValue / 3.0 < 125;*/
            //this.Background = new Brush()
            
            var WinningBetCondition = new FormatCondition()
            {
                Expression = "[IsWin]=true",
                FieldName = "IsWin",
                Format = new DevExpress.Xpf.Core.ConditionalFormatting.Format()
                {
                    Background = new SolidColorBrush(dark? Colors.Green: Colors.LightGreen)
                }
                , ApplyToRow=true
            };
            var LosingBetCondition = new FormatCondition()
            {
                Expression = "[IsWin]=false",
                FieldName = "IsWin",
                Format = new DevExpress.Xpf.Core.ConditionalFormatting.Format()
                {
                    Background = new SolidColorBrush(dark ? Colors.Brown : Colors.Pink)
                },
                ApplyToRow = true
            };
            var NoWinCondition = new FormatCondition()
            {
                Expression = "[WinnableType]=1",
                FieldName = "Roll",
                Format = new DevExpress.Xpf.Core.ConditionalFormatting.Format()
                {
                    Background = new SolidColorBrush( dark? Colors.DarkOrange: Colors.Gold)
                },
                ApplyToRow = false,
                
            };
            var AllCondition = new FormatCondition()
            {
                Expression = "[WinnableType]=2",
                FieldName = "Roll",
                Format = new DevExpress.Xpf.Core.ConditionalFormatting.Format()
                {
                    Background = new SolidColorBrush(dark? Colors.DimGray: Colors.LightGray)
                },
                ApplyToRow = false
            };
            var WinCondition = new FormatCondition()
            {
                Expression = "[WinnableType]=3",
                FieldName = "Roll",
                Format = new DevExpress.Xpf.Core.ConditionalFormatting.Format()
                {
                    Background = new SolidColorBrush(dark ? Colors.Green : Colors.LightGreen)
                },
                ApplyToRow = false
            };
            var LossCondition = new FormatCondition()
            {
                Expression = "[WinnableType]=4",
                FieldName = "Roll",
                Format = new DevExpress.Xpf.Core.ConditionalFormatting.Format()
                {
                    Background = new SolidColorBrush(dark ? Colors.Brown : Colors.Pink)
                },
                ApplyToRow = false
            };
            tvBets.FormatConditions.Add(WinningBetCondition);
            tvBets.FormatConditions.Add(LosingBetCondition);
            tvBets.FormatConditions.Add(NoWinCondition);
            tvBets.FormatConditions.Add(AllCondition);
            tvBets.FormatConditions.Add(WinCondition);
            tvBets.FormatConditions.Add(LossCondition);
            
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
                //if (!fitted)
                {
                    tvBets.BestFitColumns();
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
