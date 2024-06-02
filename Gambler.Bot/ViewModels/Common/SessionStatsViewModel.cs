using Gambler.Bot.Strategies.Helpers;
using Gambler.Bot.Common.Helpers;
using Gambler.Bot.Core.Helpers;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gambler.Bot.ViewModels.Common
{
    public class SessionStatsViewModel:ViewModelBase
    {
        public event EventHandler OnResetStats;
        private SessionStats _sessionStats;

		public SessionStats Stats
        {
			get { return _sessionStats; }
			set { _sessionStats = value; this.RaisePropertyChanged(); }
		}

        public string name { get; set; }

        public SessionStatsViewModel(Microsoft.Extensions.Logging.ILogger logger) : base(logger)
        {
            name=Guid.NewGuid().ToString();
            ResetStatsCommand = ReactiveCommand.Create(ResetStats);
        }

        public void StatsUpdated(SessionStats stats)
		{
			this.Stats = CopyHelper.CreateCopy(stats);
		}
        public ICommand ResetStatsCommand { get; }
        void ResetStats()
        {
            OnResetStats?.Invoke(this, new EventArgs());

        }
    }
}
