using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;




namespace OddsChecker
{
    class Result : INotifyPropertyChanged
    {
        private String place;

        public String Place
        {
            get { return place; }
            set { place = value; OnPropertyChanged("Place"); }
        }

        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }
        private String odds;

        public String Odds
        {
            get { return odds; }
            set { odds = value; OnPropertyChanged("Odds"); }
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
