using DevExpress.Mvvm.UI.ModuleInjection;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using Gambler.Bot.AutoBet;
using Gambler.Bot.Core.Helpers;
using KryGamesBot.Helpers;
using KryGamesBotControls.Common;
using KryGamesBotControls.Games;
using KryGamesBotControls.Games.Dice;
using KryGamesBotControls.Strategies;
using KryGamesBotControls.Strategies.Martingale;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
namespace KryGamesBot
{
    /// <summary>
    /// Interaction logic for InstanceControl.xaml
    /// </summary>
    
    public partial class InstanceControl : UserControl, INotifyPropertyChanged
    {
        string BetSettingsFile = "";
        string PersonalSettingsFile = "";
        public Gambler.Bot.AutoBet.Doormat botIns = new Gambler.Bot.AutoBet.Doormat(null);

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<RenameEventArgs> Rename;

        public iLiveBet LiveBets { get; set; }
        public iPlaceBet ManualBet { get; set; }
        public iStrategy StrategyControl { get; set; }

        public bool BotInsNotRunning { get => botIns?.Running ?? false; }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            //botIns.CurrentSite.Currencies
            
        }
            
    


        public InstanceControl()
        {
            InitializeComponent();
            DataContext = this;
            botIns.CompileSites();
            botIns.GetStrats();
            cbeStartegies.ItemsSource = botIns.Strategies.Keys;
            SelectSite1.Sites = Gambler.Bot.AutoBet.Doormat.Sites;
            botIns.NeedConstringPassword += BotIns_NeedConstringPassword;
            botIns.NeedKeepassPassword += BotIns_NeedKeepassPassword;
            botIns.OnGameChanged += BotIns_OnGameChanged;
            botIns.OnNotification += BotIns_OnNotification;
            botIns.OnSiteAction += BotIns_OnSiteAction;
            botIns.OnSiteBetFinished += BotIns_OnSiteBetFinished;
            botIns.OnSiteChat += BotIns_OnSiteChat;
            botIns.OnSiteError += BotIns_OnSiteError;
            botIns.OnSiteLoginFinished += BotIns_OnSiteLoginFinished;
            botIns.OnSiteNotify += BotIns_OnSiteNotify;
            botIns.OnSiteRegisterFinished += BotIns_OnSiteRegisterFinished;
            botIns.OnSiteStatsUpdated += BotIns_OnSiteStatsUpdated;
            botIns.OnStarted += BotIns_OnStarted;
            botIns.OnStopped += BotIns_OnStopped;
            botIns.OnStrategyChanged += BotIns_OnStrategyChanged;
            //botIns.Strategy = new Gambler.Bot.AutoBet.Strategies.ProgrammerLUA();
            //(botIns.Strategy as Gambler.Bot.AutoBet.Strategies.Martingale).MinBet = 0.00000001m;
            //botIns.Strategy.Amount = 0.00000001m;
            botIns.CurrentGame = Gambler.Bot.Core.Games.Games.Dice;
            if (MainWindow.Portable && File.Exists("personalsettings.json"))
            {
                PersonalSettingsFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\PersonalSettings.json";
                
            }
            //Check if global settings for this account exists
            else if (!MainWindow.Portable && File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\PersonalSettings.json"))
            {
                PersonalSettingsFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\PersonalSettings.json";
            }
            botIns.LoadPersonalSettings(PersonalSettingsFile);
            cbeStartegies.EditValue = botIns.Strategy?.StrategyName;
        }

        private void BotIns_OnStopped(object sender, Gambler.Bot.Core.Sites.GenericEventArgs e)
        {
            //throw new NotImplementedException();
            if (!Dispatcher.CheckAccess())
                Dispatcher.Invoke(new Action<object, Gambler.Bot.Core.Sites.GenericEventArgs>(BotIns_OnStopped),sender,e);
            else
            {
                bbtnSimulator.IsEnabled = true;
                StatusBar.Content = $"Stopping: {e.Message}";
                btcStart.IsEnabled = true;
                btnResume.IsEnabled = true;
            }
        }

        private void BotIns_OnStarted(object sender, EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
                Dispatcher.Invoke(new Action<object, Gambler.Bot.Core.Sites.GenericEventArgs>(BotIns_OnStopped), sender, e);
            else
            {
                bbtnSimulator.IsEnabled = false;
                StatusBar.Content = $"Bot Started.";
                btcStart.IsEnabled = false ;
                btnResume.IsEnabled = false;
            }
        }

        private void BotIns_OnStrategyChanged(object sender, EventArgs e)
        {
            string s = botIns.Strategy?.GetType().Name;
            switch (s)
            {
                case "DAlembert": StrategyControl = new dAlembert();break;
                case "Fibonacci": StrategyControl = new Fibonacci();break;
                case "Labouchere": StrategyControl = new KryGamesBotControls.Strategies.Labouchere.Labouchere();break;
                case "Martingale": StrategyControl = new MartingaleControl(); break;
                case "PresetList": StrategyControl = new KryGamesBotControls.Strategies.PresetList.PresetListControl();break;
                case "ProgrammerCS": StrategyControl = new ProgrammerModeCS(); break;
                case "ProgrammerJS": StrategyControl = new ProgrammerModeJS(); break;
                case "ProgrammerLUA": StrategyControl = new ProgrammerModeLUA();  break;
                case "ProgrammerPython": StrategyControl = new ProgrammerModePy(); break;
                default:
                    botIns.Strategy = new Gambler.Bot.AutoBet.Strategies.Martingale(); break;
            }
            StratContent.VerticalAlignment = StrategyControl.TopAlign() ? VerticalAlignment.Top : VerticalAlignment.Stretch;
            StrategyControl.SetStrategy(botIns.Strategy);
            if ((string)cbeStartegies.EditValue != botIns.Strategy.StrategyName) 
                cbeStartegies.EditValue = botIns.Strategy.StrategyName;
            StopAndReset.DataContext = botIns.BetSettings;
            AdvancedSettings.DataContext = botIns.BetSettings;
            OnPropertyChanged(nameof(StrategyControl));
        }

        string InstanceName = "";
        public void Closing()
        {
            botIns.StopStrategy("Application Closing");
            if (botIns.CurrentSite!=null)
                botIns.CurrentSite.Disconnect();
            string path = "";
            if (MainWindow.Portable)
                path = "";
            else
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\";
            }
            botIns.SaveBetSettings(path+InstanceName + ".betset");
            dlmMainLayout.SaveLayoutToXml(path + InstanceName +".layout");
            SaveINstanceSettings(path + InstanceName + ".siteset");
        }

        public void Removed()
        {
            botIns.StopStrategy("Application Closing");
            if (botIns.CurrentSite != null)
                botIns.CurrentSite.Disconnect();
            string path = "";
            if (MainWindow.Portable)
                path = "";
            else
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\";
            }
            try
            {
                File.Delete(path + InstanceName + ".betset");
            }
            catch
            {

            }try
            {
                File.Delete(path + InstanceName + ".layout");
            }
            catch { }
            try
            {
                File.Delete(path + InstanceName + ".siteset");
            }
            catch { }
        }

        private void BotIns_OnSiteStatsUpdated(object sender, Gambler.Bot.Core.Sites.StatsUpdatedEventArgs e)
        {
            SiteStats.Stats = e.NewStats;
        }

        private void BotIns_OnSiteRegisterFinished(object sender, Gambler.Bot.Core.Sites.GenericEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void BotIns_OnSiteNotify(object sender, Gambler.Bot.Core.Sites.GenericEventArgs e)
        {
            if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
            {
                Dispatcher.Invoke(new Gambler.Bot.Core.Sites.BaseSite.dNotify(BotIns_OnSiteNotify), sender, e);
                return;
            }           
                StatusBar.Content = e.Message;
            
        }

        private void BotIns_OnSiteLoginFinished(object sender, Gambler.Bot.Core.Sites.LoginFinishedEventArgs e)
        {
            if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
            {
                Dispatcher.Invoke(new Gambler.Bot.Core.Sites.BaseSite.dLoginFinished(BotIns_OnSiteLoginFinished), sender, e);
                return;
            }
            if (e.Success)
            {
                LoginControl.LoginSucceeded();
                //hide login, show other controls all over the place
                lciLoginControl.Visibility = Visibility.Collapsed;
                dlmMainLayout.Visibility = Visibility.Visible;
                Rename?.Invoke(this, new RenameEventArgs { newName = botIns.CurrentSite.SiteName });
            }
            else
            {
                LoginControl.LoginFailed();
            }
        }

        private void BotIns_OnSiteError(object sender, Gambler.Bot.Core.Sites.ErrorEventArgs e)
        {
            if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
            {
                Dispatcher.Invoke(new Gambler.Bot.Core.Sites.BaseSite.dError(BotIns_OnSiteError), sender, e);
                return;
            }
            StatusBar.Content = "Error! "+e.Message;
            
        }

        private void BotIns_OnSiteChat(object sender, Gambler.Bot.Core.Sites.GenericEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void BotIns_OnSiteBetFinished(object sender, Gambler.Bot.Core.Sites.BetFinisedEventArgs e)
        {
            LiveBets.AddBet(e.NewBet);
            ProfitChart1.AddPoint(e.NewBet.Profit);
            SiteStats.RefreshStats();
            if (sessionStats.Stats != botIns.Stats)
                sessionStats.Stats = botIns.Stats;
            else
                sessionStats.RefreshStats();
        }

        private void BotIns_OnSiteAction(object sender, Gambler.Bot.Core.Sites.GenericEventArgs e)
        {
            if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
            {
                Dispatcher.Invoke(new Gambler.Bot.Core.Sites.BaseSite.dAction(BotIns_OnSiteAction), sender, e);
                return;
            }
            StatusBar.Content = e.Message;

        }

        private void BotIns_OnNotification(object sender, Gambler.Bot.Core.Helpers.NotificationEventArgs e)
        {
            if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
            {
                Dispatcher.Invoke(new Action<object, NotificationEventArgs> (BotIns_OnNotification),sender, e);
                return;
            }
            switch (e.NotificationTrigger.Action)
            {
                case Gambler.Bot.Core.Helpers.TriggerAction.Alarm:                
                case Gambler.Bot.Core.Helpers.TriggerAction.Chime:
                case Gambler.Bot.Core.Helpers.TriggerAction.Email:break;
                case Gambler.Bot.Core.Helpers.TriggerAction.Popup: StatusBar.Content = "Something happened that triggered a notification.";break ;
                    
            }
            //StatusBar.Content = e.NotificationTrigger.Action = Gambler.Bot.Core.Helpers.TriggerAction.;
        }

        private void BotIns_OnGameChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            switch (botIns.CurrentGame)
            {
                case Gambler.Bot.Core.Games.Games.Crash:break;
                case Gambler.Bot.Core.Games.Games.Dice:
                    ManualBet = new PlaceDiceBetControl();
                    LiveBets = new DiceLiveBetsControl(); 
                    break;
                case Gambler.Bot.Core.Games.Games.Plinko: break;
                case Gambler.Bot.Core.Games.Games.Roulette: break;
            }
            OnPropertyChanged(nameof(ManualBet));
            OnPropertyChanged(nameof(LiveBets));
            ManualBet.PlaceBet += ManualBet_PlaceBet;
        }

        private void ManualBet_PlaceBet(object sender, PlaceBetEventArgs e)
        {
            botIns.PlaceBet(e.NewBet);            
        }

        private void BotIns_NeedKeepassPassword(object sender, Gambler.Bot.AutoBet.Helpers.PersonalSettings.GetConstringPWEventArgs e)
        {
            //MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            MainWindow.TmpInstance_NeedKeepassPassword(sender, e);
        }

        private void BotIns_NeedConstringPassword(object sender, Gambler.Bot.AutoBet.Helpers.PersonalSettings.GetConstringPWEventArgs e)
        {
            //MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            MainWindow.TmpInstance_NeedConstringPassword(sender, e);
        }

        private void SelectSite1_OnSiteSelected(object sender, KryGamesBotControls.Common.SiteSelectedEventArgs e)
        {
            
            SiteChanged(Activator.CreateInstance(e.SelectedSite.SiteType(),null as ILogger) as Gambler.Bot.Core.Sites.BaseSite, e.SelectedSite.SelectedCurrency?.Name,e.SelectedSite.SelectedGame?.Name);
        }

        void SiteChanged(Gambler.Bot.Core.Sites.BaseSite NewSite, string currency, string game)
        {
            botIns.CurrentSite = NewSite;
            if (currency != null && Array.IndexOf(botIns.CurrentSite.Currencies, currency)>=0)
                botIns.CurrentSite.Currency = Array.IndexOf(botIns.CurrentSite.Currencies, currency);
            object curGame = Gambler.Bot.Core.Games.Games.Dice;
            if (game != null && Enum.TryParse(typeof(Gambler.Bot.Core.Games.Games), game, out curGame) && Array.IndexOf(botIns.CurrentSite.SupportedGames,(Gambler.Bot.Core.Games.Games)curGame) >= 0)
                botIns.CurrentGame = (Gambler.Bot.Core.Games.Games)curGame;
            LoginControl.CurrentSite = botIns.CurrentSite;
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
            Rename?.Invoke(this, new RenameEventArgs { newName = "Log in - "+NewSite?.SiteName });
        }

        private void Itm_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if ((sender as BarCheckItem).IsChecked??false)
            {
                foreach (var x in itmCurrency.Items)
                {
                    if (x != sender)
                    {
                        (x as BarCheckItem).IsChecked = false;
                    }
                }
            }
        }

        public void LoadSettings(string Name)
        {
            string path = "";
            if (MainWindow.Portable)
                path = "";
            else
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
            if (File.Exists(path + Name +".layout"))
            {
                dlmMainLayout.RestoreLayoutFromXml(path + Name + ".layout");
            }
            if (File.Exists(path + Name + ".siteset"))
            {
                LoadInstanceSettings(path + Name + ".siteset");
                
            }
            if (File.Exists(BetSettingsFile))
                botIns.LoadBetSettings(BetSettingsFile);
            else
            {
                botIns.StoredBetSettings = new Gambler.Bot.AutoBet.Doormat.ExportBetSettings
                {
                    BetSettings = new Gambler.Bot.AutoBet.Helpers.InternalBetSettings(),
                    

                };
                botIns.Strategy =new Gambler.Bot.AutoBet.Strategies.Martingale();
            }

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
            
            var tmpsite = Gambler.Bot.AutoBet.Doormat.Sites.FirstOrDefault(m => m.Name == tmp.Site);
            if (tmpsite != null)
            {
                botIns.CurrentSite = Activator.CreateInstance(tmpsite.SiteType()) as Gambler.Bot.Core.Sites.BaseSite;
                SiteChanged(botIns.CurrentSite, tmp.Currency, tmp.Game);
            }
            if (tmp.Game!=null)
                botIns.CurrentGame =  Enum.Parse<Gambler.Bot.Core.Games.Games>(tmp.Game);
            
        }

        void SaveINstanceSettings(string FileLocation)
        {
            string Settings = JsonSerializer.Serialize<InstanceSettings>(new InstanceSettings 
            { 
                Site=botIns.CurrentSite?.GetType()?.Name, 
                AutoLogin=false, 
                Game=botIns.CurrentGame.ToString(),
                Currency=botIns.CurrentSite?.CurrentCurrency
            });
            File.WriteAllText(FileLocation, Settings); 
        }

        private void LoginControl_OnLogin(object sender, KryGamesBotControls.Common.LoginEventArgs e)
        {
            botIns.Login(e.Values.ToArray());
        }

        private void LoginControl_OnBack(object sender, EventArgs e)
        {
            lciLoginControl.Visibility = Visibility.Collapsed;
            lciSelectSite1.Visibility = Visibility.Visible;
        }

        private void LoginControl_OnSkip(object sender, EventArgs e)
        {
            lciLoginControl.Visibility = Visibility.Collapsed;
            dlmMainLayout.Visibility = Visibility.Visible;
        }

        private void btcStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StrategyControl.Saving();
                botIns.SaveBetSettings(BetSettingsFile);
                botIns.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btcStop_Click(object sender, RoutedEventArgs e)
        {
            botIns.StopStrategy("Stop button clicked");
        }

       
        private void bExit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            string x = botIns.CurrentSite?.SiteName;
        }

        private void btnStopWin_Click(object sender, RoutedEventArgs e)
        {
            botIns.StopOnWin = true;
        }

        private void cbeStartegies_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            
            if (botIns.Strategy.StrategyName != e.NewValue.ToString()&&!string.IsNullOrWhiteSpace(BetSettingsFile))
            {
                botIns.SaveBetSettings(BetSettingsFile);
                var Settings = botIns.LoadBetSettings(BetSettingsFile, false);
                IEnumerable<PropertyInfo> Props = Settings.GetType().GetProperties().Where(m => typeof(Gambler.Bot.AutoBet.Strategies.BaseStrategy).IsAssignableFrom(m.PropertyType));
                Gambler.Bot.AutoBet.Strategies.BaseStrategy newStrat = null;
                foreach (PropertyInfo x in Props)
                {
                    Gambler.Bot.AutoBet.Strategies.BaseStrategy strat = (Gambler.Bot.AutoBet.Strategies.BaseStrategy)x.GetValue(Settings);
                    if (strat != null)
                    {
                        PropertyInfo StratNameProp = strat.GetType().GetProperty("StrategyName");
                        string nm = (string)StratNameProp.GetValue(strat);
                        if (nm == e.NewValue.ToString())
                        {
                            newStrat = strat;
                            break;
                        }
                    }
                }
                if (newStrat == null)
                {
                    newStrat = Activator.CreateInstance(botIns.Strategies[e.NewValue?.ToString()]) as Gambler.Bot.AutoBet.Strategies.BaseStrategy;                    
                }
                botIns.Strategy = newStrat;
            }
        }

        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ThemedWindow tmpWindow = new ThemedWindow();
            Simulation simControl = new Simulation();
            simControl.CurrentSite = botIns.CurrentSite;
            simControl.Strategy = botIns.Strategy;
            simControl.BetSettings = botIns.BetSettings;
            tmpWindow.Content = simControl;
            tmpWindow.Height = 550;
            tmpWindow.Width = 700;
            tmpWindow.Show();
        }

        private void BarButtonItem_ItemClick_1(object sender, ItemClickEventArgs e)
        {

        }

        private void bbtnLogOut_ItemClick(object sender, ItemClickEventArgs e)
        {
            SignOut();
            //botIns.StopStrategy("Logging Out");
            //botIns.CurrentSite.Disconnect();
            //dlmMainLayout.Visibility = Visibility.Collapsed;
            //lciSelectSite1.Visibility = Visibility.Collapsed;
            //lciLoginControl.Visibility = Visibility.Visible;
        }

        private void bbtnSite_ItemClick(object sender, ItemClickEventArgs e)
        {
            ChangeSite();
        }

        private void LayoutPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
//            lcStrategy.Height = e.NewSize.Height-10;
        }

        private void lueCurrencies_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (botIns.CurrentSite.CurrentCurrency!= (string)lueCurrencies.EditValue)
                botIns.CurrentSite.Currency = Array.IndexOf(botIns.CurrentSite.Currencies, lueCurrencies.EditValue);
 
        }

        private void lueGames_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (botIns.CurrentGame != (Gambler.Bot.Core.Games.Games)lueGames.EditValue)
                botIns.CurrentGame = (Gambler.Bot.Core.Games.Games)lueGames.EditValue;
        }

        private void btnResume_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                botIns.Resume();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sessionStats_ResetStats(object sender, EventArgs e)
        {
            botIns.ResetStats();
        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            SignOut();
        }

        void SignOut()
        {
            botIns.StopStrategy("Logging Out");
            botIns.CurrentSite?.Disconnect();
            SiteChanged((Activator.CreateInstance(botIns.CurrentSite.GetType()) as Gambler.Bot.Core.Sites.BaseSite), botIns.CurrentSite.CurrentCurrency, botIns.CurrentGame.ToString());
        }

        private void btnChangeSite_Click(object sender, RoutedEventArgs e)
        {
            ChangeSite();
        }

        void ChangeSite()
        {
            var result = MessageBox.Show("Betting will be stopped and you will be signed out of the current site\n\nDo you want to continue?", "Change Site", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                botIns.StopStrategy("Changing Site");
                botIns.CurrentSite.Disconnect();
                dlmMainLayout.Visibility = Visibility.Collapsed;
                lciSelectSite1.Visibility = Visibility.Visible;
                lciLoginControl.Visibility = Visibility.Collapsed;
            }
        }
    }
    public class RenameEventArgs:EventArgs
    {
        public string newName { get; set; }
    }

}
