let bankPinBrowser = null;
let bankBrowser = null;

mp.events.add("bankterminalmoney", (money) => {
	bankBrowser.execute(`document.getElementById('bankmoney').innerHTML=` + money + `;`);
});

mp.events.add("bKontoLoginToServer", (pin) => {
	mp.events.callRemote("OnPlayerBankLogin", pin);
});

mp.events.add("bKontoRegisterToServer", (pin, pin2) => {
	mp.events.callRemote("OnPlayerBankRegister", pin, pin2);
});

mp.events.add("bKontoEinzahlen", (summe) => {
	mp.events.callRemote("OnPlayerBankEinzahlen", summe);
});

mp.events.add("bKontoAuszahlen", (summe) => {
	mp.events.callRemote("OnPlayerBankAuszahlen", summe);
});

mp.events.add("bKontoUeberweisen", (player, summe) => {
	mp.events.callRemote("OnPlayerBankUeberweisen", player, summe);
});

mp.events.add("StartEnterBankPin", () => {
	if (!mp.gui.cursor.visible && !bankPinBrowser) {
	bankPinBrowser = mp.browsers.new('package://Bank/bKontoLogin.html');
		mp.gui.cursor.show(true, true);
		mp.gui.chat.activate(false);	
	} else {
		if (bankPinBrowser) {
			bankPinBrowser.destroy();
			bankPinBrowser = null;
			mp.gui.cursor.show(false, false);
			mp.gui.chat.activate(true);	
		}
	}
});

mp.events.add("StartCreateBankPin", () => {
	if (!mp.gui.cursor.visible && !bankPinBrowser) {
	bankPinBrowser = mp.browsers.new('package://Bank/bKontoRegister.html');
		mp.gui.cursor.show(true, true);
		mp.gui.chat.activate(false);	
	} else {
		if (bankPinBrowser) {
			bankPinBrowser.destroy();
			bankPinBrowser = null;
			mp.gui.cursor.show(false, false);
			mp.gui.chat.activate(true);	
		}
	}
});

mp.events.add("ShowBankTerminal", () => {
	bankPinBrowser.destroy();
	bankPinBrowser = null;
	bankBrowser = mp.browsers.new('package://Bank/bterminal.html');
});

mp.events.add({   
    "closebterminal": closeTerminal
});

function closeTerminal() {
    if (bankBrowser) {
		bankBrowser.destroy();
		bankBrowser = null;
		mp.gui.cursor.show(false, false);
		mp.gui.chat.activate(true);	
    }
}