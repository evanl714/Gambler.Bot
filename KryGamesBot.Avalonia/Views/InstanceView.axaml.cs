using Avalonia;
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
    private Window parentWindow;
    public InstanceView()
    {
        InitializeComponent();
        if (!Design.IsDesignMode)
        {
            this.WhenActivated(action =>
            {
                ViewModel!.ShowSimulation.RegisterHandler(DoShowSimulation);
                ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync);

            });
        }
        this.AttachedToVisualTree += OnAttachedToVisualTree;
        this.DetachedFromVisualTree += OnDetachedFromVisualTree;

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
    private async Task DoShowSimulation(InteractionContext<SimulationViewModel,
                                        SimulationViewModel?> interaction)
    {
        
        
        var ParentWindow = this.FindAncestorOfType<Window>();
        ReactiveWindow< SimulationViewModel> window = new();
        window.DataContext = interaction.Input;
        var dialog = new SimulationView();
        window.Content = dialog;
        dialog.DataContext = interaction.Input;
        window.Show();
    }
    private void OnAttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e)
    {
        parentWindow = this.FindAncestorOfType<Window>();
        if (parentWindow != null)
        {
            parentWindow.Closing += OnWindowClosing;
        }
    }

    private void OnDetachedFromVisualTree(object sender, VisualTreeAttachmentEventArgs e)
    {
        if (parentWindow != null)
        {
            parentWindow.Closing -= OnWindowClosing;
        }
    }

    private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        // Handle window closing logic here
        ViewModel.OnClosing();
    }

    private void Binding(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
}