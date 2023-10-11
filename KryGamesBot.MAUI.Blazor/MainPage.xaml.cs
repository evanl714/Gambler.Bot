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
            
        }

        internal static void SetBypass(string URL)
        {
            if (instance.Dispatcher.IsDispatchRequired)
            {
                instance.Dispatcher.DispatchAsync(() =>
                {
                    if (instance.wvBypass.Cookies == null)
                        instance.wvBypass.Cookies = new CookieContainer();
                    if (instance.wvBypass.Source.ToString() != URL)
                        instance.wvBypass.Source = URL;
                    else
                        instance.wvBypass.Reload();
                    instance.ZIndex = 10;
                });
                return;
            }
            instance.wvBypass.Source = URL;
        }
        internal static CookieContainer GetBypass()
        {
            if (instance.Dispatcher.IsDispatchRequired)
            {
                
                return instance.Dispatcher.DispatchAsync<CookieContainer>(() =>
                {
                    instance.ZIndex = -1;
                    return  instance.wvBypass.Cookies;
                }).Result;
                
            }
            return instance.wvBypass.Cookies;
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