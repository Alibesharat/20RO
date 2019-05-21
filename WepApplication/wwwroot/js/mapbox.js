
mapboxgl.accessToken = 'pk.eyJ1IjoiYm5hbGluaWEiLCJhIjoiY2pzbGhlaW1iMnl6czN5bzY4aGtlNGRjMyJ9.w5OCvI1DM3vHw4iPcDsSPw';
var coordinates = document.getElementById('coordinates');
var lat = "51.399735";
var long = "35.752308";

   
function showError(error) {
    Error.innerHTML = error.message;
    switch (error.code) {
        case error.PERMISSION_DENIED:
            console.log("User denied the request for Geolocation.");
            break;
        case error.POSITION_UNAVAILABLE:
            console.log("Location information is unavailable.");
            break;
        case error.TIMEOUT:
            console.log("The request to get user location timed out.");
            break;
        case error.UNKNOWN_ERROR:
            console.log("An unknown error occurred.");
            break;
    }
}

var currentPosition = [lat,long ];

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

}), 'bottom-right');




//map.addControl(directions, 'top-left');
// disable map rotation using right click + drag
map.dragRotate.disable();

// disable map rotation using touch rotation gesture
map.touchZoomRotate.disableRotation();

