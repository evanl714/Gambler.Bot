using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gambler.Bot.Strategies.Strategies.Abstractions;
using Gambler.Bot.Common.Games;
using Gambler.Bot.Common.Games.Dice;

namespace Gambler.Bot.Classes.Strategies
{
    public interface IStrategy:IDisposable
    {        
        void GameChanged(Games newGame, IGameConfig gameSettings);
        void SetStrategy(BaseStrategy Strategy);
        bool TopAlign();
        void Saving();

    }
}
