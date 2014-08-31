using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Shell;
using System.Windows;
using System;
using System.IO.IsolatedStorage;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using slobus_v1._0_db.Resources;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace slobus_v1._0_db.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        
        
        public MainViewModel()
        {
            this.Routes = new ObservableCollection<RouteViewModel>();
            this.Stops = new ObservableCollection<StopViewModel>();
            this.RouteStops = new ObservableCollection<RouteStopViewModel>();
            this.locator = new Geolocator();
        }

        /// <summary>
        /// A collection for RouteViewModel objects, containing all of the routes in the Transit System.
        /// </summary>
        public ObservableCollection<RouteViewModel> Routes { get; private set; }

        /// <summary>
        /// A collection for StopViewModel objects, containing all of the stops in the Transit System.
        /// </summary>
        public ObservableCollection<StopViewModel> Stops { get; private set; }

        /// <summary>
        /// A collection for StopViewModel objects, containing all of the stops in the Transit System.
        /// </summary>
        public ObservableCollection<RouteStopViewModel> RouteStops { get; private set; }

        public Geolocator locator { get; private set; }
        public Geoposition location { get; private set; }
        public GeoCoordinate coordinates { get; private set; }

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }
        public bool IsStopsSorted
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few RouteViewModel objects into the Routes collection.
        /// </summary>
        public void LoadData()
        {
            // Adds Route data to Routes collection
            addRoutes();
            
            // Adds Stop data to Stops collection
            addStops();

            // Adds all RouteStops to collection
            addRouteStops();

            // Group the RouteStops into their respective Routes and Stops
            foreach(RouteStopViewModel routeStop in RouteStops)
            {
                int route = Int32.Parse(routeStop.RouteID);
                int stop = Int32.Parse((routeStop.StopID.Split('?'))[0]);
                
                if(routeStop.BusB)
                {
                    Routes[route].BusBStops.Add(routeStop);
                }
                else
                {
                    Routes[route].BusAStops.Add(routeStop);
                }

                
                Stops[stop].RouteStops.Add(routeStop);
            }
            
            // refresh the times and set the next stops
            refreshTimeUntil();

            this.IsDataLoaded = true;
        }

        #region AddingData
        public void addRoutes()
        {
            this.Routes.Add(new RouteViewModel() { RouteID = "0", Name = "route one", ShortName = "1", Desc = "Broad, Johnson, University Square", NotRunningMessage = "The one only runs on weekdays", BusAStops = new ObservableCollection<RouteStopViewModel>(), BusBStops = new ObservableCollection<RouteStopViewModel>(), RoutePath = new GeoCoordinateCollection() });
            this.Routes.Add(new RouteViewModel() { RouteID = "1", Name = "route two", ShortName = "2", Desc = "South Higuera, Suburban", NotRunningMessage = "", BusAStops = new ObservableCollection<RouteStopViewModel>(), BusBStops = new ObservableCollection<RouteStopViewModel>(), RoutePath = new GeoCoordinateCollection() });
            this.Routes.Add(new RouteViewModel() { RouteID = "2", Name = "route three", ShortName = "3", Desc = "Johnson, Broad, Marigold", NotRunningMessage = "", BusAStops = new ObservableCollection<RouteStopViewModel>(), BusBStops = new ObservableCollection<RouteStopViewModel>(), RoutePath = new GeoCoordinateCollection() });
            this.Routes.Add(new RouteViewModel() { RouteID = "3", Name = "route four", ShortName = "4", Desc = "Madonna, Laguna Lake, Cal Poly", NotRunningMessage = "", BusAStops = new ObservableCollection<RouteStopViewModel>(), BusBStops = new ObservableCollection<RouteStopViewModel>(), RoutePath = new GeoCoordinateCollection() });
            this.Routes.Add(new RouteViewModel() { RouteID = "4", Name = "route five", ShortName = "5", Desc = "Cal Poly, Laguna Lake, Madonna", NotRunningMessage = "", BusAStops = new ObservableCollection<RouteStopViewModel>(), BusBStops = new ObservableCollection<RouteStopViewModel>(), RoutePath = new GeoCoordinateCollection() });
            this.Routes.Add(new RouteViewModel() { RouteID = "5", Name = "route six a", ShortName = "6a", Desc = "Cal Poly, Highland", NotRunningMessage = "The 6a only runs on weekdays from June 14 to Labor Day and runs every day of the week except Sunday from Labor Day to June 13", BusAStops = new ObservableCollection<RouteStopViewModel>(), BusBStops = new ObservableCollection<RouteStopViewModel>(), RoutePath = new GeoCoordinateCollection() });
            this.Routes.Add(new RouteViewModel() { RouteID = "6", Name = "route six b", ShortName = "6b", Desc = "Cal Poly, Downtown", NotRunningMessage = "The 6b only runs on weekdays from June 14 to Labor Day and runs every day of the week except Sunday from Labor Day to June 13", BusAStops = new ObservableCollection<RouteStopViewModel>(), BusBStops = new ObservableCollection<RouteStopViewModel>(), RoutePath = new GeoCoordinateCollection() });
            //this.Routes.Add(new RouteViewModel() { RouteID = "7", Name = "route six e", ShortName = "6e", Desc = "Cal Poly, Downtown - Farmers Express", NotRunningMessage = "The 6e serves two stops and operates on thursday evenings only", BusAStops = new ObservableCollection<RouteStopViewModel>(), BusBStops = new ObservableCollection<RouteStopViewModel>(), RoutePath = new GeoCoordinateCollection() });

            #region Route One Line
            Routes[0].RoutePath.Add(new GeoCoordinate(35.261491, -120.635892));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.264942, -120.635820));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.264905, -120.638129));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.264966, -120.639830));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.265286, -120.640699));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.266863, -120.639519));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.267844, -120.643129));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.268198, -120.643719));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.273222, -120.648096));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.274295, -120.646348));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.280325, -120.651691));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.280509, -120.652238));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.280597, -120.653471));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.284306, -120.657286));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.283413, -120.658600));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.282620, -120.659968));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.283553, -120.660767));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.282712, -120.662178));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.283496, -120.662908));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.284341, -120.661486));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.290204, -120.666604));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.290966, -120.666920));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.293418, -120.667623));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.294079, -120.667896));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.294145, -120.677247));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.297928, -120.677209));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.298103, -120.677247));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.298260, -120.677354));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.298265, -120.679559));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.298274, -120.683561));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.296006, -120.683512));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.294140, -120.683550));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.294145, -120.677247));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.294079, -120.666754));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.294311, -120.665847));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.292726, -120.664839));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.292573, -120.664764));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.290931, -120.664790));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.290966, -120.666845));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.290935, -120.667033));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.290305, -120.666791));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.290217, -120.666620));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.285134, -120.662189));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.284310, -120.663605));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.283505, -120.662908));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.282703, -120.662183));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.280811, -120.665359));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.279739, -120.667156));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.277382, -120.665053));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.278114, -120.663819));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.275105, -120.661110));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.271229, -120.657747));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.270266, -120.657001));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.269810, -120.656561));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.266666, -120.654056));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.266512, -120.653970));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.265781, -120.653353));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.263407, -120.651540));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.261274, -120.649969));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.261318, -120.649325));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.261335, -120.648166));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.261331, -120.647195));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.261282, -120.646138));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.261318, -120.643767));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.261374, -120.643472));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.261353, -120.642271));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.261405, -120.641058));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.261431, -120.640420));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.261431, -120.639985));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.261458, -120.639545));
            Routes[0].RoutePath.Add(new GeoCoordinate(35.261491, -120.635892));
            #endregion

            #region Route Two Line
            Routes[1].RoutePath.Add(new GeoCoordinate(35.243995, -120.675411));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.254614, -120.669011));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.255096, -120.668839));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.258171, -120.668549));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.258592, -120.668603));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.261316, -120.669247));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.263690, -120.670416));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.263979, -120.670491));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.267238, -120.670416));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.268683, -120.670438));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.274797, -120.669333));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.278099, -120.663807));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.280341, -120.660117));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.281261, -120.658700));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.281979, -120.659419));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.282619, -120.659966));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.284300, -120.661479));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.283503, -120.662906));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.282706, -120.662176));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.281445, -120.664312));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.280806, -120.665341));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.279728, -120.667187));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.277399, -120.665041));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.276680, -120.664440));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.275892, -120.663721));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.272196, -120.669794));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.268596, -120.670448));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.267247, -120.670448));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.264014, -120.670513));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.263708, -120.670448));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.263550, -120.670719));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.263335, -120.670821));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.259262, -120.673289));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.258841, -120.673498));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.256931, -120.674169));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.256717, -120.674217));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.256577, -120.674002));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.254610, -120.669035));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.246812, -120.673702));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.246799, -120.671143));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.245993, -120.671202));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.245914, -120.671256));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.245883, -120.672624));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.245822, -120.672661));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.244034, -120.672613));
            Routes[1].RoutePath.Add(new GeoCoordinate(35.243995, -120.675411));
            #endregion

            #region Route Three Line
            Routes[2].RoutePath.Add(new GeoCoordinate(35.250333, -120.625113));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.250254, -120.626942));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.250263, -120.628348));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.250202, -120.629522));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.250202, -120.630638));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.250302, -120.632001));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.250250, -120.632687));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.250132, -120.633122));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.249685, -120.634334));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.249531, -120.635064));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.249501, -120.635520));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.249562, -120.636330));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.249733, -120.637601));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.249799, -120.638400));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.249794, -120.638878));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.249702, -120.639382));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.248357, -120.643014));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.251104, -120.644586));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.252598, -120.645278));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.255279, -120.646356));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.258415, -120.647949));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.263755, -120.651774));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.266545, -120.653963));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.266650, -120.654038));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.269808, -120.656538));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.270276, -120.656994));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.271231, -120.657734));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.271284, -120.657616));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.271376, -120.657519));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.271599, -120.657466));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.272217, -120.657412));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.273622, -120.657374));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.273728, -120.657380));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.273780, -120.657412));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.274516, -120.658104));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.279885, -120.662921));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.281461, -120.664300));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.282254, -120.664965));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.282968, -120.663806));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.283489, -120.662900));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.282701, -120.662181));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.283528, -120.660786));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.281965, -120.659424));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.281242, -120.658705));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.282158, -120.657224));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.282311, -120.656924));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.282985, -120.655964));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.280844, -120.653829));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.280590, -120.653469));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.280555, -120.653324));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.280489, -120.652203));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.280432, -120.651887));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.280375, -120.651774));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.280310, -120.651694));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.278978, -120.650492));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.278195, -120.649811));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.274297, -120.646340));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.269825, -120.642408));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.269584, -120.642123));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.269291, -120.641549));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.268560, -120.638969));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.268341, -120.638406));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.267723, -120.638851));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.263623, -120.641920));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.261582, -120.643389));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.261350, -120.643497));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.261346, -120.642274));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.261407, -120.641077));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.261425, -120.640418));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.261451, -120.639532));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.261468, -120.635890));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.261262, -120.635611));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.254372, -120.628492));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.254101, -120.628240));
            Routes[2].RoutePath.Add(new GeoCoordinate(35.250333, -120.625113));
            #endregion

            #region Route Four Line
            Routes[3].RoutePath.Add(new GeoCoordinate(35.278883, -120.65665));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.283483, -120.660767));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.2843, -120.66145));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.2835, -120.662867));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.282683, -120.66215));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.283483, -120.660767));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.2843, -120.66145));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.285933, -120.658717));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.286717, -120.659383));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.287767, -120.65935));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.28775, -120.658233));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.28675, -120.6573));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.289583, -120.652517));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.29035, -120.653117));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.290533, -120.653117));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.296317, -120.653067));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.296933, -120.653267));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.297283, -120.653583));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.299333, -120.656533));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.30005, -120.65705));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.30025, -120.657333));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.30045, -120.658067));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.3009, -120.658));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.301367, -120.65805));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.301833, -120.658333));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.3023, -120.658717));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.302567, -120.659017));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.302767, -120.659417));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.302883, -120.6598));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.3017, -120.666383));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.301067, -120.666333));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.300017, -120.666133));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.2971, -120.665217));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.296417, -120.664933));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.295, -120.663817));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.294167, -120.6661));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.294033, -120.666817));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.294083, -120.671683));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.292783, -120.671733));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.292767, -120.67415));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.292867, -120.674783));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.292867, -120.678033));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.29415, -120.678017));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.29415, -120.685533));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.294017, -120.686233));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.293817, -120.6866));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.292233, -120.688367));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.29195, -120.688767));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.2914, -120.689983));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.289867, -120.692317));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.287883, -120.6965));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.286367, -120.700267));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.286217, -120.700533));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.284317, -120.70295));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.28395, -120.703683));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.2821, -120.707833));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.281933, -120.7081));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.281733, -120.708367));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.28145, -120.70865));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.281067, -120.70895));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.277533, -120.711033));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.276617, -120.711617));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.275733, -120.71065));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.274883, -120.7099));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.272717, -120.707983));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.2682, -120.703967));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.267167, -120.70285));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.26345, -120.698433));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.261633, -120.70075));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.262183, -120.7013));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.26285, -120.701717));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.263167, -120.702067));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.2648, -120.70005));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.26345, -120.698433));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.2634, -120.698283));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.2583, -120.6922));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.25805, -120.691933));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.256767, -120.690883));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.256583, -120.690833));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.255033, -120.6897));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.253767, -120.6887));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.250533, -120.685833));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.250417, -120.685667));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.249867, -120.685167));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.250667, -120.68375));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.250583, -120.6834));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.250883, -120.6833));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.25095, -120.68365));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.250667, -120.68375));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.249867, -120.685167));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.250417, -120.685667));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.250417, -120.685667));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.256617, -120.690733));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.26015, -120.682567));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.2612, -120.680283));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.261267, -120.68005));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.260667, -120.679233));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.26195, -120.67775));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.262433, -120.678383));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.264483, -120.675567));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.2646, -120.67555));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.266167, -120.6734));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.26825, -120.6704));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.269517, -120.6703));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.269517, -120.663833));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.269467, -120.663733));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.269483, -120.660817));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.269517, -120.660617));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.2695, -120.6589));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.269483, -120.65875));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.269483, -120.657283));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.26965, -120.656633));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.269867, -120.6564));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.27005, -120.65635));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.270783, -120.6563));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.272817, -120.656133));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.274317, -120.65615));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.27545, -120.656367));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.2765, -120.656633));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.277333, -120.657317));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.27805, -120.657983));
            Routes[3].RoutePath.Add(new GeoCoordinate(35.278883, -120.65665));
            #endregion

            #region Route Five Line
            Routes[4].RoutePath.Add(new GeoCoordinate(35.276633, -120.654667));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.283483, -120.660767));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.2843, -120.66145));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.2835, -120.662867));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.282683, -120.66215));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.283483, -120.660767));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.2843, -120.66145));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.285933, -120.658717));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.286717, -120.659383));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.287767, -120.65935));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.28775, -120.658233));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.28675, -120.6573));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.289583, -120.652517));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.29035, -120.653117));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.290533, -120.653117));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.296317, -120.653067));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.296933, -120.653267));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.297283, -120.653583));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.299333, -120.656533));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.30005, -120.65705));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.30025, -120.657333));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.30045, -120.658067));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.3009, -120.658));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.301367, -120.65805));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.301833, -120.658333));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.3023, -120.658717));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.302567, -120.659017));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.302767, -120.659417));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.302883, -120.6598));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.3017, -120.666383));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.301067, -120.666333));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.300017, -120.666133));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.2971, -120.665217));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.296417, -120.664933));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.295, -120.663817));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.294167, -120.6661));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.294033, -120.666817));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.29415, -120.685533));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.294017, -120.686233));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.293817, -120.6866));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.292233, -120.688367));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.29195, -120.688767));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.2914, -120.689983));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.289867, -120.692317));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.287883, -120.6965));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.286367, -120.700267));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.286217, -120.700533));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.284317, -120.70295));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.28395, -120.703683));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.2821, -120.707833));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.281933, -120.7081));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.281733, -120.708367));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.28145, -120.70865));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.281067, -120.70895));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.277533, -120.711033));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.276617, -120.711617));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.275733, -120.71065));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.274883, -120.7099));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.272717, -120.707983));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.2682, -120.703967));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.267167, -120.70285));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.26345, -120.698433));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.261633, -120.70075));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.262183, -120.7013));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.26285, -120.701717));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.263167, -120.702067));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.2648, -120.70005));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.26345, -120.698433));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.2634, -120.698283));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.2583, -120.6922));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.25805, -120.691933));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.256767, -120.690883));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.256583, -120.690833));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.255033, -120.6897));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.253767, -120.6887));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.250533, -120.685833));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.250417, -120.685667));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.249867, -120.685167));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.250667, -120.68375));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.250583, -120.6834));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.250883, -120.6833));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.25095, -120.68365));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.250667, -120.68375));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.249867, -120.685167));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.250417, -120.685667));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.250417, -120.685667));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.256617, -120.690733));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.26015, -120.682567));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.2612, -120.680283));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.261267, -120.68005));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.260667, -120.679233));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.26195, -120.67775));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.262433, -120.678383));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.264483, -120.675567));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.2646, -120.67555));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.266167, -120.6734));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.26825, -120.6704));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.269517, -120.6703));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.269517, -120.663833));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.269467, -120.663733));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.269483, -120.660817));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.269517, -120.660617));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.2695, -120.6589));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.269483, -120.65875));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.269483, -120.657283));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.26965, -120.656633));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.269867, -120.6564));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.27005, -120.65635));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.270783, -120.6563));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.272817, -120.656133));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.274317, -120.65615));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.27545, -120.656367));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.276083, -120.656517));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.2761, -120.6563));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.276067, -120.6562));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.275417, -120.655583));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.27535, -120.6556));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.2753, -120.655433));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.2754, -120.655383));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.275467, -120.6553));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.276033, -120.65495));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.276283, -120.6549));
            Routes[4].RoutePath.Add(new GeoCoordinate(35.276633, -120.654667));
            #endregion

            #region Route Six A Line
            Routes[5].RoutePath.Add(new GeoCoordinate(35.298280, -120.683566));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.294129, -120.683557));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.294133, -120.678075));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.292911, -120.678091));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.292933, -120.677265));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.292916, -120.674771));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.292872, -120.674556));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.292784, -120.674315));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.292789, -120.671670));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.294089, -120.671681));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.294059, -120.668070));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.293305, -120.667759));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.292434, -120.667480));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.290919, -120.667030));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.290959, -120.666831));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.290959, -120.666434));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.290941, -120.666177));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.290924, -120.664782));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.292570, -120.664750));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.292745, -120.664814));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.293380, -120.665265));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.294313, -120.665796));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.294930, -120.664138));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.295031, -120.663859));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.295609, -120.664374));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.296414, -120.664948));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.296677, -120.665082));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.297378, -120.665336));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.297823, -120.665406));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.299765, -120.666080));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.301603, -120.666408));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.301695, -120.666397));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.302383, -120.663098));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.303867, -120.663511));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.303959, -120.663495));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.303981, -120.663683));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.303718, -120.664493));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.303538, -120.664745));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.303276, -120.664991));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.303171, -120.665201));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.303079, -120.665447));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.303013, -120.665759));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.302947, -120.666091));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.302816, -120.666402));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.302707, -120.666574));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.302615, -120.666783));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.302597, -120.667094));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.302466, -120.667872));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.302317, -120.668156));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.302146, -120.668489));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.302006, -120.668661));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.300215, -120.670420));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.299983, -120.670592));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.299401, -120.670914));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.299117, -120.671155));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.298976, -120.671434));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.298928, -120.671681));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.298898, -120.672577));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.298836, -120.672840));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.298477, -120.673875));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.298079, -120.674985));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.298061, -120.675441));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.298101, -120.675736));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.298250, -120.676407));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.298267, -120.677351));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.298289, -120.677603));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.298267, -120.679465));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.298293, -120.680897));
            Routes[5].RoutePath.Add(new GeoCoordinate(35.298280, -120.683566));
            #endregion

            #region Route Six B Line
            Routes[6].RoutePath.Add(new GeoCoordinate(35.296036, -120.653254));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.292380, -120.653287));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.291438, -120.653300));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.290416, -120.653292));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.290307, -120.653270));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.290166, -120.653209));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.289567, -120.652656));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.288857, -120.653777));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.288316, -120.654743));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.287609, -120.655947));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.286764, -120.657347));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.287708, -120.658165));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.287780, -120.658294));

            Routes[6].RoutePath.Add(new GeoCoordinate(35.287792, -120.659352));

            Routes[6].RoutePath.Add(new GeoCoordinate(35.286780, -120.659387));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.286651, -120.659354));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.285941, -120.658729));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.285131, -120.660121));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.284319, -120.661468));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.283491, -120.662892));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.282701, -120.662173));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.283535, -120.660757));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.284319, -120.661468));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.285131, -120.660121));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.285941, -120.658729));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.286651, -120.659354));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.286780, -120.659387));

            Routes[6].RoutePath.Add(new GeoCoordinate(35.287792, -120.659352));

            Routes[6].RoutePath.Add(new GeoCoordinate(35.287780, -120.658294));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.287708, -120.658165));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.286764, -120.657347));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.287609, -120.655947));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.288396, -120.656627));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.288753, -120.656999));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.288956, -120.657335));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.289488, -120.658708));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.289637, -120.659014));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.289911, -120.659381));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.290174, -120.659676));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.290903, -120.660296));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.292019, -120.661251));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.295036, -120.663877));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.295596, -120.664378));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.296408, -120.664963));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.296651, -120.665094));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.297372, -120.665352));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.297818, -120.665422));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.298506, -120.665698));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.299749, -120.666087));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.301599, -120.666409));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.301702, -120.666411));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.302393, -120.663107));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.302842, -120.660725));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.302897, -120.660395));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.302932, -120.660132));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.302936, -120.659907));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.302855, -120.659582));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.302582, -120.658990));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.302413, -120.658813));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.302096, -120.658563));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.301905, -120.658394));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.301695, -120.658244));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.301481, -120.658137));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.301310, -120.658078));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.301027, -120.658035));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.300800, -120.658040));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.300469, -120.658091));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.300390, -120.657589));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.300336, -120.657437));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.300259, -120.657313));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.300101, -120.657120));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.299322, -120.656533));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.297297, -120.653655));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.297024, -120.653373));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.296862, -120.653247));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.296669, -120.653244));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.296419, -120.653207));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.296260, -120.653236));
            Routes[6].RoutePath.Add(new GeoCoordinate(35.296036, -120.653254));
            #endregion

            #region Route Six E Line
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.296036, -120.653254));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.292380, -120.653287));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.291438, -120.653300));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.290416, -120.653292));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.290307, -120.653270));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.290166, -120.653209));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.289567, -120.652656));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.288857, -120.653777));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.288316, -120.654743));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.287609, -120.655947));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.286764, -120.657347));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.287708, -120.658165));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.287780, -120.658294));

            //Routes[7].RoutePath.Add(new GeoCoordinate(35.287792, -120.659352));

            //Routes[7].RoutePath.Add(new GeoCoordinate(35.286780, -120.659387));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.287792, -120.659352));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.286651, -120.659354));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.285941, -120.658729));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.285131, -120.660121));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.284319, -120.661468));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.283491, -120.662892));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.282701, -120.662173));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.283535, -120.660757));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.284319, -120.661468));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.289123, -120.665668));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.290022, -120.666476));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.290224, -120.666610));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.290493, -120.666749));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.290951, -120.666910));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.292455, -120.667331));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.293398, -120.667618));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.294068, -120.667892));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.294068, -120.666827));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.294084, -120.666645));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.294311, -120.665832));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.294946, -120.664137));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.295027, -120.663879));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.295596, -120.664378));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.296408, -120.664963));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.296651, -120.665094));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.297372, -120.665352));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.297818, -120.665422));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.298506, -120.665698));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.299749, -120.666087));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.301599, -120.666409));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.301702, -120.666411));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.302393, -120.663107));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.302842, -120.660725));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.302897, -120.660395));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.302932, -120.660132));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.302936, -120.659907));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.302855, -120.659582));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.302582, -120.658990));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.302413, -120.658813));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.302096, -120.658563));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.301905, -120.658394));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.301695, -120.658244));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.301481, -120.658137));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.301310, -120.658078));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.301027, -120.658035));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.300800, -120.658040));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.300469, -120.658091));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.300390, -120.657589));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.300336, -120.657437));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.300259, -120.657313));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.300101, -120.657120));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.299322, -120.656533));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.297297, -120.653655));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.297024, -120.653373));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.296862, -120.653247));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.296669, -120.653244));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.296419, -120.653207));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.296260, -120.653236));
            //Routes[7].RoutePath.Add(new GeoCoordinate(35.296036, -120.653254));
            #endregion
        }
        public void addStops()
        {
            this.Stops.Add(new StopViewModel() { StopID = "0", Index = 0, Location = new GeoCoordinate(35.276123, -120.654881), Name = "Amtrak Station", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "1", Index = 1, Location = new GeoCoordinate(35.266626, -120.654267), Name = "Broad at Caudill", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "2", Index = 2, Location = new GeoCoordinate(35.250230, -120.643890), Name = "Broad at Marigold Center", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "3", Index = 3, Location = new GeoCoordinate(35.270226, -120.656746), Name = "Broad at Santa Barbara", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "4", Index = 4, Location = new GeoCoordinate(35.291329, -120.660525), Name = "California at Taft", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "5", Index = 5, Location = new GeoCoordinate(35.292971, -120.664899), Name = "Casa at Deseret", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "6", Index = 6, Location = new GeoCoordinate(35.264652, -120.700099), Name = "Descanso at LOVR", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "7", Index = 7, Location = new GeoCoordinate(35.282911, -120.662414), Name = "Downtown Transit Center", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "8", Index = 8, Location = new GeoCoordinate(35.294208, -120.669191), Name = "Foothill at University Square", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "9", Index = 9, Location = new GeoCoordinate(35.270204, -120.670194), Name = "Higuera at South", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "10", Index = 10, Location = new GeoCoordinate(35.24434, -120.674968), Name = "Higuera at Suburban", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "11", Index = 11, Location = new GeoCoordinate(35.278726, -120.650236), Name = "Johnson at Lizzie", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "12", Index = 12, Location = new GeoCoordinate(35.302318, -120.663164), Name = "Kennedy Library", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "13", Index = 13, Location = new GeoCoordinate(35.251993, -120.687358), Name = "LOVR at Irish Hills", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "14", Index = 14, Location = new GeoCoordinate(35.259815, -120.693993), Name = "LOVR at Oceanaire", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "15", Index = 15, Location = new GeoCoordinate(35.261295, -120.643034), Name = "Orcutt at Laurel", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "16", Index = 16, Location = new GeoCoordinate(35.294431, -120.683541), Name = "Patricia at Foothill", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "17", Index = 17, Location = new GeoCoordinate(35.300079, -120.657146), Name = "Performing Arts Center", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "18", Index = 18, Location = new GeoCoordinate(35.256047, -120.672943), Name = "Prado Day Center", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "19", Index = 19, Location = new GeoCoordinate(35.261470, -120.678144), Name = "Promenade", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "20", Index = 20, Location = new GeoCoordinate(35.292684, -120.674054), Name = "Ramona at Palomar", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "21", Index = 21, Location = new GeoCoordinate(35.290876, -120.666936), Name = "Santa Rosa at Murray", RouteStops = new ObservableCollection<RouteStopViewModel>() });
            this.Stops.Add(new StopViewModel() { StopID = "22", Index = 22, Location = new GeoCoordinate(35.269613, -120.669153), Name = "South at Parker", RouteStops = new ObservableCollection<RouteStopViewModel>() });
        }
        public void addRouteStops()
        {
            // Create Reference Days based on Today
            DateTime BaseDate = DateTime.Today;
            int BaseYear = Int32.Parse(BaseDate.Year.ToString());
            int BaseMonth = Int32.Parse(BaseDate.Month.ToString());
            int BaseDay = Int32.Parse(BaseDate.Day.ToString());

            DateTime Jun14 = new DateTime(BaseYear, 6, 14);
            DateTime Jun15 = new DateTime(BaseYear, 6, 15);
            DateTime LaborDay = new DateTime(BaseYear, 9, 1);
            while (LaborDay.DayOfWeek != DayOfWeek.Monday) { LaborDay = LaborDay.AddDays(1); }
            DateTime JanFirst = new DateTime(BaseYear, 1, 1);
            DateTime DecLast = new DateTime(BaseYear, 12, 31);

            // Add all RouteStops
            int HourAdding = 0;
            while (HourAdding < 24)
            {
                // Route One
                if (HourAdding >= 7 && HourAdding <= 17)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 15, 0), RouteID = "0", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 25, 0), RouteID = "0", StopID = "1", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 31, 0), RouteID = "0", StopID = "15", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 40, 0), RouteID = "0", StopID = "11", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 47, 0), RouteID = "0", StopID = "7", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 53, 0), RouteID = "0", StopID = "8", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 58, 0), RouteID = "0", StopID = "16", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 4, 0), RouteID = "0", StopID = "21", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 9, 0), RouteID = "0", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                }

                
                if (HourAdding == 6)
                {
                    // Weekday - Route Two
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 3, 0), RouteID = "1", StopID = "10", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 10, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 20, 0), RouteID = "1", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 25, 0), RouteID = "1", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 33, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 37, 0), RouteID = "1", StopID = "18", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 43, 0), RouteID = "1", StopID = "10", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 50, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 0, 0), RouteID = "1", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 5, 0), RouteID = "1", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 13, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 17, 0), RouteID = "1", StopID = "18", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 23, 0), RouteID = "1", StopID = "10", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 30, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 40, 0), RouteID = "1", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 45, 0), RouteID = "1", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 53, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 57, 0), RouteID = "1", StopID = "18", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });

                    // Weekday - Route Three
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 4, 0), RouteID = "2", StopID = "2", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 10, 0), RouteID = "2", StopID = "3", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 17, 0), RouteID = "2", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 25, 0), RouteID = "2", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 31, 0), RouteID = "2", StopID = "11", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 37, 0), RouteID = "2", StopID = "15", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 44, 0), RouteID = "2", StopID = "2", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 50, 0), RouteID = "2", StopID = "3", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 57, 0), RouteID = "2", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 5, 0), RouteID = "2", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 11, 0), RouteID = "2", StopID = "11", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 17, 0), RouteID = "2", StopID = "15", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 24, 0), RouteID = "2", StopID = "2", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 30, 0), RouteID = "2", StopID = "3", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 37, 0), RouteID = "2", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 45, 0), RouteID = "2", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 51, 0), RouteID = "2", StopID = "11", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 57, 0), RouteID = "2", StopID = "15", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                }
                else if (HourAdding == 8 || HourAdding == 10 || HourAdding == 12 || HourAdding == 14)
                {
                    // Everyday - Route Two
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 3, 0), RouteID = "1", StopID = "10", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 10, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 20, 0), RouteID = "1", StopID = "7?Arrive", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 25, 0), RouteID = "1", StopID = "7?Depart", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 33, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 37, 0), RouteID = "1", StopID = "18", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 43, 0), RouteID = "1", StopID = "10", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 50, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 0, 0), RouteID = "1", StopID = "7?Arrive", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 5, 0), RouteID = "1", StopID = "7?Depart", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 13, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 17, 0), RouteID = "1", StopID = "18", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 23, 0), RouteID = "1", StopID = "10", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 30, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 40, 0), RouteID = "1", StopID = "7?Arrive", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 45, 0), RouteID = "1", StopID = "7?Depart", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 53, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 57, 0), RouteID = "1", StopID = "18", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });

                    // Everyday - Route Three
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 4, 0), RouteID = "2", StopID = "2", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 10, 0), RouteID = "2", StopID = "3", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 17, 0), RouteID = "2", StopID = "7?Arrive", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 25, 0), RouteID = "2", StopID = "7?Depart", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 31, 0), RouteID = "2", StopID = "11", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 37, 0), RouteID = "2", StopID = "15", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 44, 0), RouteID = "2", StopID = "2", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 50, 0), RouteID = "2", StopID = "3", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 57, 0), RouteID = "2", StopID = "7?Arrive", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 5, 0), RouteID = "2", StopID = "7?Depart", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 11, 0), RouteID = "2", StopID = "11", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 17, 0), RouteID = "2", StopID = "15", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 24, 0), RouteID = "2", StopID = "2", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 30, 0), RouteID = "2", StopID = "3", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 37, 0), RouteID = "2", StopID = "7?Arrive", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 45, 0), RouteID = "2", StopID = "7?Depart", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 51, 0), RouteID = "2", StopID = "11", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 57, 0), RouteID = "2", StopID = "15", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                }
                else if (HourAdding == 16)
                {
                    // Everyday - Route Two
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 3, 0), RouteID = "1", StopID = "10", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 10, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 20, 0), RouteID = "1", StopID = "7?Arrive", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 25, 0), RouteID = "1", StopID = "7?Depart", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 33, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 37, 0), RouteID = "1", StopID = "18", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 43, 0), RouteID = "1", StopID = "10", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 50, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 0, 0), RouteID = "1", StopID = "7?Arrive", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    
                    // Everyday - Route Three
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 4, 0), RouteID = "2", StopID = "2", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 10, 0), RouteID = "2", StopID = "3", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 17, 0), RouteID = "2", StopID = "7?Arrive", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 25, 0), RouteID = "2", StopID = "7?Depart", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 31, 0), RouteID = "2", StopID = "11", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 37, 0), RouteID = "2", StopID = "15", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 44, 0), RouteID = "2", StopID = "2", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 50, 0), RouteID = "2", StopID = "3", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 57, 0), RouteID = "2", StopID = "7?Arrive", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 5, 0), RouteID = "2", StopID = "7?Depart", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 11, 0), RouteID = "2", StopID = "11", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 17, 0), RouteID = "2", StopID = "15", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 24, 0), RouteID = "2", StopID = "2", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 30, 0), RouteID = "2", StopID = "3", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 37, 0), RouteID = "2", StopID = "7?Arrive", Weekday = true, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });

                    // Not Sunday - Route Two
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 5, 0), RouteID = "1", StopID = "7?Depart", Weekday = true, Saturday = true, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 13, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = true, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 17, 0), RouteID = "1", StopID = "18", Weekday = true, Saturday = true, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 23, 0), RouteID = "1", StopID = "10", Weekday = true, Saturday = true, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 30, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = true, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 40, 0), RouteID = "1", StopID = "7?Arrive", Weekday = true, Saturday = true, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    
                    // Weekdays - Route Three
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 45, 0), RouteID = "2", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 51, 0), RouteID = "2", StopID = "11", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 57, 0), RouteID = "2", StopID = "15", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 2, 4, 0), RouteID = "2", StopID = "2", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 2, 10, 0), RouteID = "2", StopID = "3", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 2, 17, 0), RouteID = "2", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                }
                
                if (HourAdding >= 18 && HourAdding <= 20)
                {
                    // Evening - Route Two
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 50, 0), RouteID = "1", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 56, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 3, 0), RouteID = "1", StopID = "10", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 8, 0), RouteID = "1", StopID = "9", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 18, 0), RouteID = "1", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                }
                if (HourAdding >= 18 && HourAdding <= 21)
                {
                    // Evening - Route Three
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 18, 0), RouteID = "2", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 22, 0), RouteID = "2", StopID = "11", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 28, 0), RouteID = "2", StopID = "15", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 34, 0), RouteID = "2", StopID = "2", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 39, 0), RouteID = "2", StopID = "3", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 45, 0), RouteID = "2", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                }

                // Daytime - Route Four
                    // Four First
                if (HourAdding == 6)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 34, 0), RouteID = "3", StopID = "14", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 44, 0), RouteID = "3", StopID = "20", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 53, 0), RouteID = "3", StopID = "12?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 55, 0), RouteID = "3", StopID = "12?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 5, 0), RouteID = "3", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                }
                
                if (HourAdding >= 7 && HourAdding <= 16)
                {
                    // Four First
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 10, 0), RouteID = "3", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 17, 0), RouteID = "3", StopID = "22", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 22, 0), RouteID = "3", StopID = "19", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 28, 0), RouteID = "3", StopID = "13", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 34, 0), RouteID = "3", StopID = "14", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 44, 0), RouteID = "3", StopID = "20", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 53, 0), RouteID = "3", StopID = "12?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 55, 0), RouteID = "3", StopID = "12?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 5, 0), RouteID = "3", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                
                    // Four Second
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 40, 0), RouteID = "3", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 47, 0), RouteID = "3", StopID = "22", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 52, 0), RouteID = "3", StopID = "19", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 58, 0), RouteID = "3", StopID = "13", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 04, 0), RouteID = "3", StopID = "14", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 08, 0), RouteID = "3", StopID = "6", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 14, 0), RouteID = "3", StopID = "20", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 23, 0), RouteID = "3", StopID = "12?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 25, 0), RouteID = "3", StopID = "12?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 35, 0), RouteID = "3", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                }
                if (HourAdding == 17)
                {
                    // Four First
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 10, 0), RouteID = "3", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 17, 0), RouteID = "3", StopID = "22", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 22, 0), RouteID = "3", StopID = "19", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 28, 0), RouteID = "3", StopID = "13", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 34, 0), RouteID = "3", StopID = "14", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 44, 0), RouteID = "3", StopID = "20", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 53, 0), RouteID = "3", StopID = "12?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 5, 0), RouteID = "3", StopID = "12?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 15, 0), RouteID = "3", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                
                    // Four Second
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 40, 0), RouteID = "3", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 47, 0), RouteID = "3", StopID = "22", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 52, 0), RouteID = "3", StopID = "19", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 58, 0), RouteID = "3", StopID = "13", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 04, 0), RouteID = "3", StopID = "14", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 08, 0), RouteID = "3", StopID = "6", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                }
                
                // Evening - Route Four
                if (HourAdding >= 18 && HourAdding <= 21)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 20, 0), RouteID = "3", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 27, 0), RouteID = "3", StopID = "22", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 32, 0), RouteID = "3", StopID = "19", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 38, 0), RouteID = "3", StopID = "13", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 44, 0), RouteID = "3", StopID = "14", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 54, 0), RouteID = "3", StopID = "20", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 3, 0), RouteID = "3", StopID = "12?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 5, 0), RouteID = "3", StopID = "12?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 15, 0), RouteID = "3", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                }
                if (HourAdding == 22)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 20, 0), RouteID = "3", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 27, 0), RouteID = "3", StopID = "22", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 32, 0), RouteID = "3", StopID = "19", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 38, 0), RouteID = "3", StopID = "13", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 44, 0), RouteID = "3", StopID = "14", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                }
                // Weekends - Route Four
                if (HourAdding >= 8 && HourAdding <= 17)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 10, 0), RouteID = "3", StopID = "7?Depart", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 17, 0), RouteID = "3", StopID = "22", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 22, 0), RouteID = "3", StopID = "19", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 28, 0), RouteID = "3", StopID = "13", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 34, 0), RouteID = "3", StopID = "14", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 44, 0), RouteID = "3", StopID = "20", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 53, 0), RouteID = "3", StopID = "12?Arrive", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 55, 0), RouteID = "3", StopID = "12?Depart", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 5, 0), RouteID = "3", StopID = "7?Arrive", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                }
                // Weekdays - Route Five
                if (HourAdding == 6)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 20, 0), RouteID = "4", StopID = "6", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 21, 0), RouteID = "4", StopID = "14", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 24, 0), RouteID = "4", StopID = "13", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 32, 0), RouteID = "4", StopID = "19", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 36, 0), RouteID = "4", StopID = "22", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 41, 0), RouteID = "4", StopID = "0", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 47, 0), RouteID = "4", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                }
                if (HourAdding >= 6 && HourAdding <= 10)
                {
                    // Five First
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 50, 0), RouteID = "4", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 58, 0), RouteID = "4", StopID = "17?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 3, 0), RouteID = "4", StopID = "17?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 11, 0), RouteID = "4", StopID = "8", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 20, 0), RouteID = "4", StopID = "6", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 21, 0), RouteID = "4", StopID = "14", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 24, 0), RouteID = "4", StopID = "13", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 32, 0), RouteID = "4", StopID = "19", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 36, 0), RouteID = "4", StopID = "22", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 41, 0), RouteID = "4", StopID = "0", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 47, 0), RouteID = "4", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });

                    // Five Second
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 20, 0), RouteID = "4", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 28, 0), RouteID = "4", StopID = "17?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 33, 0), RouteID = "4", StopID = "17?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 41, 0), RouteID = "4", StopID = "8", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 50, 0), RouteID = "4", StopID = "6", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 51, 0), RouteID = "4", StopID = "14", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 54, 0), RouteID = "4", StopID = "13", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 2, 2, 0), RouteID = "4", StopID = "19", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 2, 6, 0), RouteID = "4", StopID = "22", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 2, 11, 0), RouteID = "4", StopID = "0", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 2, 17, 0), RouteID = "4", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                }
                if (HourAdding >= 11 && HourAdding <= 17)
                {
                    // Five First
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 50, 0), RouteID = "4", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 58, 0), RouteID = "4", StopID = "17?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 3, 0), RouteID = "4", StopID = "17?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 11, 0), RouteID = "4", StopID = "8", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 20, 0), RouteID = "4", StopID = "6", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 21, 0), RouteID = "4", StopID = "14", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    // Stop at :24 and LOVR Irish Hills Not Serviced After 12 Noon
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 32, 0), RouteID = "4", StopID = "19", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 36, 0), RouteID = "4", StopID = "22", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 41, 0), RouteID = "4", StopID = "0", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 47, 0), RouteID = "4", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });

                    // Five Second
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 20, 0), RouteID = "4", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 28, 0), RouteID = "4", StopID = "17?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 33, 0), RouteID = "4", StopID = "17?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 41, 0), RouteID = "4", StopID = "8", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 50, 0), RouteID = "4", StopID = "6", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 51, 0), RouteID = "4", StopID = "14", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 54, 0), RouteID = "4", StopID = "13", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 2, 2, 0), RouteID = "4", StopID = "19", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 2, 6, 0), RouteID = "4", StopID = "22", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 2, 11, 0), RouteID = "4", StopID = "0", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 2, 17, 0), RouteID = "4", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast, BusB = true });
                }
                if (HourAdding == 18)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 50, 0), RouteID = "4", StopID = "7", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 58, 0), RouteID = "4", StopID = "17?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 3, 0), RouteID = "4", StopID = "17?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 11, 0), RouteID = "4", StopID = "8", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 20, 0), RouteID = "4", StopID = "6", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 21, 0), RouteID = "4", StopID = "14", Weekday = true, Saturday = false, Sunday = false, DateStart = JanFirst, DateEnd = DecLast });
                }
                // Weekends - Route Five
                if (HourAdding >= 8 && HourAdding <= 17)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 20, 0), RouteID = "4", StopID = "7", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 28, 0), RouteID = "4", StopID = "17?Arrive", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 33, 0), RouteID = "4", StopID = "17?Depart", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 41, 0), RouteID = "4", StopID = "8", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 50, 0), RouteID = "4", StopID = "6", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 51, 0), RouteID = "4", StopID = "14", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 54, 0), RouteID = "4", StopID = "13", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 2, 0), RouteID = "4", StopID = "19", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 6, 0), RouteID = "4", StopID = "22", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 11, 0), RouteID = "4", StopID = "0", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding + 1, 17, 0), RouteID = "4", StopID = "7", Weekday = false, Saturday = true, Sunday = true, DateStart = JanFirst, DateEnd = DecLast });
                }

                // Weekdays - Route Six-A
                if (HourAdding == 7)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 16, 0), RouteID = "5", StopID = "16", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 19, 0), RouteID = "5", StopID = "20", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 25, 0), RouteID = "5", StopID = "5", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 29, 0), RouteID = "5", StopID = "12?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 40, 0), RouteID = "5", StopID = "12?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 46, 0), RouteID = "5", StopID = "16", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 49, 0), RouteID = "5", StopID = "20", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 55, 0), RouteID = "5", StopID = "5", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 59, 0), RouteID = "5", StopID = "12?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });    
                }
                if (HourAdding >= 8 && HourAdding <= 17)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 10, 0), RouteID = "5", StopID = "12?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 16, 0), RouteID = "5", StopID = "16", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 19, 0), RouteID = "5", StopID = "20", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 25, 0), RouteID = "5", StopID = "5", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 29, 0), RouteID = "5", StopID = "12?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 40, 0), RouteID = "5", StopID = "12?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 46, 0), RouteID = "5", StopID = "16", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 49, 0), RouteID = "5", StopID = "20", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 55, 0), RouteID = "5", StopID = "5", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 59, 0), RouteID = "5", StopID = "12?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });    
                }
                if (HourAdding >= 18 && HourAdding <= 22)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 10, 0), RouteID = "5", StopID = "12?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 16, 0), RouteID = "5", StopID = "16", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 19, 0), RouteID = "5", StopID = "20", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 25, 0), RouteID = "5", StopID = "5", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 29, 0), RouteID = "5", StopID = "12?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                }
                // Saturdays - Route Six-A
                if (HourAdding >= 9 && HourAdding <= 17)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 10, 0), RouteID = "5", StopID = "12?Depart", Weekday = false, Saturday = true, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 16, 0), RouteID = "5", StopID = "16", Weekday = false, Saturday = true, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 19, 0), RouteID = "5", StopID = "20", Weekday = false, Saturday = true, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 25, 0), RouteID = "5", StopID = "5", Weekday = false, Saturday = true, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 29, 0), RouteID = "5", StopID = "12?Arrive", Weekday = false, Saturday = true, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                }
                // Jun15 - LaborDay
                if (HourAdding >= 9 && HourAdding <= 17)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 10, 0), RouteID = "5", StopID = "12?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = Jun15, DateEnd = LaborDay });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 16, 0), RouteID = "5", StopID = "16", Weekday = true, Saturday = false, Sunday = false, DateStart = Jun15, DateEnd = LaborDay });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 19, 0), RouteID = "5", StopID = "20", Weekday = true, Saturday = false, Sunday = false, DateStart = Jun15, DateEnd = LaborDay });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 25, 0), RouteID = "5", StopID = "5", Weekday = true, Saturday = false, Sunday = false, DateStart = Jun15, DateEnd = LaborDay });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 29, 0), RouteID = "5", StopID = "12?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = Jun15, DateEnd = LaborDay });
                }

                // Route Six B
                if (HourAdding >= 7 && HourAdding <= 17)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 2, 0), RouteID = "6", StopID = "12?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 9, 0), RouteID = "6", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 15, 0), RouteID = "6", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 20, 0), RouteID = "6", StopID = "4", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 26, 0), RouteID = "6", StopID = "12?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 32, 0), RouteID = "6", StopID = "12?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 39, 0), RouteID = "6", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 45, 0), RouteID = "6", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 50, 0), RouteID = "6", StopID = "4", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 56, 0), RouteID = "6", StopID = "12?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                
                }
                if (HourAdding == 18)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 2, 0), RouteID = "6", StopID = "12?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 9, 0), RouteID = "6", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                }
                if (HourAdding >= 18 && HourAdding <= 22)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 32, 0), RouteID = "6", StopID = "12?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 39, 0), RouteID = "6", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 45, 0), RouteID = "6", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 50, 0), RouteID = "6", StopID = "4", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 56, 0), RouteID = "6", StopID = "12?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                }
                // Evenings - Route SixB
                if (HourAdding == 8)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 45, 0), RouteID = "6", StopID = "7?Depart", Weekday = false, Saturday = true, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 50, 0), RouteID = "6", StopID = "4", Weekday = false, Saturday = true, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 56, 0), RouteID = "6", StopID = "12?Arrive", Weekday = false, Saturday = true, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                }
                if (HourAdding >= 9 && HourAdding <= 17)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 32, 0), RouteID = "6", StopID = "12?Depart", Weekday = false, Saturday = true, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 39, 0), RouteID = "6", StopID = "7?Arrive", Weekday = false, Saturday = true, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 45, 0), RouteID = "6", StopID = "7?Depart", Weekday = false, Saturday = true, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 50, 0), RouteID = "6", StopID = "4", Weekday = false, Saturday = true, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 56, 0), RouteID = "6", StopID = "12?Arrive", Weekday = false, Saturday = true, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                }
                // Jun15 - LaborDay Route SixB
                if (HourAdding == 8)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 45, 0), RouteID = "6", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = Jun15, DateEnd = LaborDay });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 50, 0), RouteID = "6", StopID = "4", Weekday = true, Saturday = false, Sunday = false, DateStart = Jun15, DateEnd = LaborDay });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 56, 0), RouteID = "6", StopID = "12?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = Jun15, DateEnd = LaborDay });
                }
                if (HourAdding >= 9 && HourAdding <= 17)
                {
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 32, 0), RouteID = "6", StopID = "12?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = Jun15, DateEnd = LaborDay });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 39, 0), RouteID = "6", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = Jun15, DateEnd = LaborDay });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 45, 0), RouteID = "6", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = Jun15, DateEnd = LaborDay });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 50, 0), RouteID = "6", StopID = "4", Weekday = true, Saturday = false, Sunday = false, DateStart = Jun15, DateEnd = LaborDay });
                    this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 56, 0), RouteID = "6", StopID = "12?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = Jun15, DateEnd = LaborDay });
                }

                // if thursday add route 6e
                //if(BaseDate.DayOfWeek == DayOfWeek.Thursday)
                //{
                //    if(HourAdding == 18)
                //    {
                //        this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 00, 0), RouteID = "7", StopID = "17?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                //        this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 10, 0), RouteID = "7", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                //        this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 15, 0), RouteID = "7", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                //        this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 25, 0), RouteID = "7", StopID = "17?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                //        this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 30, 0), RouteID = "7", StopID = "17?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                //        this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 40, 0), RouteID = "7", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                //        this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 45, 0), RouteID = "7", StopID = "7?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                //        this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 55, 0), RouteID = "7", StopID = "17?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                        
                //    }
                //    if (HourAdding == 19)
                //    {
                //        this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 00, 0), RouteID = "7", StopID = "17?Depart", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                //        this.RouteStops.Add(new RouteStopViewModel { Time = new DateTime(BaseYear, BaseMonth, BaseDay, HourAdding, 10, 0), RouteID = "7", StopID = "7?Arrive", Weekday = true, Saturday = false, Sunday = false, DateStart = LaborDay, DateEnd = Jun14 });
                        
                //    }
                //}

                

                HourAdding += 1;
            }

            // Remove RouteStops not available on current day of week
            if(BaseDate.DayOfWeek == DayOfWeek.Saturday)
            {
                RouteStops = new ObservableCollection<RouteStopViewModel>(from routestop in this.RouteStops
                                                                          where routestop.Saturday == true
                                                                          select routestop);
            }
            else if (BaseDate.DayOfWeek == DayOfWeek.Sunday)
            {
                RouteStops = new ObservableCollection<RouteStopViewModel>(from routestop in this.RouteStops
                                                                          where routestop.Sunday == true
                                                                          select routestop);
            }
            else
            {
                RouteStops = new ObservableCollection<RouteStopViewModel>(from routestop in this.RouteStops
                                                                          where routestop.Weekday == true
                                                                          select routestop);
            }

            // Remove RouteStops not available on current day of year
            // if it is before jun 14th or after labor day, remove routes between them.
            if(BaseDate.CompareTo(Jun14) <= 0 || BaseDate.CompareTo(LaborDay) >= 0)
            {
                RouteStops = new ObservableCollection<RouteStopViewModel>(from routestop in this.RouteStops
                                                                          where !(routestop.DateStart == Jun15 && routestop.DateEnd == LaborDay)
                                                                          select routestop);
            }
            else
            {
                RouteStops = new ObservableCollection<RouteStopViewModel>(from routestop in this.RouteStops
                                                                          where !(routestop.DateStart == LaborDay && routestop.DateEnd == Jun14)
                                                                          select routestop);
            }

            // Sort routeStops
            this.RouteStops = new ObservableCollection<RouteStopViewModel>(
                                      from routeStop in App.ViewModel.RouteStops
                                      orderby routeStop.Time
                                      select routeStop);

            refreshTimeUntil();
        }
        #endregion

        public void refreshTimeUntil()
        {
            foreach (RouteStopViewModel RouteStop in this.RouteStops)
            {
                RouteStop.TimeUntil = RouteStop.Time - DateTime.Now;
            }

            // set the next route for each stop
            foreach (StopViewModel stop in Stops)
            {
                stop.setNextRoute();
            }

            // set the next stop for each route
            foreach (RouteViewModel route in Routes)
            {
                route.setNextStop();
            }
        }

        public async Task getPosition()
        {
            if(locator.LocationStatus == PositionStatus.Disabled)
            {
                MessageBoxResult res = MessageBox.Show("Currently applications aren't allowed to access your location. "
                    + "Would you like to change this?",
                    "Location Services",
                    MessageBoxButton.OKCancel);
                if (res == MessageBoxResult.OK)
                {
                    await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = false;
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

            }
            else
            {
                location = await locator.GetGeopositionAsync();

                if (location != null)
                {
                    coordinates = new GeoCoordinate(location.Coordinate.Latitude, location.Coordinate.Longitude);

                    foreach (StopViewModel stop in Stops)
                    {
                        stop.Distance = coordinates.GetDistanceTo(stop.Location) / 1609.344;
                    }

                    IsStopsSorted = false;
                    sortStops();
                    stopsSetIndices();
                }
            }
        }

        public void sortStops()
        {
            Stops = new ObservableCollection<StopViewModel>(
                                from stop in this.Stops
                                orderby stop.Distance
                                select stop);

            IsStopsSorted = true;
            NotifyPropertyChanged("Stops");
        }

        public void stopsSetIndices()
        {
            for(int i = 0; i < Stops.Count; i++)
            {
                Stops[i].Index = i;
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}