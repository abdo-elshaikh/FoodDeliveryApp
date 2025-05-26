self.addEventListener('install', (event) => {
    event.waitUntil(
        caches.open('foodfast-cache-v2').then(cache => {
            return cache.addAll([
                '/',
                '/css/site.css',
                '/css/home.css',
                '/js/site.js',
                '/js/home.js',
                '/favicon.svg',
                '/apple-touch-icon.png',
                'https://source.unsplash.com/1920x1080/?food-delivery'
            ]);
        })
    );
});

self.addEventListener('fetch', (event) => {
    event.respondWith(
        caches.match(event.request).then(response => {
            return response || fetch(event.request).catch(() => {
                return caches.match('/');
            });
        })
    );
});

self.addEventListener('activate', (event) => {
    const cacheWhitelist = ['foodfast-cache-v2'];
    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames.map(cacheName => {
                    if (!cacheWhitelist.includes(cacheName)) {
                        return caches.delete(cacheName);
                    }
                })
            );
        })
    );
});
