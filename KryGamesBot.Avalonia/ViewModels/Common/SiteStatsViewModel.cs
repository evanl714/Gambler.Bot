using DoormatCore.Helpers;
using DoormatCore.Sites;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryGamesBot.Ava.ViewModels.Common
{
    public class SiteStatsViewModel:ViewModelBase
    {
		private SiteStats _stats;

		public SiteStats Stats
		{
			get { return _stats; }
			set { _stats = value; this.RaisePropertyChanged(); }
		}

		private string siteName;

		public string SiteName
		{
			get { return siteName; }
			set { siteName = value; this.RaisePropertyChanged(); this.RaisePropertyChanged(nameof(TabName)); }
		}

		private string tabName;

		public string TabName
		{
			get { return $"Site: {SiteName}"; }
		}



		public SiteStatsViewModel(Microsoft.Extensions.Logging.ILogger logger) : base(logger)
        {
            
        }
        public void StatsUpdated(SiteStats stats)
		{
			Stats = CopyHelper.CreateCopy(stats);
        }
		
	}
}
