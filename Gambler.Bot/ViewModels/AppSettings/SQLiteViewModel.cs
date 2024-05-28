using Avalonia.Interactivity;
using Gambler.Bot.Classes;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gambler.Bot.ViewModels.AppSettings
{
    public class SQLiteViewModel: ViewModelBase, iDatabaseForm
    {
        public bool Portable { get; set; }
        private string fileName = "KryGamesBot.db";

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; this.RaisePropertyChanged(); }
        }

        public SQLiteViewModel(ILogger logger):base(logger)
        {
            if (Portable)
                FileName = "KryGamesBot.db";
            else
                FileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\KryGamesBot\\KryGamesBot.db";
        }

        public string ConnectionString()
        {
            return $"Data Source={FileName}";
        }

        public string Provider()
        {
            return "SQLite";
        }

        public bool Validate()
        {
            return FileName != null;
        }

        private void SelectFile(object sender, RoutedEventArgs e)
        {
            /*SaveFileDialog dg = new SaveFileDialog();
            dg.Filter = "Sqlite Databases (*.db)|*.db";
            if (dg.ShowDialog() ?? false)
            {
                FileName = dg.FileName;
            }*/
            //TODO: Implement this
        }
    }
}
