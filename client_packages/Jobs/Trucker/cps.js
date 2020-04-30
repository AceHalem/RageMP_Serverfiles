let truckerPoint = undefined; let truckerCP = undefined;
let BlipPoint2 = undefined; let JobCP2 = undefined;
let BlipPoint3 = undefined; let JobCP3 = undefined;
let BlipPoint4 = undefined; let JobCP4 = undefined; landOBJ4 = undefined;
let BlipPoint5 = undefined; let JobCP5 = undefined;
let BlipPoint6 = undefined; let JobCP6 = undefined;
let disablekeys = undefined; const controls = mp.game.controls;

mp.events.add('ShowTruckerBlip1', (position) => {
	if(truckerPoint === undefined) {
		// Create a blip on the map
		truckerPoint = mp.blips.new(1, position, {color: 1});
	} else {
		truckerPoint.setCoords(position);
	}
});

mp.events.add('ShowTruckerCP1', (position) => {
	if(truckerCP === undefined) {
		// Create a blip on the map
		truckerCP = mp.checkpoints.new(0, position, 1.7, new mp.Vector3(), 4, 234, 35, 35, 255, false);
	} else {
		truckerCP.setCoords(position);
	}
});


mp.events.add('deleteTruckerLevelCP1', () => {
	// Destroy the blip on the map
	truckerPoint.destroy();
	truckerPoint = undefined;
	
	truckerCP.destroy();
	truckerCP = undefined;
});

mp.events.add('ShowBlip2', (position) => {
	if(BlipPoint2 === undefined) {
		// Create a blip on the map
		BlipPoint2 = mp.blips.new(1, position, {color: 1});
	} else {
		BlipPoint2.setCoords(position);
	}
});

mp.events.add('ShowCP2', (position) => {
	if(JobCP2 === undefined) {
		// Create a blip on the map
		JobCP2 = mp.checkpoints.new(0, position, 2.5, new mp.Vector3(), 4, 255, 48, 48, 255, false);
	} else {
		JobCP2.setCoords(position);
	}
});

mp.events.add('deleteJobCP2', () => {
	// Destroy the blip on the map
	BlipPoint2.destroy();
	BlipPoint2 = undefined;
	
	JobCP2.destroy();
	JobCP2 = undefined;
});

mp.events.add('ShowBlip3', (position) => {
	if(BlipPoint3 === undefined) {
		// Create a blip on the map
		BlipPoint3 = mp.blips.new(1, position, {color: 1});
	} else {
		BlipPoint3.setCoords(position);
	}
});

mp.events.add('ShowCP3', (position) => {
	if(JobCP3 === undefined) {
		// Create a blip on the map
		JobCP3 = mp.checkpoints.new(0, position, 2.5, new mp.Vector3(), 4, 255, 48, 48, 255, false);
	} else {
		JobCP3.setCoords(position);
	}
});

mp.events.add('deleteJobCP3', () => {
	// Destroy the blip on the map
	BlipPoint3.destroy();
	BlipPoint3 = undefined;
	
	JobCP3.destroy();
	JobCP3 = undefined;
});

mp.events.add('ShowBlip4', (position) => {
	if(BlipPoint4 === undefined) {
		// Create a blip on the map
		BlipPoint4 = mp.blips.new(1, position, {color: 1});
	} else {
		BlipPoint4.setCoords(position);
	}
});

mp.events.add('ShowCP4', (position) => {
	if(JobCP4 === undefined) {
		// Create a blip on the map
		JobCP4 = mp.checkpoints.new(0, position, 2.5, new mp.Vector3(), 4, 255, 48, 48, 255, false);
	} else {
		JobCP4.setCoords(position);
	}
});

mp.events.add('deleteJobCP4', () => {
	// Destroy the blip on the map
	BlipPoint4.destroy();
	BlipPoint4 = undefined;
	
	JobCP4.destroy();
	JobCP4 = undefined;
	
	landOBJ4.destroy();
	landOBJ4 = undefined;
});

mp.events.add("ShowOBJ4", (position) => {
	if(landOBJ4 === undefined) {
		landOBJ4 = mp.objects.new(1221915621, position, new mp.Vector3());
	} else {
		landOBJ4.setCoords(position);
	}
});

mp.events.add('ShowBlip5', (position) => {
	if(BlipPoint5 === undefined) {
		// Create a blip on the map
		BlipPoint5 = mp.blips.new(1, position, {color: 1});
	} else {
		BlipPoint5.setCoords(position);
	}
});

mp.events.add('ShowCP5', (position) => {
	if(JobCP5 === undefined) {
		// Create a blip on the map
		JobCP5 = mp.checkpoints.new(0, position, 2.5, new mp.Vector3(), 4, 255, 48, 48, 255, false);
	} else {
		JobCP5.setCoords(position);
	}
});

mp.events.add('deleteJobCP5', () => {
	// Destroy the blip on the map
	BlipPoint5.destroy();
	BlipPoint5 = undefined;
	
	JobCP5.destroy();
	JobCP5 = undefined;
});

mp.events.add('ShowBlip6', (position) => {
	if(BlipPoint6 === undefined) {
		// Create a blip on the map
		BlipPoint6 = mp.blips.new(1, position, {color: 1});
	} else {
		BlipPoint6.setCoords(position);
	}
});

mp.events.add('ShowCP6', (position) => {
	if(JobCP6 === undefined) {
		// Create a blip on the map
		JobCP6 = mp.checkpoints.new(0, position, 2.5, new mp.Vector3(), 4, 255, 48, 48, 255, false);
	} else {
		JobCP6.setCoords(position);
	}
});

mp.events.add('deleteJobCP6', () => {
	// Destroy the blip on the map
	BlipPoint6.destroy();
	BlipPoint6 = undefined;
	
	JobCP6.destroy();
	JobCP6 = undefined;
});

mp.events.add("DisableVehKeys", (position) => {
	player.vehicle.freezePosition(true);
	disablekeys = "true";
});
mp.events.add("EnableVehKeys", (position) => {
	player.vehicle.freezePosition(false);
	disablekeys = "false";
});

mp.events.add("StopJobEnableVehKeys", (position) => {
	disablekeys = "false";
});

mp.events.add('render', () =>
{
	if (disablekeys == "true") {
		controls.disableControlAction(27, 71, true);
		controls.disableControlAction(27, 72, true);	
		controls.disableControlAction(27, 63, true);	
		controls.disableControlAction(27, 64, true);		
		controls.disableControlAction(27, 75, true);			
	}
});


mp.events.add("playerEnterCheckpoint", (player, checkpoint) => {
    if (truckerCP)
	{
		mp.events.callRemote("OnPlayerEnterTruckerLevel");
    } else if (JobCP2) {
		mp.events.callRemote("OnPlayerEnterJobCP");
	} else if (JobCP3) {
		mp.events.callRemote("OnPlayerEnterJobCP2");
	} else if (JobCP4) {
		mp.events.callRemote("OnPlayerEnterJobCP3");
		if (disablekeys == "true") {
		}
	} else if (JobCP5) {
		mp.events.callRemote("OnPlayerEnterJobCP4");
	}
});