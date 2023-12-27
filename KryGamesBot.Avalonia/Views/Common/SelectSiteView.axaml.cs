using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace KryGamesBot.Avalonia.Views.Common;

public partial class SelectSiteView : UserControl
{
    public SelectSiteView()
    {
        InitializeComponent();

    }

    public void PointerEntered(object sender, PointerEventArgs e)
    {
        
        if (sender is Panel control)
        {
            control.Background = Brushes.LightGray;
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