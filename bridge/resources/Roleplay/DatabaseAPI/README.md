# DatabaseAPI
## Wie binde ich die API in meinen Code ein?
Trage in der Solution einfach das Projekt DatabaseAPI als Abhängigkeit für dein Projekt ein.
## Wie erhalte ich eine Verbindung?
Die DatabaseAPI verwaltet mehrere Verbindungen gleichzeitig in einem Pool, sodass keine Zeit mit dem Öffnen neuer Verbindungen verschwendet wird. Folgendes Code-Beispiel zeigt das Erhalten eines MySqlConnection-Objekts:
```csharp
MySql.Data.MySqlClient.MySqlConnection connection = DatabaseAPI.API.GetInstance().GetConnection();
```
## Wie schließe ich eine Verbindung?
Die Verbindungen werden nach der Nutzung nicht geschlossen, sondern lediglich an den Pool zurückgegeben. Das funktioniert mit folgendem Code:
```csharp
DatabaseAPI.API.GetInstance().FreeConnection(connection);
```
## Wie nutze ich eine Verbindung?
Für Details zur Nutzung solltest du dir [diesen Artikel](https://o7planning.org/de/10517/arbeiten-mit-mysql-datenbank-unter-verwendung-von-csharp) einmal ansehen. Hier allerdings noch ein kurzes Beipiel zur Erstellung einer Tabelle:
```csharp
MySql.Data.MySqlClient.MySqlConnection connection = DatabaseAPI.API.GetInstance().GetConnection();
MySql.Data.MySqlClient.MySqlCommand cmd = connection.CreateCommand();

cmd.CommandText = @"
    CREATE TABLE Persons (
    PersonID int,
    LastName varchar(255),
    FirstName varchar(255),
    Address varchar(255),
    City varchar(255)
    ); ";

cmd.ExecuteNonQuery();

DatabaseAPI.API.GetInstance().FreeConnection(connection);
```
## Ist die API thread-safe?
Die Funktionen GetConnection und FreeConnection der API können aus jedem Thread abgerufen werden und sind somit als thread-safe zu betrachten. Ein Objekt vom Typ MySql.Data.MySqlClient.MySqlConnection darf jedoch nicht von mehreren Threads gleichzeitig verwendet werden. Es sollte also pro Thread eine eigene Verbindung angefordert werden.
