using Avalonia.ReactiveUI;
using Avalonia.Threading;
using KryGamesBot.Ava.ViewModels.Common;
using ReactiveUI;
using System.Threading.Tasks;

namespace KryGamesBot.Ava.Views.Common;

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