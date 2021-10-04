import { Loader } from '@googlemaps/js-api-loader';
import { MapDefinition } from './map-definition';

const debug = false;

export function activeGoogleMaps({apiKey, domId, lat, lng, warn, warning } : MapDefinition) {    
  if(debug) console.log('build map', arguments);

  const loader = new Loader({
    apiKey: apiKey,
    version: "weekly",
    libraries: ["places"]
  });

  const mapOptions = {
    center: {
      lat: lat,
      lng: lng
    },
    zoom: 14,
    mapTypeControl: true,
    zoomControl: true,
    scaleControl: true,
    scrollwheel: false,
    mapTypeId: 'roadmap'
  };  

  if(warn) showKeyWarnings(warning);

  loader.load().then((google) => {
    var map = new google.maps.Map(document.getElementById(domId), mapOptions);

    new google.maps.Marker({
      position: {
        lat: lat,
        lng: lng
      },
      map: map,
    });

    if(debug) console.log('map loaded');
  });

  function showKeyWarnings(warning: string) {
    var googleMapsElem = document.getElementsByClassName('app-peopledirectory4-js-google-map-container');

    if(googleMapsElem.length != 0) {
      for(var i = 0; i < googleMapsElem.length; i++) {
        if(!googleMapsElem[i].classList.contains('has-warning')) {
          googleMapsElem[i].classList.add('has-warning');
          googleMapsElem[i].innerHTML = googleMapsElem[i].innerHTML + '<div class="alert alert-danger">' + warning + '</div>';

        }
      }
    }
  }
}