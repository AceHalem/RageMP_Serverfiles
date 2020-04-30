let deathBrowser = null;
mp.nametags.enabled = false;
mp.game.gameplay.setFadeOutAfterDeath(false);

mp.events.add('DeathTrue', () => {
	deathBrowser = mp.browsers.new('package://Player/death.html');
	mp.gui.chat.activate(false);	
});

mp.events.add('DeathFalse', () => {
	deathBrowser.destroy();
	mp.gui.chat.activate(true);
});