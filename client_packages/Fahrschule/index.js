let Fahrschule = null;

mp.events.add('autoButton_check', () => {
    mp.events.callRemote('Carlicense');
	mp.gui.cursor.show(false, false);
	mp.gui.chat.activate(true);
    mp.gui.chat.show(true);
	
	Fahrschule.destroy();
	Fahrschule = null;
});

mp.events.add('StartFahrschulBrowser', () => {
	if (Fahrschule != null) {
		mp.gui.cursor.show(false, false);
		mp.gui.chat.activate(true);
		mp.gui.chat.show(true);
		
		Fahrschule.destroy();
		Fahrschule = null;
		return;
	}
		
	Fahrschule = mp.browsers.new('package://Fahrschule/Fahrschule.html');
	mp.gui.cursor.show(true, true);
		
	mp.gui.chat.activate(false);
	mp.gui.chat.show(false);	
});