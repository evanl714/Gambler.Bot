using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using static IronPython.Modules._ast;

namespace Gambler.Bot.Controls;

public partial class ColourChangeSelTextBlock : SelectableTextBlock
{
public ColourChangeSelTextBlock()
{

}

public string SetClasses
{
get => Classes?.ToString();
    set
    {
    Classes.Clear();
    Classes.AddRange(value.Split(' '));
    }

    }
    }

