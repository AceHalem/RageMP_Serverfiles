var loginBrowser = mp.browsers.new('package://Login/Login.html');
	if (!mp.gui.cursor.visible) {
		mp.gui.cursor.show(true, true);
	}

mp.players.local.freezePosition(true);

mp.game.ui.displayRadar(true);
mp.game.ui.displayHud(true);

/*
mp.game.ui.displayRadar(false);
mp.game.ui.displayHud(false);
*/
mp.gui.chat.activate(false);
mp.gui.chat.show(false);

let camera = undefined;
camera = mp.cameras.new('default', new mp.Vector3(344.3341, -998.8612, -98.19622), new mp.Vector3(0, 0, 0), 40);

camera.pointAtCoord(-986.61447, 0, -186.61447); //-99.19622 Changes the rotation of the camera to point towards a location
camera.setActive(true);
mp.game.cam.renderScriptCams(true, false, 0, true, false);


mp.events.add('uiLogin_LoginButton', (username, password) => {
    mp.events.callRemote('LoginAccount', username, password);
});

mp.events.add('uiLogin_registerButton', (username, password) => {
    mp.events.callRemote('RegisterAccount', username, password);
});

mp.events.add('RegisterResult', (result) => {
    if (result == 3) {

        //loginBrowser.destroy();
        //mp.gui.cursor.show(false, false);

        //require("./Reallife/Character/Main.js");
        //mp.events.callRemote("testnach");

        //loginBrowser.reload(true);

        loginBrowser.execute('document.getElementById("p2").innerHTML = "Du kannst dich nun einloggen!";');

    }
    else if (result = 2) {
        loginBrowser.execute('document.getElementById("p2").innerHTML = "Passwort stimmt nicht uber ein!";');
    }
    else if (result = 1) {
        //loginBrowser.reload(true);
        loginBrowser.execute('document.getElementById("p2").innerHTML = "Du kannst dich nun einloggen!";');
    } else if (result = 0) {
        loginBrowser.execute('document.getElementById("p2").innerHTML = "Der Benutzername ist schon Vorhanden!";');
    }
});

mp.events.add('Notify', (msg) => {
    mp.game.graphics.notify(msg);
});


let selectChar = function(cId){
    destroyCam();
    mp.events.callRemote('login.character.select', cId);
}


let destroyCam = function(){
    mp.players.local.freezePosition(false);

    /*
    mp.game.ui.displayRadar(true);
    mp.game.ui.displayHud(true);
    */
    mp.gui.chat.activate(true);
    mp.gui.chat.show(true);

    mp.game.cam.renderScriptCams(false, false, 0, true, false);

    camera.setActive(false);
    camera.destroy();
    camera = undefined;  
}

mp.events.add('LoginSuccess', (chars) => {
    mp.events.remove(["LoginSuccess", "uiLogin_LoginButton", "uiLogin_registerButton"]);
    loginBrowser.destroy();
    mp.gui.cursor.show(false, false);

    cCount = chars.length;
    if (cCount == 0) {
        destroyCam();
        mp.events.callRemote('login.character.creator');
    } else if (cCount == 1) {
        selectChar(chars[0].id);
    } else {
        let NativeUI = require("nativeui");
        charSelect = new NativeUI.Menu("Charakterauswahl", "Charakter Auswahl", new NativeUI.Point(50, 50));
        
        chars.forEach(function(c) {
            charSelect.AddItem(new NativeUI.UIMenuItem(c.firstName + " " + c.lastName, "Charakter auswählen"));
        });  
        
        charSelect.forceOpen = true;

        charSelect.ItemSelect.on((item, index)  => {
            cId = chars[index].id;
            selectChar(cId);
            charSelect.forceOpen = false;
            charSelect.Close();
            delete charSelect;
        });

        charSelect.MenuClose.on(()  => {
            if (charSelect.forceOpen) {
                charSelect.Open();
            }
            
        });

    }
});