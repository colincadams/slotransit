using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace slobus_v1._0_db
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            // If the user has decided whether or not
            // to allow the application to use their location
            // set the toggle appropriately
            if (IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent"))
            {
                if((Boolean)IsolatedStorageSettings.ApplicationSettings["LocationConsent"])
                {
                    LocationToggle.IsChecked = true;
                }
                else
                {
                    LocationToggle.IsChecked = false;
                }
            }
        }

        private void LocationToggle_Checked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = true;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        private void LocationToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = false;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }
    }
}