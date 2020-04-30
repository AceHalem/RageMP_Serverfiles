let ubrowser = null;

mp.events.add("CreateNewNumberplate", (numberplate) => {
	mp.events.callRemote("NewNumberplate", numberplate);
});

mp.events.add("OeffneZulassungsstelle", () => {
	if (!mp.gui.cursor.visible && !ubrowser) {
		ubrowser = mp.browsers.new('package://Staat/index.html');
		mp.gui.cursor.show(true, true);
		mp.gui.chat.activate(false);
	} else {
		closeIt();
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