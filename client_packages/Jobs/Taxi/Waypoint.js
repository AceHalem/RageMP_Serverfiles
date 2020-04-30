let taxiPoint = undefined; let taxiCP = undefined; let taxiway = undefined;

mp.events.add('ShowTaxiBlip', (position) => {
	if(taxiPoint === undefined) {
		// Create a blip on the map
		taxiPoint = mp.blips.new(1, position, {color: 1});
	} else {
		taxiPoint.setCoords(position);
	}
});

mp.events.add('ShowTaxiCP', (position) => {
	if(taxiCP === undefined) {
		// Create a blip on the map
		taxiCP = mp.checkpoints.new(0, position, 1.7, new mp.Vector3(), 4, 234, 35, 35, 255, false);
	} else {
		taxiCP.setCoords(position);
	}
});

mp.events.add("waypoint", (position) => {
	if(taxiway === undefined) {
		mp.game.ui.setNewWaypoint(position.X, position.Y);
	}
	else {
		mp.game.ui.setNewWaypoint(position.X, position.Y);
	}
});

mp.events.add('deleteTaxiWP', () => {
	// Destroy the blip on the map
	taxiPoint.destroy();
	taxiPoint = undefined;
	
	taxiCP.destroy();
	taxiCP = undefined;
});