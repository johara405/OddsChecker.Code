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
    class OddChecker : INotifyPropertyChanged
    {
        // Instance Variables
        public event PropertyChangedEventHandler PropertyChanged;
        private String filePath;

        public String FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        private String fileListUrl;

        public String FileListUrl
        {
            get { return fileListUrl; }
            set { fileListUrl = value;
            OnPropertyChanged("FileListUrl");
            }
        }

        public double CouponAmount;
        public double CurrentCoupon;
        public double PercentPerCoupon;
        private Visibility processing;
        private bool idle;
        private bool updateAutomatically;
        private bool onlineMode;
        public bool OnlineMode
        {
            get { return onlineMode; }
            set
            {
                onlineMode = value;
                OnPropertyChanged("OnlineMode");
            }
        }  

        public bool UpdateAutomatically
        {
            get { return updateAutomatically; }
            set { updateAutomatically = value;
            OnPropertyChanged("UpdateAutomatically");
            }
        }

        public bool Idle
        {
            get { return idle; }
            set { idle = value;
            OnPropertyChanged("Idle");
            }
        }
        private bool downloading;

      
        // Private Properties
        private ObservableCollection<SportCoupon> coupons = new ObservableCollection<SportCoupon>();

        
        private String fileURL;
        private ObservableCollection<StatusUpdate> statusUpdates = new ObservableCollection<StatusUpdate>();
        private String status;
        double progressPercentage;
        

        // Public Properties
        public ObservableCollection<SportCoupon> Coupons
        {
            get { return coupons; }
            set { coupons = value;
            OnPropertyChanged("Coupons");
            }
        }
        public String FileURL
        {
            get { return fileURL; }
            set
            {
                fileURL = value;
                OnPropertyChanged("FileURL");
            }
        }
        public ObservableCollection<StatusUpdate> StatusUpdates
        {
            get { return statusUpdates; }
            set
            {
                statusUpdates = value;
                OnPropertyChanged("StatusUpdates");

            }
        }
        public String Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }
        public double ProgressPercentage
        {
            get { return progressPercentage; }
            set
            {
                progressPercentage = value;
                OnPropertyChanged("ProgressPercentage");
            }
        }

        public Visibility Processing
        {
            get { return processing; }
            set { processing = value;
            OnPropertyChanged("Processing");
            }
        }

        public bool Downloading
        {
            get { return downloading; }
            set
            {
                downloading = value;
                OnPropertyChanged("Downloading");
            }
        }


        // Property Changed Handler
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public OddChecker()
        {
            FileListUrl = "https://www.betbyrne.com/events/XmlWithResults.asp?Act=Filelist";
            FileURL = "https://www.betbyrne.com/events/XmlWithResults.asp?EF=";
            FilePath = @"C:\OddsChecker\";
            Processing = Visibility.Hidden;
            Idle = true;
            Downloading = false;
            UpdateAutomatically = true;
            OnlineMode = true;
       ;
            
        }
    }
}
