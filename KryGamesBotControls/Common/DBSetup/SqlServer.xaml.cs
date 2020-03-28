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
    /// Interaction logic for SqlServer.xaml
    /// </summary>
    public partial class SqlServer : BaseControl, iDatabaseForm
    {
        private string dataSource;

        public string DataSource
        {
            get { return dataSource; }
            set { dataSource = value; OnPropertyChanged(nameof(DataSource)); }
        }

        private string dbName;

        public string DbName
        {
            get { return dbName; }
            set { dbName = value; OnPropertyChanged(nameof(DbName)); }
        }

        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged(nameof(Username)); }
        }

        private string pw;

        public string Pw
        {
            get { return pw; }
            set { pw = value; OnPropertyChanged(nameof(Pw)); }
        }

        public SqlServer()
        {
            InitializeComponent();
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
            if (txtDataSource.EditValue!=null && txtInitialCatalog.EditValue!=null && txtPassword.Text!=null && txtUsername.Text!=null)
            {
                //attempt to make a connection
                return false;//if not valid
                //else return true;
            }
            return false;
            
        }
    }

    public class MySql: SqlServer
    {
        public override string Provider()
        {
            return "MySql";
        }
    }
    public class MongoDB : SqlServer
    {
        public override string Provider()
        {
            return "MongoDB";
        }
    }
    public class PostGres : SqlServer
    {
        public override string Provider()
        {
            return "PostGres";
        }
    }
}
