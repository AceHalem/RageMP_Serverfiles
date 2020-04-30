var garage;

// F10
mp.keys.bind(0x79, true, function () {
	mp.events.callRemote("IsInNearVehStore");
});

mp.events.add('OpenVehStore', () => {
    if (!garage) {
        mp.gui.cursor.show(true, true);
		mp.gui.chat.activate(false);
		mp.gui.chat.show(false);
        garage = mp.browsers.new("package://Vehicles/garage.html");
        mp.events.callRemote("getVehicles");
    } else {
        mp.gui.cursor.show(false, false);
		mp.gui.chat.activate(true);
		mp.gui.chat.show(true);
        garage.destroy();
        garage = null;
    }
});

mp.events.add({
    "receiveVehicles": (json) => {
        json.forEach(e => {
            e.name = mp.game.vehicle.getDisplayNameFromVehicleModel(e.hash);
        });

        json = JSON.stringify(json);
        mp.gui.chat.push("länge" + json.length);

        if(json.length == 2) {
            garage.execute(`loadItems(${json});`);
        } else {
            garage.execute(`loadItems(${json});`);
        }
        mp.gui.chat.push(json.length);
    },
    "spawnVehicle": (id) => {
        mp.events.callRemote("spawnVehicle", id);
        mp.gui.cursor.show(false, false);
		mp.gui.chat.activate(true);
		mp.gui.chat.show(true);
        garage.destroy();
        garage = null;
    }
});