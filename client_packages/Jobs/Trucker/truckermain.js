let truckerBrowser = null;

mp.events.add("TruckerLevelToClient1", () => {
	mp.events.callRemote("OnTruckerLevel1");
});

mp.events.add("TruckerLevelToClient2", () => {
	mp.events.callRemote("OnTruckerLevel2");
});

mp.events.add("TruckerLevelToClient3", () => {
	mp.events.callRemote("OnTruckerLevel3");
});

mp.events.add("OpenTruckerMain", () => {
	truckerBrowser = mp.browsers.new('package://Jobs/Trucker/Truckermain.html');
	mp.gui.cursor.show(true, true);
});

mp.events.add("DeleteTruckerBrowser", () => {
	truckerBrowser.destroy();
	mp.gui.cursor.show(false, false);
});