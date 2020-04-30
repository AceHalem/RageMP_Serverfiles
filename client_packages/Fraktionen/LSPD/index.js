let ubrowser = null;

mp.events.add("SearchTheNumberplate", (numberplate) => {
	mp.events.callRemote("SearchNumberplate", numberplate);
});

mp.events.add("SearchThisPeople", (firstname, lastname) => {
	mp.events.callRemote("SearchPeopleLSPD", firstname, lastname);
});

mp.events.add({"NumberplateFound": (firstname, lastname, wanted) => {
    ubrowser.execute(`document.getElementById('namelabel').innerHTML="` + firstname + " " + lastname + `";`);
	}
});

mp.events.add({"LSPDcomputerfoundpeople": (firstname, lastname, wanted, money, veh, frak) => {
    ubrowser.execute(`document.getElementById('namelabel2').innerHTML="` + firstname + " " + lastname + `";`);
	if (wanted == 0) {
		    ubrowser.execute(`document.getElementById('wantedlabel').innerHTML="nicht gesucht";`);
	} else {
		    ubrowser.execute(`document.getElementById('wantedlabel').innerHTML="gesucht (`+ wanted +`)";`);
	}
	ubrowser.execute(`document.getElementById('moneylabel').innerHTML="`+ money +`";`);
	ubrowser.execute(`document.getElementById('vehlabel').innerHTML="`+ veh +`";`);
	ubrowser.execute(`document.getElementById('fraklabel').innerHTML="`+ frak +`";`);
	}
});

mp.events.add({   
    "closelspdcomputer": closeIt
});


mp.events.add("OeffneLSPDComputer", () => {
	if (!mp.gui.cursor.visible && !ubrowser) {
		ubrowser = mp.browsers.new('package://Fraktionen/LSPD/index.html');
		mp.gui.cursor.show(true, true);
		mp.gui.chat.activate(false);
	}
});

function closeIt() {
    if (ubrowser) {
        mp.gui.cursor.show(false, false);
		mp.gui.chat.activate(true);
        ubrowser.destroy();
        ubrowser = null;
    }
}