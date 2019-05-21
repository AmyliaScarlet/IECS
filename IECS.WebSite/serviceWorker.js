const CACHE_NAME = 'v1'


self.addEventListener('install', event => {
    const preCache = async () => {
        const cache = await caches.open(CACHE_NAME);
        if (cache != null) {
            await cache.addAll([
                '/',
                '/Page/',
                '/css/PageCss.css',
                '/js/PageJs.js'
            ]);
        } else {
            console.log('install: cache null');
        }

    };

    event.waitUntil(preCache());

});


self.addEventListener('fetch', event => {
    let { request } = event

    event.respondWith(
        // 先从 caches 中寻找是否有匹配
        caches.match(request).then(res => {
            if (res) {
                return res
            }

            // 对于 CDN 资源要更改 request 的 mode
            if (request.mode !== 'navigate' && request.url.indexOf(request.referrer) === -1) {
                request = new Request(request, { mode: 'cors' })
            }

            // 对于不在 caches 中的资源进行请求
            return fetch(request).then(fetchRes => {
                // 这里只缓存成功 && 请求是 GET 方式的结果，对于 POST 等请求，可把 indexDB 给用上
                if (!fetchRes || fetchRes.status !== 200 || request.method !== 'GET') {
                    return fetchRes
                }

                let resClone = fetchRes.clone()

                const preCache = async () => {
                    const cache = await caches.open(CACHE_NAME);
                    if (cache != null) {
                        await cache.put(request, fetchRes)
                    } else {
                        console.log('fetch: cache null');
                    }

                };
                event.waitUntil(preCache());

                console.log('cache:' + request.method)

                return resClone
            })
        })
    )
})

self.addEventListener('activate', event => {
    console.log('ServiceWorker activated')
    event.waitUntil(
        // 删除旧文件
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames.map((cacheName) => {
                    return caches.delete(cacheName);
                })
            );
        })
    );
})
