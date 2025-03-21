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

public override void OnFrameworkInitializationCompleted()
{
var services = new ServiceCollection();
ConfigureServices(services);

ServiceProvider = services.BuildServiceProvider();
if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
{
//var logger = ServiceProvider.GetService<ILogger<MainWindowViewModel>>();
        desktop.MainWindow = new MainWindow
        {
        DataContext = ServiceProvider.GetService< MainWindowViewModel>(),
            };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
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
                IconProvider.Current
                .Register<MaterialDesignIconProvider>();
services.AddLogging(configure => configure.AddConsole().SetMinimumLevel(LogLevel.Debug).AddDebug());


                        services.AddTransient<MainWindowViewModel>();
                            services.AddTransient<MainViewModel>();
                                services.AddTransient<SelectSiteViewModel>();
                                    // Register other ViewModels and services
                                    }
                                    }
                                    }