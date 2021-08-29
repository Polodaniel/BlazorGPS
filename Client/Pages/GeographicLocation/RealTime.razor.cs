using BlazorGPS.Client.Shared.Component;
using BlazorGPS.Client.Shared.Component.GeographicLocation;
using BlazorGPS.Client.Utils;
using BlazorGPS.Shared;
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

        #region Int
        public int Index { get; set; }
        public int IndexTab { get; set; }
        #endregion

        #region Bool
        public bool DisabledButtons { get; set; }
        #endregion

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
        protected Navigation positioHistory { get; set; }

        protected Navigation Historico { get; set; }
        #endregion

        #endregion

        #region Constructor
        public RealTimeBase()
        {
            positioHistory = new Navigation();

            Historico = new Navigation();

            Index = 0;
            IndexTab = 0;
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
                MaximumAgeTimeSpan = TimeSpan.FromHours(8),
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
            DisabledButtons = true;

            geopositionWatcher = await geolocationWrapper.WatchPosition(async (p) =>
            {
                positioHistory.PointNavigationList.Add(new NavigationList().SetValues(Index++, p.Location.Coords));

                StateHasChanged();
            });
        }

        protected async Task StopWatch()
        {
            DisabledButtons = false;
            StateHasChanged();

            if (!Equals(geopositionWatcher, null))
            {
                await geopositionWatcher.DisposeAsync();
                geopositionWatcher = null;
            }
        }

        protected async Task Marker(double latitude, double longitude, double? altitude, double accuracy) =>
            await JS.InvokeVoidAsync("Marker", latitude, longitude, altitude, accuracy);

        protected async void ViewMapPoint(NavigationList point)
        {
            IndexTab = 0;
            StateHasChanged();

            await Task.Delay(1000);

            await Marker(point.Latitude, point.Longitude, point.Altitude, point.Accuracy);
        }

        public async ValueTask DisposeAsync() =>
            await StopWatch();

        protected async Task DeleterMarker()
        {
            await JS.InvokeVoidAsync("RemoverMarcacaoMapa");
            Index = 0;
        }

        protected void ClearDateNavegation() 
        {
            positioHistory = new Navigation();
            StateHasChanged();
        }

        protected async void SetLayout() =>
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

        protected async void ViewNavigation() 
        {
            if (!Equals(positioHistory, null))
            {
                if (!Equals(positioHistory.PointNavigationList, null) && positioHistory.PointNavigationList.Count() > 0)
                {
                    await JS.InvokeVoidAsync("NavigationLineMotion", positioHistory.StartingPoint,
                                                                     positioHistory.PointNavigationList,
                                                                     positioHistory.EndPoint);
                }
                else
                    Snackbar.Add("Ops! Não existe um historico de navegação !", Severity.Error);
            }
            else
                Snackbar.Add("Ops! Não existe um historico de navegação !", Severity.Error);
        }

        protected async void ExemploNavegacao()
        {
            #region Cria todos os Pontos
            Historico = new Navigation();
            Historico.PointNavigationList.Add(new NavigationList(-21.13590448896191, -47.821719553223230, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13588433037679, -47.821744636219190, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13577295352284, -47.821634209844810, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13564699699416, -47.821516825527750, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13555669815663, -47.821447246623090, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.1354466488957, -47.82132907326690, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13534415247524, -47.821245253982980, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.1352586968218, -47.82117076937090, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13519007193013, -47.82109837645120, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.1351984962701, -47.820990471740220, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13528120443297, -47.820858907593080, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13539685214437, -47.820726175729720, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.1354467754531, -47.820688778222550, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13552234932887, -47.820614188204670, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13563437053289, -47.820510486085970, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13583204536839, -47.820331267237570, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13593704918787, -47.820237160078040, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13614725496007, -47.820057100436450, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13623103857636, -47.819964048154890, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13631953700997, -47.819856412992170, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13639582582512, -47.819774628640320, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13649652325404, -47.81964049598690, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13671210849345, -47.819344090740340, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13678417350768, -47.819212891695270, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13682880405407, -47.81914878322210, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13684118218064, -47.819073179070240, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13680583292971, -47.81903388316940, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13670751387093, -47.818955680448090, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13657386948581, -47.818864490648980, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13648680581473, -47.818786077383180, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13643796224548, -47.818742005347530, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13639714838901, -47.818695310172320, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13629939588164, -47.818777070121780, 0, 20));
            #endregion

            await JS.InvokeVoidAsync("NavigationLine", Historico.StartingPoint,
                                                       Historico.PointNavigationList,
                                                       Historico.EndPoint);

            #region Não Funciona Corretamente
            //// Adiciona o Marcador Inicial
            //await JS.InvokeVoidAsync("Marker", Historico.StartingPoint.Latitude, 
            //                                   Historico.StartingPoint.Longitude,
            //                                   Historico.StartingPoint.Altitude,
            //                                   Historico.StartingPoint.Accuracy);

            //// Adiciona o Ciclo em volta do marcador
            //await JS.InvokeVoidAsync("Cicle", Historico.StartingPoint.Latitude,
            //                                  Historico.StartingPoint.Longitude,
            //                                  Historico.StartingPoint.Accuracy);

            //foreach (var item in Historico.PointNavigationList)
            //{
            //    await Task.Delay(1000);

            //    await JS.InvokeVoidAsync("Line", Historico.EndPoint.Latitude,
            //                                     Historico.EndPoint.Longitude);
            //}

            //// Adiciona o Marcador Final
            //await JS.InvokeVoidAsync("Marker", Historico.EndPoint.Latitude,
            //                                   Historico.EndPoint.Longitude,
            //                                   Historico.EndPoint.Altitude,
            //                                   Historico.EndPoint.Accuracy);

            //// Adiciona o Ciclo em volta do marcador
            //await JS.InvokeVoidAsync("Cicle", Historico.EndPoint.Latitude,
            //                                  Historico.EndPoint.Longitude,
            //                                  Historico.EndPoint.Accuracy);

            #endregion
        }

        protected async void ExemploNavegacaoAnimado()
        {
            #region Cria todos os Pontos
            Historico = new Navigation();
            Historico.PointNavigationList.Add(new NavigationList(-21.13590448896191, -47.821719553223230, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13588433037679, -47.821744636219190, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13577295352284, -47.821634209844810, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13564699699416, -47.821516825527750, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13555669815663, -47.821447246623090, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.1354466488957, -47.82132907326690, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13534415247524, -47.821245253982980, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.1352586968218, -47.82117076937090, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13519007193013, -47.82109837645120, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.1351984962701, -47.820990471740220, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13528120443297, -47.820858907593080, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13539685214437, -47.820726175729720, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.1354467754531, -47.820688778222550, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13552234932887, -47.820614188204670, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13563437053289, -47.820510486085970, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13583204536839, -47.820331267237570, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13593704918787, -47.820237160078040, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13614725496007, -47.820057100436450, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13623103857636, -47.819964048154890, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13631953700997, -47.819856412992170, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13639582582512, -47.819774628640320, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13649652325404, -47.81964049598690, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13671210849345, -47.819344090740340, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13678417350768, -47.819212891695270, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13682880405407, -47.81914878322210, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13684118218064, -47.819073179070240, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13680583292971, -47.81903388316940, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13670751387093, -47.818955680448090, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13657386948581, -47.818864490648980, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13648680581473, -47.818786077383180, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13643796224548, -47.818742005347530, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13639714838901, -47.818695310172320, 0, 20));
            Historico.PointNavigationList.Add(new NavigationList(-21.13629939588164, -47.818777070121780, 0, 20));
            #endregion

            await JS.InvokeVoidAsync("NavigationLineMotion", Historico.StartingPoint,
                                                             Historico.PointNavigationList,
                                                             Historico.EndPoint);
        }
        #endregion
    }
}
