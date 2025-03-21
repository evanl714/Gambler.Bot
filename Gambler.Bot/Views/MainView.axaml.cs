using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using Avalonia.VisualTree;
using DryIoc;
using Gambler.Bot.Core.Events;
using Gambler.Bot.Core.Helpers;
using Gambler.Bot.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WebViewCore.Models;

namespace Gambler.Bot.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    private Window parentWindow;
    static MainView _instance;
    bool subscribed = false;
    string cookievalue = "";
    public MainView()
    {
        InitializeComponent();
        _instance=this;
        wvBypass.NavigationCompleted += WvBypass_NavigationCompleted;
        wvBypass.WebMessageReceived += WvBypass_WebMessageReceived;
        wvBypass.Loaded += WvBypass_Loaded;
        wvBypass.WebViewCreated += WvBypass_WebViewCreated;
        //PART_WebView.
        this.AttachedToVisualTree += OnAttachedToVisualTree;
        this.DetachedFromVisualTree += OnDetachedFromVisualTree;
        
    }
    Dictionary<string,Cookie> cookies;
    private void WvBypass_WebViewCreated(object? sender, WebViewCore.Events.WebViewCreatedEventArgs e)
    {
        if (!subscribed)
        {
            var obj = wvBypass.PlatformWebView.PlatformViewContext;
            var type2 = obj.GetType();
            var tmp2 = wvBypass.PlatformWebView.GetType();

            if (tmp2.FullName == "Avalonia.WebView.Windows.Core.WebView2Core")
            {
                var properties = tmp2.GetProperties();
                object CoreWebView2 = tmp2.GetProperty("CoreWebView2").GetValue(wvBypass.PlatformWebView);
                Assembly ass = CoreWebView2.GetType().GetAssembly();
                Type[] enumTypes = ass.GetTypes();
                Type enumType = enumTypes.FirstOrDefault(x => x.Name == "CoreWebView2WebResourceContext");
                var filter = CoreWebView2.GetType().GetMethod("AddWebResourceRequestedFilter", new Type[] { typeof(string), enumType });
                object enumValue = Enum.Parse(enumType, "All");
                filter.Invoke(CoreWebView2, new object[] { "*", enumValue });
                var evnt = CoreWebView2.GetType().GetEvent("WebResourceRequested");
                var evnt2 = CoreWebView2.GetType().GetEvent("WebResourceResponseReceived");
                var evnts = CoreWebView2.GetType().GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                
                MethodInfo method = this.GetType().GetMethod("webrequestreceived", BindingFlags.Instance | BindingFlags.NonPublic);

                Delegate handler = Delegate.CreateDelegate(evnt.EventHandlerType, this, method);
                Delegate handler2 = Delegate.CreateDelegate(evnt2.EventHandlerType, this, method);


                evnt.AddEventHandler(CoreWebView2, handler);
                evnt2.AddEventHandler(CoreWebView2, handler2);
            }
        }
    }

    private void WvBypass_Loaded(object? sender, RoutedEventArgs e)
    {
       
    }

    private void webrequestreceived(object sender, object e)
    {
        if (args==null)
            return;
        Type argsType = e.GetType();
        try
        {
            string log = JsonSerializer.Serialize(e);
            MainViewModel.log.LogDebug(log);
        }
        catch (Exception ex)
        {

        }
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
                        System.Net.Cookie cookie = new System.Net.Cookie(curCookie[0], curCookie[1], "/", "." + new Uri(requesturi).Host);
                        cookie.HttpOnly = true;
                        cookie.Secure = true;

                        cookies[curCookie[0]] = (cookie);
                        if (curCookie[0].ToLower() == args.RequiredCookie.ToLower())
                        {
                            found = true;
                        }
                    }
                }
                if (requestCookies.ToLower().Contains(args.RequiredCookie.ToLower()))
                    cookievalue = requestCookies;
                if (found && status==200 && (DateTime.Now - startDate).TotalSeconds>=5 )
                    CheckCookies();
            }
        }
        if (argsType.Name == "CoreWebView2WebResourceRequestedEventArgs")
        {
           
        }
    }

    private void WvBypass_WebMessageReceived(object? sender, WebViewCore.Events.WebViewMessageReceivedEventArgs e)
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
    private async void WvBypass_NavigationCompleted(object? sender, WebViewCore.Events.WebViewUrlLoadedEventArg e)
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
            agent = await _instance.wvBypass.ExecuteScriptAsync("navigator.userAgent");
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
    async Task CheckCookies()
    {
        if (args == null)
            return;
        var bc = new BrowserConfig();
        CookieContainer cs = new CookieContainer();
        bool found = false;
        try
        {

            await GetAgent();
            string result = agent;

            var tmp = await wvBypass.PlatformWebView.ExecuteScriptAsync("document.cookie");
            var obj = wvBypass.PlatformWebView.PlatformViewContext;
            var type2 = obj.GetType();
            var tmp2 = wvBypass.PlatformWebView.GetType();
            Uri uri = new Uri(args.URL);
            if (tmp2.FullName == "Avalonia.WebView.Windows.Core.WebView2Core")
            {

                var properties = tmp2.GetProperties();
                object CoreWebView2 = tmp2.GetProperty("CoreWebView2").GetValue(wvBypass.PlatformWebView);
                properties = CoreWebView2.GetType().GetProperties();
                object CookieMan = CoreWebView2.GetType().GetProperty("CookieManager").GetValue(CoreWebView2);
                var method = CookieMan.GetType().GetMethod("GetCookiesAsync");//.Invoke(CookieMan, null);
                var cookies = await method.InvokeAsync(CookieMan, new object[] { uri.IdnHost });
                var cookies2 = await method.InvokeAsync(CookieMan, new object[] { uri.Host.Replace("/www","/.www") });

                foreach (object c in cookies as IList)
                {   
                    System.Net.Cookie svalue = (System.Net.Cookie)c.GetType().GetMethod("ToSystemNetCookie").Invoke(c, null);
                    this.cookies[svalue.Name] = svalue;
                   
                }
                foreach (var x in this.cookies.Keys)
                {
                    if (x== args.RequiredCookie)
                    {
                        found = true;                        
                    }
                    cs.Add(this.cookies[x]);
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
            wvBypass.IsVisible = false;
        }
        if (found || cts.IsCancellationRequested)
            _conf = new BrowserConfig { Cookies = cs, UserAgent = agent };
    }

    static CancellationTokenSource cts;

    static BypassRequiredArgs args = null;
    internal void internalGetBypass(BypassRequiredArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            cookies = new Dictionary<string, Cookie>();
            wvBypass.ZIndex = -1;
            wvBypass.IsVisible = true;
            wvBypass.Url = new Uri(e.URL);
            await Task.Delay(15000, cts.Token);
            if (_conf == null)
            {
                cts.Cancel();
                await CheckCookies();
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