using DryIoc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Gambler.Bot.Classes
{
    public class UISettings: INotifyPropertyChanged
    {
        public static UISettings Settings = new UISettings();
        public static bool Portable = false;
        bool? darkMode = true;
        public bool? DarkMode { get=>darkMode; set { darkMode = value; RaisePropertyChanged(); } }
        string themeName;
        public string ThemeName { get => themeName; set { themeName = value; RaisePropertyChanged(); } }
        int chartBets = 1000;
        public int ChartBets { get => chartBets; set { chartBets = value; RaisePropertyChanged(); } }

        int liveBets = 100;
        public int LiveBets { get => liveBets; set { liveBets = value; RaisePropertyChanged(); } }
        string donateMode;
        string updateMode;
        public string UpdateMode 
        { 
            get =>updateMode; 
            set 
            { updateMode = value; RaisePropertyChanged(); } 
        }
        public string DonateMode
        {
            get => donateMode;
            set { donateMode = value; RaisePropertyChanged(); }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged( [CallerMemberName] string propertyName = null)
        {

            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));

        }
    }
}
