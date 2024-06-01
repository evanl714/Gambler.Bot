using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;
using Gambler.Bot.ViewModels;
using Gambler.Bot.ViewModels.AppSettings;
using Gambler.Bot.ViewModels.Common;
using Gambler.Bot.Views.AppSettings;
using Gambler.Bot.Views.Common;
using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace Gambler.Bot.Views;

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
                ViewModel!.ShowBetHistory.RegisterHandler(ShowBetHistory);
                ViewModel!.ExitInteraction.RegisterHandler(Close);
                ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync);
                ViewModel!.ShowAbout.RegisterHandler(ShowAbout);
            });
        }
        this.AttachedToVisualTree += OnAttachedToVisualTree;
        this.DetachedFromVisualTree += OnDetachedFromVisualTree;

    }

    private void ShowAbout(InteractionContext<AboutViewModel, Unit?> context)
    {
        var ParentWindow = this.FindAncestorOfType<Window>();
        ReactiveWindow<AboutViewModel> window = new();
        window.DataContext = context.Input;
        var dialog = new AboutView();
        window.Content = dialog;
        dialog.DataContext = context.Input;
        window.Width = 600;
        window.Height = 300;
        window.Title = $"About: Gambler.Bot";
        window.Show();
    }

    private void Close(InteractionContext<Unit?, Unit?> context)
    {
        this.parentWindow.Close();
    }

    private void ShowBetHistory(InteractionContext<BetHistoryViewModel, Unit?> context)
    {
        var ParentWindow = this.FindAncestorOfType<Window>();
        ReactiveWindow<BetHistoryViewModel> window = new();
        window.DataContext = context.Input;
        var dialog = new BetHistoryView();
        window.Content = dialog;
        dialog.DataContext = context.Input;
        window.Width = 1400;
        window.Height = 700;
        window.Title = $"Bet History";
        window.Show();
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
        window.Width = 700;
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