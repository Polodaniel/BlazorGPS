using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGPS.Client.Utils
{
    public class Base : ComponentBase
    {
        #region Inject
        [Inject]
        public IJSRuntime JS { get; set; }
        #endregion

        #region Constructor
        public Base()
        {
            //this.Template = "https://api.maptiler.com/maps/hybrid/256/{z}/{x}/{y}@2x.jpg?key=OhKLq5wlAdK90y0vDvPY";
            this.Template = "http://{s}.tile.osm.org/{z}/{x}/{y}.png";
            this.Attribution = "Gerado por <a href=\"/\">BlazorGPS</a>";
            this.MinZoom = 6;
            this.MaxZoom = 50;
        }
        #endregion

        #region Properties
        public string Template { get; set; }

        public string Attribution { get; set; }

        public int MinZoom { get; set; }

        public int MaxZoom { get; set; }
        #endregion
    }
}