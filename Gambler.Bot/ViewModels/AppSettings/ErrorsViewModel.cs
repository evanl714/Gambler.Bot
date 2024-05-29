using Gambler.Bot.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static Gambler.Bot.AutoBet.Helpers.PersonalSettings;

namespace Gambler.Bot.ViewModels.AppSettings
{
    public class ErrorsViewModel : ViewModelBase
    {

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
            Errors?.Clear();
        }
    }
}
