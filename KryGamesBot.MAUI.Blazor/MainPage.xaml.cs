using Gambler.Bot.Core.Helpers;
using System.Net;

namespace KryGamesBot.MAUI.Blazor
{
    public partial class MainPage : ContentPage
    {
        static MainPage instance;
        public MainPage()
        {
            instance = this;
            InitializeComponent();
            wvBypass.Navigated += WvBypass_Navigated;
        }

        private void WvBypass_Navigated(object sender, WebNavigatedEventArgs e)
        {
            //listen here for navigation events to finish the request
            GetAgent();
        }

        internal static void SetBypass(string URL)
        {
            if (instance.Dispatcher.IsDispatchRequired)
            {
                instance.Dispatcher.DispatchAsync(() =>
                {
                    //if (instance.wvBypass.Cookies == null)
                    instance.wvBypass.Cookies = new CookieContainer();
                    if (instance.wvBypass.Source.ToString() != URL)
                        instance.wvBypass.Source = URL;
                    else
                        instance.wvBypass.Reload();
                    
                    instance.wvBypass.ZIndex = 10;
                });
                return;
            }
            instance.wvBypass.Source = URL;
        }
        static string agent = "";
        static async Task GetAgent()
        {
            if (string.IsNullOrWhiteSpace(agent))
            {
                agent = await instance.wvBypass.EvaluateJavaScriptAsync("navigator.userAgent");
                if (agent.StartsWith("\\"))
                    agent = agent.Substring(1);
                if (agent.EndsWith("\\"))
                    agent = agent.Substring(0, agent.Length - 1);
                if (agent.StartsWith("\""))
                    agent = agent.Substring(1);
            }
        }
        internal static BrowserConfig GetBypass()
        {
            if (instance.Dispatcher.IsDispatchRequired)
            {
                return instance.Dispatcher.DispatchAsync<BrowserConfig>(async () =>
                {
                    try
                    {
                        instance.wvBypass.ZIndex = -1;
                        while (agent == "")
                            Thread.Sleep(10);
                        string result = agent;
                        return new BrowserConfig { Cookies = instance.wvBypass.Cookies, UserAgent = agent };
                    }
                    catch (Exception e)
                    {

                    }
                    return new BrowserConfig { Cookies = instance.wvBypass.Cookies, UserAgent = agent };
                }).Result;
                
            }
            
            instance.wvBypass.ZIndex = -1;

            return new BrowserConfig { Cookies = instance.wvBypass.Cookies, UserAgent = agent };
        }
        protected override void OnHandlerChanged()
        {/*
            base.OnHandlerChanged();
#if WINDOWS
             var webview = wvBypass.Handler.PlatformView as Microsoft.Maui.Platform.MauiWebView;
             var cookie = webview.CoreWebView2?.CookieManager.CreateCookie("test","","","");
            
             webview.CoreWebView2?.CookieManager.AddOrUpdateCookie(cookie);
             webview.CoreWebView2?.CookieManager.GetCookiesAsync("https://primedice.com");

#elif ANDROID
    var webview = wvBypass.Handler.PlatformView as Android.Webkit.WebView;
    Android.Webkit.CookieManager.Instance.SetAcceptCookie(true);
    Android.Webkit.CookieManager.Instance.SetAcceptThirdPartyCookies(webview,true);
#endif*/
        }
    }
}