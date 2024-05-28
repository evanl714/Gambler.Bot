using KryGamesBotControls.Games;
using System;
using System.Collections.Generic;
using System.Text;

namespace KryGamesBotControls.Strategies
{
    public interface iStrategy
    {
        event EventHandler StartBetting;
        void GameChanged(Gambler.Bot.Core.Games.Games newGame);
        void SetStrategy(Gambler.Bot.AutoBet.Strategies.BaseStrategy Strategy);
        bool TopAlign();
        void Saving();

    }
}
