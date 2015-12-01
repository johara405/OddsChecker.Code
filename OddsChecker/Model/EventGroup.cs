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
    class EventGroup : INotifyPropertyChanged
        // E.g. A Football Match, a Horse Meeting (includes all races)
    {
        private int id;
        private String name;
        private ObservableCollection<Event> events = new ObservableCollection<Event>();        
      
        public EventGroup()
        {
            // TODO: Complete member initialization
        }

        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }

        public String Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }

        public ObservableCollection<Event> Events
        {
            get { return events; }
            set { events = value; OnPropertyChanged("Events"); }
        }

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
