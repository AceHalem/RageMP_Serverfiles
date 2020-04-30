using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;


namespace Roleplay.Vehicles
{
    class InitNew
    {
        public static DateTime lastSave;
        public static void SpawnAll ()
        {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = new MySqlCommand("SELECT v.*, c.hash, c.multi, fuel_tank, fuel_consumption FROM vehicles v JOIN cfg_vehicles c ON v.cfg_vehicle_id = c.id WHERE v.active = 1", conn);
            MySqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Vehicles.Spawn(r);
            }
            r.Close();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }

        public static void Spawn(int veh)
        {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = new MySqlCommand("SELECT v.*, c.hash, c.multi, fuel_tank, fuel_consumption FROM vehicles v JOIN cfg_vehicles c ON v.cfg_vehicle_id = c.id WHERE v.id = @id", conn);
            cmd.Parameters.AddWithValue("@id", veh);

            MySqlDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                Vehicles.Spawn(r);
            }
            r.Close();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }

        public static void SpawnAllfVeh()
        {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = new MySqlCommand("SELECT v.*, c.hash, c.multi, fuel_tank, fuel_consumption FROM fvehicles v JOIN cfg_vehicles c ON v.cfg_vehicle_id = c.id WHERE v.active = 1", conn);
            MySqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Vehicles.Spawnfv(r);
            }
            r.Close();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }

        public static void SpawnfVeh(int veh)
        {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = new MySqlCommand("SELECT v.*, c.hash, c.multi, fuel_tank, fuel_consumption FROM fvehicles v JOIN cfg_vehicles c ON v.cfg_vehicle_id = c.id WHERE v.id = @fid", conn);
            cmd.Parameters.AddWithValue("@fid", veh);

            MySqlDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                Vehicles.Spawnfv(r);
            }
            r.Close();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }

        public static void Save()
        {
            foreach (Vehicle v in NAPI.Pools.GetAllVehicles())
            {
                SavePos(v);
            }

            lastSave = DateTime.Now;
        }

        public static void SaveFV()
        {
            foreach (Vehicle v in NAPI.Pools.GetAllVehicles())
            {
                SavePosFV(v);
            }

            lastSave = DateTime.Now;
        }

        public static void SavePosFV(Vehicle v)
        {
            if (v.HasData("fid"))
            {
                int vId = v.GetData("fid");
                if (vId != -1)
                {
                    if (v.GetData("active"))
                    {
                        if (v.GetData("lastUsed") > lastSave)
                        {
                            MySqlCommand cmd = new MySqlCommand("UPDATE fvehicles SET " +
                                "engine = @engine, locked = @locked, km=@km, fuel=@fuel " +
                                "WHERE id = @id");

                            cmd.Parameters.AddWithValue("@engine", v.GetData("engine"));
                            cmd.Parameters.AddWithValue("@locked", v.Locked);
                            cmd.Parameters.AddWithValue("@km", v.GetData("km"));
                            cmd.Parameters.AddWithValue("@fuel", v.GetData("fuel"));

                            cmd.Parameters.AddWithValue("@id", vId);

                            DatabaseAPI.API.executeNonQuery(cmd);
                        }
                    }
                }
            }
        }

        public static void SavePos(Vehicle v)
        {
            if (v.HasData("id"))
            {
                int vId = v.GetData("id");
                if (vId != -1)
                {
                    if (v.GetData("active"))
                    {
                        if (v.GetData("lastUsed") > lastSave)
                        {
                            MySqlCommand cmd = new MySqlCommand("UPDATE vehicles SET " +
                                "engine = @engine, locked = @locked, hp = @hp, km=@km, fuel=@fuel, last_used = @last_used " +
                                "WHERE id = @id");

                            cmd.Parameters.AddWithValue("@engine", v.GetData("engine"));
                            cmd.Parameters.AddWithValue("@locked", v.Locked);
                            cmd.Parameters.AddWithValue("@hp", v.GetData("hp"));
                            cmd.Parameters.AddWithValue("@km", v.GetData("km"));
                            cmd.Parameters.AddWithValue("@fuel", v.GetData("fuel"));
                            cmd.Parameters.AddWithValue("@last_used", v.GetData("lastUsed"));

                            cmd.Parameters.AddWithValue("@id", vId);

                            DatabaseAPI.API.executeNonQuery(cmd);
                        }
                    }
                }
            }
        }
    }
}
