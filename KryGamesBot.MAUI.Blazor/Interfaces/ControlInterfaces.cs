using Gambler.Bot.Core.Games;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace KryGamesBot.MAUI.Blazor.Interfaces
{

    public interface iLiveBet
    {
        void AddBet(Bet newBet);
        EventCallback<ViewBetEventArgs> BetClicked { get; set; }
        
    }

    public interface iPlaceBet
    {
        
        EventCallback<PlaceBetEventArgs> PlaceBet { get; set; }
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
