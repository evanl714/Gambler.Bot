using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using KryGamesBot.Ava.ViewModels;
using KryGamesBot.Ava.Views;
using AvaloniaWebView;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using KryGamesBot.Ava.ViewModels.Common;
using Microsoft.Extensions.Configuration;

namespace KryGamesBot.Ava
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public override void Initialize()
        {
            
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var logger = ServiceProvider.GetService<ILogger<MainWindowViewModel>>();
                desktop.MainWindow = new MainWindow
                {                    
                    DataContext = new MainWindowViewModel(logger),
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MainView
                {
                    DataContext = new MainViewModel(ServiceProvider.GetService<ILogger<MainViewModel>>())
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole());
            services.AddTransient<MainViewModel>();
            services.AddTransient<SelectSiteViewModel>();
            // Register other ViewModels and services
        }
    }
}