using DoormatCore.Games;
using System;
using System.Collections.Generic;
using System.Text;

namespace KryGamesBotControls.Games
{

    public interface iLiveBet
    {
        void AddBet(Bet newBet);
        event EventHandler<ViewBetEventArgs> BetClicked;
        
    }

    public interface iPlaceBet
    {
        
        event EventHandler<PlaceBetEventArgs> PlaceBet;
    }

    public interface iBetResult
    {

    }

    public interface iBetHistory
    {

    }

    public class ViewBetEventArgs:EventArgs
    {
        public Bet BetToView { get; set; }
        public ViewBetEventArgs(Bet bettoview)
        {
            this.BetToView = bettoview;
        }
    }
    public class PlaceBetEventArgs : EventArgs
    {
        public PlaceBet NewBet { get; set; }
        public PlaceBetEventArgs(PlaceBet NewBet)
        {
            this.NewBet = NewBet;
        }
    }

   
}
