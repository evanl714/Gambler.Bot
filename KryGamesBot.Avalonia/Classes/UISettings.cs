using DryIoc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KryGamesBot.Ava.Classes
{
    public class UISettings: INotifyPropertyChanged
    {
        public static UISettings Settings = new UISettings();
        public static bool Portable = false;
        string themeName;
        public string ThemeName { get => themeName; set { themeName = value; RaisePropertyChanged(); } }
        int chartBets = 1000;
        public int ChartBets { get => chartBets; set { chartBets = value; RaisePropertyChanged(); } }

        int liveBets = 100;
        public int LiveBets { get => liveBets; set { liveBets = value; RaisePropertyChanged(); } }
        public string UpdateMode { get; set; }
        public string DonateMode { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged( [CallerMemberName] string propertyName = null)
        {

            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));

        }
    }
}
