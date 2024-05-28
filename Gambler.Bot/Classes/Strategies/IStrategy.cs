using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gambler.Bot.Classes.Strategies
{
    public interface IStrategy:IDisposable
    {        
        void GameChanged(Gambler.Bot.Core.Games.Games newGame);
        void SetStrategy(Gambler.Bot.AutoBet.Strategies.BaseStrategy Strategy);
        bool TopAlign();
        void Saving();

    }
}
