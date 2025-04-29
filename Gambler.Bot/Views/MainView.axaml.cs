using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Gambler.Bot.Core.Events;
using Gambler.Bot.Core.Helpers;
using Gambler.Bot.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Gambler.Bot.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    private Window parentWindow;
    static MainView _instance;
    bool subscribed = false;
    string cookievalue = "";
    static DateTime LastRequest = DateTime.Now;
    Timer timer;
    public NativeWebView wvBypass { get; set; }
    NativeWebDialog dialog = null;
    public MainView()
    {
        InitializeComponent();
        _instance=this;
        if (!OperatingSystem.IsLinux())
        {
            wvBypass = new NativeWebView();
            wvcontainer.Children.Add(wvBypass);
            wvBypass.NavigationCompleted += WvBypass_NavigationCompleted;
            wvBypass.WebMessageReceived += WvBypass_WebMessageReceived; ;
            wvBypass.Loaded += WvBypass_Loaded;
        }
        
        
        //wvBypass.WebViewCreated += WvBypass_WebViewCreated;
        //PART_WebView.
        this.AttachedToVisualTree += OnAttachedToVisualTree;
        this.DetachedFromVisualTree += OnDetachedFromVisualTree;
        timer = new Timer(cookietmrCallback, null, 1000, 1000);
        
    }

    

    private void cookietmrCallback(object? state)
    {
        if (args != null)
        {
            if ((DateTime.Now - LastRequest).TotalSeconds >= 2)
            {
                LastRequest = DateTime.Now;
                if (OperatingSystem.IsLinux() && dialog ==null)
                {
                    return;
                }
                
                CheckCookies();
            }
        }
    }

    Dictionary<string,Cookie> cookies;
    //private void WvBypass_WebViewCreated(object? sender, WebViewNavigationCompletedEventArgs e)
    //{
    //    if (!subscribed)
    //    {
    //        var obj = wvBypass.PlatformWebView.PlatformViewContext;
    //        var type2 = obj.GetType();
    //        var tmp2 = wvBypass.PlatformWebView.GetType();

    //        if (tmp2.FullName == "Avalonia.WebView.Windows.Core.WebView2Core")
    //        {
    //            var properties = tmp2.GetProperties();
    //            object CoreWebView2 = tmp2.GetProperty("CoreWebView2").GetValue(wvBypass.PlatformWebView);
    //            Assembly ass = CoreWebView2.GetType().GetAssembly();
    //            Type[] enumTypes = ass.GetTypes();
    //            Type enumType = enumTypes.FirstOrDefault(x => x.Name == "CoreWebView2WebResourceContext");
    //            var filter = CoreWebView2.GetType().GetMethod("AddWebResourceRequestedFilter", new Type[] { typeof(string), enumType });
    //            object enumValue = Enum.Parse(enumType, "All");
    //            filter.Invoke(CoreWebView2, new object[] { "*", enumValue });
    //            var evnt = CoreWebView2.GetType().GetEvent("WebResourceRequested");
    //            var evnt2 = CoreWebView2.GetType().GetEvent("WebResourceResponseReceived");
    //            var evnts = CoreWebView2.GetType().GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                
    //            MethodInfo method = this.GetType().GetMethod("webrequestreceived", BindingFlags.Instance | BindingFlags.NonPublic);

    //            Delegate handler = Delegate.CreateDelegate(evnt.EventHandlerType, this, method);
    //            Delegate handler2 = Delegate.CreateDelegate(evnt2.EventHandlerType, this, method);


    //            evnt.AddEventHandler(CoreWebView2, handler);
    //            evnt2.AddEventHandler(CoreWebView2, handler2);
    //        }
    //    }
    //}

    private void WvBypass_Loaded(object? sender, RoutedEventArgs e)
    {
       
    }

    private void webrequestreceived(object sender, object e)
    {
        if (args==null)
            return;
        Type argsType = e.GetType();
        //DO NOT UNCOMMENT THIS
        //THIS HAS A HIGH POSSIBILITY OF LEAKING YOUR TOKENS
        //try
        //{
        //    string log = JsonSerializer.Serialize(e);
        //    MainViewModel.log.LogDebug(log);
        //}
        //catch (Exception ex)
        //{

        //}
        LastRequest = DateTime.Now;
        if (argsType.Name== "CoreWebView2WebResourceResponseReceivedEventArgs")
        {
            
            
            object request = argsType.GetProperty("Request").GetValue(e);
            object response = argsType.GetProperty("Response").GetValue(e);
            int status = (int)response.GetType().GetProperty("StatusCode").GetValue(response);
            string requesturi = request.GetType().GetProperty("Uri").GetValue(request) as string;
            object requestheaders = request.GetType().GetProperty("Headers").GetValue(request);
            bool hascookie = (bool)requestheaders.GetType().GetMethod("Contains").Invoke(requestheaders, new object[] { "cookie" });
            if (hascookie)
            {
                string requestCookies = requestheaders.GetType().GetMethod("GetHeader").Invoke(requestheaders, new object[] { "cookie" }) as string;
                string[] tmpCookies = cookievalue.Split(";", StringSplitOptions.TrimEntries);
                bool found = false;
                foreach (var x in tmpCookies)
                {
                    string[] curCookie = x.Split("=");
                    if (curCookie.Length == 2)
                    {
                        try
                        {
                            System.Net.Cookie cookie = new System.Net.Cookie(curCookie[0], curCookie[1], "/", "." + new Uri(requesturi).Host);
                            cookie.HttpOnly = true;
                            cookie.Secure = true;
                            cookies[curCookie[0]] = (cookie);                           
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                if (requestCookies.ToLower().Contains(args.RequiredCookie.ToLower()))
                    cookievalue = requestCookies;
                
            }
        }
        if (argsType.Name == "CoreWebView2WebResourceRequestedEventArgs")
        {
           
        }
    }

    private void WvBypass_WebMessageReceived(object? sender, WebMessageReceivedEventArgs e)
    {
        
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
    private async void WvBypass_NavigationCompleted(object? sender, WebViewNavigationCompletedEventArgs e)
    {
        //Navigation happened here, check for cookies again.
       
        //if (e.IsSuccess)
        //{
            
        //    await CheckCookies();
        //}
    }

    public async void ClickHandler(object sender, RoutedEventArgs args)
    {
        /*var tmp = await wvBypass.PlatformWebView.ExecuteScriptAsync("document.cookie");
        var obj = wvBypass.PlatformWebView.PlatformViewContext;
        var type2 = obj.GetType();
        var tmp2 = wvBypass.PlatformWebView.GetType();
        if (tmp2.FullName=="Avalonia.WebView.Windows.Core.WebView2Core")
        {
            
            var properties = tmp2.GetProperties();
            object CoreWebView2 = tmp2.GetProperty("CoreWebView2").GetValue(wvBypass.PlatformWebView);
            properties = CoreWebView2.GetType().GetProperties();
            object CookieMan = CoreWebView2.GetType().GetProperty("CookieManager").GetValue(CoreWebView2);
            var method = CookieMan.GetType().GetMethod("GetCookiesAsync");//.Invoke(CookieMan, null);
            var result =  await method.InvokeAsync(CookieMan, new object[] { "https://primedice.com" });
            foreach (object c in result as IList)
            {
                System.Net.Cookie svalue = (System.Net.Cookie)c.GetType().GetMethod("ToSystemNetCookie").Invoke(c,null);
            }
        }

        string[] lines = tmp.Split(";");
        //message.Text = "Button clicked!";*/
        

    }

    static string agent = "";
    static async Task GetAgent()
    {
        if (string.IsNullOrWhiteSpace(agent))
        {
            if (OperatingSystem.IsLinux())
            {
                agent = await _instance.dialog.InvokeScript("navigator.userAgent");                
            }
            else
            {
                agent = await _instance.wvBypass.InvokeScript("navigator.userAgent");
            }
            if (agent == null)
                return;
            if (agent.StartsWith("\\"))
                agent = agent.Substring(1);
            if (agent.EndsWith("\\"))
                agent = agent.Substring(0, agent.Length - 1);
            if (agent.StartsWith("\""))
                agent = agent.Substring(1);
            if (agent.EndsWith("\""))
                agent = agent.Substring(0, agent.Length - 1);
        }
    }

    static BrowserConfig _conf = null;
    bool checking = false;


    async Task CheckCookies()
    {
        if (checking)
            return;
        checking = true;
        if (args == null || _conf != null)
            return;
        var bc = new BrowserConfig();
        CookieContainer cs = new CookieContainer();
        Uri cururi = new Uri(args.URL);
        bool found = false;
        try
        {

            await GetAgent();
            string result = agent;
            NativeWebViewCookieManager Cookiemanager = null;
            if (OperatingSystem.IsLinux())
            {
                Cookiemanager = dialog.TryGetCookieManager();
            }
            else
            {
                Cookiemanager = wvBypass.TryGetCookieManager();
            }
            
            if (Cookiemanager == null)
                return;
            var cookies = await Cookiemanager.GetCookiesAsync();
            foreach (var cookie in cookies)
            {
                try
                {
                    if (cookie.Domain.ToLower().Contains(cururi.Host.ToLower()))
                    { 
                        cs.Add(cookie);
                        if (cookie.Name == args.RequiredCookie)
                        {
                            found = true;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                
            }

            bc.UserAgent = agent;
            bc.Cookies = cs;
            if (found || cts.IsCancellationRequested)
                _conf = bc;
        }
        catch (Exception e)
        {

        }
        finally
        {
            checking = false;
        }
    }

    static CancellationTokenSource cts;

    static BypassRequiredArgs args = null;
    internal void internalGetBypass(BypassRequiredArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            try
            {
                cookies = new Dictionary<string, Cookie>();
                
                lblDisclaimer.IsVisible = true;
                lblDisclaimer.ZIndex =- 2;

                if (OperatingSystem.IsLinux())
                {
                    var url = new Uri(e.URL);
                    dialog = new NativeWebDialog
                    {
                        Source = url
                    };
                    dialog.Resize(400, 500);
                    dialog.Show();
                    try
                    {
                        await Task.Delay(15000, cts.Token);
                    }
                    catch (Exception ex)
                    {

                    }
                    if (_conf == null)
                    {
                        cts.Cancel();
                        await CheckCookies();
                    }
                    dialog.Close();
                    lblDisclaimer.IsVisible = false;
                    
                }
                else
                {
                    wvcontainer.ZIndex = -1;
                    wvcontainer.IsVisible = true;
                    wvBypass.Navigate(new Uri(e.URL));
                    try
                    {
                        await Task.Delay(15000, cts.Token);
                    }
                    catch (Exception ex)
                    {

                    }
                    if (_conf == null)
                    {
                        cts.Cancel();
                        await CheckCookies();
                    }
                    wvBypass.Navigate(new Uri("about:blank"));
                    wvcontainer.IsVisible = false;
                    lblDisclaimer.IsVisible = false;
                }
            }
            catch (Exception ex)
            {

            }
        });
    }
    static DateTime startDate = default;
    internal static BrowserConfig GetBypass(BypassRequiredArgs e)
    {
        cts = new CancellationTokenSource();
        _conf = null;
        args = e;
        startDate = DateTime.Now;
        _instance.internalGetBypass(e);
        LastRequest = DateTime.Now;
        while (_conf == null) { Thread.Sleep(100); }
        args = null;
        cts.Cancel();
        return _conf;
    }
}
public static class ExtensionMethods
{
    public static async Task<object> InvokeAsync(this MethodInfo @this, object obj, params object[] parameters)
    {
        var task = (Task)@this.Invoke(obj, parameters);
        await task.ConfigureAwait(false);
        var resultProperty = task.GetType().GetProperty("Result");
        return resultProperty.GetValue(task);
    }
}