using Microsoft.Extensions.Logging;
using DevExpress.Blazor;
using CommunityToolkit.Maui;

namespace KryGamesBot.MAUI.Blazor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })
                .UseMauiCommunityToolkit(); ;

            builder.Services.AddMauiBlazorWebView();
            

			builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = BootstrapVersion.v5);
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif


            return builder.Build();
        }
    }
}