let abrowser = null;

mp.events.add("dienstmarke", (c) => {
	abrowser = mp.browsers.new('package://dienstmarken/lspd/index.html');
	abrowser.execute(`document.getElementById('namelabel').innerHTML="` + c.name + `";`);
});

mp.events.add("dienstmarked", (c) => {
	abrowser.destroy();
});