
let browser = null;

mp.keys.bind(0x72, false, function () {
    if (browser != null) {
        browser.destroy();
        browser = null;
		mp.gui.cursor.show(false, false);
        return
    }
	if (mp.gui.cursor.visible)
		return;
	
	browser = mp.browsers.new('package://playerlist/index.html');

	mp.gui.cursor.show(true, true);

    mp.players.forEach(
        (player) => {
            browser.execute('createUser("' + player.name + '")');
        }
    );
})
