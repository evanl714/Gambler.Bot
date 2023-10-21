using KryGamesBot.MAUI.Blazor.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryGamesBot.MAUI.Blazor.Services
{
    public interface IThemeService
    {
        ThemeItem CurrentTheme { get; }
        public event EventHandler OnThemeChanged;

        Task SetTheme(ThemeItem Name);
    }

    public class ThemeService : IThemeService
    {
        public ThemeItem CurrentTheme { get; protected set; } = ThemeItem.Default;

        public async Task SetTheme(ThemeItem Name)
        {
            CurrentTheme = Name;
            OnThemeChanged?.Invoke(this, new EventArgs());
        }

        public event EventHandler OnThemeChanged;
    }

}
