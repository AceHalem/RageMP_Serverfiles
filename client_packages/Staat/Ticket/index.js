let ubrowser = null;

mp.events.add("CreateNewTicket", (Preis, Text) => {
	mp.events.callRemote("CNT", Preis, Text);
	closeIt();
});

mp.events.add("ReadNewTicket", (Preis) => {
	mp.events.callRemote("RNT", Preis);
	closeIt();
});

mp.events.add("OeffneNewTicket", () => {
		mp.gui.chat.activate(false);
		mp.gui.cursor.show(true, true);
		ubrowser = mp.browsers.new('package://Staat/Ticket/index.html');
});

mp.events.add("OeffneReadTicket", (Preis, Text) => {
		ubrowser = mp.browsers.new('package://Staat/Ticket/index2.html');
		ubrowser.execute(`document.getElementById('TicketBetrag').innerHTML="` + Preis + `";`);
		ubrowser.execute(`document.getElementById('TicketText').innerHTML="` + Text + `";`);
		mp.gui.cursor.show(true, true);
		mp.gui.chat.activate(false);
});

mp.events.add({   
    "closeticket": closeIt
});

function closeIt() {
    if (ubrowser) {
        mp.gui.cursor.show(false, false);
		mp.gui.chat.activate(true);
        ubrowser.destroy();
        ubrowser = null;
    }
}