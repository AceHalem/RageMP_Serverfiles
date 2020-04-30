using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Roleplay.Player
{
    class Commands : Script
    {
        [Command("unrent")]
        public void UnRent(Client c)
        {
            if (!c.HasData("temp"))
            {
                c.SendNotification("Du besitzt kein Fahrzeug das du zurück geben kannst!");
                return;
            }

            Vehicle veh = c.GetData("temp");
            veh.Delete();
            c.ResetData("temp");
        }

        [RemoteEvent("callTaxiToServer")]
        public void CallTaxi(Client c)
        {
            if (c.HasData("TaxiAccept"))
            {
                c.SendNotification("[~y~Taxizentrale~w~]: Du hast bereits einen Fahrer gerufen!");
                return;
            }

            c.SendNotification("[~y~Taxizentrale~w~]: Ein Taxi wurde alarmiert! bitte warte an deiner Position.");

            foreach (Client player in NAPI.Pools.GetAllPlayers())
            {
                if (player.HasData("InJob"))
                {
                    if (player.GetData("InJob") == "Taxi")
                    {
                        player.SendNotification($"[~y~Taxizentrale~w~]: {c.Name} benötigt einen Taxifahrer! nutze '~y~/accept~w~'");
                        player.SetData("TAuftrag", c);

                        NAPI.Task.Run(() =>
                        {
                            if (player.HasData("TaxiAccept") && player.GetData("TaxiAccept") == c)
                            {
                                c.SendNotification($"[~y~Taxizentrale~w~]: {player.Name} ist auf dem Weg!");
                            }
                            else
                            {
                                player.SendNotification("[~y~Taxizentrale~w~]: Auftrag von " + c.Name + " wurde von niemanden angenommen.");
                                c.SendNotification("[~y~Taxizentrale~w~]: Leider wurde kein Fahrer für Sie gefunden.");
                                player.ResetData("TAuftrag");
                            }
                        }, delayTime: 7000);
                    }
                }
            }
        }

        [Command("accept")]
        public void Accept(Client c)
        {
            if (c.HasData("TaxiAccept"))
            {
                c.SendNotification("[~y~Taxizentrale~w~]: Du hast bereits einen Auftrag!");
                return;
            }

            if (c.HasData("TAuftrag"))
            {
                Client player = c.GetData("TAuftrag");
                player.SetData("TaxiAccept", c);
                c.SetData("TaxiAccept", player);
                c.TriggerEvent("ShowTaxiCP", player.Position);
                c.TriggerEvent("ShowTaxiBlip", player.Position);
                foreach (Client fahrer in NAPI.Pools.GetAllPlayers())
                {
                    if (fahrer.HasData("InJob") && fahrer.GetData("InJob") == "Taxi" && fahrer.HasData("TAuftrag"))
                    {
                        fahrer.ResetData("TAuftrag");
                        fahrer.SendNotification("[~y~Taxizentrale~w~]: " + c.Name + " hat den Auftrag angenommen!");
                        return;
                    }
                }
                c.SendNotification("[~y~Taxizentrale~w~]: bitte mach dich nun auf den Weg zum Fahrgast!");
                return;
            } 
            
            if (c.HasData("schrotti"))
            {
                c.SendNotification("~g~Schrotti's Deal akzeptiert!");
                c.SetData("schrottiaccept", 1);
                c.ResetData("schrotti");
                return;
            }

            if (c.HasData("finvite"))
            {
                Client leader = c.GetData("finvite");

                MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

                MySqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "UPDATE characters SET fraktion = @frak, fraktionrank = @frank WHERE id = @pid";
                cmd.Parameters.AddWithValue("@pid", c.GetData("character_id"));
                cmd.Parameters.AddWithValue("@frak", leader.GetData("fraktion"));
                cmd.Parameters.AddWithValue("@frank", 1);
                cmd.ExecuteNonQuery();

                DatabaseAPI.API.GetInstance().FreeConnection(conn);

                c.SetData("fraktion", leader.GetData("fraktion"));
                c.SetData("fraktionrank", 1);

                leader.SendNotification($"~g~Spieler ~w~{c.Name}~g~ hat die Einladung akzeptiert!");
                c.SendNotification($"~g~Du bist nun Mitglied bei der " + Fraktionssystem.API.Frakranknames[(leader.GetData("fraktion") > Fraktionssystem.API.Frakranknames.Length) ? 0 : leader.GetData("fraktion")] + "!");

                c.ResetData("finvite");
                return;
            }

            c.SendNotification("Du hast keine Anfrage erhalten!");
        }

        [Command("schrotten")]
        public void DestroyVehicle(Client c)
        {
            if (c.Position.DistanceTo2D(new Vector3(485.187, -1308.765, 29.25349)) < 4)
            {
                if (!c.IsInVehicle)
                {
                    c.SendNotification("~r~Du musst dafür in einem Fahrzeug sitzen!");
                    return;
                }

                if (c.Vehicle.HasData("fid") || c.Vehicle.HasData("jid"))
                {
                    c.SendNotification("Dieses Fahrzeug kannst du nicht verschrotten lassen!");
                    return;
                }

                if (c.Vehicle.GetData("owner") != c.GetData("character_id"))
                {
                    c.SendNotification("~r~Dieses Fahrzeug gehört nicht dir!");
                    return;
                }

                c.SendNotification("[~r~Schrotti~w~]: Das kostet dich 100~g~$~w~! akzeptiere mit '~y~/accept~w~'");
                c.SendNotification("[~r~Schrotti~w~]: Du hast 7 Sekunden!");
                c.SetData("schrotti", 1);
                NAPI.Task.Run(() =>
                {
                    if (!c.HasData("schrottiaccept"))
                    {
                        c.SendNotification("~r~Schrotti's Deal nicht akzeptiert!");
                        c.SendNotification("[~r~Schrotti~w~]: Besuch uns wieder wenn du dein Fahrzeug doch verschrotten lassen möchtest.");
                        c.ResetData("schrotti");
                        return;
                    }

                    Vehicles.Vehicles.RemoveVehicleToPlayer(c);
                    c.Vehicle.Delete();
                    MoneyAPI.API.SubCash(c, 100);
                    c.ResetData("schrottiaccept");
                }, delayTime: 7000);
            } else
            {
                c.SendNotification("~r~Du befindest dich nicht beim Schrotti!");
            }
        }

        [Command("dc")]
        public void HandleDc(Client c)
        {
            c.Kick();
        }

        [Command("ainvite")]
        public void afInvite(Client c)
        {
            if (!c.HasData("finvite"))
            {
                c.SendNotification("Du besitzt keine Einladung");
                return;
            }

            c.SetData("acceptinvite", true);
            c.SendNotification("Du hast die Einladung angenommen!");
        }

        public static Vector3[] fuelstation = new Vector3[]
        {
            new Vector3(263.6518, 2606.583, 44.98414),
            new Vector3(-2555.247, 2334.371, 33.07914),
            new Vector3(-2098.742, -318.7727, 12.52344),
            new Vector3(-1436.104, -276.6026, 45.70399),
            new Vector3(621.0473, 268.3362, 102.6658),
            new Vector3(1181.185, -331.9545, 68.78868),
            new Vector3(1208.427, -1402.669, 34.80032),
            new Vector3(174.5453, -1563.314, 29.02609),
            new Vector3(-72.15495, -1760.97, 29.25477),
            new Vector3(2003.932, 3776.487, 31.93668),
            new Vector3(1703.198, 6418.478, 31.97831),
            new Vector3(181.8707, 6604.383, 31.19762),
            new Vector3(-94.39114, 6419.002, 30.82691),
            new Vector3(261.4218, -1260.924, 28.89927),
            new Vector3(-323.7411, -1472.909, 30.30348)
        };

        [Command("refuelveh")]
        public static void RefuelVehicleWithKanister(Client c) {
            if (!c.IsInVehicle)
            {
                c.SendNotification("Du bist in keinem Fahrzeug!");
                return;
            }

            if (c.VehicleSeat != -1) {
                c.SendNotification("Du bist nicht der Fahrer!");
                return;
            }

            if (c.Vehicle.EngineStatus)
            {
                c.SendNotification("Du musst dafür den Motor ausschalten!");
                return;
            }

            if(InventoryAPI.API.HasItem(c, 5))
            {
                InventoryAPI.API.RemoveItem(c, 5, 1);

                c.Vehicle.SetData("fuel", c.Vehicle.GetData("fuelTank") * 100 / 100);
                Vehicles.Vehicles.syncVehicle(c, c.Vehicle);

                c.SendNotification("Fahrzeug wurde aufgetankt!");
            } else {
                c.SendNotification("Du besitzt keinen Benzinkanister!");
            }
        }

        [Command("fuel")]
        public void HandleRefuel(Client c)
        {
            if (!c.IsInVehicle)
            {
                c.SendNotification("Du bist in keinem Fahrzeug!");
                return;
            }

            if (c.Vehicle.EngineStatus)
            {
                c.SendNotification("Du musst dafür den Motor ausschalten!");
                return;
            }

            for (int i = 0, max = fuelstation.Length; i < max; i++) {
                if (c.Position.DistanceTo2D(fuelstation[i]) < 12)
                {
                    int menge = 100;
                    Vehicle v = c.Vehicle;
                    c.SendNotification("[~y~TANKSTELLE~w~]: Dein Fahrzeug wird getankt, einen Moment bitte.");

                    NAPI.Task.Run(() =>
                    {
                        if (!c.IsInVehicle)
                        {
                            c.SendNotification("[~y~TANKSTELLE~w~]: Du hast das Tanken abgebrochen.");
                            return;
                        }

                        v.SetData("fuel", v.GetData("fuelTank") * menge / 100);
                        Vehicles.Vehicles.syncVehicle(c, v);
                        c.SendNotification("[~y~TANKSTELLE~w~]: Du hast dein Fahrzeug getankt! dir wurden dafür 100~g~$~w~ abgezogen.");
                        MoneyAPI.API.SubCash(c, 100);
                    }, delayTime: 5000);
                }
            }
        }

        public static Vector3[] vehstore = new Vector3[]
        {
            new Vector3(53.97897, -880.0446, 30.10506),
            new Vector3(-339.7374, 288.5088, 85.24472),
            new Vector3(-1137.178, -860.3455, 13.23336),
            new Vector3(123.7082, 6620.354, 31.58156)
        };

        [Command("store")]
        public void HandleStore(Client c)
        {
            if (!c.IsInVehicle)
            {
                c.SendNotification("Du musst dafür in einem Fahrzeug sitzen!");
                return;
            }

            if (c.Vehicle.HasData("fid") || c.Vehicle.HasData("jid") || c.Vehicle.GetData("owner") != c.GetData("character_id"))
            {
                c.SendNotification("Dieses Fahrzeug kannst du ~r~NICHT~w~ einparken!");
                return;
            }

            if (c.Position.DistanceTo2D(vehstore[0]) < 5 || c.Position.DistanceTo2D(vehstore[1]) < 5
                || c.Position.DistanceTo2D(vehstore[2]) < 5 || c.Position.DistanceTo2D(vehstore[3]) < 5)
            {
                MySqlCommand cmd = new MySqlCommand("UPDATE vehicles SET active = 0 WHERE id = @id");
                cmd.Parameters.AddWithValue("@id", c.Vehicle.GetData("id"));
                DatabaseAPI.API.executeNonQuery(cmd);

                c.Vehicle.Delete();

                c.SendNotification("Dein Fahrzeug wurde eingeparkt und kann mit '~y~F10~w~' wieder ausgeparkt werden!");
            } else
            {
                c.SendNotification("Du kannst dein Fahrzeug hier nicht einparken!");
            }
        }
    }
}
