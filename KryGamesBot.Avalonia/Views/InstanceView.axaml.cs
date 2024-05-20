using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;
using KryGamesBot.Ava.ViewModels;
using KryGamesBot.Ava.ViewModels.AppSettings;
using KryGamesBot.Ava.ViewModels.Common;
using KryGamesBot.Ava.Views.AppSettings;
using KryGamesBot.Ava.Views.Common;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

namespace KryGamesBot.Ava.Views;

public partial class InstanceView : ReactiveUserControl<InstanceViewModel>
{
    private Window parentWindow;
    public InstanceView()
    {
        InitializeComponent();
        Loaded += InstanceView_Loaded;
        if (!Design.IsDesignMode)
        {
            this.WhenActivated(action =>
            {
                ViewModel!.ShowSimulation.RegisterHandler(DoShowSimulation);
                ViewModel!.ShowRollVerifier.RegisterHandler(ShowRollVerifier);
                ViewModel!.ShowSettings.RegisterHandler(ShowSettings);
                ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync);

            });
        }
        this.AttachedToVisualTree += OnAttachedToVisualTree;
        this.DetachedFromVisualTree += OnDetachedFromVisualTree;

    }

    private void InstanceView_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ViewModel.Loaded();
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

    private async Task ShowRollVerifier(InteractionContext<RollVerifierViewModel,
                                        Unit?> interaction)
    {
        var ParentWindow = this.FindAncestorOfType<Window>();
        ReactiveWindow<RollVerifierViewModel> window = new();
        window.DataContext = interaction.Input;
        var dialog = new RollVerifierView();
        window.Content = dialog;
        dialog.DataContext = interaction.Input;
        window.Width = 500;
        window.Height = 500;
        window.Title = $"Roll Verifier - {interaction.Input.Site?.SiteName}";
        window.Show();
    }
    private async Task ShowSettings(InteractionContext<GlobalSettingsViewModel,
                                        Unit?> interaction)
    {
        var ParentWindow = this.FindAncestorOfType<Window>();
        ReactiveWindow<GlobalSettingsViewModel> window = new();
        window.DataContext = interaction.Input;
        var dialog = new GlobalSettingsView();
        window.Content = dialog;
        dialog.DataContext = interaction.Input;
        window.Width = 600;
        window.Height = 500;
        window.Title = $"Settings";
        window.Show();
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
        window.Width = 800;
        window.Height = 450;
        window.Title=$"Simulation - {interaction.Input.CurrentSite?.SiteName} - {interaction.Input.Strategy?.StrategyName}";
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

    private void OnMenuItemClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem)
        {
            if (menuItem.Icon is CheckBox checkBox)
            {
                // Toggle check state
                checkBox.IsChecked = !checkBox.IsChecked;
            }
            else if (menuItem.Icon is RadioButton radioButton)
            {
                // Select new radio button and other radio buttons
                // with same group name will be unselected
                radioButton.IsChecked = true;
            }
        }
           
	
    }
}