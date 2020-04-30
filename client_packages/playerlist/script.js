// by CommanderDonkey & ByTropical

function createUser(name, health, armor){
	var first = document.createElement("li");
	first.className='player';
	var second = document.createElement("li");
	second.className='playerstats';
	
	var textnode = document.createTextNode(name);
	second.appendChild(textnode);
	
	document.getElementById("scoreboard").appendChild(first);
	document.getElementById("scoreboard").appendChild(second);
};
