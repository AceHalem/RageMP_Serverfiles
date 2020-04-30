const updateInterval = 60;

var speedo = mp.browsers.new("package://Vehicles/speedometer.html");
var showed = false;
var player = mp.players.local;

var veh = null;
var eng = false;

var vId = -1;
var vHp = 0;
var vKm = 0;
var vMulti = 0;
var vFuel = 0;
var vFuelTank = 0;
var vFuelConsumption = 0;
var updateI = 0;
var vMode = 0;
var vCurMulti = 0;
var vCMulti = 0;

mp.events.add('render', () => {
	if(showed) {
		if (player.vehicle) {
			let vel = player.vehicle.getSpeed() * 3.6 ; 
			speedo.execute('update(' + vel.toFixed(0) + ');');
		}
	}
});

mp.events.add('epm', (multi) => {
	vMulti = multi;
	player.vehicle.setEnginePowerMultiplier(vMulti);
	mp.gui.chat.push("EPM: " + vMulti);
});

/*mp.keys.bind(0x47, true, function() {
	if(!player.vehicle)
		return;
	
	if(mp.gui.cursor.visible)
		return;
	
	vMode = vMode + 1;
	if (vMode > 2) {
		vMode = 0;
	}
	setMulti()
});*/


mp.events.add('vehicleEnter', (id, hp, km, multi, fuel, fuelTank, fuelConsumption) => {
	vId = id;
	vHp = hp;
	vKm = km;
	vMulti = multi;
	vFuel = fuel;
	vFuelTank = fuelTank;
	vFuelConsumption = fuelConsumption;
	vMode = 0;
	veh = player.vehicle;
	
	if(!showed) {
		showed = true;
		speedo.execute("show();");
	} /*else {
		mp.gui.chat.push("NOT Showing Speedo");
	}*/

	if (vFuel == 0) {
		mp.game.graphics.notify("~r~ Achtung Tank ist leer!");
	} 

	speedo.execute('updateG(' + (vFuel / vFuelTank * 100 ) + ',' + vKm.toFixed(2) + ');');

	//setMulti()
	//mp.gui.chat.push("EPM: " + vMulti + "Reduced: " + vCurMulti);

});


mp.events.add("playerEnterVehicle", (vehicle, seat) => {
	if (seat == 0) {
		if (!vehicle.getIsEngineRunning()) {
			vehicle.setEngineOn(false, true, true);
		}
	}
	veh = vehicle;
});

mp.events.add("StopJobVehSpeedo", () => {
	speedo.execute("hide();");
	showed = false;
});

mp.events.add("playerLeaveVehicle", () => {
	speedo.execute("hide();");
	showed = false;

	updateVeh();

	/*if (eng) {
		veh.setEngineOn(true, true, true);
	}*/

	vId = -1;
	vHp = 0;
	vKm = 0;
	vMulti = 0;
	vCurMulti = 0;
	vFuel = 0;
	vFuelTank = 0;
	vFuelConsumption = 0;
	vMode = 0;

	updateI = 0;
	eng = false;
});

/*function setMulti() {
	switch(vMode) {
		case 1:
			vCurMulti = 1 / 2;
			vCMulti = 12.5;
			mp.gui.chat.push("Cruise Modus");
		  break;
		case 2:
			vCurMulti = 1 / 10;
			vCMulti = 10;
			mp.gui.chat.push("Eco Modus");
		  break;
		default:
			vCurMulti = vMulti;
			vCMulti = 7.5;
			mp.gui.chat.push("Sport Modus");	
	  }
	mp.gui.chat.push("EPM: " + vCurMulti);
	
	//etm
	player.vehicle.setEnginePowerMultiplier(vCurMulti);
}*/

function updateVeh() {
	if (vId != -1) {
		mp.events.callRemote("updateVehicle", vId, vKm, vFuel);
	}
}

function setEng(v, s) {
	if (v.getIsEngineRunning() != s) {
		v.setEngineOn(s, true, true);
	}	
}

setInterval(function(){_intervalFunction();},1000);

function _intervalFunction() {
	let vehicle = player.vehicle;
	if (vehicle) {
		let speed =  vehicle.getSpeed(); 
		let trip = speed / 1000;
		if (vFuel > 0) {
			vFuel = vFuel - (vFuelConsumption * trip);
			if (vFuel < 0) {
				vFuel = 0;
			}
			if (vFuel == 0) {
				updateVeh();
			} 
		}

		vKm = vKm + trip;
		speedo.execute('updateG(' + (vFuel / vFuelTank * 100 ) + ',' + vKm.toFixed(2) + ');');
		updateI++;
		if (updateI == updateInterval) {
			updateI = 0;
			updateVeh();
		}
		eng = vehicle.getIsEngineRunning();
	}
};

mp.keys.bind(0x57, true, function() {
	if (state == 1 && localPlayer.vehicle) {
		wDown = true;
		localPlayer.vehicle.setMaxSpeed(9999);
	}
});

mp.keys.bind(0x57, false, function() {
	if (state == 1 && localPlayer.vehicle) {
		let speed = localPlayer.vehicle.getSpeed();
		localPlayer.vehicle.setMaxSpeed(speed);
		cSpeed = speed;
		wDown = false;
		speedo.execute('updateC(' + (speed * 3.6).toFixed(0) + ');');		
	}   
});
