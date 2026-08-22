// Self-removing service worker.
//
// The previous worker cached "/" and "/index.html" under a hard-coded cache name
// (smart-girl-algebra-v20251224) and served them cache-first. Because the worker
// file itself never changed, browsers never re-installed it, its cleanup code
// never ran, and anyone who visited in December kept being served December's
// homepage forever — no matter what was deployed.
//
// This replacement exists only to undo that. Changing the file's bytes is what
// makes browsers pick it up: the old page polls registration.update(), fetches
// this, sees a difference, and installs it. It then wipes every cache, removes
// itself, and reloads any open tab, which now loads from the network.
//
// index.html no longer registers a worker, so the reloaded page installs nothing
// and the cycle ends. Do not re-add a registration without a cache name derived
// from the build, or this exact bug comes straight back.

self.addEventListener('install', () => {
    self.skipWaiting();
});

self.addEventListener('activate', event => {
    event.waitUntil((async () => {
        const names = await caches.keys();
        await Promise.all(names.map(name => caches.delete(name)));

        await self.registration.unregister();

        const clients = await self.clients.matchAll({ type: 'window' });
        for (const client of clients) {
            client.navigate(client.url);
        }
    })());
});

// While this worker is briefly in control, never answer from a cache.
self.addEventListener('fetch', event => {
    event.respondWith(fetch(event.request));
});
