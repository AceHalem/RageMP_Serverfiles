let vehenter = null;

mp.events.add('OpenVehEnterInfo', () => {
    vehenter = mp.browsers.new("package://Vehicles/vehenter.html");
});

mp.events.add('ExitVehEnterInfo', () => {
    vehenter.destroy();
	vehenter = null;
});

