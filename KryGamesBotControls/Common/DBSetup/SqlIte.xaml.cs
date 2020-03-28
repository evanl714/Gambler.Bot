using DevExpress.Xpf.Dialogs;
using Microsoft.Win32;
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

namespace KryGamesBotControls.Common.DBSetup
{
    /// <summary>
    /// Interaction logic for SqlIte.xaml
    /// </summary>
    public partial class SqlIte : BaseControl, iDatabaseForm
    {
        private string fileName="KryGamesBot.db";

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; OnPropertyChanged(nameof(FileName)); }
        }

        public SqlIte()
        {
            InitializeComponent();
            DataContext = this;
            FileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\KryGamesBot.db";
        }

        public string ConnectionString()
        {
            return $"Data Source={FileName};Version=3;Compress=True;";
        }

        public string Provider()
        {
            return "SQLite";
        }

        public bool Validate()
        {
            return FileName != null;
        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            DXSaveFileDialog dg = new DXSaveFileDialog();
            dg.Filter = "Sqlite Databases (*.db)|*.db";
            if (dg.ShowDialog()??false)
            {
                FileName = dg.FileName;
            }
        }
    }
}
