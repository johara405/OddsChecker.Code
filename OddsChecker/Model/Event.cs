using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;
using System.Windows;

namespace OddsChecker
{
    class Event : INotifyPropertyChanged
     
    {
        private int id;
        private String name;
        private String date;
        private ObservableCollection<Result> results = new ObservableCollection<Result>();

        public ObservableCollection<Result> Results
        {
            get { return results; }
            set { results = value; 
                OnPropertyChanged("Results"); }
        }

        public String Date
        {
            get { return date; }
            set { date = value;
            OnPropertyChanged("Date");
            }
        }

        private String time;

        public String Time
        {
            get { return time; }
            set { time = value;
            OnPropertyChanged("Time");
            }
        }
        private ObservableCollection<Selection> selections = new ObservableCollection<Selection>();

   

        public int Id
        {
            get { return id; }
            set { id = value;
            OnPropertyChanged("Id");
            }
        }

        public String Name
        {
            get { return name; }
            set { name = value;
            OnPropertyChanged("Name");
            }
        }

        public ObservableCollection<Selection> Selections
        {
            get { return selections; }
            set { selections = value;
            OnPropertyChanged("Selections");
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
