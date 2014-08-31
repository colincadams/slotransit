using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Shell;
using slobus_v1._0_db.Gestures;
using slobus_v1._0_db.Resources;
using slobus_v1._0_db.ViewModels;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace slobus_v1._0_db
{
    public partial class MainPage : PhoneApplicationPage
    {
        MapLayer locationLayer;
        MapLayer stopLayer;
        MapLayer busLayer;
        MapGestureBase gestureBase;
        bool mapExtended;
        Pushpin expandedPin;

        #region Initialization
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the LongListSelector control to the sample data
            DataContext = App.ViewModel;

            // initialize variables
            locationLayer = new MapLayer();
            stopLayer = new MapLayer();
            busLayer = new MapLayer();
            gestureBase = new MapGestureBase(Map);
            mapExtended = false;
            expandedPin = null;

            // Supress Map and Set Up Tap
            gestureBase.SuppressMapGestures = true;
            Map.Tap += MinimizedMap_Tap;

            // Add Map Layers
            Map.Layers.Add(busLayer);
            Map.Layers.Add(locationLayer);
            Map.Layers.Add(stopLayer);

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();
        }
        #endregion

        #region Navigation
        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent"))
            {
                MessageBoxResult result =
                    MessageBox.Show("Would you like to enable location services?",
                    "Location Services",
                    MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = true;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = false;
                }

                IsolatedStorageSettings.ApplicationSettings.Save();
            }
            
            // Load Data
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }


            // Refresh Location
            if(App.ViewModel.coordinates == null)
            {
                refreshLocation();
            }
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

        private async Task refreshLocation()
        {
            locationLayer.Clear();
            
            // If we can access their location, refresh location
            // and center on it.
            if((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"])
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
                drawLocationMarker();

                // Center map on location
                Map.SetView(App.ViewModel.coordinates, 14, Microsoft.Phone.Maps.Controls.MapAnimationKind.Parabolic);

                // if the stop pivot is active, redraw the stops so they match
                // the new order from the new location
                if(MainPivot.SelectedItem.Equals(StopsPivotItem))
                {
                    drawStopMarkers();
                }
                else if(MainPivot.SelectedItem.Equals(RoutesPivotItem))
                {
                    drawBuses();
                }
            }
            else
            {
                Map.SetView(new GeoCoordinate(App.SLO_LAT, App.SLO_LONG), 12.5, Microsoft.Phone.Maps.Controls.MapAnimationKind.Parabolic);
            }
        }

        #region Map
        private void Map_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "e4eb341e-5704-4f44-866a-5c7231eb5d56";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "ocGjndaUVUWbfGsrBofjKg";
        }
        private void drawLocationMarker()
        {
            if (App.ViewModel.coordinates != null)
            {
                UserLocationMarker locMarker = new UserLocationMarker();

                MapOverlay markerOverlay = new MapOverlay();
                markerOverlay.Content = locMarker;
                markerOverlay.PositionOrigin = new Point(0.5, 0.5);
                markerOverlay.GeoCoordinate = App.ViewModel.coordinates;
                locationLayer.Add(markerOverlay);
            }
        }
        private void drawStopMarkers()
        {
            stopLayer.Clear();
            
            foreach (StopViewModel stop in App.ViewModel.Stops)
            {
                drawStop(stop, stop.Index + 1, stopLayer);
            }
        }
        private void drawStop(StopViewModel stop, int label, MapLayer layer)
        {
            TextBlock text = new TextBlock();
            text.Text = label + "";
            text.TextAlignment = TextAlignment.Center;
            text.Margin = new Thickness(2, 0, 2, 0);
            
            Pushpin pushpin = new Pushpin();
            pushpin.Content = text;
            pushpin.HorizontalContentAlignment = HorizontalAlignment.Center;
            pushpin.PositionOrigin = new Point(0, 1);
            pushpin.GeoCoordinate = stop.Location;
            pushpin.Tap += StopPin_Tap;
            MapOverlay overlay = new MapOverlay();
            overlay.GeoCoordinate = stop.Location;
            overlay.PositionOrigin = new Point(0, 1);
            overlay.Content = pushpin;
            layer.Add(overlay);
        }

        private void drawRouteLines()
        {
            Map.MapElements.Clear();
            foreach (RouteViewModel route in App.ViewModel.Routes)
            {
                drawRoute(route);
            }
        }
        private void drawRoute(RouteViewModel route)
        {
            MapPolyline polyLine = new MapPolyline();
            polyLine.Path = route.RoutePath;
            polyLine.StrokeThickness = App.RTE_THICKNESS;
            polyLine.StrokeColor = (Color)Application.Current.Resources["PhoneAccentColor"];
            Map.MapElements.Add(polyLine);
        }
        private void drawBuses()
        {
            busLayer.Clear();
            foreach (RouteViewModel route in App.ViewModel.Routes)
            {
                drawBus(route);
            }
        }
        private void drawBus(RouteViewModel route)
        {
            if (route.NextAStop != null)
            {
                StopViewModel stop = new StopViewModel();
                String ID = route.NextAStop.StopID.Split('?')[0];
                foreach (StopViewModel search in App.ViewModel.Stops)
                {
                    if (search.StopID == ID)
                    {
                        stop = search;
                        break;
                    }
                }

                Canvas canvas = new Canvas();
                canvas.Height = 30;
                canvas.Width = 30;

                Rectangle rect = new Rectangle();
                rect.Height = 30;
                rect.Width = 30;
                rect.Fill = (Brush)Application.Current.Resources["PhoneAccentBrush"];
                rect.RadiusX = 5;
                rect.RadiusY = 5;
                canvas.Children.Add(rect);

                TextBlock text = new TextBlock();
                text.Text = route.ShortName;
                text.Style = (Style)Application.Current.Resources["PhoneTextNormalStyle"];
                text.TextAlignment = TextAlignment.Center;
                text.Width = 30;
                text.Margin = new Thickness(0);
                canvas.Children.Add(text);

                canvas.Tap += canvas_Tap;

                MapOverlay overlay = new MapOverlay();
                overlay.Content = canvas;
                overlay.PositionOrigin = new Point(0.5, 0.5);
                if (stop != null)
                {
                    overlay.GeoCoordinate = stop.Location;
                }

                busLayer.Add(overlay);
            }

            if (route.NextBStop != null)
            {
                StopViewModel stop = new StopViewModel();
                String ID = route.NextBStop.StopID.Split('?')[0];
                foreach (StopViewModel search in App.ViewModel.Stops)
                {
                    if (search.StopID == ID)
                    {
                        stop = search;
                        break;
                    }
                }

                Canvas canvas = new Canvas();
                canvas.Height = 30;
                canvas.Width = 30;

                Rectangle rect = new Rectangle();
                rect.Height = 30;
                rect.Width = 30;
                rect.Fill = (Brush)Application.Current.Resources["PhoneAccentBrush"];
                rect.RadiusX = 5;
                rect.RadiusY = 5;
                canvas.Children.Add(rect);

                TextBlock text = new TextBlock();
                text.Text = route.ShortName;
                text.Style = (Style)Application.Current.Resources["PhoneTextNormalStyle"];
                text.TextAlignment = TextAlignment.Center;
                text.Width = 30;
                text.Margin = new Thickness(0);
                canvas.Children.Add(text);

                canvas.Tap += canvas_Tap;

                MapOverlay overlay = new MapOverlay();
                overlay.Content = canvas;
                overlay.PositionOrigin = new Point(0.5, 0.5);
                if (stop != null)
                {
                    overlay.GeoCoordinate = stop.Location;
                }

                busLayer.Add(overlay);
            }
        }

        
        private void MinimizedMap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LayoutRoot.RowDefinitions.ElementAt(0).Height = new GridLength(5, GridUnitType.Star);
            gestureBase.SuppressMapGestures = false;
            mapExtended = true;
            Map.Tap -= MinimizedMap_Tap;
            Map.Tap += ExpandedMap_Tap;
        }
        private void ExpandedMap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MinimizePin();
        }
        private void StopPin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // If the map is extended, allow the pin to be expanded
            if (mapExtended)
            {
                Pushpin pin = sender as Pushpin;

                // get the stop associated with the pin
                StopViewModel stop = null;
                int i = 0;
                while (stop == null && i < App.ViewModel.Stops.Count)
                {
                    if (App.ViewModel.Stops[i].Location.Equals(pin.GeoCoordinate))
                        stop = App.ViewModel.Stops[i];
                    i++;
                }

                // if its already been expanded, navigate to page
                if (pin.Equals(expandedPin))
                    NavigationService.Navigate(new Uri("/StopDetailsPage.xaml?selectedStop=" + stop.StopID, UriKind.Relative));
                // if it hasn't been expanded, then expand
                else
                {
                    // Minimize any expanded pins
                    MinimizePin();

                    // Scroll to that stop in stop selector
                    StopLongListSelector.ScrollTo(stop);

                    // Determine which overlay its on
                    int overlayIndex = 0;
                    MapOverlay overlay = null;
                    while (overlay == null && overlayIndex < stopLayer.Count)
                    {
                        if (stopLayer[overlayIndex].Content == pin)
                        {
                            overlay = stopLayer[overlayIndex];
                        }
                        overlayIndex++;
                    }

                    // Bring that overlay to the front
                    stopLayer.Remove(overlay);
                    stopLayer.Add(overlay);

                    // Expand this pin
                    (pin.Content as TextBlock).Text = stop.Name;

                    // Set as the expanded pin
                    expandedPin = pin;
                }

                // Handle the tap as to not pass it up to Map. 
                e.Handled = true;
            }
        }
        private void canvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(mapExtended)
            {
                Canvas canvas = sender as Canvas;

                TextBlock text = canvas.Children[1] as TextBlock;

                foreach (RouteViewModel route in App.ViewModel.Routes)
                {
                    if (text.Text == route.ShortName)
                    {
                        NavigationService.Navigate(new Uri("/RouteDetailsPage.xaml?selectedRoute=" + route.RouteID, UriKind.Relative));
                        break;
                    }
                }

                e.Handled = true;
            }
        }
        private void MinimizePin()
        {
            if (expandedPin != null)
            {
                StopViewModel stop = null;
                int i = 0;
                while (stop == null && i < App.ViewModel.Stops.Count)
                {
                    if (App.ViewModel.Stops[i].Location.Equals(expandedPin.GeoCoordinate))
                        stop = App.ViewModel.Stops[i];
                    i++;
                }

                (expandedPin.Content as TextBlock).Text = (stop.Index + 1) + "";
                expandedPin = null;
            }
        }
        
        private void MinimizeMap()
        {
            LayoutRoot.RowDefinitions.ElementAt(0).Height = new GridLength(1, GridUnitType.Star);
            gestureBase.SuppressMapGestures = true;
            mapExtended = false;
            Map.Tap += MinimizedMap_Tap;
            Map.Tap -= ExpandedMap_Tap;
        }

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainPivot.SelectedItem.Equals(RoutesPivotItem))
            {
                stopLayer.Clear();
                //drawRouteLines();
                drawBuses();
            }
            else if(MainPivot.SelectedItem.Equals(StopsPivotItem))
            {
                Map.MapElements.Clear();
                busLayer.Clear();
                drawStopMarkers();
            }
        }
        #endregion

        #region Selectors
        // Handle selection changed on LongListSelector
        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (MainLongListSelector.SelectedItem == null)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/RouteDetailsPage.xaml?selectedRoute=" + (MainLongListSelector.SelectedItem as RouteViewModel).RouteID, UriKind.Relative));

            // Reset selected item to null (no selection)
            MainLongListSelector.SelectedItem = null;
        }

        private void StopLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (StopLongListSelector.SelectedItem == null)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/StopDetailsPage.xaml?selectedStop=" + (StopLongListSelector.SelectedItem as StopViewModel).StopID, UriKind.Relative));

            // Reset selected item to null (no selection)
            StopLongListSelector.SelectedItem = null;
        }
        #endregion

        #region AppBar
        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            ApplicationBar.Opacity = 1;
            ApplicationBar.IsMenuEnabled = true;
            ApplicationBar.Mode = ApplicationBarMode.Default;

            // Create a new button and set the text value to the localized string from AppResources.
            //ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
            //appBarButton.Text = AppResources.AppBarButtonText;
            //ApplicationBar.Buttons.Add(appBarButton);
            ApplicationBarIconButton appBarRefreshButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/refresh.png", UriKind.Relative));
            appBarRefreshButton.Text = AppResources.AppBarRefreshButtonText;
            ApplicationBar.Buttons.Add(appBarRefreshButton);
            appBarRefreshButton.Click += new EventHandler(RefreshButton_Click);

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
            refreshLocation();
            App.ViewModel.refreshTimeUntil();
        }
        private void CenterButton_Click(object sender, EventArgs e)
        {
            Map.SetView(new GeoCoordinate(App.SLO_LAT, App.SLO_LONG), 12.5, Microsoft.Phone.Maps.Controls.MapAnimationKind.Parabolic);
        }
        private void LocationButton_Click(object sender, EventArgs e)
        {
            if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] && App.ViewModel.coordinates != null)
            {
                Map.SetView(App.ViewModel.coordinates, 14, Microsoft.Phone.Maps.Controls.MapAnimationKind.Parabolic);

                drawLocationMarker();
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