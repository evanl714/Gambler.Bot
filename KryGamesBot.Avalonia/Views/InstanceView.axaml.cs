using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;
using KryGamesBot.Ava.ViewModels;
using KryGamesBot.Ava.ViewModels.Common;
using KryGamesBot.Ava.Views.Common;
using ReactiveUI;
using System.Threading.Tasks;

namespace KryGamesBot.Ava.Views;

public partial class InstanceView : ReactiveUserControl<InstanceViewModel>
{
    public InstanceView()
    {
        InitializeComponent();
        this.WhenActivated(action =>
         action(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
    }
    private async Task DoShowDialogAsync(InteractionContext<LoginViewModel,
                                        LoginViewModel?> interaction)
    {
        var dialog = new LoginView();
        dialog.DataContext = interaction.Input;
        var ParentWindow = this.FindAncestorOfType<Window>();
        var result = await dialog.ShowDialog<LoginViewModel?>(ParentWindow);
        interaction.SetOutput(result);
    }
}