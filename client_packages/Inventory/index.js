var inv;

// I Key
mp.keys.bind(0x49, true, function () {
    if (!inv && !mp.gui.cursor.visible) {
        mp.events.callRemote("getItems");
    } else {
        closeInventory();
    }
});

mp.events.add({
    "recieveItems": (json) => {
        //	freezeControls, cursorNotVisible;
        mp.gui.cursor.show(true, true);
        inv = mp.browsers.new("package://Inventory/inventory.html");

        json = JSON.stringify(json);
        inv.execute(`loadItems(${json});`);
    },
    "closeInventory": closeInventory
});

function closeInventory() {
    if (inv) {
        mp.gui.cursor.show(false, false);
        inv.destroy();
        inv = null;
    }
}