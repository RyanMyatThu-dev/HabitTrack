window.indexedDbManager = {
    dbName: "HabitTrackerDB",
    dbVersion: 1,

    init: function () {
        return new Promise((resolve, reject) => {
            const request = indexedDB.open(this.dbName, this.dbVersion);
            request.onupgradeneeded = (event) => {
                const db = event.target.result;
                if (!db.objectStoreNames.contains("habits")) {
                    db.createObjectStore("habits", { keyPath: "Id" });
                }
            };
            request.onsuccess = () => resolve();
            request.onerror = (e) => reject(e);
        });
    },

    getAll: async function (storeName) {
        await this.init();
        return new Promise((resolve, reject) => {
            const request = indexedDB.open(this.dbName);
            request.onsuccess = (event) => {
                const db = event.target.result;
                const transaction = db.transaction(storeName, "readonly");
                const store = transaction.objectStore(storeName);
                const getAllRequest = store.getAll();
                getAllRequest.onsuccess = () => resolve(JSON.stringify(getAllRequest.result));
                getAllRequest.onerror = (e) => reject(e);
            };
        });
    },

    getById: async function (storeName, id) {
        await this.init();
        return new Promise((resolve, reject) => {
            const request = indexedDB.open(this.dbName);
            request.onsuccess = (event) => {
                const db = event.target.result;
                const transaction = db.transaction(storeName, "readonly");
                const store = transaction.objectStore(storeName);
                const getRequest = store.get(id);
                getRequest.onsuccess = () => resolve(JSON.stringify(getRequest.result));
                getRequest.onerror = (e) => reject(e);
            };
        });
    },

    add: async function (storeName, json) {
        await this.init();
        const item = JSON.parse(json);
        return new Promise((resolve, reject) => {
            const request = indexedDB.open(this.dbName);
            request.onsuccess = (event) => {
                const db = event.target.result;
                const transaction = db.transaction(storeName, "readwrite");
                const store = transaction.objectStore(storeName);
                store.add(item);
                transaction.oncomplete = () => resolve();
                transaction.onerror = (e) => reject(e);
            };
        });
    },

    update: async function (storeName, json) {
        await this.init();
        const item = JSON.parse(json);
        return new Promise((resolve, reject) => {
            const request = indexedDB.open(this.dbName);
            request.onsuccess = (event) => {
                const db = event.target.result;
                const transaction = db.transaction(storeName, "readwrite");
                const store = transaction.objectStore(storeName);
                store.put(item);
                transaction.oncomplete = () => resolve();
                transaction.onerror = (e) => reject(e);
            };
        });
    },

    delete: async function (storeName, id) {
        await this.init();
        return new Promise((resolve, reject) => {
            const request = indexedDB.open(this.dbName);
            request.onsuccess = (event) => {
                const db = event.target.result;
                const transaction = db.transaction(storeName, "readwrite");
                const store = transaction.objectStore(storeName);
                store.delete(id);
                transaction.oncomplete = () => resolve();
                transaction.onerror = (e) => reject(e);
            };
        });
    }
};
