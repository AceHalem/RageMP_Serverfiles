/*mp.keys.bind(0x45, true, function () { // Taste: E
    const camera = mp.cameras.new("gameplay");
    const distance = 2;

    let position = new mp.Vector3(mp.players.local.position.x, mp.players.local.position.y, mp.players.local.position.z + 0.7);
    let direction = camera.getDirection();
    let target = new mp.Vector3((direction.x * distance) + position.x, (direction.y * distance) + position.y, (direction.z * distance) + position.z);
    let raycastResult = mp.raycasting.testPointToPoint(position, target, [], -1);

    if (raycastResult != undefined) {
        let entityHandle = raycastResult.entity;
        let entityModelHash = mp.game.invoke("0x9F47B058362C84B5", entityHandle); // native GET_ENTITY_MODEL
        let entityPosition = raycastResult.position;

        if (entityModelHash != entityHandle && entityModelHash != 0) {
            mp.events.callRemote("PlayerInteract", entityModelHash, entityPosition);
            mp.gui.chat.push("DEBUG: Called PlayerInteract with entityModelHash " + entityModelHash + " and entityPosition x=" + entityPosition.x + " y=" + entityPosition.y + " z=" + entityPosition.z);
        }
    }
});*/

var isopen = 0;

mp.events.add("StartHouseMenu", (lockedhouse, c) => {
	if (mp.gui.cursor.visible)
		return;

    if (isopen == 1)
        return;
	
	isopen = 1;
    let NativeUI = require("nativeui");
    housemenu = new NativeUI.Menu("Haus", "Haussystem", new NativeUI.Point(50, 50));

    mp.gui.chat.show(false);

    housemenu.AddItem(new NativeUI.UIMenuItem("Menü schließen"));
    if (lockedhouse == 0) {       
        housemenu.AddItem(new NativeUI.UIMenuItem("Haus abschließen", "~r~Schlüssel wird benötigt!"));
        housemenu.AddItem(new NativeUI.UIMenuItem("Haus betreten"));
    } else {
        housemenu.AddItem(new NativeUI.UIMenuItem("Haus öffnen", "~r~Schlüssel wird benötigt!"));
    }

    housemenu.ItemSelect.on((item, index)  => {
        switch(index) {
            case 1:
                    mp.events.callRemote("LockThatHouse", c);
                break;
            case 2:
                    mp.events.callRemote("EnterThatHouse", c);
                break;
        };
		isopen = 0;
        housemenu.Close();
        delete housemenu;
        mp.gui.chat.show(true);
    });

});

mp.events.add("StartExitHouseMenu", (c, locked) => {
	if (mp.gui.cursor.visible)
	return;
	
    if (isopen == 1)
        return;
	
	isopen = 1
    let NativeUI = require("nativeui");
    garagemenu = new NativeUI.Menu("Haus", "Haussystem", new NativeUI.Point(50, 50));

    mp.gui.chat.show(false);
      
    garagemenu.AddItem(new NativeUI.UIMenuItem("Menü schließen"));
    garagemenu.AddItem(new NativeUI.UIMenuItem("Haus verlassen"));
    if (locked == 0) {
        garagemenu.AddItem(new NativeUI.UIMenuItem("Haus abschließen"));
    } else {
        garagemenu.AddItem(new NativeUI.UIMenuItem("Haus öffnen"));
    }

    garagemenu.ItemSelect.on((item, index)  => {
        switch(index) {
            case 1:
                mp.events.callRemote("ExitThatHouse", c);
                break;
            case 2:
                mp.events.callRemote("LockThatHouse", c);
                break;
        };
		isopen = 0;
        garagemenu.Close();
        delete garagemenu;
        mp.gui.chat.show(true);
    });
});

mp.events.add("PlayerInteraction", (duty) => {
	if (mp.gui.cursor.visible)
		return;
	
    if (isopen == 1)
        return;
	
	isopen = 1;
    let NativeUI = require("nativeui");
    imenu = new NativeUI.Menu("Interaktion`s Menü", "", new NativeUI.Point(50, 50));

    mp.gui.chat.show(false);
      
	imenu.AddItem(new NativeUI.UIMenuItem("~r~Verlassen", "~r~Menü Schließen!"));
	imenu.AddItem(new NativeUI.UIMenuItem("Ausweis Zeigen", "Zeigt einem Spieler in deiner Nähe deinen Ausweis!"));
	if (duty == 1) {
        imenu.AddItem(new NativeUI.UIMenuItem("Dienstausweis zeigen"));
    }

    imenu.ItemSelect.on((item, index)  => {
        switch(index) {
            case 1:
                mp.events.callRemote("ShowPlayerAusweis");
                break;
			case 2:
                mp.events.callRemote("ShowPlayerDienstAusweis");
                break;
        };
		isopen = 0;
        imenu.Close();
        delete imenu;
        mp.gui.chat.show(true);
    });
});

mp.events.add("VehicleInteraction", (isinvehicle, isinrepair) => {
	if (mp.gui.cursor.visible)
		return;
	
    if (isopen == 1)
        return;
	
	isopen = 1;
    let NativeUI = require("nativeui");
    imenu = new NativeUI.Menu("Interaktion`s Menü", "", new NativeUI.Point(50, 50));

    mp.gui.chat.show(false);
      
	imenu.AddItem(new NativeUI.UIMenuItem("~r~Verlassen", "~r~Menü Schließen!"));
	imenu.AddItem(new NativeUI.UIMenuItem("Fahrzeug abschließen"));
    if (isinvehicle == 1) {
	imenu.AddItem(new NativeUI.UIMenuItem("Motor An/Aus"));
	imenu.AddItem(new NativeUI.UIMenuItem("Fahrzeug parken"));
	}
	if (isinrepair == 1) {
		imenu.AddItem(new NativeUI.UIMenuItem("Fahrzeug reparieren", "Repariere dein Fahrzeug für ~g~100$"));
	}

    imenu.ItemSelect.on((item, index)  => {
        switch(index) {
            case 1:
                mp.events.callRemote("LockOrUnlockVeh");
                break;
			case 2:
				mp.events.callRemote("toggleEngine");
				break;
			case 3:
				mp.events.callRemote("ParkVehicle");
				break;
			case 4:
				mp.events.callRemote("RepairVehicle");
				break;
        };
		isopen = 0;
        imenu.Close();
        delete imenu;
        mp.gui.chat.show(true);
    });
});

mp.keys.bind(0x4D, false, function() {
	mp.events.callRemote("OpenPlayerInteraction", player);
});

mp.keys.bind(0x58, true, function() {
	mp.events.callRemote("OpenVehicleInteraction", player);
});

mp.keys.bind(0x45, false, function() {
    mp.events.callRemote("ExitHouseMenuOpen", player);
	mp.events.callRemote("HouseMenuOpen", player);
	mp.events.callRemote("OpenShopMenu", player);
	mp.events.callRemote("Fahrradverleih", player);
	mp.events.callRemote("Jobs", player);
	mp.events.callRemote("StartATM", player);
	mp.events.callRemote("StartLicenses", player);
	mp.events.callRemote("OpenShopBrowser", player);
    mp.events.callRemote("StartHanfSammeln", player);
    mp.events.callRemote("OpenKleidung", player);
	mp.events.callRemote("Egd", player);
	mp.events.callRemote("Ekey", player);
	mp.events.callRemote("OPC", player);
	mp.events.callRemote("VehicleInteraction", player);
});