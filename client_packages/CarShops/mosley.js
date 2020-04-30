let autoBrowser = null;

mp.events.add("BuySurferVeh", () => {
	mp.events.callRemote("BuySurferVehToServer");
});
mp.events.add("BuyInjectionVeh", () => {
	mp.events.callRemote("BuyInjectionVehToServer");
});
mp.events.add("BuyEmperorVeh", () => {
	mp.events.callRemote("BuyEmperorVehToServer");
});
mp.events.add("BuyBodhiVeh", () => {
	mp.events.callRemote("BuyBodhiVehToServer");
});

//Fahrzeughändler
mp.events.add("BuyNovakVeh", () => {
	mp.events.callRemote("BuyNovakVehToServer");
});
mp.events.add("BuyCaracaraVeh", () => {
	mp.events.callRemote("BuyCaracaraVehToServer");
});
mp.events.add("BuyDrafterVeh", () => {
	mp.events.callRemote("BuyDrafterVehToServer");
});
mp.events.add("BuySchlagenVeh", () => {
	mp.events.callRemote("BuySchlagenVehToServer");
});

mp.events.add("OpenGebrauchtwagen", () => {
	if (!mp.gui.cursor.visible && !autoBrowser) {
		autoBrowser = mp.browsers.new('package://CarShops/Gebrauchtwagen.html');
		mp.gui.cursor.show(true, true);
		mp.gui.chat.activate(false);
	} else {
		if (autoBrowser) {
			autoBrowser.destroy();
			autoBrowser = null;
			mp.gui.cursor.show(false, false);
			mp.gui.chat.activate(true);	
		}
	}
});

mp.events.add("OpenAutohausStein", () => {
	if (!mp.gui.cursor.visible && !autoBrowser) {
		autoBrowser = mp.browsers.new('package://CarShops/AutoStein.html');
		mp.gui.cursor.show(true, true);
		mp.gui.chat.activate(false);
	} else {
		if (autoBrowser) {
			autoBrowser.destroy();
			autoBrowser = null;
			mp.gui.cursor.show(false, false);
			mp.gui.chat.activate(true);	
		}
	}
});

mp.events.add("CloseAutohausMenu", () => {
	autoBrowser.destroy();
	autoBrowser = null;
	mp.gui.cursor.show(false, false);
	mp.gui.chat.activate(true);
});










