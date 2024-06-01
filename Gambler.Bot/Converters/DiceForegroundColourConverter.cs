using ActiproSoftware.UI.Avalonia.Themes;
using Amazon.Auth.AccessControlPolicy;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Gambler.Bot.Core.Games;
using Gambler.Bot.Core.Sites;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gambler.Bot.Converters
{
    public class DiceForegroundColourConverter : IMultiValueConverter
    {
        public object? Convert(IList<object> values, Type targetType, object? parameter, CultureInfo culture)
        {
            string resource = "";
            
            if (values[0] is DiceBet bet && values[1] is BaseSite site)
            {
                if (!((bool)bet.High ? (decimal)bet.Roll > (decimal)site.MaxRoll - (decimal)(bet.Chance) : (decimal)bet.Roll < (decimal)(bet.Chance)))
                {
                    if (bet.Chance <= 50)
                    {
                        if (
                            (decimal)bet.Roll < (decimal)site.MaxRoll - (decimal)(bet.Chance) &&
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

                        if ((decimal)bet.Roll > (decimal)site.MaxRoll - (decimal)(bet.Chance) &&
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
