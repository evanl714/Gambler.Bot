using ActiproSoftware.UI.Avalonia.Themes;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Gambler.Bot.Views.Common;

public partial class SelectSiteView : UserControl
{
    public SelectSiteView()
    {
        InitializeComponent();

    }

    public void PointerEntered(object sender, PointerEventArgs e)
    {
        
        if (sender is Panel control )
        {   
            string resourceKey = ThemeResourceKind.Container4BackgroundBrush.ToResourceKey();
            if (Application.Current.TryGetResource(resourceKey, App.Current.ActualThemeVariant, out object obj))
            {
                control.Background = (Brush)obj;
            }
            
        }
    }

    public void PointerExited(object sender, PointerEventArgs e)
    {
        
        if (sender is Panel control)
        {
            control.Background = Brushes.Transparent;
        }
    }
}