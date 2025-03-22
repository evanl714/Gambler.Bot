using ActiproSoftware.UI.Avalonia.Controls;
using ActiproSoftware.UI.Avalonia.Themes;
using Gambler.Bot.Classes;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Gambler.Bot.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        private InstanceViewModel _instance;
        public InstanceViewModel Instance { get =>_instance; set { _instance = value; this.RaisePropertyChanged(); } }
        static string uiSettingsFile;
        internal static ILogger log = null;
        public MainViewModel(Microsoft.Extensions.Logging.ILogger logger) : base(logger)
        {
            log = logger;
            uiSettingsFile = string.Empty;
            UISettings.Portable = File.Exists("portable"); ;
            if (UISettings.Portable)
                uiSettingsFile = "";
            else
            {
                uiSettingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Gambler.Bot");
            }
            Instance = new InstanceViewModel(logger);
            uiSettingsFile = Path.Combine(uiSettingsFile, "UISettings.json");
            if (File.Exists(uiSettingsFile))
            {
                UISettings.Settings = JsonSerializer.Deserialize<UISettings>(File.ReadAllText(uiSettingsFile));
                //change the theme somehow?
                if (UISettings.Settings.DarkMode != null)
                {
                    App.Current.RequestedThemeVariant = UISettings.Settings.DarkMode == true ? Avalonia.Styling.ThemeVariant.Dark : Avalonia.Styling.ThemeVariant.Light;
                }
                if ((UISettings.Settings?.ThemeName??"Default") != "Default")
                {
                    if (!ModernTheme.TryGetCurrent(out var theme))
                    {
                        return;
                    }
                   
                    theme.Definition.AccentColorRampName = UISettings.Settings.ThemeName;
                    theme.RefreshResources();
                }
                _= Loaded();
            }
        }

        public async Task Loaded()
        {
            try
            {
                if (UISettings.Settings.UpdateMode == "Auto")
                {
                    await App.UpdateMyApp();
                }
                else if (UISettings.Settings.UpdateMode == "Prompt")
                {
                    if (App.HasUpdate().Result)
                    {
                        var result = await MessageBox.Show("There is an update available. Would you like to update now?", "Update Available", MessageBoxButtons.YesNo, MessageBoxImage.Information);
                        if (result == MessageBoxResult.Yes)
                        {
                            await App.UpdateMyApp();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error checking for updates");
            }
        }

        public void SaveUISettings()
        {
            if (!UISettings.Resetting)
            {
                
                File.WriteAllText(uiSettingsFile, JsonSerializer.Serialize(UISettings.Settings));
            }
        }

        internal static void ClearUiSettings()
        {
            if (File.Exists(uiSettingsFile))
                File.Delete(uiSettingsFile);
        }

        internal void OnClosing()
        {
            SaveUISettings();
        }
    }
}
