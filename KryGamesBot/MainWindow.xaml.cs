using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using Gambler.Bot.AutoBet;
using Gambler.Bot.Core.Helpers;
using KryGamesBot.Helpers;
using KryGamesBotControls;
using KryGamesBotControls.Common;
using KryGamesBotControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KryGamesBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DXWindow
    {
        
        public static bool Portable { get{return File.Exists("portable"); } }
        public static string Path { get { return Portable ? "" : Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\"; } }
        List<DocumentPanel> documents = new List<DocumentPanel>();
        private static string dbPw { get; set; }
        private static string kpPw { get; set; }
        public ICommand CloseCommand { get; private set; }
        public ICommand AddNewCommand { get; private set; }
        public MainWindow()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.WindowState = WindowState.Maximized;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
            //dlmMainMainLayout.DataContext = this;
            AddNewCommand = new DelegateCommand(AddNew);
            ApplicationThemeHelper.ApplicationThemeName = "Office2019Black";
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //.DumpLog(e.ExceptionObject as Exception);
        }

        public void AddNew()
        {
            DocumentPanel newPanel = new DocumentPanel();
            newPanel.Name = "_" + Guid.NewGuid().ToString().Replace("-", "");
            AddNew(newPanel, true);
            
            mainTabs.Add(newPanel);
            mainTabs.Visibility = Visibility.Visible;
            
        }

        public void AddNew(DocumentPanel newPanel, bool ActualNew=false)
        {
            newPanel.Caption = "Select a site";
            newPanel.Content = new InstanceControl();
            if (ActualNew && dlmMainMainLayout.ActiveDockItem is DocumentPanel ActivePanel && ActivePanel.Content is InstanceControl ActiveInstance)
            {

                (newPanel.Content as InstanceControl).botIns.StoredBetSettings = 
                    Gambler.Bot.Core.Helpers.CopyHelper.CreateCopy<Gambler.Bot.AutoBet.Doormat.ExportBetSettings>(
                        ActiveInstance.botIns.StoredBetSettings);
                (newPanel.Content as InstanceControl).botIns.Strategy = (newPanel.Content as InstanceControl).botIns.StoredBetSettings.GetStrat();

            }
            (newPanel.Content as InstanceControl).Rename += MainWindow_Rename;
            documents.Add(newPanel);            
            (newPanel.Content as InstanceControl).LoadSettings(newPanel.Name);
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            
            dlmMainMainLayout.DockItemRestored += DlmMainMainLayout_DockItemRestored;
            dlmMainMainLayout.LayoutItemRestored += DlmMainMainLayout_LayoutItemRestored1;
            string DocsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\";
            
            Gambler.Bot.AutoBet.Doormat tmpInstance = new Gambler.Bot.AutoBet.Doormat(null);
            tmpInstance.NeedConstringPassword += TmpInstance_NeedConstringPassword;
            tmpInstance.NeedKeepassPassword += TmpInstance_NeedKeepassPassword;
            //check if there's a local settings file
            if (Portable && File.Exists("personalsettings.json"))
            {   
                tmpInstance.LoadPersonalSettings("PersonalSettings.json");
            }
            //Check if global settings for this account exists
            else if (!Portable && File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\PersonalSettings.json"))
            {
                tmpInstance.LoadPersonalSettings(DocsPath+"PersonalSettings.json");
            }
            else
            {
                FirstRunWizard tmp = new FirstRunWizard();
                tmp.Owner = this;
                tmp.Height = 550;
                tmp.Width = 700;
                tmp.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //tmp.Owner = this;
                tmp.ShowDialog();
                //if this is a new install
                /*dbSetup.Visibility = Visibility.Visible;
                tcMainTabs.Visibility = Visibility.Hidden;*/
            }
            if (Portable && File.Exists("mainlayout"))
                dlmMainMainLayout.RestoreLayoutFromXml("mainlayout");
            else if (!Portable && File.Exists(DocsPath + "mainlayout"))
                dlmMainMainLayout.RestoreLayoutFromXml(DocsPath + "mainlayout");

            if (File.Exists(Path + "UISettings.json"))
            {
                UISettings.Settings = JsonSerializer.Deserialize<UISettings>(File.ReadAllText(Path + "UISettings.json"));
                ApplicationThemeHelper.ApplicationThemeName = UISettings.Settings.ThemeName;
            }
            
            //Load UI Settings

            /*DevExpress.Xpf.Core.DXTabItem NewTab = tcMainTabs.AddNewTabItem() as DevExpress.Xpf.Core.DXTabItem;
            NewTab.Header = "Select a site";*/
                        bool added = false;
            var result = dlmMainMainLayout.GetItems();
            foreach (var x in result)
            {
                //mainTabs.it
                if (x is DocumentPanel)
                {                    
                    if (!x.Closed)
                    {
                        AddNew(x as DocumentPanel);
                        added = true;
                    }
                }
            }
            if (!added)
                AddNew();
        }

        private void DlmMainMainLayout_DockItemRestored(object sender, DevExpress.Xpf.Docking.Base.ItemEventArgs e)
        {
           if (e.Item is DocumentPanel && (e.Item as DocumentPanel).Content == null)
            {
                (e.Item as DocumentPanel).Content = new InstanceControl();
                ((e.Item as DocumentPanel).Content as InstanceControl).Rename += MainWindow_Rename;
            }
        }

        private void DlmMainMainLayout_LayoutItemRestored1(object sender, DevExpress.Xpf.Docking.Base.LayoutItemRestoredEventArgs e)
        {
            
        }


        internal static void TmpInstance_NeedKeepassPassword(object sender, Gambler.Bot.AutoBet.Helpers.PersonalSettings.GetConstringPWEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(kpPw))
            {
                e.Password = kpPw;
                return;
            }

            GetPasswordDialog tmpdiag = new GetPasswordDialog();
            
            if (tmpdiag.ShowDialog()??false)
            {
                kpPw = e.Password = tmpdiag.Pw;
                
            }
        }

        internal static void TmpInstance_NeedConstringPassword(object sender, Gambler.Bot.AutoBet.Helpers.PersonalSettings.GetConstringPWEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(dbPw))
            {
                e.Password = dbPw;
                return;
            }

            GetPasswordDialog tmpdiag = new GetPasswordDialog();

            if (tmpdiag.ShowDialog() ?? false)
            {
                dbPw = e.Password = tmpdiag.Pw;

            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            for (int i =0; i< documents.Count;i++)
            {
                var Tab = documents[i];
                if (Tab.Closed)
                {
                    mainTabs.Remove(Tab);
                    try
                    {
                        dlmMainMainLayout.ClosedPanels.Remove(Tab);                        
                    }
                    catch { }
                    documents.RemoveAt(i--);
                    (Tab.Content as InstanceControl).Removed();
                }
            }
            string layoutresult = "";
            dlmMainMainLayout.SaveLayoutToXml(Path+"mainlayout");
            File.WriteAllText(Path + "UISettings.json", JsonSerializer.Serialize<UISettings>(UISettings.Settings));
                
            foreach (var Tab in documents)
            {
                if (Tab.Content is InstanceControl)
                {
                    try
                    {
                        (Tab.Content as InstanceControl).Closing();
                    }

                    catch
                    {

                    }
                }
            }
            foreach (FileInfo x in new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory).GetFiles())
            {
                if (x.Name.EndsWith("." + Process.GetCurrentProcess().Id))
                {
                    try
                    {
                        x.Delete();
                    }
                    catch
                    {

                    }
                }
            }
            base.OnClosing(e);
        }

        private void tcMainTabs_TabAdded(object sender, TabControlTabAddedEventArgs e)
        {   
            (e.Item as DevExpress.Xpf.Core.DXTabItem).Content = new InstanceControl();
            ((e.Item as DevExpress.Xpf.Core.DXTabItem).Content as InstanceControl).Rename += MainWindow_Rename; 
        }

        private void MainWindow_Rename(object sender, RenameEventArgs e)
        {
            foreach (var Tab in documents)
            {  
                if (Tab.Content == sender)
                {
                    Tab.Caption = e.newName;
                }
            }
        }

        private void tcMainTabs_TabRemoved(object sender, TabControlTabRemovedEventArgs e)
        {

        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            DocumentPanel newPanel = new DocumentPanel();
            newPanel.Caption = "Select a site";
            newPanel.Content = new InstanceControl();
            (newPanel.Content as InstanceControl).Rename += MainWindow_Rename;
            //mainTabs.Add(newPanel);
            
        }

        private void SimpleButton_Click_1(object sender, RoutedEventArgs e)
        {
            AddNew();
        }

        private void bbtnNewTab_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AddNew();
        }

        private void bbtnCloseTab_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void bchk_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GlobalSettings tmp = new GlobalSettings();
            Gambler.Bot.AutoBet.Doormat botins = new Gambler.Bot.AutoBet.Doormat(null);
            botins.LoadPersonalSettings(Path + "personalsettings.json");
            tmp.Settings = botins.PersonalSettings;
            //tmp.Settings = personal settings instance being used?
            tmp.Show();
        }

        private void bsContact_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void bsTutorials_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void bsSource_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void bsReset_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void bsAbout_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void dlmMainMainLayout_DockItemClosed(object sender, DevExpress.Xpf.Docking.Base.DockItemClosedEventArgs e)
        {
            // ((sender as DocumentPanel).Content as InstanceControl).Removed();
            if (e.Item is DocumentPanel pnl)
            {
                (pnl.Content as InstanceControl).Closing();
                //documents.Remove(e.Item as DocumentPanel);
            }
        }
    }
}
