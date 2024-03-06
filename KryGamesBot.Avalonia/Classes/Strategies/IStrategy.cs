using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryGamesBot.Ava.Classes.Strategies
{
    public interface IStrategy
    {        
        void GameChanged(DoormatCore.Games.Games newGame);
        void SetStrategy(DoormatBot.Strategies.BaseStrategy Strategy);
        bool TopAlign();
        void Saving();

    }
}
