using Gambler.Bot.Strategies.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gambler.Bot.ViewModels.Common
{
    public class ResetSettingsViewModel:ViewModelBase
    {
		private InternalBetSettings _betSettings;

		public InternalBetSettings BetSettings
        {
			get { return _betSettings; }
			set { _betSettings = value; }
		}

        public ResetSettingsViewModel(Microsoft.Extensions.Logging.ILogger logger) : base(logger)
        {
            
        }
    }
}
