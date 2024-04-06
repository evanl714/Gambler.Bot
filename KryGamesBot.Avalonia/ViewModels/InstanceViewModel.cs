using DoormatBot;
using DoormatBot.Strategies;
using DoormatBot.Strategies.PresetListModels;
using KryGamesBot.Ava.Classes;
using KryGamesBot.Ava.Classes.BetsPanel;
using KryGamesBot.Ava.Classes.Strategies;
using KryGamesBot.Ava.ViewModels.Common;
using KryGamesBot.Ava.ViewModels.Games.Dice;
using KryGamesBot.Ava.ViewModels.Strategies;
using KryGamesBot.Ava.Views;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KryGamesBot.Ava.ViewModels
{
    public class InstanceViewModel:ViewModelBase
    {
        string BetSettingsFile = "";
        string PersonalSettingsFile = "";
        public string InstanceName { get; set; }
        public SelectSiteViewModel SelectSite { get; set; }
        public bool IsSelectSiteViewVisible { get; set; }
        private Doormat botIns;
        public Doormat? BotInstance { get => botIns; set { botIns = value; this.RaisePropertyChanged(); } }
        public Interaction<LoginViewModel, LoginViewModel?> ShowDialog { get; }
        private bool showSites=true;
        public ProfitChartViewModel ChartData { get; set; } = new ProfitChartViewModel();
        public SiteStatsViewModel SiteStatsData { get; set; } = new SiteStatsViewModel();
        public SessionStatsViewModel SessionStatsData { get; set; } = new SessionStatsViewModel();
        

        public string[] Currencies
        {
            get { return BotInstance?.CurrentSite?.Currencies; }
        }
        public int? CurrentCurrency
        {
            get { return BotInstance?.CurrentSite?.Currency; }
            set { if (BotInstance?.CurrentSite!=null) BotInstance.CurrentSite.Currency = value??0; }
        }

        iLiveBet _liveBets = new DiceLiveBetViewModel();
        public iLiveBet LiveBets { get => _liveBets; set { _liveBets = value; this.RaisePropertyChanged(); } }
        private IStrategy _strategyVM;

        public IStrategy StrategyVM
        {
            get { return _strategyVM; }
            set { _strategyVM = value; this.RaisePropertyChanged(); }
        }

        public AdvancedViewModel AdvancedSettingsVM { get; set; } = new AdvancedViewModel();
        public ResetSettingsViewModel ResetSettingsVM { get; set; } = new ResetSettingsViewModel();

        iPlaceBet _placeBetVM = new DicePlaceBetViewModel();
        public iPlaceBet PlaceBetVM { get=> _placeBetVM; set { _placeBetVM = value; this.RaisePropertyChanged(); } } 

        public bool ShowSites
        {
            get { return showSites; }
            set { showSites = value; this.RaisePropertyChanged(); this.RaisePropertyChanged(nameof(ShowBot)); }
        }




        public string SelectedStrategy
        {
            get { return BotInstance?.Strategy?.StrategyName; }
            set { SetStrategy( value); }
        }

        void SetStrategy(string name)
        {
            if (botIns.Strategy.StrategyName != name && !string.IsNullOrWhiteSpace(BetSettingsFile))
            {
                StrategyVM?.Saving();

                botIns.SaveBetSettings(BetSettingsFile);
                var Settings = botIns.LoadBetSettings(BetSettingsFile, false);
                IEnumerable<PropertyInfo> Props = Settings.GetType().GetProperties().Where(m => typeof(DoormatBot.Strategies.BaseStrategy).IsAssignableFrom(m.PropertyType));
                DoormatBot.Strategies.BaseStrategy newStrat = null;
                foreach (PropertyInfo x in Props)
                {
                    DoormatBot.Strategies.BaseStrategy strat = (DoormatBot.Strategies.BaseStrategy)x.GetValue(Settings);
                    if (strat != null)
                    {
                        PropertyInfo StratNameProp = strat.GetType().GetProperty("StrategyName");
                        string nm = (string)StratNameProp.GetValue(strat);
                        if (nm == BetSettingsFile.ToString())
                        {
                            newStrat = strat;
                            break;
                        }
                    }
                }
                if (newStrat == null)
                {
                    newStrat = Activator.CreateInstance(botIns.Strategies[name]) as DoormatBot.Strategies.BaseStrategy;
                }
                botIns.Strategy = newStrat;
            }
        }


        public bool ShowBot
        {
            get { return !ShowSites; }
            
        }

        public InstanceViewModel()
        {
            StartCommand = ReactiveCommand.Create(Start);
            StopCommand = ReactiveCommand.Create(Stop);
            ResumeCommand = ReactiveCommand.Create(Resume);
            StopOnWinCommand = ReactiveCommand.Create(StopOnWin);

            var tmp =  new Doormat();
            SelectSite = new SelectSiteViewModel();
            SelectSite.SelectedSiteChanged += SelectSite_SelectedSiteChanged;
            IsSelectSiteViewVisible = true;
            ShowDialog = new Interaction<LoginViewModel, LoginViewModel?>();
            tmp.Strategy = new Martingale();
            tmp.GetStrats();
            PlaceBetVM.PlaceBet += PlaceBetVM_PlaceBet;
            tmp.OnGameChanged += BotIns_OnGameChanged;
            tmp.OnNotification += BotIns_OnNotification;
            tmp.OnSiteAction += BotIns_OnSiteAction;
            tmp.OnSiteBetFinished += BotIns_OnSiteBetFinished;
            tmp.OnStarted += BotIns_OnStarted;
            tmp.OnStopped += BotIns_OnStopped;
            tmp.OnStrategyChanged += BotIns_OnStrategyChanged;
            tmp.OnSiteLoginFinished += BotIns_OnSiteLoginFinished;
            BotInstance = tmp;
            botIns.CurrentGame = DoormatCore.Games.Games.Dice;
            /*if (MainWindow.Portable && File.Exists("personalsettings.json"))
            {
                PersonalSettingsFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\PersonalSettings.json";

            }
            //Check if global settings for this account exists
            else*/ if (/*!MainWindow.Portable &&*/ File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\PersonalSettings.json"))
            {
                PersonalSettingsFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\PersonalSettings.json";
                botIns.LoadPersonalSettings(PersonalSettingsFile);
            }
           
            LoadSettings("default");
            
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
            IStrategy tmpStrat = null;
            //this needs to set the istrategy property to the appropriate viewmodel
            switch(BotInstance.Strategy?.StrategyName)
            {
                case "Martingale": tmpStrat = new MartingaleViewModel(); break;
                case "D'Alembert": tmpStrat = new DAlembertViewModel(); break;
                case "Fibonacci": tmpStrat = new FibonacciViewModel(); break;
                case "Labouchere": tmpStrat = new LabouchereViewModel(); break;
                case "PresetList": tmpStrat = new PresetListViewModel(); break;
                case "ProgrammerLUA": tmpStrat = new ProgrammerModeLUAViewModel(); break;
                case "ProgrammerCS": tmpStrat = new ProgrammerModeCSViewModel(); break;
                case "ProgrammerJS": tmpStrat = new ProgrammerModeCSViewModel(); break;
                case "ProgrammerPython": tmpStrat = new ProgrammerModePYViewModel(); break;
                default: tmpStrat = null; break; ;
            }
            if (tmpStrat != null)
            {
                tmpStrat.SetStrategy(BotInstance.Strategy);
                tmpStrat.GameChanged(BotInstance.CurrentGame);
            }
            StrategyVM?.Dispose();
            StrategyVM = tmpStrat;
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
            this.RaisePropertyChanged(nameof(Currencies));
            this.RaisePropertyChanged(nameof(CurrentCurrency));
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

        public void LoadSettings(string Name)
        {
            string path = "";
            //if (MainView.Portable)
            //    path = "";
            //else
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\";
            }
            InstanceName = Name;
            //load bet settings
            //if (File.Exists(path+Name + ".betset"))
            {
                BetSettingsFile = path + Name + ".betset";

            }

            //load layout
            //if (File.Exists(path + Name + ".layout"))
            //{
            //    dlmMainLayout.RestoreLayoutFromXml(path + Name + ".layout");
            //}
            if (File.Exists(path + Name + ".siteset"))
            {
                //LoadInstanceSettings(path + Name + ".siteset");

            }
            if (File.Exists(BetSettingsFile))
                botIns.LoadBetSettings(BetSettingsFile);
            else
            {
                botIns.StoredBetSettings = new Doormat.ExportBetSettings
                {
                    BetSettings = new DoormatBot.Helpers.InternalBetSettings(),


                };
                botIns.Strategy = new DoormatBot.Strategies.Martingale();
            }
            this.RaisePropertyChanged(nameof(SelectedStrategy));
            //load instance settings: site, currency, game, account, password if keepass is active and logged in.
            //if password is available, log in.
            //do all of this async to the gui somewhow?
        }

        //void LoadInstanceSettings(string FileLocation)
        //{
        //    string Settings = "";
        //    using (StreamReader sr = new StreamReader(FileLocation))
        //    {
        //        Settings = sr.ReadToEnd();
        //    }
        //    InstanceSettings tmp = JsonSerializer.Deserialize<InstanceSettings>(Settings);
        //    //botIns.ga

        //    var tmpsite = Doormat.Sites.FirstOrDefault(m => m.Name == tmp.Site);
        //    if (tmpsite != null)
        //    {
        //        botIns.CurrentSite = Activator.CreateInstance(tmpsite.SiteType()) as DoormatCore.Sites.BaseSite;
        //        SiteChanged(botIns.CurrentSite, tmp.Currency, tmp.Game);
        //    }
        //    if (tmp.Game != null)
        //        botIns.CurrentGame = Enum.Parse<DoormatCore.Games.Games>(tmp.Game);

        //}

        //void SaveINstanceSettings(string FileLocation)
        //{
        //    string Settings = JsonSerializer.Serialize<InstanceSettings>(new InstanceSettings
        //    {
        //        Site = botIns.CurrentSite?.GetType()?.Name,
        //        AutoLogin = false,
        //        Game = botIns.CurrentGame.ToString(),
        //        Currency = botIns.CurrentSite?.CurrentCurrency
        //    });
        //    File.WriteAllText(FileLocation, Settings);
        //}

        public ICommand StartCommand { get; set; }
        void Start()
        {
            if (!botIns.Running)
            {
                StrategyVM?.Saving();
                botIns.SaveBetSettings(BetSettingsFile);
                botIns.Start();
            }           
        }

        public ICommand StopCommand { get; set; }
        void Stop()
        {
            botIns.StopStrategy("Stop button clicked");
        }

        public ICommand ResumeCommand { get; set; }
        void Resume()
        {
            botIns.Resume();
        }

        public ICommand StopOnWinCommand { get; set; }
        void StopOnWin()
        {
            botIns.StopOnWin = true;
        }
    }
}
