using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;
using Gambler.Bot.ViewModels.Common;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;
using WebSocket4Net.Command;

namespace Gambler.Bot.Views.Common
{
    public partial class UserInputView : ReactiveUserControl<UserInputViewModel>
    {
        public UserInputView()
        {
            InitializeComponent();
            if (!Design.IsDesignMode)
            {
                this.WhenActivated(action =>
                {
                    ViewModel!.CloseDialogInteraction.RegisterHandler(CloseDialog);
                });
            }
        }

        private async Task CloseDialog(InteractionContext<Unit?, Unit?> context)
        {
            var ParentWindow = this.FindAncestorOfType<Window>();
            ParentWindow.Close();
        }
    }
}
