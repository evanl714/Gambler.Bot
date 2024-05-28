using DevExpress.Xpf.LayoutControl;
using Gambler.Bot.AutoBet.Helpers;
using KryGamesBotControls.Common.DBSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for DatabaseSetup.xaml
    /// </summary>
    public partial class DatabaseSetup : UserControl, INotifyPropertyChanged
    {
        public PersonalSettings Settings { get; set; }
        public iDatabaseForm dbForm { get; set; }
        public DatabaseSetup()
        {
            dbForm = new SqlIte();
            InitializeComponent();
            this.DataContext = this;
            txtdbIntro.Text = @"KryGamesBot stores bets made through it and your session stats in a database so that it can show you pretty graphs and keep track of your betting progress. To be able to do this, you need to set up a databasse for it to store to.
                
If you're not sure what to enter here, it's recommended you use the default settings and continue.";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ComboBoxEdit_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            switch(cbeProvider.SelectedIndex)
            {
                case 0: dbForm = new SqlIte(); break;
                case 1: dbForm = new SqlServer(); break;
                case 2: dbForm = new DBSetup.MySql(); break;
                case 3: dbForm = new DBSetup.MongoDB(); break;
                case 4: dbForm = new PostGres(); break;
                default: dbForm = new SqlIte(); break;
            }
            OnPropertyChanged("dbForm");
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {

        }

        public void UpdateSettings()
        {
            Settings.Provider = dbForm.Provider();
            Settings.EncryptConstring = txtDBPassword.EditValue != null;
            Settings.SetConnectionString(dbForm.ConnectionString(), txtDBPassword.Text);
        }

        public bool Verify()
        {
            return dbForm.Validate();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Settings!=null)
            {
                cbeProvider.EditValue = Settings.Provider;
                
            }
        }
    }
}
