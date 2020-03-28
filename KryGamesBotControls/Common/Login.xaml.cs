using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;
using DoormatCore.Sites;
using DoormatCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static DoormatCore.Sites.BaseSite;

namespace KryGamesBotControls.Common
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public event EventHandler<LoginEventArgs> OnLogin;
        public event EventHandler OnSkip;
        public event EventHandler OnBack;

        private BaseSite currentsite;

        public BaseSite CurrentSite
        {
            get { return currentsite; }
            set { currentsite = value; CreateLoginControls(); }
        }

        Dictionary<BaseEdit, LoginParameter> logincontrols = new Dictionary<BaseEdit, LoginParameter>();
        private void CreateLoginControls()
        {
            lciError.Visibility =  Visibility.Hidden;
            if (currentsite != null)
            {
                logincontrols.Clear();
                mainPnl.Children.Clear();
                
                foreach (var x in currentsite.LoginParams)
                {
                    LayoutItem tmpPanel = new LayoutItem();
                    tmpPanel.Label = x.Name;
                    if (x.Required)
                        tmpPanel.Label += "*";
                    BaseEdit Input = null;
                    if (x.Masked)
                    {
                        Input = new PasswordBoxEdit();
                    }
                    else
                    {
                        Input = new TextEdit();
                    }
                    logincontrols.Add(Input, x);
                    
                    Input.MinWidth = 200;
                    Input.MaxWidth = 200;
                    tmpPanel.Content = Input;
                    mainPnl.Children.Add(tmpPanel);
                }
            }
        }

        public Login()
        {
            InitializeComponent();
        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            OnSkip?.Invoke(this, new EventArgs());
        }

        public void LoginFailed()
        {
            if (!Dispatcher.CheckAccess())
                Dispatcher.Invoke(LoginFailed);
            else
            {
                //hide login adnimation and show failure text
                lblError.Content = "Could not log in.";
                lciError.Visibility = Visibility.Visible;
            }
            waitind.Visibility = Visibility.Collapsed;
        }

        public void LoginSucceeded()
        {
            foreach (BaseEdit x in logincontrols.Keys)
            {
                LoginParameter tmp = logincontrols[x];
                if (tmp.ClearAfterLogin)
                {
                    //set required label
                    x.EditValue = null;
                }
            }
            waitind.Visibility = Visibility.Collapsed;
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            waitind.Visibility = Visibility.Visible;
            //show login animation
            LoginEventArgs arg = new LoginEventArgs();
            foreach (BaseEdit x in logincontrols.Keys)
            {
                LoginParameter tmp = logincontrols[x];
                if (tmp.Required && x.EditValue==null)
                {
                    //set required label
                    lciError.Visibility = Visibility.Visible;
                    lblError.Content = tmp.Name + " is required.";
                    return;
                }
                
                arg.Values.Add(new LoginParamValue {  Param = tmp, Value = x.EditValue?.ToString() ?? "" }) ;
                if (tmp.ClearAfterEnter)
                    x.EditValue = null;
                

            }
            lblError.Visibility = Visibility.Hidden;
            OnLogin?.Invoke(this, arg);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            OnBack?.Invoke(this, new EventArgs());
        }
    }
    public class LoginEventArgs:EventArgs
    {
        public List<LoginParamValue> Values { get; set; } = new List<LoginParamValue>();
    }
}
