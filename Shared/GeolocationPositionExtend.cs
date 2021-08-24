using BrowserInterop.Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGPS.Shared
{
    public class GeolocationPositionExtend : GeolocationPosition
    {
        public GeolocationPositionExtend(long Index, GeolocationPosition location)
        {
            this.Index = Index;
            this.Coords = location.Coords;
            this.Timestamp = location.Timestamp;

            Data = DateTime.Now;
        }

        public DateTime Data { get; set; }
        public long Index { get; set; }
    }
}
