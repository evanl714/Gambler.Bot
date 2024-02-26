using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;
using KryGamesBot.Avalonia.ViewModels;
using KryGamesBot.Avalonia.ViewModels.Common;
using KryGamesBot.Avalonia.Views.Common;
using ReactiveUI;
using System.Threading.Tasks;

namespace KryGamesBot.Avalonia.Views;

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

        var result = await dialog.ShowDialog<LoginViewModel?>(this.FindAncestorOfType<Window>());
        interaction.SetOutput(result);
    }
}