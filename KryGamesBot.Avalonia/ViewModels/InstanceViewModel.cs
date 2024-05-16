using Avalonia.Threading;
using DoormatBot.Strategies;
using DryIoc;
using KryGamesBot.Ava.Classes;
using KryGamesBot.Ava.Classes.BetsPanel;
using KryGamesBot.Ava.Classes.Strategies;
using KryGamesBot.Ava.ViewModels.Common;
using KryGamesBot.Ava.ViewModels.Games.Dice;
using KryGamesBot.Ava.ViewModels.Strategies;
using KryGamesBot.Ava.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KryGamesBot.Ava.ViewModels
{
    public class InstanceViewModel : ViewModelBase
    {
        string BetSettingsFile = "";
        string PersonalSettingsFile = "";
        public string InstanceName { get; set; }
        public SelectSiteViewModel SelectSite { get; set; }
        public bool IsSelectSiteViewVisible { get; set; }
        private DoormatBot.Doormat botIns;
        public DoormatBot.Doormat? BotInstance { get => botIns; set { botIns = value; this.RaisePropertyChanged(); } }
        public Interaction<LoginViewModel, LoginViewModel?> ShowDialog { get; }
        public Interaction<SimulationViewModel, SimulationViewModel?> ShowSimulation { get; }
        private bool showSites = true;
        public ProfitChartViewModel ChartData { get; set; }// = new ProfitChartViewModel();
        public SiteStatsViewModel SiteStatsData { get; set; }// = new SiteStatsViewModel();
        public SessionStatsViewModel SessionStatsData { get; set; }// = new SessionStatsViewModel();
        public TriggersViewModel TriggersVM { get; set; }
        private bool showChart = true;

        public bool ShowChart
        {
            get { return showChart; }
            set { showChart = value; this.RaisePropertyChanged(); }
        }

        private bool showLiveBets = true;

        public bool ShowLiveBets
        {
            get { return showLiveBets; }
            set { showLiveBets = value; this.RaisePropertyChanged(); }
        }
        private bool showStats = true;

        public bool ShowStats
        {
            get { return showStats; }
            set { showStats = value; this.RaisePropertyChanged(); }
        }

        public string[] Currencies
        {
            get { return BotInstance?.CurrentSite?.Currencies; }
        }
        public int? CurrentCurrency
        {
            get { return BotInstance?.CurrentSite?.Currency; }
            set { if (BotInstance?.CurrentSite != null) BotInstance.CurrentSite.Currency = (value >= 0 ? value : 0) ?? 0; this.RaisePropertyChanged(); }
        }
        public DoormatCore.Games.Games[] Games
        {
            get { return BotInstance?.CurrentSite?.SupportedGames; }
        }
        public int? CurrentGame
        {
            get { return Array.IndexOf(BotInstance?.CurrentSite?.SupportedGames, BotInstance?.CurrentGame); }
            set { if (BotInstance?.CurrentSite != null) BotInstance.CurrentGame = BotInstance?.CurrentSite?.SupportedGames[(value >= 0 ? value : 0) ?? 0] ?? DoormatCore.Games.Games.Dice; }
        }

        public bool LoggedIn
        {
            get { return botIns?.LoggedIn ?? false; }
        }

        public bool NotLoggedIn
        {
            get { return !(botIns?.LoggedIn ?? false); }
        }
        public bool Running
        {
            get { return botIns?.Running ?? false; }
        }

        public bool Stopped
        {
            get { return !(botIns?.Running ?? false); }
        }

        private bool canStart;

        public bool CanStart
        {
            get { return canStart; }
            set { canStart = value; this.RaisePropertyChanged(); }
        }
        private bool canResume;

        public bool CanResume
        {
            get { return canResume; }
            set { canResume = value; this.RaisePropertyChanged(); }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; this.RaisePropertyChanged(); }
        }

        void setTitle()
        {
            Title =$"{botIns?.CurrentSite?.SiteName} - {botIns?.CurrentGame.ToString()} - {botIns?.Strategy?.StrategyName} ({(Running?"Running":"Sopped")}";
        }
        void setCanStart()
        {
            CanStart = ((botIns?.LoggedIn ?? false) && botIns.Strategy != null && !botIns.Running && !botIns.RunningSimulation);            
        }

        void setCanResume()
        {
            CanResume = ((botIns?.LoggedIn ?? false) && botIns.Strategy != null && !botIns.Running && !botIns.RunningSimulation);
        }

        iLiveBet _liveBets;
        public iLiveBet LiveBets { get => _liveBets; set { _liveBets = value; this.RaisePropertyChanged(); } }
        private IStrategy _strategyVM;

        public IStrategy StrategyVM
        {
            get { return _strategyVM; }
            set { _strategyVM = value; this.RaisePropertyChanged(); }
        }

        public AdvancedViewModel AdvancedSettingsVM { get; set; }// = new AdvancedViewModel();
        public ResetSettingsViewModel ResetSettingsVM { get; set; }// = new ResetSettingsViewModel();

        iPlaceBet _placeBetVM=null;// = new DicePlaceBetViewModel();
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

        private string _status;

        public string StatusMessage
        {
            get { return _status; }
            set { _status = value; this.RaisePropertyChanged(); }
        }

        private string lastAction;

        public string LastAction
        {
            get { return lastAction; }
            set { lastAction = value; this.RaisePropertyChanged(); }
        }



        public InstanceViewModel(Microsoft.Extensions.Logging.ILogger logger) : base(logger)
        {
            AdvancedSettingsVM= new AdvancedViewModel(_logger);
            ResetSettingsVM = new ResetSettingsViewModel(_logger);
            ChartData = new ProfitChartViewModel(_logger);
            SiteStatsData = new SiteStatsViewModel(_logger);
            SessionStatsData = new SessionStatsViewModel(_logger);
            TriggersVM = new TriggersViewModel(_logger);

            SessionStatsData.OnResetStats += SessionStatsData_OnResetStats;

            StartCommand = ReactiveCommand.Create(Start);
            StopCommand = ReactiveCommand.Create(Stop);
            ResumeCommand = ReactiveCommand.Create(Resume);
            StopOnWinCommand = ReactiveCommand.Create(StopOnWin);

            LogOutCommand = ReactiveCommand.Create(LogOut);
            ChangeSiteCommand = ReactiveCommand.Create(ChangeSite);
            SimulateCommand = ReactiveCommand.Create(Simulate);

            ExitCommand = ReactiveCommand.Create(Exit);
            OpenCommand = ReactiveCommand.Create(Open);
            SaveCommand = ReactiveCommand.Create(Save);

            var tmp =  new DoormatBot.Doormat(_logger);
            SelectSite = new SelectSiteViewModel(_logger);
            SelectSite.SelectedSiteChanged += SelectSite_SelectedSiteChanged;
            IsSelectSiteViewVisible = true;
            ShowDialog = new Interaction<LoginViewModel, LoginViewModel?>();
            ShowSimulation = new Interaction<SimulationViewModel, SimulationViewModel?>();
            tmp.Strategy = new Martingale(_logger);
            tmp.GetStrats();
            PlaceBetVM = new DicePlaceBetViewModel(_logger);
            PlaceBetVM.PlaceBet += PlaceBetVM_PlaceBet;
            tmp.OnGameChanged += BotIns_OnGameChanged;
            tmp.OnNotification += BotIns_OnNotification;
            tmp.OnSiteAction += BotIns_OnSiteAction;
            tmp.OnSiteBetFinished += BotIns_OnSiteBetFinished;
            tmp.OnStarted += BotIns_OnStarted;
            tmp.OnStopped += BotIns_OnStopped;
            tmp.OnStrategyChanged += BotIns_OnStrategyChanged;
            tmp.OnSiteLoginFinished += BotIns_OnSiteLoginFinished;
            tmp.OnBypassRequired += Tmp_OnBypassRequired;
            tmp.OnSiteNotify += Tmp_OnSiteNotify;
            tmp.OnSiteError += Tmp_OnSiteError;
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

        private void SessionStatsData_OnResetStats(object? sender, EventArgs e)
        {
            botIns.ResetStats();
            SessionStatsData.StatsUpdated(botIns.Stats);
        }

        private void Tmp_OnSiteError(object sender, DoormatCore.Sites.ErrorEventArgs e)
        {
            if (!Dispatcher.UIThread.CheckAccess())
                Dispatcher.UIThread.Invoke(() => { Tmp_OnSiteError(sender, e); });
            else
            {
                StatusMessage = e.Message;
            }
        }

        private void Tmp_OnSiteNotify(object sender, DoormatCore.Sites.GenericEventArgs e)
        {
            if (!Dispatcher.UIThread.CheckAccess())
                Dispatcher.UIThread.Invoke(() => { Tmp_OnSiteNotify(sender, e); });
            else
            {
                StatusMessage = e.Message;
            }
        }

        private void Tmp_OnBypassRequired(object? sender, DoormatCore.Sites.BypassRequiredArgs e)
        {
            e.Config = MainView.GetBypass(e);
        }

        private void BotIns_OnSiteLoginFinished(object sender, DoormatCore.Sites.LoginFinishedEventArgs e)
        {
            SiteStatsData.Stats = e.Stats;
            SiteStatsData.RaisePropertyChanged(nameof(SiteStatsData.Stats));
            this.RaisePropertyChanged(nameof(LoggedIn));
            this.RaisePropertyChanged(nameof(NotLoggedIn));
            setCanResume();
            setCanStart();
            setTitle();
        }

        private void BotIns_OnStrategyChanged(object? sender, EventArgs e)
        {
            AdvancedSettingsVM.BetSettings = botIns.BetSettings;
            ResetSettingsVM.BetSettings = botIns.BetSettings;
            TriggersVM.SetTriggers(botIns.BetSettings?.Triggers);
            IStrategy tmpStrat = null;
            //this needs to set the istrategy property to the appropriate viewmodel
            switch(BotInstance.Strategy?.StrategyName)
            {
                case "Martingale": tmpStrat = new MartingaleViewModel(_logger); break;
                case "D'Alembert": tmpStrat = new DAlembertViewModel(_logger); break;
                case "Fibonacci": tmpStrat = new FibonacciViewModel(_logger); break;
                case "Labouchere": tmpStrat = new LabouchereViewModel(_logger); break;
                case "PresetList": tmpStrat = new PresetListViewModel(_logger); break;
                case "ProgrammerLUA": tmpStrat = new ProgrammerModeLUAViewModel(_logger); break;
                case "ProgrammerCS": tmpStrat = new ProgrammerModeCSViewModel(_logger); break;
                case "ProgrammerJS": tmpStrat = new ProgrammerModeCSViewModel(_logger); break;
                case "ProgrammerPython": tmpStrat = new ProgrammerModePYViewModel(_logger); break;
                default: tmpStrat = null; break; ;
            }
            if (tmpStrat != null)
            {
                tmpStrat.SetStrategy(BotInstance.Strategy);
                tmpStrat.GameChanged(BotInstance.CurrentGame);
            }
            StrategyVM?.Dispose();
            StrategyVM = tmpStrat;
            setTitle();
        }

        private void BotIns_OnStopped(object? sender, DoormatCore.Sites.GenericEventArgs e)
        {
            //if (!Dispatcher.CheckAccess())
            //    Dispatcher.Invoke(new Action<object, DoormatCore.Sites.GenericEventArgs>(BotIns_OnStopped), sender, e);
            //else
            //{
            //    bbtnSimulator.IsEnabled = true;
            //    StatusBar.Content = $"Stopping: {e.Message}";
            //    btcStart.IsEnabled = true;
            //    btnResume.IsEnabled = true;

            //}
            StatusMessage = "Stopping: "+ e.Message;
            this.RaisePropertyChanged(nameof(Running));
            this.RaisePropertyChanged(nameof(Stopped));
            setCanResume();
            setCanStart();
            setTitle();
        }
            private void BotIns_OnStarted(object? sender, EventArgs e)
        {
            SessionStatsData.Stats = botIns.Stats;
            SessionStatsData.RaisePropertyChanged(nameof(SessionStatsData.Stats));
            this.RaisePropertyChanged(nameof(Running));
            this.RaisePropertyChanged(nameof(Stopped));
            setCanResume();
            setCanStart();
            setTitle();
        }

        private void BotIns_OnSiteBetFinished(object sender, DoormatCore.Sites.BetFinisedEventArgs e)
        {
            SiteStatsData.StatsUpdated(botIns.CurrentSite.Stats);
            SessionStatsData.StatsUpdated(botIns.Stats);
            ChartData.AddPoint(e.NewBet.Profit,e.NewBet.IsWin);
            LiveBets.AddBet(e.NewBet);
        }

        private void BotIns_OnSiteAction(object sender, DoormatCore.Sites.GenericEventArgs e)
        {
            LastAction = e.Message;
        }

        private void BotIns_OnNotification(object? sender, DoormatCore.Helpers.NotificationEventArgs e)
        {
            throw new NotImplementedException();
            switch (e.NotificationTrigger.Action)
            {
                case DoormatCore.Helpers.TriggerAction.Alarm: break;
                case DoormatCore.Helpers.TriggerAction.Chime: break;
                case DoormatCore.Helpers.TriggerAction.Email: break;
                case DoormatCore.Helpers.TriggerAction.Popup: break;                
            }
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
                    PlaceBetVM = new DicePlaceBetViewModel(_logger);
                    LiveBets = new DiceLiveBetViewModel(_logger);
                        break;

            }
            if (PlaceBetVM != null)
                PlaceBetVM.PlaceBet += PlaceBetVM_PlaceBet;

            setTitle();
        }

        private void PlaceBetVM_PlaceBet(object? sender, PlaceBetEventArgs e)
        {
            botIns.PlaceBet(e.NewBet);
        }

        private void SelectSite_SelectedSiteChanged(object? sender, DoormatCore.Helpers.SitesList e)
        {
            SiteChanged(Activator.CreateInstance(e.SiteType(),_logger) as DoormatCore.Sites.BaseSite, e.SelectedCurrency?.Name, e.SelectedGame?.Name);
            if (SiteStatsData!=null)
                SiteStatsData.SiteName = botIns.CurrentSite?.SiteName;   
        }

        async Task ShowLogin()
        {
            var store = new LoginViewModel(botIns.CurrentSite, _logger);
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
                LoadInstanceSettings(path + Name + ".siteset");

            }
            if (File.Exists(BetSettingsFile))
                botIns.LoadBetSettings(BetSettingsFile);
            else
            {
                botIns.StoredBetSettings = new DoormatBot.Doormat.ExportBetSettings
                {
                    BetSettings = new DoormatBot.Helpers.InternalBetSettings(),


                };
                botIns.Strategy = new DoormatBot.Strategies.Martingale(_logger);
            }
            this.RaisePropertyChanged(nameof(SelectedStrategy));
            //load instance settings: site, currency, game, account, password if keepass is active and logged in.
            //if password is available, log in.
            //do all of this async to the gui somewhow?
        }

        void LoadInstanceSettings(string FileLocation)
        {
            string Settings = "";
            using (StreamReader sr = new StreamReader(FileLocation))
            {
                Settings = sr.ReadToEnd();
            }
            InstanceSettings tmp = JsonSerializer.Deserialize<InstanceSettings>(Settings);
            //botIns.ga

            var tmpsite = DoormatBot.Doormat.Sites.FirstOrDefault(m => m.Name == tmp.Site);
            if (tmpsite != null)
            {
                botIns.CurrentSite = Activator.CreateInstance(tmpsite.SiteType(),_logger) as DoormatCore.Sites.BaseSite;
                SiteChanged(botIns.CurrentSite, tmp.Currency, tmp.Game);
            }
            if (tmp.Game != null)
                botIns.CurrentGame = Enum.Parse<DoormatCore.Games.Games>(tmp.Game);

        }

        void SaveINstanceSettings(string FileLocation)
        {
            string Settings = JsonSerializer.Serialize<InstanceSettings>(new InstanceSettings
            {
                Site = botIns.CurrentSite?.GetType()?.Name,
                AutoLogin = false,
                Game = botIns.CurrentGame.ToString(),
                Currency = botIns.CurrentSite?.CurrentCurrency
            });
            File.WriteAllText(FileLocation, Settings);
        }

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
        public ICommand LogOutCommand { get; set; }
        void LogOut()
        {
            botIns.StopStrategy("Logging Out");
            botIns.CurrentSite.Disconnect();
            ShowLogin();
        }

        public ICommand ChangeSiteCommand { get; set; }
        void ChangeSite()
        {
            botIns.StopStrategy("Logging Out");
            botIns.CurrentSite.Disconnect();
            ShowSites = true;
        }

        public ICommand SimulateCommand { get; }

        async Task Simulate()
        {
            SimulationViewModel simControl = new SimulationViewModel(_logger);
            simControl.CurrentSite = botIns.CurrentSite;
            simControl.Strategy = botIns.Strategy;
            simControl.BetSettings = botIns.BetSettings;
            await ShowSimulation.Handle(simControl);
        }

        public ICommand ExitCommand { get; }

        void Exit()
        {
            throw new NotImplementedException();
        }

        public ICommand OpenCommand { get; }

        void Open()
        {
            throw new NotImplementedException();
        }

        public ICommand SaveCommand { get; }

        void Save()
        {
            throw new NotImplementedException();
        }

        public void OnClosing()
        {
            botIns.StopStrategy("Application Closing");
            if (botIns.CurrentSite != null)
                botIns.CurrentSite.Disconnect();
            string path = "";            
            path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\";            
            botIns.SaveBetSettings(path + InstanceName + ".betset");
            SaveINstanceSettings(path + InstanceName + ".siteset");
        }
    }
}
