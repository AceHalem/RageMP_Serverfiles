# Remote Events
Remote Events werden genutzt, um den Server auf ein Ereignis eines Clients, oder einen Client auf ein Ereignis des Servers aufmerksam zu machen.
## Event auf dem Server auslösen
Beispiel-Event: OnClientPokeServer

Zuerst muss auf der Serverseite ein Event-Handler erstellt werden:
```csharp
[RemoteEvent("OnClientPokeServer")]
public void OnClientPokeServer(Client client)
{
    // Event verarbeiten
}
```
Danach kann auf der Clientseite das Event ausgelöst werden:
```js
mp.events.callRemote("OnClientPokeServer");
```
Hier das gleiche Beispiel noch einmal mit einem Argument, das mit dem Aufruf des Events übergeben wird:

Serverseite:
```csharp
[RemoteEvent("OnClientPokeServer")]
public void OnClientPokeServer(Client client, int randomInt)
{
    // Event verarbeiten
}
```
Clientseite:
```js
mp.events.callRemote("OnClientPokeServer", 666);
```
## Event auf dem Client auslösen
Beispiel-Event: OnServerPokeClient

Zuerst muss auf der Clientseite ein Event-Handler erstellt werden:
```js
mp.events.add("OnServerPokeClient", function() {
	// Event verarbeiten
});
```
Danach kann auf der Serverseite das Event ausgelöst werden:
```csharp
Client client = ...;
client.TriggerEvent("OnServerPokeClient");
```
Hier das gleiche Beispiel noch einmal mit einem Argument, das mit dem Aufruf des Events übergeben wird:

Clientseite:
```js
mp.events.add("OnServerPokeClient", function(randomInt) {
	// Event verarbeiten
});
```

Serverseite:
```csharp
Client client = ...;
client.TriggerEvent("OnServerPokeClient", 666);
```
