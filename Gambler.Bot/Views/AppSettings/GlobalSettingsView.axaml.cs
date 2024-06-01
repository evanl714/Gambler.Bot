using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;
using Gambler.Bot.ViewModels.AppSettings;
using Gambler.Bot.ViewModels.Common;
using Gambler.Bot.Views.Common;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

namespace Gambler.Bot.Views.AppSettings
{
    public partial class GlobalSettingsView :  ReactiveUserControl<GlobalSettingsViewModel>
    {
        public GlobalSettingsView()
        {
            InitializeComponent();
            if (!Design.IsDesignMode)
            {
                this.WhenActivated(action =>
                {
                    ViewModel!.CloseWindow.RegisterHandler(DoCloseWindow);
                    

                });
            }
        }
        private async Task DoCloseWindow(InteractionContext<Unit?,Unit?> interaction)
        {
            var ParentWindow = this.FindAncestorOfType<Window>();
            ParentWindow.Close();
            
        }
    }
}
