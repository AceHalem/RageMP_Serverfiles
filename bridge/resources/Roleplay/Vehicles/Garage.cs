using System;
using System.Collections.Generic;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Roleplay.Vehicles
{
    class Garage : Script
    {
        public static Vector3[] vehstore = new Vector3[]
        {
            new Vector3(53.97897, -880.0446, 30.10506),
            new Vector3(-339.7374, 288.5088, 85.24472),
            new Vector3(-1137.178, -860.3455, 13.23336),
            new Vector3(123.7082, 6620.354, 31.58156)
        };

        [RemoteEvent("IsInNearVehStore")]
        public void IsInNearVehStore(Client c)
        {
            if (c.IsInVehicle)
            {
                c.SendNotification("Du musst dafür aussteigen!");
                return;
            }

            if (c.Position.DistanceTo2D(vehstore[0]) < 5 || c.Position.DistanceTo2D(vehstore[1]) < 5
                || c.Position.DistanceTo2D(vehstore[2]) < 5 || c.Position.DistanceTo2D(vehstore[3]) < 5)
            {
                c.TriggerEvent("OpenVehStore");
            } else
            {
                c.SendNotification("Du bist bei keiner Garage!");
            }
        }

        [RemoteEvent("spawnVehicle")]
        public static void SpawnVehicles(Client c, int vId)
        {
            Console.WriteLine("SpawnVehicle: " + vId);
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            //Update vehicle to new Pos before spawn
            MySqlCommand cmd = new MySqlCommand("UPDATE vehicles SET active = 1, p_x = @p_x, p_y = @p_y, p_z = @p_z, r = @r WHERE id = @id AND active = 0", conn);
            cmd.Parameters.AddWithValue("@p_x", c.Position.X);
            cmd.Parameters.AddWithValue("@p_y", c.Position.Y);
            cmd.Parameters.AddWithValue("@p_z", c.Position.Z);
            cmd.Parameters.AddWithValue("@r", c.Rotation.Z);

            cmd.Parameters.AddWithValue("@id", vId);
            //Check if vehicle was not active
            int rowCount = cmd.ExecuteNonQuery();
            if (rowCount == 1)
            {
                cmd = new MySqlCommand("SELECT  v.*, c.hash, c.multi, fuel_tank, fuel_consumption FROM vehicles v JOIN cfg_vehicles c ON v.cfg_vehicle_id = c.id WHERE v.character_id = @c_id AND v.id = @id", conn);
                cmd.Parameters.AddWithValue("@c_id", c.GetData("character_id"));
                cmd.Parameters.AddWithValue("@id", vId);

                MySqlDataReader r = cmd.ExecuteReader();
                if (r.Read())
                {
                    Console.WriteLine("SpawnVehicle ok");
                    Vehicle veh = Vehicles.Spawn(r);
                    Vehicles.MoveIn(c, veh);
                }
                r.Close();
            }
            else
            {
                Console.WriteLine("SpawnVehicle err: " + rowCount);
            } 

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }

        [RemoteEvent("spawnVehicleLive")]
        public static void SpawnVehicleLive(Client c, Vehicle veh)
        {
            int vId = veh.GetData("id");
            Console.WriteLine("SpawnVehicleLive: " + vId);
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            //Update vehicle to new Pos before spawn
            MySqlCommand cmd = new MySqlCommand("UPDATE vehicles SET active = 1, p_x = @p_x, p_y = @p_y, p_z = @p_z, r = @r WHERE id = @id AND active = 0", conn);
            //Auspark position setzen (hier muss der Garagenausparkpukt rein)
            Vector3 pos = new Vector3();
            float rot = 0;

            cmd.Parameters.AddWithValue("@p_x", pos.X);
            cmd.Parameters.AddWithValue("@p_y", pos.Y);
            cmd.Parameters.AddWithValue("@p_z", pos.Z);
            cmd.Parameters.AddWithValue("@r", rot);

            cmd.Parameters.AddWithValue("@id", vId);


            //Check if vehicle was not active
            int rowCount = cmd.ExecuteNonQuery();
            if (rowCount == 1)
            {
                veh.Position = pos;
                veh.SetData("active", true);
            }
            else
            {
                //Fahrzeug konnte nicht ausgeparkt werden. 
                //Fahrzeug löschen und auf Spieler ausparkpunkt porten
                veh.Delete();
                c.Position = pos;
                Console.WriteLine("SpawnVehicle err: " + rowCount);
            }

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }
        public struct GarageVehicle
        {
            public int id;
            public uint hash;
            public float fuel;
            public Color color;
            public byte insurance;
            public float km;
            public float fuelConsumption;
            public DateTime buyDate;
        }


        [RemoteEvent("getVehicles")]
        public static void GetVehicles(Client c)
        {
            Console.WriteLine("getVehicles: ");

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM vehicles v JOIN cfg_vehicles c ON v.cfg_vehicle_id = c.id WHERE v.character_id = @c_id AND active = 0", conn);
            cmd.Parameters.AddWithValue("@c_id", c.GetData("character_id"));

            List<GarageVehicle> vehicles = new List<GarageVehicle> { };

            MySqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Color color = new Color(r.GetByte("c_r"), r.GetByte("c_g"), r.GetByte("c_b"));
                vehicles.Add(new GarageVehicle
                {
                    id = r.GetInt32("id"),
                    hash = r.GetUInt32("hash"),
                    fuel = r.GetFloat("fuel"),
                    color = color,
                    insurance = 0,
                    km = r.GetFloat("km"),
                    fuelConsumption = r.GetFloat("fuel_consumption"),
                    buyDate = r.GetDateTime("last_owner_change")
                });
            }
            r.Close();

            c.TriggerEvent("receiveVehicles", vehicles);

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }

    }
}
