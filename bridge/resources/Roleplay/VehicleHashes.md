# Vehicle Hashes
Ein Vehicle Hash ist ein unsigned int, welcher jedem Fahrzeugtypen einen eindeutigen Identifier zuordnet.
## Vehicle Hash von einem Vehicle-Objekt
Beispiel:
```csharp
[ServerEvent(Event.PlayerEnterVehicle)]
public void OnPlayerEnterVehicle(Client c, Vehicle veh, sbyte seatID)
{
    unint hash = veh.Model;
}
```
Die Variable hash wäre hier der Vehicle-Hash.
## Display Name zu Vehicle Hash
Beispiel:
```csharp
[ServerEvent(Event.PlayerEnterVehicle)]
public void OnPlayerEnterVehicle(Client c, Vehicle veh, sbyte seatID)
{
    string vehName = veh.DisplayName;
    unint hash = NAPI.Util.GetHashKey(vehName);
}
```

**ACHTUNG**:
Die Variable hash hätte hier einen anderen Wert als im Obigen Beispiel, da ``veh.DisplayName`` keine Namen der Untertypen zurückliefert.
Wenn bspw. das Fahrzeug vom Typ ``police3`` ist, so würde ``veh.DisplayName`` nur ``police`` zurückgeben.
Daher am besten nutzen, wenn ``vehName`` bekannt ist bzw. direkt als Parameter übergeben wird.
## Vehicle Hash von einem Vehicle-Object (OOP)
```csharp
unint hash = veh.GetHashCode();
```
Gleiches Problem wie zuvor, es wird nur der Hash vom übergeordeten Typ zurückgegeben.
Daher eher zur nicht OOP-Variante greifen.
## Vehicle Hash zu Hex-String
```csharp
uint hash = veh.Model;
string hexHash = hash.ToString("X");
```