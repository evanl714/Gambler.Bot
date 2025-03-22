using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using Gambler.Bot.ViewModels.Common;

namespace Gambler.Bot.Views.Common
{
    public partial class ConsoleView : ReactiveUserControl<ConsoleViewModel>
    {
        public ConsoleView()
        {
            InitializeComponent();
            txtCommand.AddHandler(InputElement.KeyDownEvent, TextBox_KeyDown, RoutingStrategies.Tunnel);
        }

        private void TextBox_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
        {
            
            if (e.Key== Avalonia.Input.Key.Enter && e.KeyModifiers == Avalonia.Input.KeyModifiers.None)                
            {
                ViewModel.ExecuteCommand();
                e.Handled = true;
            }
            else if (e.Key == Avalonia.Input.Key.Up && e.KeyModifiers == Avalonia.Input.KeyModifiers.None)
            {
                ViewModel.NavigateCommand(true);
                e.Handled = true;
            }
            else if (e.Key == Avalonia.Input.Key.Down && e.KeyModifiers == Avalonia.Input.KeyModifiers.None)
            {
                ViewModel.NavigateCommand(false);
                e.Handled = true;
            }
        }
    }
}
