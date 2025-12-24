// Smart Girl Algebra - Service Worker
// This enables offline functionality and faster loading

const CACHE_NAME = 'smart-girl-algebra-v20251224';
const urlsToCache = [
    '/',
    '/index.html',
    '/css/app.css',
    '/css/cheer-theme.css',
    '/images/gogirl.png',
    '/images/blucheer.png',
    '/images/cheery.png',
    '/images/pompoms.png',
    '/manifest.json',
    '/appsettings.json'
];

// Install event - cache resources
self.addEventListener('install', event => {
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(cache => {
                console.log('Opened cache');
                return cache.addAll(urlsToCache);
            })
    );
});

// Fetch event - serve from cache when offline, but ALWAYS fetch fresh images and API calls
self.addEventListener('fetch', event => {
    // NEVER cache images - always fetch fresh!
    if (event.request.url.includes('/images/')) {
        event.respondWith(
            fetch(event.request, { cache: 'no-store' })
        );
        return;
    }

    // NEVER cache API calls - always fetch fresh!
    if (event.request.url.includes('azurewebsites.net') ||
        event.request.url.includes('/api/') ||
        event.request.url.includes('localhost:7217')) {
        event.respondWith(
            fetch(event.request, { cache: 'no-store' })
        );
        return;
    }

    // For other resources, use cache
    event.respondWith(
        caches.match(event.request)
            .then(response => {
                // Cache hit - return response
                if (response) {
                    return response;
                }
                return fetch(event.request);
            }
        )
    );
});

// Activate event - clean up old caches
self.addEventListener('activate', event => {
    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames.map(cacheName => {
                    if (cacheName !== CACHE_NAME) {
                        console.log('Deleting old cache:', cacheName);
                        return caches.delete(cacheName);
                    }
                })
            );
        })
    );
});

