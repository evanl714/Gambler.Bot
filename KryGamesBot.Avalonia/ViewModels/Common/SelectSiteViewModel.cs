using DoormatBot;
using DoormatCore.Helpers;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IronPython.Modules._ast;

namespace KryGamesBot.Avalonia.ViewModels.Common
{
    public class SelectSiteViewModel:ViewModelBase
    {
        private ObservableCollection<SitesList> _sites = new ObservableCollection<SitesList>();
        public ObservableCollection<SitesList> Sites { get=> _sites; set => this.RaiseAndSetIfChanged(ref _sites, value); }
        public string Text { get; set; }

        public List<string> strings { get; set; } = new List<string> { "1", "2", "three", "fuck", "you" };
        public SelectSiteViewModel()
        {
            Text = "fuckl you";
            Doormat botIns = new Doormat();
            botIns.CompileSites();
            botIns.GetStrats();

            Sites = new ObservableCollection<SitesList>(Doormat.Sites);
        }

    }
}
