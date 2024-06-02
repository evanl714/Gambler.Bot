using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gambler.Bot.Strategies.Strategies.Abstractions;
using Gambler.Bot.Common.Games;

namespace Gambler.Bot.Classes.Strategies
{
    public interface IStrategy:IDisposable
    {        
        void GameChanged(Games newGame);
        void SetStrategy(BaseStrategy Strategy);
        bool TopAlign();
        void Saving();

    }
}
