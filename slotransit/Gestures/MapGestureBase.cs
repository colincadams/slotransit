using Microsoft.Phone.Maps.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace slobus_v1._0_db.Gestures
{
    /// <summary>
    /// A base class for map gestures, which allows them to suppress the built-in map gestures.
    /// </summary>
    public class MapGestureBase
    {
        /// <summary>
        /// Gets or sets whether to suppress the existing gestures/
        /// </summary>
        public bool SuppressMapGestures { get; set; }

        protected Map Map { get; private set; }

        public MapGestureBase(Map map)
        {
            Map = map;
            map.Loaded += (s, e) => CrawlTree(Map);
        }

        private void CrawlTree(FrameworkElement el)
        {
            el.ManipulationDelta += MapElement_ManipulationDelta;
            for (int c = 0; c < VisualTreeHelper.GetChildrenCount(el); c++)
            {
                CrawlTree(VisualTreeHelper.GetChild(el, c) as FrameworkElement);
            }
        }

        private void MapElement_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (SuppressMapGestures)
            {
                if (e.DeltaManipulation.Scale.X != 0.0 || e.DeltaManipulation.Scale.Y != 0.0)
                    e.Handled = true;
                if (e.DeltaManipulation.Translation.X != 0.0 || e.DeltaManipulation.Translation.Y != 0.0)
                    e.Handled = true;
            }
        }
    }
}
