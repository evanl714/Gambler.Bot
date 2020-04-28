using DevExpress.Xpf.Core;
using DoormatBot.Helpers;
using DoormatBot.Strategies;
using DoormatCore.Sites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KryGamesBot
{
    /// <summary>
    /// Interaction logic for FirstRunWizard.xaml
    /// </summary>
    public partial class FirstRunWizard : DXWindow, INotifyPropertyChanged
    {
        private bool isPortable=false;

        public event PropertyChangedEventHandler PropertyChanged;
        PersonalSettings settings = new PersonalSettings();

        public bool IsPortable
        {
            get { return isPortable; }
            set { isPortable = value;OnPropertyChanged(nameof(IsPortable)); }
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public FirstRunWizard()
        {
            InitializeComponent();
            settings = PersonalSettings.Default();

            foreach (var x in settings.ErrorSettings)
            {
                ErrorSettings.AddItem(x);
            }
            tbMainIntro.Text = @"Thank you for downloading KryGamesBot.

It looks like this is the first time you're using KryGamesBot on this computer, so we need to do a little bit of setup first.

If you would like to use completely default settings, you can cancel this Wizard now. If you would like to configure the bot, click next.

All the settings you will configure in this wizard can also be configured from the settings menu.";

            tbPortable.Text = @"Would you like to run KryGamesBot in portable mode?

In portable mode, all settings are stored in the root folder of the application and can be taken with one a flash drive etc. Whenever you update KryGamesBot, you will need to copy all settings accross or you will need to re-configure from scratch.

In normal mode, settings are stored in your application settings, thus cannot be easily copied over, but any update will not cause you to lose your settings.";

            tbKeepasstmp.Text = @"If you are using KeePass 2 to manage your passwords, you can open your passwords database using KryGamesBot to simplify your login experience.";

            tbNotificationsTmp.Text = @"KryGamesBot can show and send notifications when certain events happen. Here you can configure which kind of notification should happen for different events.";
            tbErrorsTmp.Text = @"Unfortunately, errors are unavoidable. But luckily you can tell the bot what to do when it experiences some kind of error.";
            tbFinished.Text = @"That's it!

You're all set up and ready to start. Finish this wizard to choose the site you want to play at and start betting!";
            
            DataContext = this;
            //DevExpress.Xpf.Core.Theme.Themes
            
        }

        private void Wizard_Cancel(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DoormatBot.Doormat tmpInstance = new DoormatBot.Doormat();
            tmpInstance.PersonalSettings = settings;
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot");
            tmpInstance.SavePersonalSettings(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\PersonalSettings.json");
            tmpInstance.Strategy = new Martingale();
            tmpInstance.SaveBetSettings(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\BetSettings_001.json");

        }

       

        private void Wizard_Next(object sender, CancelEventArgs e)
        {
            switch (wzrd.SelectedIndex)
            {
                case 0:
                case 1:
                case 2:break;
                case 3: 
                    e.Cancel = !dbsetup1.Verify(); 
                    if (!e.Cancel)
                    {
                        dbsetup1.UpdateSettings(settings);
                    }
                //update conenction settings
                    break;
                case 4:
                    KeePassSettings1.UpdateSettings(settings);break; //update keepass
                case 5://update notifications
                case 6: settings.ErrorSettings = ErrorSettings.Errors.ToArray(); break;//udpate error settings
                default: e.Cancel = false; break;
            }
        }

        private void wzrd_Finish(object sender, CancelEventArgs e)
        {
            DoormatBot.Doormat tmpInstance = new DoormatBot.Doormat();
            tmpInstance.PersonalSettings = settings;
            if (IsPortable)
                tmpInstance.SavePersonalSettings("PersonalSettings.json");
            else
            {
                string docspath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\";
                if (!Directory.Exists(docspath))
                {
                    Directory.CreateDirectory(docspath);
                }
                tmpInstance.SavePersonalSettings(docspath+"PersonalSettings.json");

            }
        }
    }
}
