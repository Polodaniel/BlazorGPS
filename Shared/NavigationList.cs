using BrowserInterop.Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGPS.Shared
{
    public class NavigationList
    {
        public NavigationList()
        {

        }

        public int Index { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Altitude { get; set; }
        public double Accuracy { get; set; }

        public NavigationList SetValues(int Index,GeolocationCoordinates Coords) 
        {
            this.Index = Index;
            this.Latitude = Coords.Latitude;
            this.Longitude = Coords.Longitude;
            this.Altitude = Coords.Altitude;
            this.Accuracy = Coords.Accuracy;

            return this;
        }
    }
}
