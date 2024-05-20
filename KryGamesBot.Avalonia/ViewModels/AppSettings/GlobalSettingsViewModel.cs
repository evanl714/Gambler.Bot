using DoormatBot.Helpers;
using KryGamesBot.Ava.Classes;
using KryGamesBot.Ava.ViewModels.Common;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryGamesBot.Ava.ViewModels.AppSettings
{
    public class GlobalSettingsViewModel : ViewModelBase
    {
        public GeneralSettingsViewModel General { get; set; }
        public ErrorsViewModel Errors { get; set; }
        public BetStorageViewModel Storage { get; set; }
        public TriggersViewModel Triggers { get; set; }

        public GlobalSettingsViewModel(ILogger logger) : base(logger)
        {
            General = new GeneralSettingsViewModel(logger);
            Errors = new ErrorsViewModel(logger);
            Storage = new BetStorageViewModel(logger);
            Triggers = new TriggersViewModel(logger);

        }
    }
}
