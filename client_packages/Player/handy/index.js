let handy = null;

mp.events.add("callTaxiNow", () => {
	mp.events.callRemote("callTaxiToServer");
});

mp.events.add("PlayerSmartphone", () => {
    if (handy != null) {
		handy.destroy() 
		handy = null 
		mp.gui.chat.activate(true);
		mp.gui.chat.show(true);
		if (mp.gui.cursor.visible) {
			mp.gui.cursor.show(false, false);
		}
		return;
    }
	if (mp.gui.cursor.visible)
		return;
	
	handy = mp.browsers.new("package://Player/handy/handy.html");  
	mp.gui.cursor.show(true, true);
	mp.gui.chat.activate(false);
	mp.gui.chat.show(false);
});
