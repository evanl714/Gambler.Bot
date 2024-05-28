using Gambler.Bot.AutoBet.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace KryGamesBotControls.Common
{
    /// <summary>
    /// Interaction logic for KeePassSettings.xaml
    /// </summary>
    public partial class KeePassSettings : BaseControl
    {
        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; OnPropertyChanged(nameof(FileName)); }
        }
        bool CouldOpen = false;
        public KeePassSettings()
        {
            InitializeComponent();
        }

        private void txtFileName_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            
            if (File.Exists(FileName))
            {

                CouldOpen = false;
//show a password dialog
//attempt to open the file using keepass
//if cannot open, try password again
            }
        }

        public void UpdateSettings(PersonalSettings settings)
        {
            if (CouldOpen && File.Exists(FileName))
            {
                settings.KeepassDatabase = FileName;
            }
        }
    }
}
