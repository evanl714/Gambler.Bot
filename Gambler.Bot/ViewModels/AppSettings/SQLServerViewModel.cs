using Gambler.Bot.Classes;
using Gambler.Bot.ViewModels;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gambler.Bot.ViewModels.AppSettings
{
    public class SQLServerViewModel:ViewModelBase, iDatabaseForm
    {
        public SQLServerViewModel(ILogger logger):base (logger)
        {
            
        }

        private string dataSource;

        public string DataSource
        {
            get { return dataSource; }
            set { dataSource = value; this.RaisePropertyChanged(nameof(DataSource)); }
        }

        private string dbName;

        public string Database
        {
            get { return dbName; }
            set { dbName = value; this.RaisePropertyChanged(nameof(Database)); }
        }

        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; this.RaisePropertyChanged(nameof(Username)); }
        }

        private string pw;

        public string Pw
        {
            get { return pw; }
            set { pw = value; this.RaisePropertyChanged(nameof(Pw)); }
        }
        public string ConnectionString()
        {
            return $"Data Source={DataSource};Initial Catalog={dbName};User Id={Username};Password={pw};persist security info=true;";
        }

        public virtual string Provider()
        {
            return "MSSQL";
        }

        public bool Validate()
        {
            if (DataSource != null && dbName != null && Username != null && pw != null)
            {
                //attempt to make a connection
                return false;//if not valid
                //else return true;
            }
            return false;

        }


    }

    public class MySqlViewModel : SQLServerViewModel
    {
        public MySqlViewModel(ILogger logger) : base(logger)
        {
        }

        public override string Provider()
        {
            return "MySql";
        }
    }
    public class MongoDBViewModel : SQLServerViewModel
    {
        public MongoDBViewModel(ILogger logger) : base(logger)
        {
        }

        public override string Provider()
        {
            return "MongoDB";
        }
    }
    public class PostGresViewModel : SQLServerViewModel
    {
        public PostGresViewModel(ILogger logger) : base(logger)
        {
        }

        public override string Provider()
        {
            return "PostGres";
        }
    }
}
