using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Roleplay.Player
{
    class PlayerEvent : Script
    {
        public struct UserVehicle
        {
            public int id;
            public uint hash;
        }

        [RemoteEvent("getUserpanelVehicles")]
        public static void getUserpanelVehicles(Client c)
        {

            List<UserVehicle> vehicles = new List<UserVehicle> { };

            foreach (Vehicle veh in NAPI.Pools.GetAllVehicles())
            {
                if (veh.HasData("id"))
                {
                    if (veh.GetData("owner") == c.GetData("character_id"))
                    {
                        vehicles.Add(new UserVehicle
                        {
                            id = veh.GetData("id"),
                            hash = veh.GetData("hash")
                        });
                    }
                }
            }

            c.TriggerEvent("GetUserpanelVehicles", vehicles);
        }

        [RemoteEvent("getUserpanel")]
        public void GetUserPanel(Client c)
        {
            int autoschein = 0;

            if (c.HasData("führerschein"))
            {
                autoschein = 1;
            }

            c.TriggerEvent("UserPanel", c, Fraktionssystem.API.Frakranknames[(c.GetData("fraktion") > Fraktionssystem.API.Frakranknames.Length) ? 0 : c.GetData("fraktion")], c.GetData("money_cash") + c.GetData("money_bank"), c.GetData("PlayerPaydayTimer"), autoschein, c.GetData("registriertseitdem"), c.GetData("autoscheinpunkte"));
        }

        [RemoteEvent("OpenPlayerInteraction")]
        public void OpenPlayerInteraction(Client c)
        {
            int duty = 0;

            if (c.HasData("onduty") && c.GetData("onduty") == 1)
            {
                duty = 1;
            }

             c.TriggerEvent("PlayerInteraction", duty);
        }

        [RemoteEvent("LockOrUnlockVeh")]
        public void LockOrUnlockVeh(Client c)
        {
            Vehicle[] veh = NAPI.Pools.GetAllVehicles().FindAll(x => x.Position.DistanceTo2D(c.Position) <= 3).ToArray();

            for (int i = 0; i < veh.Length; i++)
            {

                if (veh[i].HasData("jid") || veh[i].HasData("temp"))
                {
                    c.SendNotification("Du kannst dieses Fahrzeug NICHT abschließen!");
                    return;
                }

                if (veh[i].HasData("owner") && veh[i].GetData("owner") != c.GetData("character_id") || veh[i].HasData("fraktion") && veh[i].GetData("fraktion") != Fraktionssystem.API.Frakranknames[(c.GetData("fraktion") > Fraktionssystem.API.Frakranknames.Length) ? 0 : c.GetData("fraktion")])
                {
                    c.SendNotification("Du besitzt keinen Schlüssel!");
                    return;
                }

                Vehicle vehicleToLock = null;

                vehicleToLock = veh[i];

                if (vehicleToLock != null)
                {

                    vehicleToLock.Locked = !vehicleToLock.Locked;

                    if (!vehicleToLock.Locked)
                    {
                        c.SendNotification($"~g~Das Fahrzeug wurde aufgeschlossen!");
                    }
                    else
                    {
                        c.SendNotification($"~r~Das Fahrzeug wurde abgeschlossen!");
                    }
                }
                else
                {
                    c.SendNotification("Du befindest dich nicht in der Nähe von einem Fahrzeug!");
                }
            }
        }

        [RemoteEvent("ShowPlayerAusweis")]
        public void ShowPlayerAusweis(Client c)
        {
            foreach (Client target in NAPI.Pools.GetAllPlayers())
            {
                if (c.Equals(target)) continue;
                if (Init.Init.IsInRangeOfPoint(c.Position, new Vector3(target.Position.X, target.Position.Y, target.Position.Z), 1f))
                {
                    if (target.Name == c.Name)
                        return;

                    c.SendNotification($"~g~Du zeigst ~w~{target.Name}~g~ deinen Ausweis!");
                    target.SendNotification($"[~b~Ausweis von~w~]: {c.Name}");
                    target.SendNotification($"[~b~Fraktion~w~]: {Fraktionssystem.API.Frakranknames[(c.GetData("fraktion") > Fraktionssystem.API.Frakranknames.Length) ? 0 : c.GetData("fraktion")]}");
                }
            }
        }
    }
}
