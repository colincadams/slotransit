using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using slobus_v1._0_db.Gestures;
using slobus_v1._0_db.ViewModels;
using slobus_v1._0_db.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Shell;

namespace slobus_v1._0_db
{
    
    
    public partial class StopDetailsPage : PhoneApplicationPage
    {
        StopViewModel Stop;
        MapGestureBase gestureBase;
        bool mapExtended;
        MapLayer stopLayer;
        MapLayer locationLayer;

        #region Initialization
        public StopDetailsPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();

            // Initialize the variables
            gestureBase = new MapGestureBase(Map);
            mapExtended = false;
            stopLayer = new MapLayer();
            locationLayer = new MapLayer();

            // Supress Map and Set Up Tap
            gestureBase.SuppressMapGestures = true;
            Map.Tap += MinimizedMap_Tap;
            Map.Layers.Add(stopLayer);
            Map.Layers.Add(locationLayer);

            // Refresh timeUntils
            App.ViewModel.refreshTimeUntil();
        }
        #endregion

        #region Navigation
        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int index = 0;
            if (DataContext == null)
            {
                String selectedID = "";
                if (NavigationContext.QueryString.TryGetValue("selectedStop", out selectedID))
                {
                    
                    while ( index < App.ViewModel.Stops.Count && selectedID != App.ViewModel.Stops[index].StopID )
                    {
                        index++;
                    }
                    Stop = App.ViewModel.Stops[index];
                    DataContext = Stop;
                }
            }

            Coordinates.Text = Stop.Location.ToString();

            // If there are no stops, show a message
            if (Stop.RouteStops.Count() == 0)
            {
                Message.Text = AppResources.NoBusesText;
            }

            if (Stop.NextRoute != null || Stop.RouteStops.Count() == 0)
            {
                NoBuses.Visibility = Visibility.Collapsed;
            }
            else
            {
                NoBuses.Visibility = Visibility.Visible;
            }


            // draw the stop and center the map
            drawStop();
            drawLocation();
            centerMap();
        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (mapExtended)
            {
                MinimizeMap();
                e.Cancel = true;
            }
            else
            {
                base.OnBackKeyPress(e);
            }
        }
        #endregion

        #region Selector
        private void ScrollToSoonest()
        {
            // Makes sure that the selector has items before scrolling. 
            if (Stop.NextRoute != null)
            {
                RouteStopSelector.ScrollTo(Stop.NextRoute);
            }
        }

        private void RouteStopSelector_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollToSoonest();
        }

        //private void RouteStopSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    // If selected item is null (no selection) do nothing
        //    if (RouteStopSelector.SelectedItem == null)
        //        return;

        //    // Navigate to the new page
        //    NavigationService.Navigate(new Uri("/RouteDetailsPage.xaml?selectedRoute=" + (RouteStopSelector.SelectedItem as RouteStopViewModel).RouteID, UriKind.Relative));

        //    // Reset selected item to null (no selection)
        //    RouteStopSelector.SelectedItem = null;
        //}
        #endregion

        #region Map
        private void Map_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "e4eb341e-5704-4f44-866a-5c7231eb5d56";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "ocGjndaUVUWbfGsrBofjKg";
        }
        private void centerMap()
        {
            Map.SetView(new GeoCoordinate(Stop.Location.Latitude + 0.005, Stop.Location.Longitude), 13, MapAnimationKind.Parabolic);
        }
        private async Task refreshLocation()
        {
            // If we can access their location, refresh location
            // and center on it.
            if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"])
            {
                // Create progress indicator in System Tray
                ProgressIndicator pi = new ProgressIndicator();
                pi.IsVisible = true;
                pi.IsIndeterminate = true;
                pi.Text = "Getting location...";
                SystemTray.SetProgressIndicator(this, pi);

                // Get the location
                await App.ViewModel.getPosition();

                // Remove progress indicator
                pi.IsVisible = false;

                // Draw the marker for the location
                drawLocation();
            }
        }
        private void MinimizedMap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LayoutRoot.RowDefinitions.ElementAt(1).Height = GridLength.Auto;
            ContentPanel.Children.Remove(RouteStopSelector);
            ContentPanel.Children.Remove(Message);
            gestureBase.SuppressMapGestures = false;
            mapExtended = true;
            Map.Tap -= MinimizedMap_Tap;
            TitlePanel.Tap += TitlePanel_Tap;
        }
        private void MinimizeMap()
        {
            LayoutRoot.RowDefinitions.ElementAt(1).Height = new GridLength(4, GridUnitType.Star);
            ContentPanel.Children.Add(RouteStopSelector);
            ContentPanel.Children.Add(Message);
            gestureBase.SuppressMapGestures = true;
            mapExtended = false;
            Map.Tap += MinimizedMap_Tap;
            TitlePanel.Tap -= TitlePanel_Tap;
        }
        private void TitlePanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MinimizeMap();
        }
        private void drawStop()
        {
            // Clear any stops that are drawn
            stopLayer.Clear();

            // Create a text block to put in the pushpin
            TextBlock text = new TextBlock();
            text.Text = Stop.Name;
            text.TextAlignment = TextAlignment.Center;
            text.Margin = new Thickness(2, 0, 2, 0);

            // Create the pushpin with that location and label
            Pushpin pushpin = new Pushpin();
            pushpin.Content = text;
            pushpin.HorizontalContentAlignment = HorizontalAlignment.Center;
            pushpin.PositionOrigin = new Point(0, 1);
            pushpin.GeoCoordinate = Stop.Location;

            // Create an overlay to hold the pushpin
            MapOverlay overlay = new MapOverlay();
            overlay.GeoCoordinate = Stop.Location;
            overlay.PositionOrigin = new Point(0, 1);
            overlay.Content = pushpin;

            // Add the overlay to the layer
            stopLayer.Add(overlay);
        }
        private void drawLocation()
        {
            if (App.ViewModel.coordinates != null)
            {
                locationLayer.Clear();

                UserLocationMarker locMarker = new UserLocationMarker();

                MapOverlay markerOverlay = new MapOverlay();
                markerOverlay.Content = locMarker;
                markerOverlay.PositionOrigin = new Point(0.5, 0.5);
                markerOverlay.GeoCoordinate = App.ViewModel.coordinates;
                locationLayer.Add(markerOverlay);
            }
            else
                refreshLocation();
        }
        #endregion

        #region AppBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create refresh button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarRefreshButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/refresh.png", UriKind.Relative));
            appBarRefreshButton.Text = AppResources.AppBarRefreshButtonText;
            ApplicationBar.Buttons.Add(appBarRefreshButton);
            appBarRefreshButton.Click += new EventHandler(RefreshButton_Click);

            // Create a new menu item with the localized string from AppResources.
            //ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
            //ApplicationBar.MenuItems.Add(appBarMenuItem);

            ApplicationBarIconButton appBarCenterButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/center.png", UriKind.Relative));
            appBarCenterButton.Text = AppResources.AppBarCenterButtonText;
            ApplicationBar.Buttons.Add(appBarCenterButton);
            appBarCenterButton.Click += new EventHandler(CenterButton_Click);

            ApplicationBarIconButton appBarLocationButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/location.png", UriKind.Relative));
            appBarLocationButton.Text = AppResources.AppBarLocationButtonText;
            ApplicationBar.Buttons.Add(appBarLocationButton);
            appBarLocationButton.Click += new EventHandler(LocationButton_Click);

            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem appBarSettingsItem = new ApplicationBarMenuItem(AppResources.AppBarSettingsItemText);
            ApplicationBar.MenuItems.Add(appBarSettingsItem);
            appBarSettingsItem.Click += new EventHandler(SettingsItem_Click);

            ApplicationBarMenuItem appBarAboutItem = new ApplicationBarMenuItem(AppResources.AppBarAboutItemText);
            ApplicationBar.MenuItems.Add(appBarAboutItem);
            appBarAboutItem.Click += new EventHandler(AboutItem_Click);
        }
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            // Refresh the times and scroll to the next route
            App.ViewModel.refreshTimeUntil();
            ScrollToSoonest();

            // Refresh the location
            refreshLocation();
        }
        private void CenterButton_Click(object sender, EventArgs e)
        {
            centerMap();
        }
        private void LocationButton_Click(object sender, EventArgs e)
        {
            if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"])
            {
                Map.SetView(App.ViewModel.coordinates, 14, Microsoft.Phone.Maps.Controls.MapAnimationKind.Parabolic);

                drawLocation();
            }
            else
            {
                Map.SetView(new GeoCoordinate(App.SLO_LAT, App.SLO_LONG), 12.5, Microsoft.Phone.Maps.Controls.MapAnimationKind.Parabolic);
            }
        }
        private void SettingsItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }
        private void AboutItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }
        #endregion

    }
}