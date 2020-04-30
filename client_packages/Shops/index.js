let shopBrowser = null;

mp.events.add("BuyBenzinkanister", () => {
	mp.events.callRemote("BuyBenzinkanisterToServer");
});

mp.events.add("OpenShop", () => {
	if (!mp.gui.cursor.visible && !shopBrowser) {
		shopBrowser = mp.browsers.new('package://Shops/index.html');
		mp.gui.cursor.show(true, true);
		mp.gui.chat.activate(false);
	} else {
		if (shopBrowser) {
			shopBrowser.destroy();
			shopBrowser = null;
			mp.gui.cursor.show(false, false);
			mp.gui.chat.activate(true);	
		}
	}
});







