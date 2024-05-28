using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace KryGamesBot.MAUI.Blazor.Interfaces
{
    public interface iStrategy
    {
        EventCallback StartBetting { get; set; }
        void GameChanged(Gambler.Bot.Core.Games.Games newGame);
        void SetStrategy(Gambler.Bot.AutoBet.Strategies.BaseStrategy Strategy);
        bool TopAlign();
        Task Saving();
        Gambler.Bot.AutoBet.Doormat BotInstance { get; set; }

    }
}
