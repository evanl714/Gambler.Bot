using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KryGamesBot.Avalonia.ViewModels.Common
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
                
        
            LoginCommand = ReactiveCommand.Create<object>(LogIn);
            SkipCommand = ReactiveCommand.Create<object>(Skip);
            CancelCommand = ReactiveCommand.Create<object>(Cancel);
            
            
        }


    public ICommand LoginCommand { get; }

    void LogIn(object site)
    {
        if (site is AvaSitesList Site)
        {
            /*BypassLogIn = false;
            SelectedSiteChanged.Invoke(this, Site.Site);*/
        }
    }
    public ICommand SkipCommand { get; }

    void Skip(object site)
    {
        if (site is AvaSitesList Site)
        {
            
        }
    }
    public ICommand CancelCommand { get; }

    void Cancel(object site)
    {
        if (site is AvaSitesList Site)
        {
            
        }
    }
}
}
