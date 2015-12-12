using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Windows;

namespace OddsChecker
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private DatabaseWorker _database;
        private Window caller;

        public DatabaseWorker Database
        {
            get { return _database; }
            set { _database = value;
            OnPropertyChanged("Database");
            }
        }

        private String username;

        public String Username
        {
            get { return username; }
            set { username = value;
            OnPropertyChanged("Username");
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



        public LoginViewModel(Window Caller)
        {
            _database = new DatabaseWorker();
            Username = "jamie";
            Password = "test";
            caller = Caller;
        }

        //public void InsertAUserSP()
        //{
        //    SqlCommand cmd = new SqlCommand("InsertAUser", _database.Connection);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@Username", "bob");
        //    cmd.Parameters.AddWithValue("@Password", "bob1");
        //    //int rowAffected = cmd.ExecuteNonQuery();
        //}

        public void CheckLogIn(bool OnlineMode)
        {
            if (OnlineMode)
            {
                if (ConnectToDatabase())
                {
                    SqlCommand cmd = new SqlCommand("SelectAUser", _database.Connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", Username);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        MainWindow main = new MainWindow();
                        main.Show();
                        caller.Close();

                    }
                    else
                        MessageBox.Show("User doesn't exist");

                    CloseDatabaseConnection();
                }
            }
            else
            {
                MainWindow main = new MainWindow();
                main.Show();
                caller.Close();
            }
            
           


        }

        public bool ConnectToDatabase()
        {
            bool Connected = false;
            _database.Connection = new SqlConnection("user id=" + _database.Userid + ";password=" + _database.Password + ";data source=" + _database.Server +
                                       ";Trusted_Connection=yes;database=" + _database.Database + ";connection timeout=5");
            try
            {
                _database.Connection.Open();
                Connected = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return Connected;
            
        }

        public  void CloseDatabaseConnection()
        {
            try
            {
                _database.Connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
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
