using DoormatBot.Helpers;
using DoormatCore.Helpers;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryGamesBot.Ava.ViewModels.Common
{
    public class SessionStatsViewModel:ViewModelBase
    {
		private SessionStats _sessionStats;

		public SessionStats Stats
        {
			get { return _sessionStats; }
			set { _sessionStats = value; this.RaisePropertyChanged(); }
		}

        public string name { get; set; }

        public SessionStatsViewModel()
        {
            name=Guid.NewGuid().ToString();
        }

        public void StatsUpdated(SessionStats stats)
		{
			this.Stats = CopyHelper.CreateCopy(stats);
		}
	}
}
