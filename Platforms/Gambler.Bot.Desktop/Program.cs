using System;
using Avalonia;
using Avalonia.ReactiveUI;
using Gambler.Bot;
using Avalonia.WebView.Desktop;
using Microsoft.Extensions.Configuration;
using Gambler.Bot.Classes;
using Projektanker.Icons.Avalonia.MaterialDesign;
using Projektanker.Icons.Avalonia;
using Velopack;

namespace Gambler.Bot.Desktop;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            // It's important to Run() the VelopackApp as early as possible in app startup.
            VelopackApp.Build()
                .Run();

            // Now it's time to run Avalonia
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

        }
        catch (Exception ex)
        {
            string message = "Unhandled exception: " + ex.ToString();
            Console.WriteLine(message);
            throw;
        }

    }

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
