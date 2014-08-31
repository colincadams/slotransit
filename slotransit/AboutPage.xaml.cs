using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Reflection;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace slobus_v1._0_db
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            VersionTextBlock.Text = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Version.ToString();
        }
        private void ContactPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.To = "slotransit@colincadams.com";
            emailComposeTask.Subject = "SLO Transit: ";

            emailComposeTask.Show();
        }
    }
}