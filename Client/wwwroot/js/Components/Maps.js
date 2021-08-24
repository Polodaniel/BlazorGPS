var map;

var ZoomMin;
var ZoomMax;
var AttributionConfig;
var TemplateConfig;

var MapaInicializado = false;

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

    //https://tile.thunderforest.com/cycle/{z}/{x}/{y}.png?apikey=3b495e9cd83e42f18f72300d39d88d8c
    //https://api.maptiler.com/maps/hybrid/256/{z}/{x}/{y}@2x.jpg?key=OhKLq5wlAdK90y0vDvPY
    //http://{s}.tile.osm.org/{z}/{x}/{y}.png

    L.tileLayer(TemplateConfig,
        {
            attribution: AttributionConfig,
            maxZoom: ZoomMax,
            minZoom: ZoomMin
        }).addTo(map);

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

function Reset(Attribution, MinZoom, MaxZoom, Template)
{
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

function CriaMarcacaoMapa2() {
    L.polygon([
        [-19.98006230129525, -48.75050174688201],
        [-19.97988123958509, -48.75434365592279],
        [-19.9793630473937, -48.75421220552353],
        [-19.97865956100201, -48.75435150997696],
        [-19.97835151603908, -48.75485300430238],
        [-19.97840973134366, -48.75523719368764],
        [-19.97749563091907, -48.75575497765626],
        [-19.97660641229459, -48.7561052826625],
        [-19.97620714225813, -48.75621973235034],
        [-19.97587182653603, -48.75610769741199],
        [-19.97571794126815, -48.75584458160029],
        [-19.97534333749529, -48.75583920888977],
        [-19.97502119851406, -48.75547384423003],
        [-19.97453498187801, -48.7553121131579],
        [-19.97397819567833, -48.75520425799537],
        [-19.97344338745764, -48.75527396774693],
        [-19.97303789236807, -48.75528244403509],
        [-19.97286663380627, -48.75510222766017],
        [-19.97311412880019, -48.75424994890075],
        [-19.97355105456743, -48.75265649343087],
        [-19.97432851828836, -48.7497607455932],
        [-19.98006230129525, -48.75050174688201]
    ], { color: 'white' })
        .bindPopup("<div>" +
            "<div><strong>Fazenda:</strong> Santa Terezinha</div>" +
            "<div><strong>Talhão:</strong> 001</div>" +
            "<div><strong>Área:</strong> 309,45</div>" +
            "</div>")
        .addTo(map);

    L.polygon([
        [-19.97897341617843, -48.7500842244343],
        [-19.97845237199517, -48.74401734469281],
        [-19.97750390982696, -48.74384710913308],
        [-19.97550152094561, -48.74380996418343],
        [-19.97467413205581, -48.74381343786755],
        [-19.97453436258367, -48.74425098614498],
        [-19.97416602830711, -48.7476071442539],
        [-19.9743308441694, -48.74845055672814],
        [-19.97442570392741, -48.74959122149464],
        [-19.97897341617843, -48.7500842244343]
    ], { color: 'white' })
        .bindPopup("<div>" +
            "<div><strong>Fazenda:</strong> Santa Terezinha</div>" +
            "<div><strong>Talhão:</strong> 002</div>" +
            "<div><strong>Área:</strong> 400,32</div>" +
            "</div>")
        .addTo(map);

    L.polygon([
        [-19.97905865893481, -48.7500970484087],
        [-19.97996307449961, -48.75027507472177],
        [-19.9811020746387, -48.74983458222321],
        [-19.98389042524932, -48.74852924713382],
        [-19.98346366005398, -48.7445897186723],
        [-19.98318148872516, -48.74450317239178],
        [-19.9818524302237, -48.74477537426722],
        [-19.97854631377889, -48.744053340756],
        [-19.97905865893481, -48.7500970484087]
    ], { color: 'white' })
        .bindPopup("<div>" +
            "<div><strong>Fazenda:</strong> Santa Terezinha</div>" +
            "<div><strong>Talhão:</strong> 003</div>" +
            "<div><strong>Área:</strong> 400,32</div>" +
            "</div>")
        .addTo(map);
}

