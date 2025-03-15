using System;
using Avalonia;
using Avalonia.ReactiveUI;
using Gambler.Bot;
using Avalonia.WebView.Desktop;
using Microsoft.Extensions.Configuration;
using Gambler.Bot.Classes;
using Projektanker.Icons.Avalonia.MaterialDesign;
using Projektanker.Icons.Avalonia;

namespace Gambler.Bot.Desktop;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        var config = new ConfigurationBuilder()
           .AddUserSecrets<Program>()           
           .Build();
        
        return  AppBuilder.Configure<App>()
                .RegisterActiproLicense(config.GetValue<string>("ActiproLicense:Licensee"), config.GetValue<string>("ActiproLicense:LisenceKey"))
              .UsePlatformDetect()
              .LogToTrace(Avalonia.Logging.LogEventLevel.Debug)
              .WithInterFont()
              .LogToTrace()
              .UseReactiveUI()
              .UseDesktopWebView()
              
              ;
    }
}
