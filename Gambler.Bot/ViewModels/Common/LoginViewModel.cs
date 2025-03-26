using Avalonia.Threading;
using Gambler.Bot.Classes;
using Gambler.Bot.Core.Events;
using Gambler.Bot.Core.Sites;
using Gambler.Bot.Core.Sites.Classes;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gambler.Bot.ViewModels.Common
{
    public class LoginViewModel: ViewModelBase
    {
        //public Interaction<LoginViewModel, LoginViewModel?> CloseDialog { get; }
        public event EventHandler ChangeSite;
        public Action<bool> LoginFinished;

        public string Title
        {
            get { return $"Log in: {Site.SiteName}"; }            
        }
        private List<string> mirrors;

        public List<string> Mirrors
        {
            get { return mirrors; }
            set { mirrors = value; this.RaisePropertyChanged(); }
        }
        private int selectedMirror;

        public int SelectedMirror
        {
            get { return selectedMirror; }
            set { selectedMirror = value; this.RaisePropertyChanged(); }
        }


        private AutoBet _site;

        public AutoBet Site
        {
            get { return _site; }
            set {
                if (_site!=null)
                {
                    _site.OnSiteLoginFinished -= _site_LoginFinished;
                }
                _site = value; 
                
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(Title));
                if (_site != null)
                {
                    _site.OnSiteLoginFinished += _site_LoginFinished;
                }
            }
             
        }

        private void _site_LoginFinished(object sender, LoginFinishedEventArgs e)
        {
            if (!e.Success)
            {
                foreach(var x in LoginParams)
                {
                    if (x.Param.ClearAfterEnter)
                    {
                        x.Value = null;
                    }
                }
                CanLogIn = true;
                ShowError = true;
                
                
            }
            else
            {                
                foreach (var x in LoginParams)
                {
                    if (x.Param.ClearAfterLogin)
                    {
                        x.Value = null;
                    }
                }
                LoginParams.Clear();
                CanLogIn = true;
                ShowError = false;
                //Cancel();
                Dispatcher.UIThread.Invoke(() => { LoginFinished(true); });

            }

        }

        private ObservableCollection<LoginParamValue> _loginParams;

        public ObservableCollection<LoginParamValue> LoginParams
        {
            get { return _loginParams; }
            set { _loginParams = value; this.RaisePropertyChanged(); }
        }

        private bool _loggingIn=true;

        public bool CanLogIn
        {
            get { return _loggingIn; }
            set { _loggingIn = value; this.RaisePropertyChanged(); this.RaisePropertyChanged(nameof(LoggingIn)); }
        }

        public bool LoggingIn { get => !CanLogIn; }

        private bool showError;

        public bool ShowError
        {
            get { return showError; }
            set { showError = value; this.RaisePropertyChanged(); }
        }


        public LoginViewModel(Microsoft.Extensions.Logging.ILogger logger) : base(logger)
        {
            LoginCommand = ReactiveCommand.Create(LogIn);
            SkipCommand = ReactiveCommand.Create(Skip);
            CancelCommand = ReactiveCommand.Create(Cancel);
            //CloseDialog = new Interaction<LoginViewModel, LoginViewModel?>();

        }


        public LoginViewModel(AutoBet site, ILogger logger): this(logger)
        {
            Site = site;
            LoginParams = new ObservableCollection<LoginParamValue>(site.GetLoginParams());
            SelectedMirror = 0;
        }

        public void RefreshParams()
        {
            LoginParams = new ObservableCollection<LoginParamValue>(Site.GetLoginParams());
            Mirrors = Site.GetCurrentSite()?.Mirrors;
            SelectedMirror = 0;
        }

        public ICommand LoginCommand { get; }

        async Task LogIn()
        {
            if (Site != null)
            {
                CanLogIn = false;
                ShowError = false;
                await Site.Login(Mirrors[SelectedMirror], LoginParams.ToArray());
            }
        }
        public ICommand SkipCommand { get; }

        void Skip()
        {
            //close the dialog and show the bot controls
            //LoginFinished(true);
            //Cancel();
        }
        public ICommand CancelCommand { get; }

        async Task Cancel()
        {
            //close the dialog without changing anything about the display
            //var result = await CloseDialog.Handle(this);
            ChangeSite?.Invoke(this, new EventArgs());
        }
    }
}
