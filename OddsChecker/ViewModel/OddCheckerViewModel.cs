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
using System.Reflection;
using System.IO;

namespace OddsChecker
{
    class OddCheckerViewModel : INotifyPropertyChanged
    {
        private OddChecker oddchecker = new OddChecker();
        private Event selectedEvent = new Event();
        private SportCoupon selectedCoupon = new SportCoupon();
        private EventGroup selectedEventGroup = new EventGroup();
        int selectedCouponId = -1;
        int selectedEventGroupid = -1;
        int selectedEventid = -1;        
        public OddChecker OC
        {
            get { return oddchecker; }
            set { oddchecker = value;
            OnPropertyChanged("OC");
            }
        }
        public Event SelectedEvent
        {
            get { return selectedEvent; }
            set { selectedEvent = value;
            OnPropertyChanged("SelectedEvent");
            }
        }
        public SportCoupon SelectedCoupon
        {
            get { return selectedCoupon; }
            set { selectedCoupon = value;  
            try {
                    if (SelectedCoupon.EventGroups.Count != 0)
                    SelectedEventGroup = value.EventGroups.First(); 
                    SelectedEvent = SelectedEventGroup.Events.FirstOrDefault();
                    OnPropertyChanged("SelectedCoupon");
            }
            catch 
            { }
            }
        }
        public EventGroup SelectedEventGroup
        {
            get { return selectedEventGroup; }
            set { selectedEventGroup = value;
            OnPropertyChanged("SelectedEventGroup");
            try {SelectedEvent = SelectedEventGroup.Events.FirstOrDefault();}
            catch { }
            }
        }
        bool KeepUpdating;
        bool stop;
        

        public void StopProcess()
        {
            stop = true;
            AddStatusUpdate("Stopping after current process","");
            KeepUpdating = false;           
        }

        public void StartProcess()
        {
            stop = false;
            OC.Idle = false;
            OC.Processing = Visibility.Visible;
            KeepUpdating = true;
            
          
            if (OC.OnlineMode)
            {
                OC.Coupons = new ObservableCollection<SportCoupon>();
                GetFilesToDownload();

            }
                
            if (!OC.OnlineMode)
                GetFilesToOpen();

            UpdateAllCoupons();   
            OC.Idle = true;
            OC.Processing = Visibility.Hidden;
            OC.OnlineMode = true;            
        }

        public void OfflineMode()
        {
            OC.OnlineMode = false;
        }

        private void GetFilesToOpen()
        {
            
                AddStatusUpdate("Starting", "Locating Sports XML files from Local Path");
                
                
                DirectoryInfo fileDirectory = new DirectoryInfo(OC.FilePath);
                try
                {
                    FileInfo[] Files = fileDirectory.GetFiles("*.xml");

                    OC.Coupons = new ObservableCollection<SportCoupon>();

                    foreach (FileInfo file in Files)
                    {
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            OC.Coupons.Add(new SportCoupon() { Stem = file.Name });
                        });
                        AddStatusUpdate("Local Stem File Located. Empty Coupon created.", file.Name);
                    }

                    AddStatusUpdate("Local File gathering finished", OC.Coupons.Count + " Empty Coupons created.");

                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Folder location is not valid." + ex);
                }

                OC.CouponAmount = OC.Coupons.Count;
                OC.CurrentCoupon = 1;
                OC.PercentPerCoupon = 100 / OC.CouponAmount;
            

        }     

        private void UpdateAllCoupons()
        {
           
            while (KeepUpdating)
            {
                UpdateProcessedPercentage(0);
                OC.CurrentCoupon = 1;

                foreach (SportCoupon coupon in OC.Coupons)
                {
                    if (!stop)
                    {                        
                        UpdateProcessedPercentage(OC.CurrentCoupon / OC.CouponAmount * 100);                        
                        CheckForCouponUpdates(coupon);
                        OC.CurrentCoupon++;
                    }

                }

                if (!OC.UpdateAutomatically || !OC.OnlineMode)
                {
                    KeepUpdating = false;                   
                }
                if (!OC.UpdateAutomatically)
                {
                    selectedCouponId = -1;
                    selectedEventGroupid = -1;
                    selectedEventid = -1;
                }
                    

                AddStatusUpdate("Complete", "All coupons have finished downloading");

              
            }
        }

        private void GetFilesToDownload()
        {
            
                AddStatusUpdate("Starting", "Locating Sports XML files");
                XmlDocument fileList = new XmlDocument();

                try
                {
                    fileList.Load(OC.FileListUrl);
                    fileList.Save("C:\\Users\\Jamie\\Documents\\FileList.xml");
                    foreach (XmlNode node in fileList.DocumentElement.SelectSingleNode("/data"))
                    {
                        if (node.NodeType != XmlNodeType.Comment)
                        {
                            String stem = node.Attributes["stem"].InnerText;
                            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                            {
                                OC.Coupons.Add(new SportCoupon() { Stem = stem, Name = stem + " Awaiting Download..." });
                            }); 
                            AddStatusUpdate("Stem File Located. Empty Coupon created.", stem);
                        }

                    }

                    AddStatusUpdate("Finished", OC.Coupons.Count + " Empty Coupons created.");

                    OC.CouponAmount = OC.Coupons.Count;
                    OC.CurrentCoupon = 1;
                    OC.PercentPerCoupon = 100 / OC.CouponAmount;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retreiving File List from Bet Byrne website!" + ex);
                    StopProcess();
                    AddStatusUpdate("Stopped", "");

                }

                
            

        }

        private void CheckForCouponUpdates(SportCoupon coupon)
        {
            if (coupon.EventGroups.Count < 1)
                AddStatusUpdate("Downloading Coupon",coupon.Stem); 
           
            bool xmlFileDownloadSuccessfully = false;
            XmlDocument eventData = new XmlDocument();

            // if Online mode

            switch (OC.OnlineMode)
            {
                case true:
                    {
                        xmlFileDownloadSuccessfully = DownloadCoupon(coupon, xmlFileDownloadSuccessfully, eventData);
                    }

                    break;
                case false:
                    {
                        xmlFileDownloadSuccessfully = OpenLocalCoupon(coupon, xmlFileDownloadSuccessfully, eventData);
                    }
                    break;
            }

                if (xmlFileDownloadSuccessfully && eventData.DocumentElement.SelectSingleNode("/data").HasChildNodes)
                {
                    XmlNode XMLEventFileNode = eventData.DocumentElement.SelectSingleNode("/data/eventfile");
                    DateTime xmlLastUpdated = DateTime.Parse(XMLEventFileNode.Attributes["lastmod"].InnerText);

                    if (coupon.LastModified < DateTime.Parse("01/01/1000"))
                    {

                        AddStatusUpdate("First Time Download", coupon.Name);
                        switch (OC.OnlineMode)
                        {
                            case true:
                                {
                                    coupon.Id = Int32.Parse(XMLEventFileNode.Attributes["id"].InnerText);

                                    DownloadCopyOfXml(coupon, eventData);
                                }

                                break;
                            case false:
                                {
                                    coupon.Id = (int)OC.CurrentCoupon - 1;
                                }
                                break;
                        }
                        
                        coupon.Name = XMLEventFileNode.Attributes["name"].InnerText;
                        UpdateCoupon(coupon, XMLEventFileNode);
                    }
                    else
                    {
                        // THERE IS A RECENT UPDATE
                        if (xmlLastUpdated > coupon.LastModified)
                        {
                            AddStatusUpdate("New prices found, Downloading.", coupon.Name);
                            UpdateCoupon(coupon, XMLEventFileNode);

                            DownloadCopyOfXml(coupon, eventData);
                        }
                        else
                        {
                            AddStatusUpdate("File Skipped, no new updates", coupon.Name);
                        }

                    }          
                }
                else
                {
                    MessageBox.Show("Error");
                }
          
            
        }

        private static void DownloadCopyOfXml(SportCoupon coupon, XmlDocument eventData)
        {
            String path = "C:\\OddsChecker\\" + DateTime.Today.Date.ToString().Replace('/', '-').Substring(0, 10) + "\\";
            bool exists = System.IO.Directory.Exists(path);
            if (!exists)
                System.IO.Directory.CreateDirectory(path);

            eventData.Save(path + coupon.Stem + ".xml");
        }

        private void UpdateCoupon(SportCoupon coupon, XmlNode eventFile)
        {            
            coupon.LastModified = DateTime.Parse(eventFile.Attributes["lastmod"].InnerText);

            if (SelectedEvent != null && SelectedEventGroup != null && SelectedCoupon != null)
            {
                selectedEventGroupid = selectedEventGroup.Id;
                selectedEventid = SelectedEvent.Id;
                selectedCouponId = SelectedCoupon.Id;
            }            
                    
            // Populate EventGroups
            foreach (XmlNode _eventGroupNode in eventFile)
            {
                PopulateEventGroup(coupon, _eventGroupNode);
            }

            RefreshCouponsView();
        }

        private void PopulateEventGroup(SportCoupon coupon, XmlNode _eventGroupNode)
        {
            EventGroup eventGroup = new EventGroup();
            eventGroup.Id = Int32.Parse(_eventGroupNode.Attributes["id"].InnerText);
            eventGroup.Name = _eventGroupNode.Attributes["name"].InnerText;

            // Populate Events
            foreach (XmlNode _eventNode in _eventGroupNode)
            {
                PopulateEvent(eventGroup, _eventNode);
            }

            App.Current.Dispatcher.Invoke((Action)delegate
            {
                // if EventGroup isn't on coupon, add it
                if (coupon.EventGroups.Count < eventGroup.Id + 1)
                {
                    coupon.EventGroups.Add(eventGroup);
                    AddStatusUpdate("New Event downloaded. Adding to " + coupon.Name + " coupon", eventGroup.Name);

                }

                // Else update the current eventgroup on the coupon
                else
                {
                    AddStatusUpdate("Update Found for " + coupon.Name, eventGroup.Name);
                    coupon.EventGroups[eventGroup.Id] = eventGroup;

                }

                RefreshCouponsView();
                coupon.LastDownload = DateTime.Now;

            });
        }

        private static void PopulateEvent(EventGroup eventGroup, XmlNode _eventNode)
        {
            if (_eventNode.NodeType != XmlNodeType.Comment)
            {
                Event _event = new Event();
                _event.Id = Int32.Parse(_eventNode.Attributes["id"].InnerText);
                _event.Name = _eventNode.Attributes["time"].InnerText + " - " + _eventNode.Attributes["name"].InnerText;
                _event.Date = _eventNode.Attributes["date"].InnerText;
                _event.Time = _eventNode.Attributes["time"].InnerText;

                // Populate Selections
                foreach (XmlNode _selectionNode in _eventNode)
                {
                    PopulateSelection(_event, _selectionNode);
                }

                PopulateResults(_eventNode, _event);
                

               eventGroup.Events.Add(_event);
            }
        }

        private static void PopulateResults(XmlNode _eventNode, Event _event)
        {
            // Parse Results
            XmlNode _resultsNode = _eventNode.SelectSingleNode("results");

            if (_resultsNode != null)
            {
                foreach (XmlNode _resultNode in _resultsNode)
                {
                    Result _result = new Result();
                    _result.Name = _resultNode.Attributes["name"].InnerText;
                    _result.Odds = _resultNode.Attributes["price"].InnerText;
                    _result.Place = _resultNode.Attributes["place"].InnerText;

                    _event.Results.Add(_result);
                }
            }
        }

        private static void PopulateSelection(Event _event, XmlNode _selectionNode)
        {
            if (_selectionNode.NodeType != XmlNodeType.Comment && _selectionNode.Name != "results")
            {
                Selection _selection = new Selection();
                _selection.ID = Int32.Parse(_selectionNode.Attributes["id"].InnerText);
                _selection.Name = _selectionNode.Attributes["name"].InnerText;
                if (_selectionNode.Attributes["runnercode"].InnerText == "N")
                    _selection.Odds = "NR";
                else
                    _selection.Odds = _selectionNode.Attributes["price"].InnerText;

                _event.Selections.Add(_selection);
            }
        }      

        private bool DownloadCoupon(SportCoupon coupon, bool xmlFileDownloaded, XmlDocument eventData)
        {
            try
            {
                OC.Downloading = true;
                eventData.Load(OC.FileURL + coupon.Stem); 
                OC.Downloading = false;
                xmlFileDownloaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to download " + coupon.Stem + " Coupon from website, check if website is online \n \n \n" + ex);
                OC.Downloading = false;
            }

            return xmlFileDownloaded;
        }

        private bool OpenLocalCoupon(SportCoupon coupon, bool xmlFileDownloaded, XmlDocument eventData)
        {
            try
            {
                OC.Downloading = true;                
                eventData.Load(OC.FilePath + @"\" + coupon.Stem);
                
                OC.Downloading = false;
                xmlFileDownloaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open " + coupon.Stem + " From " + OC.FilePath + "    \n" + ex);
                OC.Downloading = false;
            }

            return xmlFileDownloaded;
        }

        // UI Updates
        public void AddStatusUpdate(String title, String description)
        {
            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                OC.StatusUpdates.Add(new StatusUpdate(DateTime.Now, title, description));
                OC.Status = title + " / " + description;
            });
        }
        private void RefreshCouponsView()
        {
            

            // If the selected Eventgroups / event isn't still downloading
            if (selectedEventid != -1 && selectedEventGroupid != -1)
            {
                switch (OC.OnlineMode)
                {
                    case true:
                        {
                            SelectedCoupon = OC.Coupons[selectedCouponId];
                            SelectedEventGroup = SelectedCoupon.EventGroups[selectedEventGroupid];
                            SelectedEvent = SelectedEventGroup.Events[selectedEventid];
                        }

                        break;
                    case false:
                        {
                            SelectedCoupon = OC.Coupons.FirstOrDefault();
                            SelectedEventGroup = SelectedCoupon.EventGroups.FirstOrDefault();
                            SelectedEvent = SelectedEventGroup.Events.FirstOrDefault();
                        }
                        break;
                }

                

            }
        }
        public void UpdateProcessedPercentage(double perc)
        {
            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                OC.ProgressPercentage = perc;
            });
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
