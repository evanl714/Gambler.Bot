using Avalonia.Threading;
using DoormatCore.Games;
using DoormatCore.Helpers;
using IronPython.Runtime;
using Gambler.Bot.Classes;
using Gambler.Bot.Classes.BetsPanel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gambler.Bot.ViewModels.Games.Dice
{
    internal class DiceLiveBetViewModel : ViewModelBase, iLiveBet
    {
        public event EventHandler<ViewBetEventArgs> BetClicked;
        public ObservableCollection<DiceBet> Bets { get; set; } = new ObservableCollection<DiceBet>();
        
        public DiceLiveBetViewModel(ILogger logger):base(logger)
        {
            
        }

        public void AddBet(Bet newBet)
        {
            try
            {
                if (Dispatcher.UIThread.CheckAccess())
                {


                    //if (Bets.CanRemove)
                    while (Bets.Count > UISettings.Settings.LiveBets + 1)
                    {

                        //System.Threading.Thread.Sleep(10);
                        //Bets.Remove(Bets.list);
                        Bets.RemoveAt(Bets.Count - 1);
                    }

                    Bets.Insert(0, newBet as DiceBet);
                }
                else
                {
                    Dispatcher.UIThread.Invoke(() => AddBet(newBet));
                }
            }
            catch (Exception e)
            {
                _logger?.LogError(e.ToString());
            }
            
        }
    }
}
