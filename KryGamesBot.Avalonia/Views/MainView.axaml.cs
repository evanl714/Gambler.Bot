using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using DoormatBot;
using DoormatCore.Helpers;
using DoormatCore.Sites;
using DryIoc;
using Esprima.Ast;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace KryGamesBot.Ava.Views;

public partial class MainView : UserControl
{
    static MainView _instance;
    public MainView()
    {
        InitializeComponent();
        _instance=this;
        //PART_WebView.
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

    internal void internalGetBypass(BypassRequiredArgs e)
    {
       
        
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            wvBypass.ZIndex = -1;
            wvBypass.IsVisible = true;
            wvBypass.Url = new Uri(e.URL);
            await Task.Delay(5000);
                var bc = new BrowserConfig();
                CookieContainer cs = new CookieContainer();
                try
                {
                    
                    await GetAgent();
                    string result = agent;
                   
                    var tmp = await wvBypass.PlatformWebView.ExecuteScriptAsync("document.cookie");
                    var obj = wvBypass.PlatformWebView.PlatformViewContext;
                    var type2 = obj.GetType();
                    var tmp2 = wvBypass.PlatformWebView.GetType();
                    if (tmp2.FullName == "Avalonia.WebView.Windows.Core.WebView2Core")
                    {

                        var properties = tmp2.GetProperties();
                        object CoreWebView2 = tmp2.GetProperty("CoreWebView2").GetValue(wvBypass.PlatformWebView);
                        properties = CoreWebView2.GetType().GetProperties();
                        object CookieMan = CoreWebView2.GetType().GetProperty("CookieManager").GetValue(CoreWebView2);
                        var method = CookieMan.GetType().GetMethod("GetCookiesAsync");//.Invoke(CookieMan, null);
                        var cookies = await method.InvokeAsync(CookieMan, new object[] { e.URL });
                        foreach (object c in cookies as IList)
                        {
                            System.Net.Cookie svalue = (System.Net.Cookie)c.GetType().GetMethod("ToSystemNetCookie").Invoke(c, null);
                            cs.Add(svalue);
                        }
                    }
                    bc.UserAgent = agent;
                    bc.Cookies = cs;
                    _conf= bc;
                }
                catch (Exception e)
                {

                }
                finally
                {
                    wvBypass.IsVisible = false;
                }
                _conf = new BrowserConfig { Cookies = cs, UserAgent = agent };
            });

        

       /* wvBypass.ZIndex = -1;

        return new BrowserConfig { Cookies = wvBypass.Cookies, UserAgent = agent };*/
    }

    internal static BrowserConfig GetBypass(BypassRequiredArgs e)
    {
        _conf = null;
        _instance.internalGetBypass(e);
        while (_conf == null) { Thread.Sleep(100); }
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