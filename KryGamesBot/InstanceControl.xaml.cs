using DevExpress.Mvvm.UI.ModuleInjection;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DoormatBot;
using DoormatCore.Helpers;
using KryGamesBot.Helpers;
using KryGamesBotControls.Common;
using KryGamesBotControls.Games;
using KryGamesBotControls.Games.Dice;
using KryGamesBotControls.Strategies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
namespace KryGamesBot
{
    /// <summary>
    /// Interaction logic for InstanceControl.xaml
    /// </summary>
    
    public partial class InstanceControl : UserControl, INotifyPropertyChanged
    {
        Doormat botIns = new Doormat();

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<RenameEventArgs> Rename;

        public iLiveBet LiveBets { get; set; }
        public iPlaceBet ManualBet { get; set; }
        public iStrategy StrategyControl { get; set; }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public InstanceControl()
        {
            InitializeComponent();
            DataContext = this;
            botIns.CompileSites();
            botIns.GetStrats();
            cbeStartegies.ItemsSource = botIns.Strategies.Keys;
            SelectSite1.Sites = Doormat.Sites;
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
            
            botIns.OnStrategyChanged += BotIns_OnStrategyChanged;
            botIns.Strategy = new DoormatBot.Strategies.ProgrammerLUA();
            //(botIns.Strategy as DoormatBot.Strategies.Martingale).MinBet = 0.00000001m;
            botIns.Strategy.Amount = 0.00000001m;
            botIns.CurrentGame = DoormatCore.Games.Games.Dice;
            if (File.Exists("personalsettings.json"))
            {
                botIns.LoadPersonalSettings(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\PersonalSettings.json");
            }
            //Check if global settings for this account exists
            else if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\PersonalSettings.json"))
            {
                botIns.LoadPersonalSettings(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\PersonalSettings.json");
            }
            cbeStartegies.EditValue = botIns.Strategy.GetType().Name;
        }

        private void BotIns_OnStrategyChanged(object sender, EventArgs e)
        {
            string s = botIns.Strategy?.GetType().Name;
            switch (s)
            {
                case "DAlembert": StrategyControl = new dAlembert();break;
                case "Fibonacci": StrategyControl = new Fibonacci();break;
                case "Labouchere": StrategyControl = new Labouchere();break;
                case "Martingale": StrategyControl = new Martingale(); break;
                case "PresetList":
                case "ProgrammerCS": StrategyControl = new ProgrammerModeCS(); break;
                case "ProgrammerJS": StrategyControl = new ProgrammerModeJS(); break;
                case "ProgrammerLUA": StrategyControl = new ProgrammerModeLUA();  break;
                case "ProgrammerPython": StrategyControl = new ProgrammerModePY(); break;
                default:
                    botIns.Strategy = new DoormatBot.Strategies.Martingale(); break;
            }
            StratContent.VerticalAlignment = StrategyControl.TopAlign() ? VerticalAlignment.Top : VerticalAlignment.Stretch;
            StrategyControl.SetStrategy(botIns.Strategy);
            OnPropertyChanged(nameof(StrategyControl));
        }

        string InstanceName = "";
        public void Closing()
        {
            botIns.StopDice("Application Closing");
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

        private void BotIns_OnSiteStatsUpdated(object sender, DoormatCore.Sites.StatsUpdatedEventArgs e)
        {
            SiteStats.Stats = e.NewStats;
        }

        private void BotIns_OnSiteRegisterFinished(object sender, DoormatCore.Sites.GenericEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void BotIns_OnSiteNotify(object sender, DoormatCore.Sites.GenericEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void BotIns_OnSiteLoginFinished(object sender, DoormatCore.Sites.LoginFinishedEventArgs e)
        {
            if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
            {
                Dispatcher.Invoke(new DoormatCore.Sites.BaseSite.dLoginFinished(BotIns_OnSiteLoginFinished), sender, e);
                return;
            }
            if (e.Success)
            {
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

        private void BotIns_OnSiteError(object sender, DoormatCore.Sites.ErrorEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void BotIns_OnSiteChat(object sender, DoormatCore.Sites.GenericEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void BotIns_OnSiteBetFinished(object sender, DoormatCore.Sites.BetFinisedEventArgs e)
        {
            LiveBets.AddBet(e.NewBet);
            ProfitChart1.AddPoint(e.NewBet.Profit);
            SiteStats.RefreshStats();
            if (sessionStats.Stats != botIns.Stats)
                sessionStats.Stats = botIns.Stats;
            else
                sessionStats.RefreshStats();
        }

        private void BotIns_OnSiteAction(object sender, DoormatCore.Sites.GenericEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void BotIns_OnNotification(object sender, DoormatCore.Helpers.NotificationEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void BotIns_OnGameChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            switch (botIns.CurrentGame)
            {
                case DoormatCore.Games.Games.Crash:break;
                case DoormatCore.Games.Games.Dice:
                    ManualBet = new PlaceDiceBetControl();
                    LiveBets = new DiceLiveBetsControl(); 
                    break;
                case DoormatCore.Games.Games.Plinko: break;
                case DoormatCore.Games.Games.Roulette: break;
            }
            OnPropertyChanged(nameof(ManualBet));
            OnPropertyChanged(nameof(LiveBets));
            ManualBet.PlaceBet += ManualBet_PlaceBet;
        }

        private void ManualBet_PlaceBet(object sender, PlaceBetEventArgs e)
        {
            botIns.PlaceBet(e.NewBet);            
        }

        private void BotIns_NeedKeepassPassword(object sender, DoormatBot.Helpers.PersonalSettings.GetConstringPWEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.TmpInstance_NeedKeepassPassword(sender, e);
        }

        private void BotIns_NeedConstringPassword(object sender, DoormatBot.Helpers.PersonalSettings.GetConstringPWEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.TmpInstance_NeedKeepassPassword(sender, e);
        }

        private void SelectSite1_OnSiteSelected(object sender, KryGamesBotControls.Common.SiteSelectedEventArgs e)
        {
            
            SiteChanged(Activator.CreateInstance(e.SelectedSite.SiteType()) as DoormatCore.Sites.BaseSite);
        }

        void SiteChanged(DoormatCore.Sites.BaseSite NewSite )
        {
            botIns.CurrentSite = NewSite;
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
            if (File.Exists(path+Name + ".betset"))
            {   
                botIns.LoadBetSettings(path + Name + ".betset");
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
            InstanceSettings tmp = json.JsonDeserialize<InstanceSettings>(Settings);
            //botIns.ga
            var tmpsite = Doormat.Sites.FirstOrDefault(m => m.Name == tmp.Site);
            if (tmpsite != null)
            {
                botIns.CurrentSite = Activator.CreateInstance(tmpsite.SiteType()) as DoormatCore.Sites.BaseSite;
                SiteChanged(botIns.CurrentSite);
            }
            if (tmp.Game!=null)
                botIns.CurrentGame =  Enum.Parse<DoormatCore.Games.Games>(tmp.Game);
            
        }

        void SaveINstanceSettings(string FileLocation)
        {
            string Settings = json.JsonSerializer<InstanceSettings>(new InstanceSettings { Site=botIns.CurrentSite?.GetType()?.Name, AutoLogin=false, Game=botIns.CurrentGame.ToString() });
            File.WriteAllText(Settings, FileLocation); 
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
            botIns.StartDice();
        }

        private void btcStop_Click(object sender, RoutedEventArgs e)
        {
            botIns.StopDice("Stop button clicked");
        }

        private void btnDark_Click(object sender, RoutedEventArgs e)
        {
            ApplicationThemeHelper.ApplicationThemeName = "Office2019Black";
        }

        private void btnLight_Click(object sender, RoutedEventArgs e)
        {
            ApplicationThemeHelper.ApplicationThemeName = "Office2019White";
        }

        private void bExit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            string x = botIns.CurrentSite?.SiteName;
        }

        private void btnStopWin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbeStartegies_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (botIns.Strategy.StrategyName != e.NewValue.ToString())
                botIns.Strategy = Activator.CreateInstance(botIns.Strategies[e.NewValue?.ToString()]) as DoormatBot.Strategies.BaseStrategy;
            
        }

        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ThemedWindow tmpWindow = new ThemedWindow();
            Simulation simControl = new Simulation();
            simControl.CurrentSite = botIns.CurrentSite;
            simControl.Strategy = botIns.Strategy;
            simControl.BetSettings = botIns.BetSettings;
            tmpWindow.Content = simControl;
            tmpWindow.Height = 500;
            tmpWindow.Width = 700;
            tmpWindow.Show();
        }

        private void BarButtonItem_ItemClick_1(object sender, ItemClickEventArgs e)
        {

        }

        private void bbtnLogOut_ItemClick(object sender, ItemClickEventArgs e)
        {
            botIns.StopDice("Logging Out");
            botIns.CurrentSite.Disconnect();
            dlmMainLayout.Visibility = Visibility.Collapsed;
            LoginControl.Visibility = Visibility.Visible;
        }

        private void bbtnSite_ItemClick(object sender, ItemClickEventArgs e)
        {
            botIns.StopDice("Changing Site");
            botIns.CurrentSite.Disconnect();
            dlmMainLayout.Visibility = Visibility.Collapsed;
            SelectSite1.Visibility =  Visibility.Visible;
        }

        private void LayoutPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
//            lcStrategy.Height = e.NewSize.Height-10;
        }
    }
    public class RenameEventArgs:EventArgs
    {
        public string newName { get; set; }
    }

}
