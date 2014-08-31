using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading.Tasks;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using slobus_v1._0_db.Gestures;
using slobus_v1._0_db.Resources;
using slobus_v1._0_db.ViewModels;
using Windows.Storage;

namespace slobus_v1._0_db
{
    public partial class RouteDetailsPage : PhoneApplicationPage
    {
        int index;
        RouteViewModel route;
        MapGestureBase gestureBase;
        bool mapExtended;
        MapLayer locationLayer;
        MapLayer busLayer;

        #region Initialization
        // Constructor
        public RouteDetailsPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();

            // Initialize the variables
            gestureBase = new MapGestureBase(Map);
            mapExtended = false;
            locationLayer = new MapLayer();
            busLayer = new MapLayer();

            // Supress Map and Set Up Tap
            gestureBase.SuppressMapGestures = true;
            Map.Tap += MinimizedMap_Tap;

            Map.Layers.Add(locationLayer);
            Map.Layers.Add(busLayer);

            // Refresh timeUntils
            App.ViewModel.refreshTimeUntil();
        }
        #endregion

        #region Navigation
        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (DataContext == null)
            {
                String selectedIndex = "";
                if (NavigationContext.QueryString.TryGetValue("selectedRoute", out selectedIndex))
                {
                    index = int.Parse(selectedIndex);
                    route = App.ViewModel.Routes[index];
                    DataContext = route;


                    
                    //App.ViewModel.Routes[index].RouteStops = new ObservableCollection<RouteStopViewModel>(
                    //                                         from routeStop in App.ViewModel.RouteStops
                    //                                         where routeStop.RouteID == selectedIndex
                    //                                         orderby routeStop.Time
                    //                                         select routeStop);
                }
            }

            // If there's only one bus for this route
            // adjust the pivot
            if (route.BusBStops.Count() == 0)
            {
                BusesPivot.Items.Remove(BusBPivotItem);
                BusesPivot.Margin = new Thickness(-12, -78, 0, 0);
                BusAPivotItem.Header = "";
            }

            // If there are no stops, show a message
            if(route.BusAStops.Count() == 0 && route.BusBStops.Count() == 0)
            {
                Message.Text = route.NotRunningMessage;
            }


            if (route.NextAStop != null || route.BusAStops.Count() == 0)
            {
                NoStopsA.Visibility = Visibility.Collapsed;
            }
            else
            {
                NoStopsA.Visibility = Visibility.Visible;
            }

            if (route.NextBStop != null || route.BusBStops.Count() == 0)
            {
                NoStopsB.Visibility = Visibility.Collapsed;
            }
            else
            {
                NoStopsB.Visibility = Visibility.Visible;
            }

            drawRoute();
            drawBus();
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

        #region AppBar
        // Code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create refresh button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarRefreshButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/refresh.png", UriKind.Relative));
            appBarRefreshButton.Text = AppResources.AppBarRefreshButtonText;
            ApplicationBar.Buttons.Add(appBarRefreshButton);
            appBarRefreshButton.Click += new EventHandler(RefreshButton_Click);

            // Create pdf button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarPDFButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/download.png", UriKind.Relative));
            appBarPDFButton.Text = AppResources.AppBarPDFButtonText;
            ApplicationBar.Buttons.Add(appBarPDFButton);
            appBarPDFButton.Click += new EventHandler(PDFButton_Click);

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
        private void PDFButton_Click(object sender, EventArgs e)
        {
            PDFLaunch();
        }
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            App.ViewModel.refreshTimeUntil();
            ScrollToSoonest();

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
        private void PDFLaunch()
        {
            //// Get File Uri
            //var fileUri = new Uri("ms-app-web://www.slocity.org/publicworks/download/transit/route1.pdf");

            //// Access the bug query file.
            //StorageFile pdf = await StorageFile.GetFileFromApplicationUriAsync(fileUri);

            //// Launch the bug query file.
            //await Windows.System.Launcher.LaunchFileAsync(pdf);
            String routeUrl = route.ShortName;

            if (routeUrl != "")
            {
                WebBrowserTask webBrowserTask = new WebBrowserTask();
                webBrowserTask.Uri = new Uri("http://www.slocity.org/publicworks/download/transit/route" + routeUrl + ".pdf");
                webBrowserTask.Show();
            }
            else
            {
                MessageBox.Show("Not a valid route");
            }
        }
        #endregion

        #region StopSelector
        private void ScrollToSoonest()
        {
            // Makes sure that the selector has items before scrolling. 
            if (route.NextAStop != null)
            {
                BusAStopSelector.ScrollTo(route.NextAStop);
            }

            if (route.NextBStop != null)
            {
                BusBStopSelector.ScrollTo(route.NextBStop);
            }
        }
        private void StopSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector Selector = (LongListSelector)sender;

            // If selected item is null (no selection) do nothing
            if (Selector.SelectedItem == null)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/StopDetailsPage.xaml?selectedStop=" + (Selector.SelectedItem as RouteStopViewModel).StopID.Split('?')[0], UriKind.Relative));

            // Reset selected item to null (no selection)
            Selector.SelectedItem = null;
        }

        private void StopSelector_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollToSoonest();
        }
        #endregion

        #region Map
        private void Map_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "e4eb341e-5704-4f44-866a-5c7231eb5d56";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "ocGjndaUVUWbfGsrBofjKg";
        }
        private void drawRoute()
        {
            Map.MapElements.Clear();

            MapPolyline polyLine = new MapPolyline();
            polyLine.Path = route.RoutePath;
            polyLine.StrokeThickness = App.RTE_THICKNESS;
            polyLine.StrokeColor = (Color)Application.Current.Resources["PhoneAccentColor"];
            Map.MapElements.Add(polyLine);
        }
        private void drawBus()
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
                if(route.NextBStop == null)
                {
                    text.Text = route.ShortName;
                }
                else
                {
                    text.Text = "A";
                }
                text.Style = (Style)Application.Current.Resources["PhoneTextNormalStyle"];
                text.TextAlignment = TextAlignment.Center;
                text.Width = 30;
                text.Margin = new Thickness(0);
                canvas.Children.Add(text);

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
                text.Text = "B";
                text.Style = (Style)Application.Current.Resources["PhoneTextNormalStyle"];
                text.TextAlignment = TextAlignment.Center;
                text.Width = 30;
                text.Margin = new Thickness(0);
                canvas.Children.Add(text);

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
        private void centerMap()
        {
            if(BusesPivot.SelectedItem == null)
            {
                Map.SetView(new GeoCoordinate(App.SLO_LAT, App.SLO_LONG), 13, MapAnimationKind.Parabolic);
            }
            else if(BusesPivot.SelectedItem.Equals(BusAPivotItem)) 
            {
                CenterMapOnBus(route.NextAStop);
            }
            else if(BusesPivot.SelectedItem.Equals(BusBPivotItem))
            {
                CenterMapOnBus(route.NextBStop);
            }
            else
            {
                Map.SetView(new GeoCoordinate(App.SLO_LAT, App.SLO_LONG), 13, MapAnimationKind.Parabolic);
            }
        }
        private void CenterMapOnBus(RouteStopViewModel stop)
        {
            if (stop == null)
            {
                Map.SetView(new GeoCoordinate(App.SLO_LAT, App.SLO_LONG), 13, MapAnimationKind.Parabolic);
            }
            else
            {
                foreach (StopViewModel search in App.ViewModel.Stops)
                {
                    if (search.StopID == stop.StopID.Split('?')[0])
                    {
                        Map.SetView(search.Location, 13, MapAnimationKind.Parabolic);
                        break;
                    }

                }
            }
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
        private void MinimizedMap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LayoutRoot.RowDefinitions.ElementAt(1).Height = GridLength.Auto;
            MainGrid.Children.Remove(ContentPanel);
            gestureBase.SuppressMapGestures = false;
            mapExtended = true;
            Map.Tap -= MinimizedMap_Tap;
            TitlePanel.Tap += TitlePanel_Tap;
        }
        private void MinimizeMap()
        {
            LayoutRoot.RowDefinitions.ElementAt(1).Height = new GridLength(4, GridUnitType.Star);
            MainGrid.Children.Add(ContentPanel);
            gestureBase.SuppressMapGestures = true;
            mapExtended = false;
            Map.Tap += MinimizedMap_Tap;
            TitlePanel.Tap -= TitlePanel_Tap;
        }
        private void TitlePanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MinimizeMap();
        }
        #endregion

        private void BusesPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            centerMap();
        }
    }
}