using DoormatBot;
using DoormatBot.Strategies;
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
        public ProfitChartViewModel ChartData { get; set; } = new ProfitChartViewModel();
        public SiteStatsViewModel SiteStatsData { get; set; } = new SiteStatsViewModel();
        public SessionStatsViewModel SessionStatsData { get; set; } = new SessionStatsViewModel();

        iLiveBet _liveBets = new DiceLiveBetViewModel();
        public iLiveBet LiveBets { get => _liveBets; set { _liveBets = value; this.RaisePropertyChanged(); } }
        public AdvancedViewModel AdvancedSettingsVM { get; set; } = new AdvancedViewModel();
        public ResetSettingsViewModel ResetSettingsVM { get; set; } = new ResetSettingsViewModel();

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
            botIns.Strategy = new Martingale();
            
            PlaceBetVM.PlaceBet += PlaceBetVM_PlaceBet;
            botIns.OnGameChanged += BotIns_OnGameChanged;
            botIns.OnNotification += BotIns_OnNotification;
            botIns.OnSiteAction += BotIns_OnSiteAction;
            botIns.OnSiteBetFinished += BotIns_OnSiteBetFinished;
            botIns.OnStarted += BotIns_OnStarted;
            botIns.OnStopped += BotIns_OnStopped;
            botIns.OnStrategyChanged += BotIns_OnStrategyChanged;
            botIns.OnSiteLoginFinished += BotIns_OnSiteLoginFinished;
        }

        private void BotIns_OnSiteLoginFinished(object sender, DoormatCore.Sites.LoginFinishedEventArgs e)
        {
            SiteStatsData.Stats = e.Stats;
            SiteStatsData.RaisePropertyChanged(nameof(SiteStatsData.Stats));
        }

        private void BotIns_OnStrategyChanged(object? sender, EventArgs e)
        {
            AdvancedSettingsVM.BetSettings = botIns.BetSettings;
            ResetSettingsVM.BetSettings = botIns.BetSettings;
        }

        private void BotIns_OnStopped(object? sender, DoormatCore.Sites.GenericEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BotIns_OnStarted(object? sender, EventArgs e)
        {
            SessionStatsData.Stats = botIns.Stats;
            SessionStatsData.RaisePropertyChanged(nameof(SessionStatsData.Stats));
        }

        private void BotIns_OnSiteBetFinished(object sender, DoormatCore.Sites.BetFinisedEventArgs e)
        {
            SiteStatsData.StatsUpdated(botIns.CurrentSite.Stats);
            SessionStatsData.RaisePropertyChanged(nameof(SessionStatsData.Stats));
            ChartData.AddPoint(e.NewBet.Profit);
            LiveBets.AddBet(e.NewBet);
        }

        private void BotIns_OnSiteAction(object sender, DoormatCore.Sites.GenericEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BotIns_OnNotification(object? sender, DoormatCore.Helpers.NotificationEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BotIns_OnGameChanged(object? sender, EventArgs e)
        {
            if (PlaceBetVM != null)
                PlaceBetVM.PlaceBet -= PlaceBetVM_PlaceBet;

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
            if (PlaceBetVM != null)
                PlaceBetVM.PlaceBet += PlaceBetVM_PlaceBet;
        }

        private void PlaceBetVM_PlaceBet(object? sender, PlaceBetEventArgs e)
        {
            botIns.PlaceBet(e.NewBet);
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
