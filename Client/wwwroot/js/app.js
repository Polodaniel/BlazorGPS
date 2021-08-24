function UpdateSystem() {

    var cacheWhitelist = ['v2'];

    caches.keys().then(function (keyList) {

        keyList.map(function (key) {
            if (cacheWhitelist.indexOf(key) === -1) {
                caches.delete(key);
            }
        });
        window.location.reload(true);
    });
}