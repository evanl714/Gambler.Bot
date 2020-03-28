using DevExpress.Xpf.Core;
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
using System.Windows.Shapes;

namespace KryGamesBotControls
{
    /// <summary>
    /// Interaction logic for GetPasswordDialog.xaml
    /// </summary>
    public partial class GetPasswordDialog : DXDialog
    {
        

        public string Pw
        {
            get;
            set;
        }

        public GetPasswordDialog()
        {
            InitializeComponent();
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            Pw = pwbPass.EditValue?.ToString();
            this.DialogResult = true;
            this.Close();
        }
    }
}
