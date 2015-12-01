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
    class SportCoupon : INotifyPropertyChanged
    {
        private int id;
        private String name;
        private String stem;
        private ObservableCollection<EventGroup> eventGroups = new ObservableCollection<EventGroup>();
        private DateTime lastModified;
        private DateTime lastDownload;

        public DateTime LastDownload
        {
            get { return lastDownload; }
            set { lastDownload = value; 
                OnPropertyChanged("LastDownload");
            //
            }
        }
        

        public SportCoupon()
        {
           
        }


        public String Stem
        {
            get { return stem; }
            set { stem = value; OnPropertyChanged("Stem"); }
        }

        public DateTime LastModified
        {
            get { return lastModified; }
            set { lastModified = value; OnPropertyChanged("LastModified"); }
        }
        
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
            set { name = value; OnPropertyChanged("Name"); }
        }

        public ObservableCollection<EventGroup> EventGroups
        {
            get { return eventGroups; }
            set { eventGroups = value; OnPropertyChanged("EventGroups"); }
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
