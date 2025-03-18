using ActiproSoftware.UI.Avalonia.Themes;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Gambler.Bot.Common.Games;
using Gambler.Bot.Common.Games.Dice;
using Gambler.Bot.Core.Sites;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Gambler.Bot.Converters
{
    public class DiceForegroundColourConverter : IMultiValueConverter
    {
        public object? Convert(IList<object> values, Type targetType, object? parameter, CultureInfo culture)
        {
            string resource = "";
            
            if (values[0] is DiceBet bet && values[1] is iDice site)
            {
                if (!((bool)bet.High ? (decimal)bet.Roll > (decimal)site.DiceSettings.MaxRoll - (decimal)(bet.Chance) : (decimal)bet.Roll < (decimal)(bet.Chance)))
                {
                    if (bet.Chance <= 50)
                    {
                        if (
                            (decimal)bet.Roll < (decimal)site.DiceSettings.MaxRoll - (decimal)(bet.Chance) &&
                            (decimal)bet.Roll > (decimal)(bet.Chance))
                        {
                            resource = ThemeResourceKind.ControlBackgroundBrushSoftPressed.ToResourceKey();
                        }
                        else
                            resource = ThemeResourceKind.ControlBackgroundBrushSoftDanger.ToResourceKey();
                    }

                    
                }
                else
                {
                    if (bet.Chance > 50)
                    {

                        if ((decimal)bet.Roll > (decimal)site.DiceSettings.MaxRoll - (decimal)(bet.Chance) &&
                            (decimal)bet.Roll < (decimal)(bet.Chance))
                        {
                            resource = ThemeResourceKind.ControlBackgroundBrushSoftWarningPressed.ToResourceKey();
                        }
                        else
                        {
                            resource = ThemeResourceKind.ControlBackgroundBrushSoftSuccess.ToResourceKey();
                        }
                    }

                    
                    
                }
            }
            else if (values[0] is Bet bbet)
            {
                if (bbet.IsWin)
                    resource = ThemeResourceKind.ControlBackgroundBrushSoftSuccess.ToResourceKey();
                else
                    resource = ThemeResourceKind.ControlBackgroundBrushSoftWarningPressed.ToResourceKey();

            }
            if (Application.Current.TryFindResource(resource, App.Current.ActualThemeVariant, out object brush))
            {
                return (Brush)brush;
            }
            return Brushes.Transparent;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
