let charBrowser = null;

mp.events.add("StartCharBrowser", () => {
	charBrowser = mp.browsers.new('package://Character/Character.html');
	mp.gui.cursor.show(true, true);	
	mp.gui.chat.activate(false);
	mp.gui.chat.show(false);	
});
	
mp.events.add('CharacterInformationToServer', (vorname, nachname) => {
    mp.events.callRemote('OnPlayerCharacterAttempt', vorname, nachname);
});

mp.events.add('FinishCharacter', () => {
    charBrowser.destroy();
	if (mp.gui.cursor.visible) {
	    mp.gui.cursor.show(false, false);	
	}
	mp.gui.chat.activate(true);
	mp.gui.chat.show(true);
});