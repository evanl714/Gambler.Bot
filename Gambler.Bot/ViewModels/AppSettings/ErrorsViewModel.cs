using Doormat.Bot.Helpers;
using Gambler.Bot.AutoBet.Helpers;
using Gambler.Bot.Classes;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Gambler.Bot.AutoBet.Helpers.PersonalSettings;

namespace Gambler.Bot.ViewModels.AppSettings
{
    public class ErrorsViewModel : ViewModelBase
    {
        private PersonalSettings settings;

        public PersonalSettings Settings
        {
            get { return settings; }
            set { settings = value; this.RaisePropertyChanged(); }
        }

        public ObservableCollection<ErrorSetting> Errors { get; set; } = new ObservableCollection<ErrorSetting>() ;
        
        public List<ErrorActions> Actions { get; set; } = new List<ErrorActions>();



        public ErrorsViewModel(ILogger logger) : base(logger)
        {
            foreach (ErrorActions x in Enum.GetValues(typeof(ErrorActions)))
            {
                Actions.Add(x);
            }
        }
        public void AddItem(ErrorSetting error)
        {
            Errors.Add(error);            
        }

        internal void Clear()
        {
            Errors.Clear();
        }
    }
}
