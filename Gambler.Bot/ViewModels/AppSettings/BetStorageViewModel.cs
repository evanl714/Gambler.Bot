using Avalonia.Interactivity;
using Gambler.Bot.AutoBet.Helpers;
using Gambler.Bot.Classes;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System.Collections.Generic;

namespace Gambler.Bot.ViewModels.AppSettings
{
    public class BetStorageViewModel: ViewModelBase
    {
        public List<string> Storages { get; set; }
        private PersonalSettings settings;

        public PersonalSettings Settings
        {
            get { return settings; }
            set { settings = value; this.RaisePropertyChanged();
                Password = settings.EncryptConstring ? settings.EncrConnectionString : "";
                SelectedStorageTypeIndex = settings.Provider;
                 
            }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        private string selectedStorageTypeIndex= "SQLite (Default)";

        public string SelectedStorageTypeIndex
        {
            get { return selectedStorageTypeIndex; }
            set { selectedStorageTypeIndex = value; this.RaisePropertyChanged(); SetStorageType(); }
        }

        private void SetStorageType()
        {
            switch (SelectedStorageTypeIndex)
            {
                case "SQLite (Default)": DBVM = new SQLiteViewModel(_logger) ; break;
                case "SQL Server": DBVM = new SQLServerViewModel(_logger); break;
                case "MySQL": DBVM = new MySqlViewModel(_logger); break;
                case "MongoDb": DBVM = new MongoDBViewModel(_logger); break;
                case "PostGres": DBVM = new PostGresViewModel(_logger); break;
                default: DBVM = new SQLiteViewModel(_logger); break;
            }
        }

        private iDatabaseForm dbVM;

        public iDatabaseForm DBVM
        {
            get { return dbVM; }
            set { dbVM = value; this.RaisePropertyChanged(); }
        }


        public BetStorageViewModel(ILogger logger) : base(logger)
        {
            SelectedStorageTypeIndex = "SQLite (Default)";
            Storages = new List<string>() { "SQLite (Default)", "SQL Server", "MySQL", "MongoDb", "PostGres" };
        }

        public void UpdateSettings()
        {
            Settings.Provider = DBVM.Provider();
            Settings.EncryptConstring = !string.IsNullOrWhiteSpace(Password);
            Settings.SetConnectionString(DBVM.ConnectionString(), Password);
        }

        public bool Verify()
        {
            return DBVM.Validate();
        }

        
    }
}
