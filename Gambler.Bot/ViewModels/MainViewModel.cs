using ActiproSoftware.UI.Avalonia.Themes;
using Gambler.Bot.Classes;
using ReactiveUI;
using System;
using System.IO;
using System.Text.Json;

namespace Gambler.Bot.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        private InstanceViewModel _instance;
        public InstanceViewModel Instance { get =>_instance; set { _instance = value; this.RaisePropertyChanged(); } }
        public MainViewModel(Microsoft.Extensions.Logging.ILogger logger) : base(logger)
        {
            string path = string.Empty;
            UISettings.Portable = File.Exists("portable"); ;
            if (UISettings.Portable)
                path = "";
            else
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Gambler.Bot");
            }
            Instance = new InstanceViewModel(logger);
            if (File.Exists(Path.Combine(path, "UISettings.json")))
            {
                UISettings.Settings = JsonSerializer.Deserialize<UISettings>(File.ReadAllText(Path.Combine(path, "UISettings.json")));
                //change the theme somehow?
                if ((UISettings.Settings?.ThemeName??"Default") != "Default")
                {
                    if (!ModernTheme.TryGetCurrent(out var theme))
                    {
                        return;
                    }
                    theme.Definition.AccentColorRampName = UISettings.Settings.ThemeName;
                    theme.RefreshResources();
                }
                
            }
        }

        public void SaveUISettings()
        {
            string path = "";
            if (UISettings.Portable)
                path = "";
            else
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Gambler.Bot");
            }
            File.WriteAllText(Path.Combine(path, "UISettings.json"), JsonSerializer.Serialize(UISettings.Settings));
            
        }

        internal void OnClosing()
        {
            SaveUISettings();
        }
    }
}
