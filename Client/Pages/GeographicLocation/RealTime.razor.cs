using BlazorGPS.Client.Shared.Component;
using BlazorGPS.Client.Shared.Component.GeographicLocation;
using BlazorGPS.Client.Utils;
using BrowserInterop.Extensions;
using BrowserInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGPS.Client.Pages.GeographicLocation
{
    public class RealTimeBase : Base, IAsyncDisposable
    {
        #region Inject
        [Inject]
        public IDialogService DialogService { get; set; }

        [Inject]
        protected ISnackbar Snackbar { get; set; }

        [Inject]
        public IConfiguration Configuration { get; set; }
        #endregion

        #region Interface
        private IAsyncDisposable geopositionWatcher;
        #endregion 

        #region Properties

        #region String
        private string typeMaps;

        public string TypeMaps
        {
            get => typeMaps;
            set
            {
                typeMaps = value;
            }
        }
        #endregion

        #region Object
        public Maps MapsReference { get; set; }
        protected GeolocationPosition currentPosition { get; set; }
        protected WindowNavigatorGeolocation geolocationWrapper { get; set; }
        #endregion

        #region List
        protected List<GeolocationPosition> positioHistory { get; set; }
        #endregion

        #endregion

        #region Constructor
        public RealTimeBase()
        {
            positioHistory = new List<GeolocationPosition>();
            typeMaps = "http://{s}.tile.osm.org/{z}/{x}/{y}.png";
        }
        #endregion

        #region Events Base
        protected override async Task OnInitializedAsync()
        {
            var window = await JS.Window();
            var navigator = await window.Navigator();

            geolocationWrapper = navigator.Geolocation;

            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
            Snackbar.Configuration.MaxDisplayedSnackbars = 10;
        }
        #endregion

        #region Events
        protected async Task GetGeolocation()
        {
            currentPosition = (await geolocationWrapper.GetCurrentPosition(new PositionOptions()
            {
                EnableHighAccuracy = true,
                MaximumAgeTimeSpan = TimeSpan.FromHours(1),
                TimeoutTimeSpan = TimeSpan.FromMinutes(1),
            })).Location;

            if (!Equals(currentPosition, null))
            {
                var SB = new StringBuilder();

                SB.Append($"<ul>");
                SB.Append($"  <li>");
                SB.Append($"    Latitude: {currentPosition.Coords.Latitude}");
                SB.Append($"  </li>");
                SB.Append($"  <li>");
                SB.Append($"    Longitude: {currentPosition.Coords.Longitude}");
                SB.Append($"  </li>");
                SB.Append($"  <li>");
                SB.Append($"    Altitude: {currentPosition.Coords.Altitude}");
                SB.Append($"  </li>");
                SB.Append($"  <li>");
                SB.Append($"    Accuracy: {currentPosition.Coords.Accuracy}");
                SB.Append($"  </li>");
                SB.Append($"</ul>");

                Snackbar.Add(SB.ToString(), Severity.Info);

                await Marker(currentPosition.Coords.Latitude,
                             currentPosition.Coords.Longitude,
                             currentPosition.Coords.Altitude,
                             currentPosition.Coords.Accuracy);
            }
        }

        protected async Task WatchPosition()
        {
            geopositionWatcher = await geolocationWrapper.WatchPosition(async (p) =>
            {
                //positioHistory.Add(p.Location);

                //var SB = new StringBuilder();

                //SB.Append($"<ul>");
                //SB.Append($"  <li>");
                //SB.Append($"    Latitude: {p.Location.Coords.Latitude}");
                //SB.Append($"  </li>");
                //SB.Append($"  <li>");
                //SB.Append($"    Longitude: {p.Location.Coords.Longitude}");
                //SB.Append($"  </li>");
                //SB.Append($"  <li>");
                //SB.Append($"    Altitude: {p.Location.Coords.Altitude}");
                //SB.Append($"  </li>");
                //SB.Append($"  <li>");
                //SB.Append($"    Accuracy: {p.Location.Coords.Accuracy}");
                //SB.Append($"  </li>");
                //SB.Append($"</ul>");

                //Snackbar.Add(SB.ToString(), Severity.Info);

                await Marker(p.Location.Coords.Latitude,
                             p.Location.Coords.Longitude,
                             p.Location.Coords.Altitude,
                             p.Location.Coords.Accuracy);

                StateHasChanged();
            });
        }

        protected async Task StopWatch()
        {
            if (!Equals(geopositionWatcher, null))
            {
                await geopositionWatcher.DisposeAsync();
                geopositionWatcher = null;
            }
        }

        public async Task Marker(double latitude, double longitude, double? altitude, double accuracy) =>
            await JS.InvokeVoidAsync("Marker", latitude, longitude, altitude, accuracy);

        public async ValueTask DisposeAsync() =>
            await StopWatch();

        public async Task RemoverMarcacoes() =>
            await JS.InvokeVoidAsync("RemoverMarcacaoMapa");

        public async void SetLayout() =>
            await JS.InvokeVoidAsync("Reset", Attribution, MinZoom, MaxZoom, TypeMaps);

        protected async void GetLayout()
        {
            var ParametersDialog = new DialogParameters();
            ParametersDialog.Add("TypeMapsDialog", typeMaps);

            var OptionDialog = new DialogOptions();
            OptionDialog.MaxWidth = MaxWidth.Large;
            OptionDialog.CloseButton = false;
            OptionDialog.DisableBackdropClick = true;
            OptionDialog.Position = DialogPosition.Center;

            var Dialog = DialogService.Show<LayoutDialog>($"Layouts", ParametersDialog, OptionDialog);
            var Result = await Dialog.Result;

            if (!Result.Cancelled)
            {
                TypeMaps = (string)Result.Data;
                SetLayout();
            }

            StateHasChanged();
        }
        #endregion
    }
}
