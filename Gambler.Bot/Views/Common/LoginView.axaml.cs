using Avalonia.ReactiveUI;
using Avalonia.Threading;
using Gambler.Bot.ViewModels.Common;
using ReactiveUI;
using System.Threading.Tasks;

namespace Gambler.Bot.Views.Common;

public partial class LoginView : ReactiveWindow<LoginViewModel>
{
    public LoginView()
    {
        InitializeComponent();
        this.WhenActivated(action => 
        action(ViewModel!.CloseDialog.RegisterHandler(CloseDialogAsync)));
    }

    private async Task CloseDialogAsync(InteractionContext<LoginViewModel,
                                        LoginViewModel?> interaction)
    {
        Dispatcher.UIThread.Invoke(Close);
        
    }
}