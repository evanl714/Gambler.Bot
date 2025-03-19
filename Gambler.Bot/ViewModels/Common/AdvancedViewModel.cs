using Gambler.Bot.Strategies.Helpers;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Gambler.Bot.Strategies.Helpers.InternalBetSettings;

namespace Gambler.Bot.ViewModels.Common
{
    public class AdvancedViewModel:ViewModelBase
    {
		private InternalBetSettings _settings;

		public InternalBetSettings BetSettings
		{
			get { return _settings; }
			set { _settings = value; this.RaisePropertyChanged(); }
		}
        public List<LimitAction> Actions { get; set; } = new List<LimitAction> { LimitAction.Bank, LimitAction.Invest, LimitAction.Stop, LimitAction.Tip, LimitAction.Withdraw };
        public List<string> Compares { get; set; } = new List<string> { "Balance", "Profit" };

		

		public LimitAction? UpperLimitAction
		{
			get { return BetSettings?.UpperLimitAction; }
			set 
            { 
                BetSettings.UpperLimitAction = value ?? LimitAction.Stop;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(ShowUpperAmount));
                this.RaisePropertyChanged(nameof(ShowUpperTo));
            }
		}
        public LimitAction? LowerLimitAction
        {
            get { return BetSettings?.LowerLimitAction; }
            set 
            {
                
                BetSettings.LowerLimitAction = value ?? LimitAction.Stop; 
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(ShowLowerAmount));
                this.RaisePropertyChanged(nameof(ShowLowerTo));
            }
        }
        public bool ShowUpperAmount
		{
			get => UpperLimitAction != LimitAction.Stop &&	UpperLimitAction != LimitAction.Reset;
		}
        public bool ShowUpperTo
        {
            get => UpperLimitAction == LimitAction.Withdraw || UpperLimitAction == LimitAction.Tip;
        }
        public bool ShowLowerAmount
        {
            get => LowerLimitAction != LimitAction.Stop && LowerLimitAction != LimitAction.Reset;
        }
        public bool ShowLowerTo
        {
            get => LowerLimitAction == LimitAction.Withdraw || LowerLimitAction == LimitAction.Tip;
        }
        public AdvancedViewModel(Microsoft.Extensions.Logging.ILogger logger) : base(logger)
        {
            
        }
    }
}
