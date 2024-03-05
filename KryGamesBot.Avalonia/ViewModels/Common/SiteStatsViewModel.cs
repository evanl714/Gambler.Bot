using DoormatCore.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryGamesBot.Ava.ViewModels.Common
{
    public class SiteStatsViewModel
    {
		private SiteStats _stats;

		public SiteStats Stats
		{
			get { return _stats; }
			set { _stats = value; }
		}

	}
}
