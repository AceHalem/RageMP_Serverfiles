using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace Roleplay.Init
{
    class Init : Script
    {
        public static int LSPDGateLock { get; set; }
        public static int LSPDDoorLock { get; set; }

        private static readonly Random rnd = new Random();

        [Command("restart")]
        public void HandleShutDown (Client cc, int sekunden)
        {
            if (sekunden < 5 || sekunden > 15)
            {
                cc.SendNotification("Mindestens 5 und Maximal 15 Sekunden!");
                return;
            }

            foreach (Client c in NAPI.Pools.GetAllPlayers())
            {
                SavePlayer(c);
            }

            Vehicles.InitNew.Save();
            Vehicles.InitNew.SaveFV();

            NAPI.Chat.SendChatMessageToAll("[~r~SERVER~w~]: Server Neustart in " + sekunden + " Sekunden!");

            Task.Run(() =>
            {
                Task.Delay(1000 * sekunden * 1).Wait();

                Environment.Exit(0);
            });
        }


        public void SavePlayer (Client c)
        {
            if (c.HasData("character_id"))
            {
                int cId = c.GetData("character_id");
                if (cId != -1)
                {
                    MySqlCommand cmd = new MySqlCommand("UPDATE characters SET p_x = @p_x, p_y = @p_y, p_z = @p_z, dim=@did, payday=@pd WHERE id = @id");
                    cmd.Parameters.AddWithValue("@id", cId);
                    cmd.Parameters.AddWithValue("@p_x", c.Position.X);
                    cmd.Parameters.AddWithValue("@p_y", c.Position.Y);
                    cmd.Parameters.AddWithValue("@p_z", c.Position.Z);
                    cmd.Parameters.AddWithValue("@pd", c.GetData("PlayerPaydayTimer"));
                    
                    if (c.HasData("houseid")) {
                        cmd.Parameters.AddWithValue("@did", c.Dimension);
                    } else {
                        cmd.Parameters.AddWithValue("@did", 0);
                    }
                    DatabaseAPI.API.executeNonQuery(cmd);
                    Log.WriteM("Player Saving last pos " + c.Name + " " + cId);
                }
            }
        }


        [ServerEvent(Event.ResourceStart)]
        public void ResourceStart()
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("====== TestServer Console Start =========");
            Console.WriteLine("=========================================");

            NAPI.Server.SetAutoSpawnOnConnect(false);
            NAPI.Server.SetAutoRespawnAfterDeath(false);
            NAPI.Server.SetGlobalServerChat(false);

            NAPI.Server.SetCommandErrorMessage("[~r~SERVER~w~] Dieser Befehl existiert nicht!");

            Log.WriteM("Loading Housesystem ");
            Housesystem.API.LoadDatabaseHouses();

            Log.WriteM("Loading Vehicles ");
            Vehicles.InitNew.lastSave = DateTime.Now;

            Vehicles.InitNew.SpawnAll();

            Log.WriteM("Loading Frak. Vehicles");

            Vehicles.InitNew.SpawnAllfVeh();

            Task.Run(() =>
            {
                while (true)
                {
                    //TODO: Zeit anpassen. Ist nur zum testen
                    Task.Delay(100).Wait();
                    foreach (Vehicle v in NAPI.Pools.GetAllVehicles())
                    {
                        if (v.HasData("engine"))
                        {
                            bool e = v.GetData("engine");
                            if (v.EngineStatus != e)
                            {
                                v.EngineStatus = e;
                                //Log.WriteI("TEST ENGINE");
                            }
                        }
                    }
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    Task.Delay(1000 * 45 * 1).Wait();
                    Log.WriteM("Saving Vehicles and Players [START]");
                    Task updatePlayers = Task.Run(() =>
                    {
                        foreach (Client c in NAPI.Pools.GetAllPlayers())
                        {
                            SavePlayer(c);
                        }
                    });
                    Task updateVehicles = Task.Run(() =>
                    {
                        Vehicles.InitNew.Save();
                    });
                    Task updateFVehicles = Task.Run(() =>
                    {
                        Vehicles.InitNew.SaveFV();
                    });

                    updatePlayers.Wait();
                    updateVehicles.Wait();
                    updateFVehicles.Wait();
                    Log.WriteM("Saving Vehicles and Players [DONE]");
                }
            });



            Console.WriteLine("==========================================");
            Console.WriteLine("====== TestServer Console Finish =========");
            Console.WriteLine("==========================================");
        }


        [RemoteEvent("add_voice_listener")]

        public void add_voice_listener(Client c, Client t)
        {
            NAPI.Player.EnablePlayerVoiceTo(c, t);
        }

        [RemoteEvent("remove_voice_listener")]

        public void remove_voice_listener(Client c, Client t)
        {
            NAPI.Player.DisablePlayerVoiceTo(c, t);
        }

        [ServerEvent(Event.PlayerConnected)]
        public void PlayerConnected(Client c)
        {
            c.Dimension = uint.MaxValue - (uint)rnd.Next(999999);
            c.Position = new Vector3(344.3341, -998.8612, -99.19622);
            //c.SendNotification("~g~Dimension: ~w~" + c.Dimension);
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerConnected(Client c, DisconnectionType type, string reason)
        {
            if (c.HasData("InJob"))
            {
                Jobs.Main.DeleteJobVehicle(c, c.Vehicle);
            }

            if (c.HasData("cuffed"))
            {
                Player.PlayerUpdate.UpdatePlayerWanteds(c, true, 2);
            }

            switch (type)
            {
                case DisconnectionType.Left:
                    SavePlayer(c);
                    Log.WriteI("Player Disconnected: " + c.Name);
                    break;

                case DisconnectionType.Timeout:
                    SavePlayer(c);
                    Log.WriteI("Player Timeout: " + c.Name);
                    break;

                case DisconnectionType.Kicked:
                    SavePlayer(c);
                    Log.WriteI("Player Kicked: " + c.Name);
                    break;
            }
        }


        [ServerEvent(Event.PlayerDeath)]
        public void OnPlayerDeath(Client c, Client killer, uint reason)
        {
            c.SetData("death", true);

            if (c.HasData("InJob"))
            {
                Jobs.Main.DeleteJobVehicle(c, c.Vehicle);
            }

            if (c.HasData("godmode") && c.GetData("godmode"))
            {
                c.SendChatMessage("Ohne Godmode wärste jz tot du kek :D");
                NAPI.Player.SpawnPlayer(c, c.Position);
                c.Health = 100;
                c.Armor = 100;
            }
            else
            {

                if (killer == null)
                {
                    c.SendChatMessage($"Du bist gestorben!");
                } else if (killer == c)
                {
                    c.SendChatMessage($"Du hast selbstmord begangen!");
                } else
                {
                    c.SendChatMessage($"Du wurdest von {killer.Name} getötet!");
                }

                foreach (Client saru in NAPI.Pools.GetAllPlayers())
                {
                    if (saru.GetData("fraktion") == 2 && saru.HasData("onduty"))
                    {
                        Blip PlayerDeathBlip = NAPI.Blip.CreateBlip(84, c.Position, 1, 4, c.Name);

                        c.SetData("deathblip", PlayerDeathBlip);

                        NAPI.Task.Run(() =>
                        {
                            if (c.HasData("deathblip"))
                            {
                                PlayerDeathBlip.Delete();
                                c.ResetData("deathblip");
                            }
                        }, delayTime: 12000);
                    }
                }

                c.TriggerEvent("DeathTrue");

                NAPI.Task.Run(() =>
                {
                    if (c.HasData("death"))
                    {
                        if (c.GetData("jailtime") != 0)
                        {
                            NAPI.Player.SpawnPlayer(c, new Vector3(1729.212, 2563.543, 45.56488));
                        } else
                        {
                            NAPI.Player.SpawnPlayer(c, new Vector3(355.9892, -597.8624, 28.77746));
                        }

                        c.SendNotification("Du wurdest respawnt!");
                        c.TriggerEvent("DeathFalse");
                        c.ResetData("death");
                    }
                }, delayTime: 12000);
            }
        }

        public static bool IsInRangeOfPoint(Vector3 playerPos, Vector3 target, float range)
        {
            var direct = new Vector3(target.X - playerPos.X, target.Y - playerPos.Y, target.Z - playerPos.Z);
            var len = direct.X * direct.X + direct.Y * direct.Y + direct.Z * direct.Z;
            return range * range > len;
        }
    }
}
