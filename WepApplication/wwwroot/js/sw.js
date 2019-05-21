var cacheName = 'ilicar-v1';
var appShellFiles = [
  '/css/materialize-rtl.css',
  '/css/site.css',
  '/fonts/font.css',
  '/fonts/Shabnam-Bold-FD.eot',
  '/fonts/Shabnam-Bold-FD.ttf',
  '/fonts/Shabnam-Bold-FD.woff',
  '/fonts/Shabnam-Bold-FD.woff2',
  '/fonts/Shabnam-FD.eot',
  '/fonts/Shabnam-FD.ttf',
  '/fonts/Shabnam-FD.woff',
  '/fonts/Shabnam-FD.woff2',
  '/fonts/Shabnam-Light-FD.eot',
  '/fonts/Shabnam-Light-FD.ttf',
  '/fonts/Shabnam-Light-FD.woff',
  '/fonts/Shabnam-Light-FD.woff2',
  '/js/fastclick.js',
  '/js/initmatrailcss.js',
  '/js/mapbox.js',
  '/js/statatics/loc3.png',
  '/js/statatics/photo-camera-lines2.png',
  '/js/statatics/wh512.png',
 

];

self.addEventListener('install', function (e) {
    e.waitUntil(
      caches.open(cacheName).then(function (cache) {
          console.log('[Service Worker] Caching all: app shell and content');
          return cache.addAll(contentToCache);
      })
    );
});

