using BlazorGPS.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGPS.Client.Extensions
{
    public static class Extensions
    {
        public static List<LayoutList> CreateLayoutList(this List<LayoutList> List)
        {
            List.Add(new LayoutList("http://{s}.tile.osm.org/{z}/{x}/{y}.png", "Default"));
            List.Add(new LayoutList("https://tile.opentopomap.org/{z}/{x}/{y}.png", "Open Topo Map"));
            List.Add(new LayoutList("https://api.maptiler.com/maps/hybrid/256/{z}/{x}/{y}@2x.jpg?key=OhKLq5wlAdK90y0vDvPY", "Maptiler - Satellite Hybrid"));
            List.Add(new LayoutList("https://tile.thunderforest.com/cycle/{z}/{x}/{y}.png?apikey=3b495e9cd83e42f18f72300d39d88d8c", "Thunderforest - OpenCycleMap"));
            List.Add(new LayoutList("https://tile.thunderforest.com/transport/{z}/{x}/{y}.png?apikey=3b495e9cd83e42f18f72300d39d88d8c", "Thunderforest - Transport"));
            List.Add(new LayoutList("https://tile.thunderforest.com/landscape/{z}/{x}/{y}.png?apikey=3b495e9cd83e42f18f72300d39d88d8c", "Thunderforest - Landscape"));
            List.Add(new LayoutList("https://tile.thunderforest.com/outdoors/{z}/{x}/{y}.png?apikey=3b495e9cd83e42f18f72300d39d88d8c", "Thunderforest - Outdoors"));
            List.Add(new LayoutList("https://tile.thunderforest.com/atlas/{z}/{x}/{y}.png?apikey=3b495e9cd83e42f18f72300d39d88d8c", "Thunderforest - Atlas"));
            List.Add(new LayoutList("https://tile.thunderforest.com/spinal-map/{z}/{x}/{y}.png?apikey=3b495e9cd83e42f18f72300d39d88d8c", "Thunderforest - Spinal Map"));
            List.Add(new LayoutList("https://tile.thunderforest.com/transport-dark/{z}/{x}/{y}.png?apikey=3b495e9cd83e42f18f72300d39d88d8c", "Thunderforest - Transport Dark"));
            List.Add(new LayoutList("https://tile.thunderforest.com/pioneer/{z}/{x}/{y}.png?apikey=3b495e9cd83e42f18f72300d39d88d8c", "Thunderforest - Pioneer"));
            List.Add(new LayoutList("https://tile.thunderforest.com/neighbourhood/{z}/{x}/{y}.png?apikey=3b495e9cd83e42f18f72300d39d88d8c", "Thunderforest - Neighbourhood"));
            List.Add(new LayoutList("https://tile.thunderforest.com/mobile-atlas/{z}/{x}/{y}.png?apikey=3b495e9cd83e42f18f72300d39d88d8c", "Thunderforest - Mobile Atlas"));

            return List;
        }
    }
}
