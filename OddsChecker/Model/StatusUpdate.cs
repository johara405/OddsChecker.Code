using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OddsChecker
{
    public class StatusUpdate : INotifyPropertyChanged
    {
        public StatusUpdate(DateTime time, String title, String description)
        {
            StartTime = time;
            Title = title;
            Description = description;
        }
        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                if (value.Equals(_startTime)) return;
                _startTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description) return;
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        private DateTime _startTime;
        private string _title;
        private string _description;


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
