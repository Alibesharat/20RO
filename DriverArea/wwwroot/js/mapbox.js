
mapboxgl.accessToken = 'pk.eyJ1IjoiYm5hbGluaWEiLCJhIjoiY2pzbGhlaW1iMnl6czN5bzY4aGtlNGRjMyJ9.w5OCvI1DM3vHw4iPcDsSPw';
var coordinates = document.getElementById('coordinates');
var currentPosition = [51.399735, 35.752308];

var map = new mapboxgl.Map({
    container: 'map',
    style: 'mapbox://styles/mapbox/navigation-guidance-day-v4?optimize=true',
    center: currentPosition,
    zoom: 13,
    maxTileCacheSize: 100,
    localIdeographFontFamily: "'Shabnam', sans-serif"



});

mapboxgl.setRTLTextPlugin('https://api.mapbox.com/mapbox-gl-js/plugins/mapbox-gl-rtl-text/v0.2.0/mapbox-gl-rtl-text.js');


function onDragEnd() {
    var lngLat = map.getCenter();

    //coordinates.style.display = 'block';
    coordinates.innerHTML = 'طول جغرافیایی: ' + lngLat.lng + '<br />عرض جفرافیایی: ' + lngLat.lat;
}


map.on('dragend', function () {

    onDragEnd();
});

function GetCenter() {

    onDragEnd();
}


map.addControl(new mapboxgl.GeolocateControl({
    positionOptions: {
        enableHighAccuracy: true
    },
    trackUserLocation: true,

}));
map.addControl(new mapboxgl.FullscreenControl());

map.addControl(directions, 'top-left');
// disable map rotation using right click + drag
map.dragRotate.disable();

// disable map rotation using touch rotation gesture
map.touchZoomRotate.disableRotation();

