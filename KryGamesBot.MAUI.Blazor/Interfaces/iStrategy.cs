using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace KryGamesBot.MAUI.Blazor.Interfaces
{
    public interface iStrategy
    {
        EventCallback StartBetting { get; set; }
        void GameChanged(DoormatCore.Games.Games newGame);
        void SetStrategy(DoormatBot.Strategies.BaseStrategy Strategy);
        bool TopAlign();
        Task Saving();
        DoormatBot.Doormat BotInstance { get; set; }

    }
}
