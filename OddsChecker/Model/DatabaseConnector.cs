using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;

namespace OddsChecker
{
    public class DatabaseWorker : INotifyPropertyChanged
    {
        private String userid;

        public String Userid
        {
            get { return userid; }
            set { userid = value;
            OnPropertyChanged("Userid");

            }
        }

        private String password;

        public String Password
        {
            get { return password; }
            set { password = value;
            OnPropertyChanged("Password");
            }
        }

        private String server;

        public String Server
        {
            get { return server; }
            set { server = value;
            OnPropertyChanged("Server");
            }
        }

        private String database;

        public String Database
        {
            get { return database; }
            set { database = value;
            OnPropertyChanged("Database");
            }
        }

        private SqlConnection connection;

        public SqlConnection Connection
        {
            get { return connection; }
            set { connection = value;
            OnPropertyChanged("Connection");
            }
        }

        public DatabaseWorker()
        {
            Userid = "sa";
            Password = "test";
            Server = "localhost\\SQLEXPRESS";
            Database = "OddsChecker";
        }


        
       

        // Property Changed Handler
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
