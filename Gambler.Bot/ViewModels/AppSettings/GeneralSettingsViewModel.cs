using ActiproSoftware;
using ActiproSoftware.UI.Avalonia.Themes;
using ActiproSoftware.UI.Avalonia.Themes.Generation;
using Avalonia.Controls;
using Gambler.Bot.AutoBet.Helpers;
using Gambler.Bot.Classes;
using Gambler.Bot.ViewModels.AppSettings.Utilities;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gambler.Bot.ViewModels.AppSettings
{
    public class GeneralSettingsViewModel: ObservableObjectBase
    {
        public List<string> UpdateModes { get; set; } = new List<string> { "Never", "Prompt", "Auto" };
        private readonly ColorPalette _colorPalette;
        private PersonalSettings settings;

        private IEnumerable<ColorRampViewModel>? _ramps;

        public PersonalSettings Settings
        {
            get { return settings; }
            set { settings = value; this.OnPropertyChanged(); }
        }

        private UISettings uiSettings;

        public UISettings UiSettings
        {
            get { return uiSettings; }
            set { uiSettings = value; OnPropertyChanged(); }
        }

        private ColorRampViewModel selectedRamp;

        public ColorRampViewModel SelectedRamp
        {
            get { return selectedRamp; }
            set { selectedRamp = value; UpdateTheme(); }
        }

        private void UpdateTheme()
        {
            if (!ModernTheme.TryGetCurrent(out var theme))
            {
                return;
            }
            if (SelectedRamp?.Name != null)
            {
                theme.Definition.AccentColorRampName = SelectedRamp.Name;
                theme.RefreshResources();
                UiSettings.ThemeName = SelectedRamp.Name;
            }
        }

        public bool ShowDonatePercentage { get => UiSettings?.DonateMode == "Prompt"|| UiSettings ?.DonateMode== "Auto"; }

        public GeneralSettingsViewModel(ILogger logger) //: base(logger)
        {
            UiSettings = UISettings.Settings;
            UISettings.Settings.PropertyChanged += Settings_PropertyChanged;
            _colorPalette = new DefaultColorPaletteFactory().Create();

            UpdateRamps();
        }
        private void UpdateRamps()
        {
            Ramps = _colorPalette.Ramps.Select(colorRamp => new ColorRampViewModel(colorRamp, 5));
        }
        public IEnumerable<ColorRampViewModel>? Ramps
        {
            get => _ramps;
            set => SetProperty(ref _ramps, value);
        }
        private void Settings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ShowDonatePercentage));
            
            
        }

        public void ThemeToggled()
        {
            ModernTheme.TryGetCurrent(out var theme);
            UiSettings.DarkMode =  App.Current.ActualThemeVariant.Key=="Light";
            //determine if dark theme
            //set uisettings
        }
    }
}
