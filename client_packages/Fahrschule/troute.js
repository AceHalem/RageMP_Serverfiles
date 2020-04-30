let schoolPoint = undefined; let schoolCP = undefined;


mp.events.add('ShowFahrschulBlip', (position) => {
	if(schoolPoint === undefined) {
		schoolPoint = mp.blips.new(1, position, {color: 1});
	} else {
		schoolPoint.setCoords(position);
	}
});

mp.events.add('ShowFahrschulCP', (position) => {
	if(schoolCP === undefined) {
		schoolCP = mp.checkpoints.new(0, position, 3.5, new mp.Vector3(), 4, 255, 48, 48, 255, false);
	} else {
		schoolCP.setCoords(position);
	}
});

mp.events.add('deleteFahrschulCP', () => {
	schoolPoint.destroy();
	schoolPoint = undefined;
	
	schoolCP.destroy();
	schoolCP = undefined;
});

mp.events.add("playerEnterCheckpoint", (player, checkpoint) => {
    if (schoolCP)
	{
		mp.events.callRemote("OnPlayerEnterFahrschulCP");
    }
});