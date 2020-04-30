/*function sleep(ms) {
  return new Promise(resolve => setTimeout(resolve, ms));
}

async function tp(coord) {
  coord.z = mp.game.gameplay.getGroundZFor3dCoord(coord.x, coord.y, 800, 0, false)
  if (!mp.players.local.isInAnyVehicle(false)) {
    mp.players.local.position = coord
    if(coord.z == 0){
      mp.gui.chat.push("Could not find elevation at waypoint position!") 
      let trys = 0
      let max = 50
      while (trys < max) {
        await sleep(100);
        coord.z = mp.game.gameplay.getGroundZFor3dCoord(coord.x, coord.y, 8000, 0, false)
        if (coord.z != 0) {
          mp.gui.chat.push("z found " + coord.z)
          mp.players.local.position = coord
          break;
        }
        trys++;
      }
      if (coord.z == 0) {
        mp.gui.chat.push("Could not find elevation at waypoint position! " + trys)
      }

    }
  } else {
    mp.gui.chat.push("Your are inside a vehicle") 
  }
}

mp.events.add("playerCommand", (command) => {
  const args = command.split(/[ ]+/)
  const commandName = args[0]
  args.shift()
  if(commandName === "tp"){
    if (mp.game.invoke('0x1DD1F58F493F1DA5')) {
      let blipIterator = mp.game.invoke('0x186E5D252FA50E7D')
      let FirstInfoId = mp.game.invoke('0x1BEDE233E6CD2A1F', blipIterator)
      let NextInfoId = mp.game.invoke('0x14F96AA50D6FBEA7', blipIterator)
      for (let i = FirstInfoId; mp.game.invoke('0xA6DB27D19ECBB7DA', i) != 0; i = NextInfoId) {
        if (mp.game.invoke('0xBE9B0959FFD0779B', i) == 4 ) {
          let coord = mp.game.ui.getBlipInfoIdCoord(i)
          tp(coord)
        }
      }
    }
  }
})*/