using KryGamesBotControls.Games;
using System;
using System.Collections.Generic;
using System.Text;

namespace KryGamesBotControls.Strategies
{
    public interface iStrategy
    {
        event EventHandler StartBetting;
        void GameChanged(DoormatCore.Games.Games newGame);
        void SetStrategy(DoormatBot.Strategies.BaseStrategy Strategy);
        bool TopAlign();

    }
}
