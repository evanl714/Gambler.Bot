using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryGamesBot.Ava.Classes
{
    internal class UISettings
    {
        public static UISettings Settings = new UISettings();
        public string ThemeName { get; set; } = "Office2019Black";
        public int ChartBets { get; set; } = 1000;
        public int LiveBets { get; set; } = 100;

        
    }
}
