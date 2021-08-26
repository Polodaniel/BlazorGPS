var map;
var baseMap;

var ZoomMin;
var ZoomMax;
var AttributionConfig;
var TemplateConfig;

var MapaInicializado = false;

var CONFIG_OPTIONS = new CreateConfigOptions();

function CreateConfigOptions()
{
    this.color = "#004647";
    this.fillColor = "#004647";
    this.fillOpacity = 0.2;
    this.duration = 10000;
}

function InitializeMap(Attribution, MinZoom, MaxZoom, Template) {

    map = L.map(document.getElementById('mapDIV'), {
        center: [-21.1767, -47.8208],
        zoom: 12
    });

    ConfigureMap(Attribution, MinZoom, MaxZoom, Template);
}

function ConfigureMap(Attribution, MinZoom, MaxZoom, Template) {

    ZoomMin = MinZoom;
    ZoomMax = MaxZoom;
    AttributionConfig = Attribution;
    TemplateConfig = Template;

    ConfigureTheMainMap();
}

function ConfigureTheMainMap(Template) {
    baseMap = L.tileLayer(TemplateConfig,
        {
            attribution: AttributionConfig,
            maxZoom: ZoomMax,
            minZoom: ZoomMin
        });

    baseMap.addTo(map);

    if (MapaInicializado == false) {
        //var scale = L.control.scale()
        //scale.addTo(map)
    }

    //Adiciona os creditos
    //map.attributionControl.addAttribution('Daniel Polo');
}

function RemoverMarcacaoMapa() {
    map.eachLayer((layer) => {
        layer.remove();
    });

    MapaInicializado = false;

    ConfigureTheMainMap();

    map.setView([-21.1767, -47.8208], 12);
}

function RemoverMarcacaoMapaLoad() {
    map.eachLayer((layer) => {
        layer.remove();
    });
}

function Marker(latitude, longitude, altitude, accuracy) {

    var Popup = "<ul>" +
        "   <li>" +
        "       Latitude: " + latitude +
        "   </li>" +
        "   <li>" +
        "       Longitude: " + longitude +
        "   </li>" +
        "   <li>" +
        "       Altitude: " + altitude +
        "   </li>" +
        "   <li>" +
        "       Accuracy: " + accuracy +
        "   </li>" +
        "</ul>";

    L.marker([latitude, longitude])
        .bindPopup(Popup)
        .addTo(map);

    map.setView([latitude, longitude], 16);

}

function Cicle(latitude, longitude, radius) {
    L.circle([latitude, longitude], {
        color: CONFIG_OPTIONS.color,
        fillColor: CONFIG_OPTIONS.fillColor,
        fillOpacity: CONFIG_OPTIONS.fillOpacity,
        radius: radius
    }).addTo(map);
}

// Não Funciona
function Line(latitude, longitude) {
    var Options = { color: CONFIG_OPTIONS.color };

    var latlngs = [[latitude, longitude]];

    L.polyline(latlngs, Options).addTo(map);
}

function LineMotion(latlngs)
{
    L.motion.polyline(latlngs, {
        color: CONFIG_OPTIONS.color
    }, {
        auto: true,
        duration: CONFIG_OPTIONS.duration,
        easing: L.Motion.Ease.easeInOutQuart
        //easing: L.Motion.Ease.linear
	    //easing: L.Motion.Ease.swing
	    //easing: L.Motion.Ease.easeInQuad
	    //easing: L.Motion.Ease.easeOutQuad
	    //easing: L.Motion.Ease.easeInOutQuad
	    //easing: L.Motion.Ease.easeInCubic
	    //easing: L.Motion.Ease.easeOutCubic
	    //easing: L.Motion.Ease.easeInOutCubic
	    //easing: L.Motion.Ease.easeInQuart
	    //easing: L.Motion.Ease.easeOutQuart
	    //easing: L.Motion.Ease.easeInQuint
	    //easing: L.Motion.Ease.easeOutQuint
	    //easing: L.Motion.Ease.easeInOutQuint
	    //easing: L.Motion.Ease.easeInSine
	    //easing: L.Motion.Ease.easeOutSine
	    //easing: L.Motion.Ease.easeInOutSine
	    //easing: L.Motion.Ease.easeInExpo
	    //easing: L.Motion.Ease.easeOutExpo
	    //easing: L.Motion.Ease.easeInOutExpo
	    //easing: L.Motion.Ease.easeInCirc
	    //easing: L.Motion.Ease.easeOutCirc
	    //easing: L.Motion.Ease.easeInOutCirc
	    //easing: L.Motion.Ease.easeInElastic
	    //easing: L.Motion.Ease.easeOutElastic
	    //easing: L.Motion.Ease.easeInOutElastic
	    //easing: L.Motion.Ease.easeInBack
	    //easing: L.Motion.Ease.easeOutBack
	    //easing: L.Motion.Ease.easeInOutBack
	    //easing: L.Motion.Ease.easeInBounce
	    //easing: L.Motion.Ease.easeOutBounce
	    //easing: L.Motion.Ease.easeInOutBounce
    }, {
        removeOnEnd: false, // Remove o marcador do mapa no final do movimento.
        showMarker: false // Adicione um marcador ao mapa no primeiro ponto da linha quando o movimento acabou de ser adicionado (o início pode ser atrasado) ao mapa.
        /*icon: L.divIcon({ html: "<i class='fa fa-car fa-2x' aria-hidden='true'></i>", iconSize: L.point(27.5, 24) })*/
    }).addTo(map);
}

function NavigationLine(startingPoint, pointNavigationList, endPoint) {
    Marker(startingPoint.latitude, startingPoint.longitude, startingPoint.altitude, startingPoint.accuracy);
    Cicle(startingPoint.latitude, startingPoint.longitude, startingPoint.accuracy);

    var Options = { color: CONFIG_OPTIONS.color };
    var latlngs = [];

    for (var i = 0; i < pointNavigationList.length; i++) {
        if (pointNavigationList[i].latitude != 0 && pointNavigationList[i].longitude != 0) {
            latlngs.push([pointNavigationList[i].latitude, pointNavigationList[i].longitude]);
        }
    }

    L.polyline(latlngs, Options).addTo(map);

    Marker(endPoint.latitude, endPoint.longitude, endPoint.altitude, endPoint.accuracy);
    Cicle(endPoint.latitude, endPoint.longitude, endPoint.accuracy);
}

function NavigationLineMotion(startingPoint, pointNavigationList, endPoint) {
    Marker(startingPoint.latitude, startingPoint.longitude, startingPoint.altitude, startingPoint.accuracy);
    Cicle(startingPoint.latitude, startingPoint.longitude, startingPoint.accuracy);

    var Options = { color: CONFIG_OPTIONS.color };
    var latlngs = [];

    for (var i = 0; i < pointNavigationList.length; i++) {
        if (pointNavigationList[i].latitude != 0 && pointNavigationList[i].longitude != 0) {
            latlngs.push([pointNavigationList[i].latitude, pointNavigationList[i].longitude]);
        }
    }

    LineMotion(latlngs);
    //Marker(endPoint.latitude, endPoint.longitude, endPoint.altitude, endPoint.accuracy);

    setTimeout(function () {
        Cicle(endPoint.latitude, endPoint.longitude, endPoint.accuracy);
    }, (CONFIG_OPTIONS.duration + 500))

}

function Reset(Attribution, MinZoom, MaxZoom, Template) {
    MapaInicializado = false;

    map.remove();

    map = L.map(document.getElementById('mapDIV'), {
        center: [-21.1767, -47.8208],
        zoom: 12
    });

    ConfigureMap(Attribution, MinZoom, MaxZoom, Template);
}

function InicializaMapa(Latitude, Longitude, Zoom) {

    map = L.map(document.getElementById('mapDIV'), {
        center: [Latitude, Longitude],
        zoom: Zoom
    });

    ConfiguracaoMapaPrincipal();
}

function CriaMarcacaoMapa(dados, result) {

    var dataRow = [];

    for (var i = 0; i < result.length; i++) {
        if (result[i].latitude != 0 && result[i].longitude != 0) {
            dataRow.push([result[i].latitude, result[i].longitude]);

            if (i == 0 && MapaInicializado == false) {
                MapaInicializado = true;

                map.remove();

                InicializaMapa(result[i].latitude, result[i].longitude, 13);

            }
        }
    }

    var PopupOriginal = "<div>" +
        "   <div><strong>Fazenda:</strong> #Fazenda#</div>" +
        "   <div><strong>Talhão:</strong> #Talhao#</div>" +
        "   <div><strong>Área:</strong> #Area#</div>" +
        "</div>"

    Popup = PopupOriginal.replace('#Fazenda#', dados.fazenda);
    Popup = Popup.replace('#Talhao#', dados.talhao);
    Popup = Popup.replace('#Area#', dados.area);

    L.polygon([dataRow], { color: 'white' })
        .bindPopup(Popup)
        .addTo(map);
}

function BASE() {
    //// Inicializa o Mapa
    //var map = L.map(document.getElementById('mapDIV'), {
    //    center: [-21.13590448896191, -47.82171955322323],
    //    zoom: 16
    //});

    //// Define o Template do Mapa e o limite de zoom
    //L.tileLayer('https://tile.opentopomap.org/{z}/{x}/{y}.png',
    //    {
    //        attribution: 'Gerado por Daniel Polo',
    //        maxZoom: 25,
    //        minZoom: 1
    //    }).addTo(map);

    //// Adiciona Dois Marcadores no Mapa
    //L.marker([-21.13590448896191, -47.82171955322323],)
    //    .bindPopup("Ínicio")
    //    .addTo(map);

    //L.marker([-21.13629939588164, -47.81877707012178])
    //    .bindPopup("Fim")
    //    .addTo(map);

    //// Adiciona o Circulo da Acuracia
    //var circle = L.circle([-21.13590448896191, -47.82171955322323], {
    //    color: '#004647',
    //    fillColor: '#004647',
    //    fillOpacity: 0.2,
    //    radius: 20
    //}).addTo(map);

    //// Cria Array de Latitudes e Longitudes
    //var latlngs =
    //    [
    //        [-21.13590448896191, -47.82171955322323],
    //        [-21.13588433037679, -47.82174463621919],
    //        [-21.13577295352284, -47.82163420984481],
    //        [-21.13564699699416, -47.82151682552775],
    //        [-21.13555669815663, -47.82144724662309],
    //        [-21.1354466488957, -47.8213290732669],
    //        [-21.13534415247524, -47.82124525398298],
    //        [-21.1352586968218, -47.8211707693709],
    //        [-21.13519007193013, -47.8210983764512],
    //        [-21.1351984962701, -47.82099047174022],
    //        [-21.13528120443297, -47.82085890759308],
    //        [-21.13539685214437, -47.82072617572972],
    //        [-21.1354467754531, -47.82068877822255],
    //        [-21.13552234932887, -47.82061418820467],
    //        [-21.13563437053289, -47.82051048608597],
    //        [-21.13583204536839, -47.82033126723757],
    //        [-21.13593704918787, -47.82023716007804],
    //        [-21.13614725496007, -47.82005710043645],
    //        [-21.13623103857636, -47.81996404815489],
    //        [-21.13631953700997, -47.81985641299217],
    //        [-21.13639582582512, -47.81977462864032],
    //        [-21.13649652325404, -47.8196404959869],
    //        [-21.13671210849345, -47.81934409074034],
    //        [-21.13678417350768, -47.81921289169527],
    //        [-21.13682880405407, -47.8191487832221],
    //        [-21.13684118218064, -47.81907317907024],
    //        [-21.13680583292971, -47.8190338831694],
    //        [-21.13670751387093, -47.81895568044809],
    //        [-21.13657386948581, -47.81886449064898],
    //        [-21.13648680581473, -47.81878607738318],
    //        [-21.13643796224548, -47.81874200534753],
    //        [-21.13639714838901, -47.81869531017232],
    //        [-21.13629939588164, -47.81877707012178]
    //    ];

    //// Cria o objeto de configuração da Linha
    //var Options = { color: '#004647' };

    //// Grava no Mapa a Linha
    //L.polyline(latlngs, Options).addTo(map);
}

