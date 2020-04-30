let Shop = null;
let sceneryCamera = null;

const creatorCoords = {
    camera: new mp.Vector3(429.826, -810.155, 29.49115),
    cameraLookAt: new mp.Vector3(429.826, -809.155, 29.49115)
};

mp.events.add("KleidungAbbrechen", () => {
    mp.events.callRemote("KleidungSchliessen");
    Shop.destroy();
    mp.gui.cursor.show(false, false);
    mp.gui.chat.activate(true);
    mp.players.local.freezePosition(false);
    mp.game.ui.displayRadar(true);
    mp.game.ui.displayHud(true);
    mp.gui.chat.show(true);
    sceneryCamera.setActive(false);
    sceneryCamera.destroy();
    mp.game.cam.renderScriptCams(false, false, 0, false, false);
});

mp.events.add("KleidungKaufen", () => {
    mp.events.callRemote("KleidungBuy");
    Shop.destroy();
    mp.gui.cursor.show(false, false);
    mp.gui.chat.activate(true);
    mp.players.local.freezePosition(false);
    mp.game.ui.displayRadar(true);
    mp.game.ui.displayHud(true);
    mp.gui.chat.show(true);
    sceneryCamera.setActive(false);
    sceneryCamera.destroy();
    mp.game.cam.renderScriptCams(false, false, 0, false, false);
});

mp.events.add("Hose1", () => {
    mp.events.callRemote("Hose1Buy");
});

mp.events.add("Hose2", () => {
    mp.events.callRemote("Hose2Buy");
});

mp.events.add("Hose3", () => {
    mp.events.callRemote("Hose3Buy");
});

mp.events.add("Oberteil1", () => {
    mp.events.callRemote("Oberteil1Buy");
});

mp.events.add("Oberteil2", () => {
    mp.events.callRemote("Oberteil2Buy");
});

mp.events.add("Oberteil3", () => {
    mp.events.callRemote("Oberteil3Buy");
});

mp.events.add("Schuhe1", () => {
    mp.events.callRemote("Schuhe1Buy");
});

mp.events.add("Schuhe2", () => {
    mp.events.callRemote("Schuhe2Buy");
});

mp.events.add("Schuhe3", () => {
    mp.events.callRemote("Schuhe3Buy");
});

mp.events.add("OpenKleidungsgeschäft", () => {
	if (!mp.gui.cursor.visible) {
		Shop = mp.browsers.new('package://Kleidung/index.html');
		mp.gui.cursor.show(true, true);
        mp.gui.chat.activate(false);
        mp.players.local.freezePosition(true);
        mp.game.ui.displayRadar(false);
        mp.game.ui.displayHud(false);
        mp.gui.chat.show(false);
        sceneryCamera = mp.cameras.new('default', new mp.Vector3(429.5963, -808.9404, 29.49114), new mp.Vector3(0, 0, 180.1772), 40)
        sceneryCamera.setActive(true);
        mp.game.cam.renderScriptCams(true, false, 0, true, false);
	}
});