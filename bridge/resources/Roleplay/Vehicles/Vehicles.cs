using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Roleplay.Vehicles
{
    class Vehicles : Script
    {

        public static bool Createjv(Client c, string vehName)
        {

            Vehicle veh = Spawnjv(c, vehName);
            MoveIn(c, veh);

            return true;
        }

        public static bool Createfv(Client c, string frak, string vehName)
        {

            uint hash = NAPI.Util.GetHashKey(vehName);

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = new MySqlCommand("INSERT INTO fvehicles(cfg_vehicle_id, fuel, fraktion, p_x, p_y, p_z, r) (SELECT id, fuel_tank / 2, @frak, @p_x, @p_y, @p_z, @r FROM cfg_vehicles WHERE hash = @hash)", conn);
            cmd.Parameters.AddWithValue("@hash", hash);
            cmd.Parameters.AddWithValue("@p_x", c.Position.X);
            cmd.Parameters.AddWithValue("@p_y", c.Position.Y);
            cmd.Parameters.AddWithValue("@p_z", c.Position.Z);
            cmd.Parameters.AddWithValue("@r", c.Rotation.Z);
            cmd.Parameters.AddWithValue("@frak", frak);

            int rows = 0;
            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Veh insert hash not found: " + ex.Message);
                DatabaseAPI.API.GetInstance().FreeConnection(conn);
            }

            if (rows != 0)
            {
                cmd = new MySqlCommand("SELECT v.*, c.hash, c.multi, fuel_tank, fuel_consumption FROM fvehicles v JOIN cfg_vehicles c ON v.cfg_vehicle_id = c.id WHERE v.id = LAST_INSERT_ID()", conn);
                MySqlDataReader r = cmd.ExecuteReader();
                if (r.Read())
                {
                    Vehicle veh = Spawnfv(r);
                    MoveIn(c, veh);
                }
                r.Close();
            }
            else
            {
                Console.WriteLine("Veh not found: " + vehName);
            }


            DatabaseAPI.API.GetInstance().FreeConnection(conn);
            return true;
        }

        public static bool Create(Client c, string vehName)
        {
            int cId = c.GetData("character_id");

            uint hash = NAPI.Util.GetHashKey(vehName);

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd2 = new MySqlCommand("UPDATE characters SET vehicles = @veh WHERE id = @cid", conn);
            cmd2.Parameters.AddWithValue("@cid", cId);
            cmd2.Parameters.AddWithValue("@veh", c.GetData("vehicles") + 1);
            cmd2.ExecuteNonQuery();

            c.SetData("vehicles", +1);

            MySqlCommand cmd = new MySqlCommand("INSERT INTO vehicles(cfg_vehicle_id, fuel, character_id, p_x, p_y, p_z, r) (SELECT id, fuel_tank / 2, @c_id, @p_x, @p_y, @p_z, @r FROM cfg_vehicles WHERE hash = @hash)", conn);
            cmd.Parameters.AddWithValue("@hash", hash);
            cmd.Parameters.AddWithValue("@p_x", c.Position.X);
            cmd.Parameters.AddWithValue("@p_y", c.Position.Y);
            cmd.Parameters.AddWithValue("@p_z", c.Position.Z);
            cmd.Parameters.AddWithValue("@r", c.Rotation.Z);
            cmd.Parameters.AddWithValue("@c_id", cId);

            int rows = 0;
            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Veh insert hash not found: " + ex.Message);
                DatabaseAPI.API.GetInstance().FreeConnection(conn);
            }

            if (rows !=0 )
            {
                cmd = new MySqlCommand("SELECT v.*, c.hash, c.multi, fuel_tank, fuel_consumption FROM vehicles v JOIN cfg_vehicles c ON v.cfg_vehicle_id = c.id WHERE v.id = LAST_INSERT_ID()", conn);
                MySqlDataReader r = cmd.ExecuteReader();
                if (r.Read())
                {
                    Vehicle veh = Spawn(r);
                    MoveIn(c, veh);
                }
                r.Close();
            } else
            {
                Console.WriteLine("Veh not found: " + vehName);
            }


            DatabaseAPI.API.GetInstance().FreeConnection(conn);
            return true;
        }

        public static Vehicle Spawn(MySqlDataReader r)
        {

            byte alpha = 255;
            bool engine = r.GetBoolean("engine");
            int c = r.GetInt32("c");
            int s = r.GetInt32("s");
            Vector3 pos;
            bool active = r.GetBoolean("active");
            uint dim;

            if (active)
            {
                pos = new Vector3(r.GetFloat("p_x"), r.GetFloat("p_y"), r.GetFloat("p_z"));
                dim = r.GetUInt32("dim");
            }
            else
            {
                pos = new Vector3(r.GetFloat("p_x"), r.GetFloat("p_y"), r.GetFloat("p_z"));
                dim = 0;
            }

            Vehicle veh = NAPI.Vehicle.CreateVehicle(
                r.GetUInt32("hash"),
                pos,
                r.GetFloat("r"),
                c,
                s,
                GetNumberplate(r.GetInt32("character_id")),
                alpha,
                r.GetBoolean("locked"),
                engine,
                dim
            );

            if (c == 0 && s == 0)
            {
                veh.CustomPrimaryColor = new Color(r.GetByte("c_r"), r.GetByte("c_g"), r.GetByte("c_b"));
                veh.CustomSecondaryColor = new Color(r.GetByte("s_r"), r.GetByte("s_g"), r.GetByte("s_b"));
            }

            veh.SetData("LastParkedPosition", veh.Position);
            veh.SetData("LastParkedRotation", veh.Rotation);
            veh.SetData("Numberplate", GetNumberplate(r.GetInt32("character_id")));

            veh.SetData("active", active);
            veh.SetData("id", r.GetInt32("id"));
            veh.SetData("owner", r.GetInt32("character_id"));
            veh.SetData("hash", r.GetUInt32("hash"));

            veh.SetData("hp", r.GetFloat("hp"));

            veh.SetData("multi", r.GetInt32("multi"));

            veh.SetData("km", r.GetFloat("km"));
            veh.SetData("fuel", r.GetFloat("fuel"));
            veh.SetData("fuelTank", r.GetFloat("fuel_tank"));
            veh.SetData("fuelConsumption", r.GetFloat("fuel_consumption"));

            veh.SetData("engine", engine);

            veh.SetData("lastUsed", r.GetDateTime("last_used"));
            veh.SetData("lastDriver", -1);

            return veh;
        }

        public static string GetNumberplate(int charid)
        {

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand ccmd = new MySqlCommand("SELECT * FROM numberplates WHERE character_id = @cid", conn);
            ccmd.Parameters.AddWithValue("@cid", charid);
            MySqlDataReader reader = ccmd.ExecuteReader();
            bool scCheck = reader.Read();
            if (!scCheck)
            {
                reader.Close();
                DatabaseAPI.API.GetInstance().FreeConnection(conn);
                return "";
            } else
            {
                string Plate = "LS|" + reader.GetString("numberplate");
                reader.Close();
                DatabaseAPI.API.GetInstance().FreeConnection(conn);
                return Plate;
            }
        }

        public static Vehicle Spawnfv(MySqlDataReader r)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[6];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var numberPlate = new String(stringChars);
            byte alpha = 255;
            bool engine = r.GetBoolean("engine");
            int c = r.GetInt32("c");
            int s = r.GetInt32("s");
            bool active = r.GetBoolean("active");

            Vehicle veh = NAPI.Vehicle.CreateVehicle(
                r.GetUInt32("hash"),
                new Vector3(r.GetFloat("p_x"), r.GetFloat("p_y"), r.GetFloat("p_z")),
                r.GetFloat("r"),
                c,
                s,
                numberPlate,
                alpha,
                r.GetBoolean("locked"),
                engine
            );

            if (c == 0 && s == 0)
            {
                veh.CustomPrimaryColor = new Color(r.GetByte("c_r"), r.GetByte("c_g"), r.GetByte("c_b"));
                veh.CustomSecondaryColor = new Color(r.GetByte("s_r"), r.GetByte("s_g"), r.GetByte("s_b"));
            }

            veh.SetData("active", active);
            veh.SetData("fid", r.GetInt32("id"));
            veh.SetData("fraktion", r.GetString("fraktion"));

            veh.SetData("hp", r.GetFloat("hp"));

            veh.SetData("multi", r.GetInt32("multi"));

            veh.SetData("km", r.GetFloat("km"));
            veh.SetData("fuel", r.GetFloat("fuel"));
            veh.SetData("fuelTank", r.GetFloat("fuel_tank"));
            veh.SetData("fuelConsumption", r.GetFloat("fuel_consumption"));

            veh.SetData("engine", engine);

            veh.SetData("lastUsed", r.GetDateTime("last_used"));
            veh.SetData("lastDriver", -1);

            return veh;
        }

        public static Vehicle Spawnjv(Client p, string vehName)
        {
            byte alpha = 255;
            bool engine = false;
            int c = 1;
            int s = 1;
            bool active = true;

            uint VehHash = NAPI.Util.GetHashKey(vehName);

            Vehicle veh = NAPI.Vehicle.CreateVehicle(
                VehHash,
                p.Position,
                p.Rotation.Z,
                c,
                s,
                "LYD",
                alpha,
                false,
                engine
            );

            veh.SetData("active", active);
            veh.SetData("jid", 123);
            veh.SetData("owner", p.GetData("character_id"));

            veh.SetData("hp", 100);

            veh.SetData("multi", 1);

            veh.SetData("km", 0);
            veh.SetData("fuel", 65);
            veh.SetData("fuelTank", 65);
            veh.SetData("fuelConsumption", 2);

            veh.SetData("engine", engine);

            veh.SetData("lastDriver", -1);

            return veh;
        }

        [RemoteEvent("clog")]
        public static void OnCLog(Client c, string s)
        {
            Console.WriteLine(s);
        }

        public static int VehicleID(Client c)
        {
            Vehicle veh = c.GetData("lastVehicle");

            if (veh.HasData("jid"))
            {
                int vehid = veh.GetData("jid");
                return vehid;
            }
            else if (veh.HasData("fid"))
            {
                int vehid = veh.GetData("fid");
                return vehid;
            }
            else if (veh.HasData("id"))
            {
                int vehid = veh.GetData("id");
                return vehid;
            }

            return -1;
        }

        [RemoteEvent("updateVehicle")]
        public static void UpdateVehicle(Client c, int vId, float km, float fuel)
        {
            if (c.Vehicle.HasData("temp"))
                return;

            Vehicle veh = c.GetData("lastVehicle");

            if (VehicleID(c) == vId)
            {
                veh.SetData("km", km);
                veh.SetData("fuel", fuel);
                if (fuel == 0)
                {
                    if (veh.GetData("engine"))
                    {
                        veh.SetData("engine", false);
                        veh.EngineStatus = false;
                        syncVehicle(c, veh);
                    }
                }
            } else
            {
                Console.WriteLine("Veh update error: " + vId);
            }

            veh.SetData("lastUsed", DateTime.Now);
        }

        [RemoteEvent("ParkVehicle")]
        public void ParkOwnVehicle(Client c)
        {
            Vehicle v = c.Vehicle;

            if (!c.IsInVehicle)
            {
                c.SendNotification("Du bist in keinem Fahrzeug!");
                return;
            }

            if (!v.HasData("id"))
            {
                c.SendNotification("Dieses Fahrzeug kannst du nicht parken!");
                return;
            }

            if (v.GetData("owner") != c.GetData("character_id"))
            {
                c.SendNotification("Dieses Fahrzeug gehört nicht dir!");
                return;
            }

            MySqlCommand cmd = new MySqlCommand("UPDATE vehicles SET " +
            "p_x = @p_x, p_y = @p_y, p_z = @p_z, r = @r, " +
            "engine = @engine, locked = @locked, hp = @hp, km=@km, fuel=@fuel, last_used = @last_used " +
            "WHERE id = @id");
            cmd.Parameters.AddWithValue("@p_x", v.Position.X);
            cmd.Parameters.AddWithValue("@p_y", v.Position.Y);
            cmd.Parameters.AddWithValue("@p_z", v.Position.Z);
            cmd.Parameters.AddWithValue("@r", v.Rotation.Z);

            cmd.Parameters.AddWithValue("@engine", v.GetData("engine"));
            cmd.Parameters.AddWithValue("@locked", v.Locked);
            cmd.Parameters.AddWithValue("@hp", v.GetData("hp"));
            cmd.Parameters.AddWithValue("@km", v.GetData("km"));
            cmd.Parameters.AddWithValue("@fuel", v.GetData("fuel"));
            cmd.Parameters.AddWithValue("@last_used", v.GetData("lastUsed"));

            cmd.Parameters.AddWithValue("@id", v.GetData("id"));

            DatabaseAPI.API.executeNonQuery(cmd);
            c.SendNotification("Dein Fahrzeug wurde ~g~erfolgreich~w~ geparkt!");
        }

        [RemoteEvent("OpenVehicleInteraction")]
        public void OpenPlayerInteraction(Client c)
        {

            Vector3[] RepairPos = new Vector3[]
            {
                new Vector3(-965.0243, -3024.913, 13.56587),
                new Vector3(111.7056, 6625.828, 31.40783),
                new Vector3(732.7979, -1088.876, 21.68531)
            };

            Vehicle[] veh = NAPI.Pools.GetAllVehicles().ToArray();

            for (int i = 0; i < veh.Length; i++)
            {
                if (c.Position.DistanceTo2D(veh[i].Position) <= 3)
                {
                    int isinvehicle = 0;
                    int isinrepair = 0;

                    if (c.IsInVehicle && c.VehicleSeat == -1)
                    {
                        isinvehicle = 1;

                        for (int rp = 0, max = RepairPos.Length; rp < max; rp++)
                        {
                            if (c.Position.DistanceTo2D(RepairPos[rp]) < 5)
                            {
                                isinrepair = 1;
                            }
                        }
                    }

                    c.TriggerEvent("VehicleInteraction", isinvehicle, isinrepair);
                }
            }
        }

        [RemoteEvent("RepairVehicle")]
        public void RepairVehicle(Client c)
        {
            if (c.GetData("money_cash") >= 100)
            {
                MoneyAPI.API.SubCash(c, 100);
            } else if (c.GetData("money_bank") >= 100)
            {
                BankAPI.API.SubCash(c, 100);
            } else
            {
                c.SendNotification("Du besitzt nicht genügend Geld!");
                return;
            }

            c.Vehicle.Repair();
            c.SendNotification("Fahrzeug repariert!");
        }

        [RemoteEvent("toggleEngine")]
        public static void ToggleEngine(Client c)
        {
            if (c.Vehicle.HasData("temp"))
                return;

            if (c.Vehicle.HasData("fraktion"))
            {
                if (Fraktionssystem.API.Frakranknames[(c.GetData("fraktion") > Fraktionssystem.API.Frakranknames.Length) ? 0 : c.GetData("fraktion")] == c.Vehicle.GetData("fraktion"))
                {
                    Vehicle v = c.Vehicle;
                    bool engine = v.GetData("engine");
                    if (engine)
                    {
                        v.SetData("engine", false);
                        v.EngineStatus = false;
                    }
                    else
                    {
                        if (v.GetData("fuel") == 0)
                        {
                            c.SendNotification("Der Tank ist leer");
                        }
                        else
                        {
                            v.SetData("engine", true);
                            v.EngineStatus = true;
                        }
                    }
                }
                else
                {
                    c.SendNotification("~r~Du besitzt keinen Schlüssel!");
                }
                return;
            }

            if (c.GetData("character_id") == c.Vehicle.GetData("owner"))
            {
                Vehicle v = c.Vehicle;
                bool engine = v.GetData("engine");
                if (engine)
                {
                    v.SetData("engine", false);
                    v.EngineStatus = false;
                }
                else
                {
                    if (v.GetData("fuel") == 0)
                    {
                        c.SendNotification("Der Tank ist leer");
                    }
                    else
                    {
                        v.SetData("engine", true);
                        v.EngineStatus = true;
                    }
                }
            } else
            {
                c.SendNotification("~r~Du besitzt keinen Schlüssel!");
            }
        }


        public static void MoveIn(Client c, Vehicle veh)
        {
            c.SetIntoVehicle(veh, -1);
            syncVehicle(c, veh);

        }

        [ServerEvent(Event.PlayerEnterVehicle)]
        public void OnPlayerEnterVehicle(Client c, Vehicle veh, sbyte seatID)
        {
            if (c.Vehicle.HasData("temp"))
                return;

            if (seatID == -1)
            {
                syncVehicle(c, veh);
                c.TriggerEvent("OpenVehEnterInfo");
                NAPI.Task.Run(() =>
                {
                    c.TriggerEvent("ExitVehEnterInfo");
                }, delayTime: 5000);
            }

            c.TriggerEvent("VehicleEnter", seatID);

            if (c.HasData("InJobTP"))
            {
                c.TriggerEvent("deleteTruckerLevelCP1");
                c.ResetData("InJobTP");
                c.SendChatMessage("[~g~Trucker~w~]: Du bist in ein Fahrzeug gestiegen und hast somit den Job beendet.");
            }

            if (c.Vehicle.HasData("JobVeh"))
            {
                if (c.Vehicle.GetData("JobVeh") == "Taxi")
                {
                    if (c.VehicleSeat != -1)
                    {
                        c.SendChatMessage("[~y~Taxifahrer~w~]: Willkommen im Taxi!");
                        c.SendChatMessage("[~y~Taxifahrer~w~]: Deine ersten Kosten betragen 15~g~$~w~!");
                        MoneyAPI.API.SubCash(c, 15);
                        Client fahrer = c.Vehicle.GetData("JobVehicleFahrer");
                        MoneyAPI.API.AddCash(fahrer, 15);
                        fahrer.SendNotification("Neuer Fahrgast!");
                        c.SetData("TaxiFahrt", 1);
                        TaxiFahrtKosten(c);
                    }
                }
            }
        }

        public static void TaxiFahrtKosten(Client c)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (!c.HasData("TaxiFahrt"))
                    {
                        break;
                    }

                    Task.Delay(1 * 30 * 1000).Wait();

                    Task Fahrtkosten = Task.Run(() =>
                    {
                        MoneyAPI.API.SubCash(c, 30);
                        Client fahrer = c.Vehicle.GetData("JobVehicleFahrer");
                        MoneyAPI.API.AddCash(fahrer, 30);
                    });

                    Fahrtkosten.Wait();
                }
            });
        }

        public static void syncVehicle (Client c, Vehicle veh)
        {
            try
            {
                float fuel = veh.GetData("fuel");
                bool engine = veh.GetData("engine");
                if (fuel == 0 && engine)
                {
                    veh.SetData("engine", false);
                    engine = false;
                }

                if (veh.HasData("jid"))
                {
                    c.TriggerEvent("vehicleEnter",
                    veh.GetData("jid"),
                    veh.GetData("hp"),
                    veh.GetData("km"),
                    veh.GetData("multi"),
                    fuel,
                    veh.GetData("fuelTank"),
                    veh.GetData("fuelConsumption"));
                } else if (veh.HasData("fid"))
                {
                    c.TriggerEvent("vehicleEnter",
                    veh.GetData("fid"),
                    veh.GetData("hp"),
                    veh.GetData("km"),
                    veh.GetData("multi"),
                    fuel,
                    veh.GetData("fuelTank"),
                    veh.GetData("fuelConsumption")
                );
                } else
                {
                    c.TriggerEvent("vehicleEnter",
                    veh.GetData("id"),
                    veh.GetData("hp"),
                    veh.GetData("km"),
                    veh.GetData("multi"),
                    fuel,
                    veh.GetData("fuelTank"),
                    veh.GetData("fuelConsumption")
                );
                }

                //c.SendChatMessage("Cur Veh: " + veh.DisplayName);
                //uint hash = veh.Model;
                //c.SendChatMessage("Hash: " + hash);

                veh.SetData("lastUsed", DateTime.Now);
                veh.SetData("lastDriver", c.GetData("character_id"));
                c.SetData("lastVehicle", veh);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " +e.Message);
            }
        }

        public static void RemoveVehicleToPlayer(Client c)
        {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();
            MySqlCommand cmd = new MySqlCommand("UPDATE characters SET vehicles = @veh WHERE id = @cid", conn);
            cmd.Parameters.AddWithValue("@cid", c.GetData("character_id"));
            cmd.Parameters.AddWithValue("@veh", c.GetData("vehicles") - 1);
            cmd.ExecuteNonQuery();

            MySqlCommand cmd2 = new MySqlCommand("DELETE FROM vehicles WHERE id = @vid", conn);
            cmd2.Parameters.AddWithValue("@vid", c.Vehicle.GetData("id"));
            cmd2.ExecuteNonQuery();
            DatabaseAPI.API.GetInstance().FreeConnection(conn);
            c.SetData("vehicles", -1);
        }

        [ServerEvent(Event.PlayerExitVehicle)]
        public void OnPlayerExitVehicle(Client c, Vehicle veh)
        {
            if (c.HasData("InFahrschule"))
            {
                NAPI.Task.Run(() =>
                {
                    if (c.IsInVehicle)
                    {
                        if (c.VehicleSeat == -1)
                        {
                            if (c.Vehicle.HasData("FahrschulFahrzeug"))
                            {
                                if (c.Vehicle.GetData("owner") == c.GetData("character_id"))
                                {
                                    return;
                                }
                            }
                        }
                    }

                    if (!c.HasData("InFahrschule"))
                        return;

                    c.SendChatMessage("[~b~Fahrlehrer~w~]: Du hast 10 Sekunden um wieder ins Fahrzeug zu steigen!");

                    NAPI.Task.Run(() =>
                    {
                        if (c.IsInVehicle)
                        {
                            if (c.VehicleSeat == -1)
                            {
                                if (c.Vehicle.HasData("FahrschulFahrzeug"))
                                {
                                    if (c.Vehicle.GetData("owner") == c.GetData("character_id"))
                                    {
                                        return;
                                    }
                                }
                            }
                        }

                        Jobs.Main.DeleteJobVehicle(c, c.GetData("InFahrschule"));
                        c.SendChatMessage("[~b~Fahrlehrer~w~]: Du hast deine Prüfung abgebrochen.");
                        return;
                    }, delayTime: 10000);
                }, delayTime: 1000);
            }

            if (c.HasData("TaxiFahrt"))
            {
                c.ResetData("TaxiFahrt");
                return;
            }

            if (c.HasData("InJob"))
            {
                NAPI.Task.Run(() =>
                {
                    if (c.IsInVehicle)
                    {
                        if (c.VehicleSeat == -1)
                        {
                            if (c.Vehicle.GetData("owner") == c.GetData("character_id"))
                            {
                                return;
                            }
                        }
                    }

                    c.SendNotification("~y~ACHTUNG~w~: Dein Job wird in 30 Sekunden beendet.");

                    NAPI.Task.Run(() =>
                    {
                        if (c.IsInVehicle)
                        {
                            if (c.VehicleSeat == -1)
                            {
                                if (c.Vehicle.GetData("owner") == c.GetData("character_id"))
                                {
                                    return;
                                }
                            }
                        }

                        Jobs.Main.DeleteJobVehicle(c, veh);
                    }, delayTime: 30000);
                }, delayTime: 1000);
            }
        }

        [ServerEvent(Event.VehicleDeath)]
        public void IsVehicleDeath(Vehicle veh)
        {
            if (veh.HasData("id"))
            {
                int vehid = veh.GetData("id");
                InitNew.Spawn(vehid);
                return;
            }

            if (veh.HasData("fid"))
            {
                int vehid = veh.GetData("fid");
                InitNew.SpawnfVeh(vehid);
                return;
            }
        }

        [RemoteEvent("radiochange")]
        public void VehRadioSync(Client c, int radiodata)
        {
            try {
                c.Vehicle.SetSharedData("radio", radiodata);
            } catch {
                Console.WriteLine("Radio konnte nicht gesynct werden!");
            }
            
        }

        [RemoteEvent("syncSirens")]
        public void VehSirenSync(Client c)
        {
            if (c.Vehicle.HasSharedData("silentMode") && c.Vehicle.GetSharedData("silentMode") == true)
            {
                c.Vehicle.SetSharedData("silentMode", false);
            } else
            {
                c.Vehicle.SetSharedData("silentMode", true);
            }
        }
    }
}