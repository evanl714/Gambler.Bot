using DoormatBot;
using KryGamesBot.Ava.Classes.BetsPanel;
using KryGamesBot.Ava.ViewModels.Common;
using KryGamesBot.Ava.ViewModels.Games.Dice;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace KryGamesBot.Ava.ViewModels
{
    public class InstanceViewModel:ViewModelBase
    {
        public SelectSiteViewModel SelectSite { get; set; }
        public bool IsSelectSiteViewVisible { get; set; }
        Doormat botIns = new Doormat();
        public Interaction<LoginViewModel, LoginViewModel?> ShowDialog { get; }
        private bool showSites=true;
        public ProfitChartViewModel ChartData { get; set; }
        public SiteStatsViewModel SiteStatsData { get; set; }
        public SessionStatsViewModel SessionStatsData { get; set; }

        iLiveBet _liveBets = new DiceLiveBetViewModel();
        public iLiveBet LiveBets { get => _liveBets; set { _liveBets = value; this.RaisePropertyChanged(); } }

        iPlaceBet _placeBetVM = new DicePlaceBetViewModel();
        public iPlaceBet PlaceBetVM { get=> _placeBetVM; set { _placeBetVM = value; this.RaisePropertyChanged(); } } 

        public bool ShowSites
        {
            get { return showSites; }
            set { showSites = value; this.RaisePropertyChanged(); this.RaisePropertyChanged(nameof(ShowBot)); }
        }

       
        public bool ShowBot
        {
            get { return !ShowSites; }
            
        }
        

        public InstanceViewModel()
        {
            SelectSite = new SelectSiteViewModel();
            SelectSite.SelectedSiteChanged += SelectSite_SelectedSiteChanged;
            IsSelectSiteViewVisible = true;
            ShowDialog = new Interaction<LoginViewModel, LoginViewModel?>();
            botIns.OnGameChanged += BotIns_OnGameChanged;
        }

        private void BotIns_OnGameChanged(object? sender, EventArgs e)
        {
            switch (botIns.CurrentGame)
            {
                case DoormatCore.Games.Games.Crash:
                case DoormatCore.Games.Games.Roulette:
                case DoormatCore.Games.Games.Plinko:
                    break;
                case
                    DoormatCore.Games.Games.Dice:
                    PlaceBetVM = new DicePlaceBetViewModel();
                    LiveBets = new DiceLiveBetViewModel();
                        break;

            }
        }

        private void SelectSite_SelectedSiteChanged(object? sender, DoormatCore.Helpers.SitesList e)
        {
            SiteChanged(Activator.CreateInstance(e.SiteType()) as DoormatCore.Sites.BaseSite, e.SelectedCurrency?.Name, e.SelectedGame?.Name);
        }

        async Task ShowLogin()
        {
            var store = new LoginViewModel(botIns.CurrentSite);
            store.LoginFinished = LoginFinished;
            var result = await ShowDialog.Handle(store);
        }

        private void LoginFinished(bool ChangeScreens)
        {
            if (ChangeScreens)
            {
                ShowSites = false;
            }
        }

        void SiteChanged(DoormatCore.Sites.BaseSite NewSite, string currency, string game)
        {
            botIns.CurrentSite = NewSite;
            if (currency != null && Array.IndexOf(botIns.CurrentSite.Currencies, currency) >= 0)
                botIns.CurrentSite.Currency = Array.IndexOf(botIns.CurrentSite.Currencies, currency);
            object curGame = DoormatCore.Games.Games.Dice;
            if (game != null && Enum.TryParse(typeof(DoormatCore.Games.Games), game, out curGame) && Array.IndexOf(botIns.CurrentSite.SupportedGames, (DoormatCore.Games.Games)curGame) >= 0)
                botIns.CurrentGame = (DoormatCore.Games.Games)curGame;
            ShowLogin();//.Wait();
            /*LoginControl.CurrentSite = botIns.CurrentSite;
            lciSelectSite1.Visibility = Visibility.Collapsed;
            lciLoginControl.Visibility = Visibility.Visible;
            itmCurrency.Items.Clear();
            foreach (string x in botIns.CurrentSite.Currencies)
            {
                var itm = new BarCheckItem();
                itm.Content = x;
                itm.CheckedChanged += Itm_CheckedChanged;
                itmCurrency.Items.Add(itm);
            }
            itmGame.Items.Clear();
            foreach (var x in botIns.CurrentSite.SupportedGames)
            {
                var itm = new BarCheckItem();
                itm.Content = x.ToString();
                itm.CheckedChanged += Itm_CheckedChanged;
                itmGame.Items.Add(itm);
            }
            lueCurrencies.ItemsSource = botIns.CurrentSite.Currencies;
            lueCurrencies.EditValue = botIns.CurrentSite.CurrentCurrency;
            lueGames.ItemsSource = botIns.CurrentSite.SupportedGames;
            lueGames.EditValue = botIns.CurrentGame;
            Rename?.Invoke(this, new RenameEventArgs { newName = "Log in - " + NewSite?.SiteName });*/
        }
    }
}
