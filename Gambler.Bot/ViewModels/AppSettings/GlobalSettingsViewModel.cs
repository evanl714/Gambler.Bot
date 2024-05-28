using Gambler.Bot.AutoBet.Helpers;
using Gambler.Bot.Classes;
using Gambler.Bot.ViewModels.Common;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gambler.Bot.ViewModels.AppSettings
{
    public class GlobalSettingsViewModel : ViewModelBase
    {
        public GeneralSettingsViewModel General { get; set; }
        public ErrorsViewModel Errors { get; set; }
        public BetStorageViewModel Storage { get; set; }
        public TriggersViewModel Triggers { get; set; }

        private PersonalSettings settings;

        public PersonalSettings Settings
        {
            get { return settings; }
            set { settings = value; this.RaisePropertyChanged(); SetSettings(); }
        }

        private void SetSettings()
        {
            General.Settings = Settings;
            Errors.Settings = Settings;
            Storage.Settings = Settings;
            Triggers.SetTriggers(Settings?.Notifications);
        }

        public GlobalSettingsViewModel(ILogger logger) : base(logger)
        {
            General = new GeneralSettingsViewModel(logger);
            Errors = new ErrorsViewModel(logger);
            Storage = new BetStorageViewModel(logger);
            Triggers = new TriggersViewModel(logger) { Notifications=true };

        }
    }
}
