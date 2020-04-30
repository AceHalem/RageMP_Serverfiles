using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Roleplay.Jobs.Taxi
{
    class Main : Script
    {
        public static void TaxiJobStart(Client c)
        {
            c.SetData("InJob", "Taxi");
            c.Position = new Vector3(902.0617, -172.6862, 73.68024);
            c.Rotation = new Vector3(0, 0, 238.1272);
            Vehicles.Vehicles.Createjv(c, "taxi");
            c.SetData("JobVehicle", c.Vehicle);
            c.Vehicle.SetData("JobVeh", "Taxi");
            c.Vehicle.SetData("JobVehicleFahrer", c);
            c.Vehicle.PrimaryColor = 88;
            //c.SendChatMessage("[~y~WARNUNG~w~]: Sobald du das Fahrzeug verlässt wird in wenigen Sekunden dein Job beendet!.");
        }
    }
}
