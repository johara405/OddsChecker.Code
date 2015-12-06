using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;


namespace OddsChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        OddCheckerViewModel od = new OddCheckerViewModel();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = od;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            var thread = new Thread(new ThreadStart(this.od.StartProcess));
            thread.Start(); 
            
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            var thread = new Thread(new ThreadStart(this.od.StopProcess));
            thread.Start(); 
        }

        private void StopButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            od.OfflineMode();
            var thread = new Thread(new ThreadStart(this.od.StartProcess));
            thread.Start(); 
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        

       

        

        

        
    }
}
