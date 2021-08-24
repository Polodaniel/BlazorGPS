using BlazorGPS.Client.Utils;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGPS.Client.Shared.Component.GeographicLocation
{
    public class MapsBase : Base
    {
        #region Properties

        #endregion

        #region Events
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                await JS.InvokeVoidAsync("InitializeMap", Attribution, MinZoom, MaxZoom, "http://{s}.tile.osm.org/{z}/{x}/{y}.png");
        }
        #endregion
    }
}
