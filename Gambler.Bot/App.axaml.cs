using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Gambler.Bot.ViewModels;
using Gambler.Bot.Views;
using AvaloniaWebView;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using Gambler.Bot.ViewModels.Common;
using Microsoft.Extensions.Configuration;
using Projektanker.Icons.Avalonia.MaterialDesign;
using Projektanker.Icons.Avalonia;
using Velopack;
using System.Threading.Tasks;
using Serilog;

namespace Gambler.Bot
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            // Workaround for default ToggleThemeButton theme in Actipro Avalonia v24.1.0
            _ = ActiproSoftware.Properties.Shared.AssemblyInfo.Instance;
            
        }
        internal static async Task<bool> HasUpdate()
        {
            var mgr = new UpdateManager("https://github.com/Seuntjie900/Gambler.Bot");

            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();
            return newVersion != null;               
        }
        internal static async Task UpdateMyApp()
        {
            var mgr = new UpdateManager("https://github.com/Seuntjie900/Gambler.Bot");

            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
                return; // no update available

            // download new version
            await mgr.DownloadUpdatesAsync(newVersion);

            // install new version and restart app
            mgr.ApplyUpdatesAndRestart(newVersion);
        }
        public override void OnFrameworkInitializationCompleted()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();
            if(ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                //var logger = ServiceProvider.GetService<ILogger<MainWindowViewModel>>();
                desktop.MainWindow = new MainWindow { DataContext = ServiceProvider.GetService<MainWindowViewModel>(), };
            } else if(ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MainView
                {
                    DataContext = ServiceProvider.GetService<MainWindowViewModel>()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var serilogLogger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .WriteTo.File("gamblerbotlog.log") // Serilog.Sinks.Debug
    .CreateLogger();
            Log.Logger = serilogLogger;
            IconProvider.Current.Register<MaterialDesignIconProvider>();
            services.AddLogging(configure => configure.AddSerilog().AddConsole().SetMinimumLevel(LogLevel.Information).AddDebug());
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<SelectSiteViewModel>();
            // Register other ViewModels and services
        }
    }
}