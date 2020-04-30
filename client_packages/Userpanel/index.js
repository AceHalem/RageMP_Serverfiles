let ubrowser = null;

mp.keys.bind(0x55, true, function() {
	if (!mp.gui.cursor.visible && !ubrowser) {
		mp.events.callRemote("getUserpanel");
		mp.events.callRemote("getUserpanelVehicles");
	} else {
		closeUserpanel();
	}
});

mp.events.add({"UserPanel": (c, frak, money, payday, autoschein, register, afpunkte) => {
	ubrowser = mp.browsers.new('package://Userpanel/index.html');
	mp.gui.cursor.show(true, true);
    ubrowser.execute(`document.getElementById('namelabel').innerHTML="` + c.name + `";`);
    ubrowser.execute(`document.getElementById('fraklabel').innerHTML="` + frak + `";`);
    ubrowser.execute(`document.getElementById('moneylabel').innerHTML="` + money + `";`);
	ubrowser.execute(`document.getElementById('paydaylabel').innerHTML="` + payday + `";`);
	ubrowser.execute(`document.getElementById('registerlabel').innerHTML="` + register + `";`);
	if (autoschein == 1) {
		ubrowser.execute(`document.getElementById('autoscheinlabel').innerHTML="Vorhanden - (` + afpunkte + ` Punkte)";`);
	} else {
		ubrowser.execute(`document.getElementById('autoscheinlabel').innerHTML="Nicht vorhanden";`);
	}
	},
	"closeUserpanel": closeUserpanel
});

function closeUserpanel() {
    if (ubrowser) {
        mp.gui.cursor.show(false, false);
        ubrowser.destroy();
        ubrowser = null;
    }
}

mp.events.add({
    "GetUserpanelVehicles": (json) => {
        json.forEach(e => {
            e.name = mp.game.vehicle.getDisplayNameFromVehicleModel(e.hash);
        });

        json = JSON.stringify(json);

        if(json.length == 2) {
            ubrowser.execute(`loadItems(${json});`);
        } else {
            ubrowser.execute(`loadItems(${json});`);
        }
        mp.gui.chat.push(json.length);
    }
});