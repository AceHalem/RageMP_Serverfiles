using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Roleplay.Player
{
    class PlayerUpdate
    {
        public static void UpdatePlayerWanteds(Client c, bool plus, int wanteds)
        {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();
            MySqlCommand cmd = new MySqlCommand("UPDATE characters SET wanteds = @wanteds WHERE id=@cid", conn);
            cmd.Parameters.AddWithValue("@cid", c.GetData("character_id"));

            if (plus)
            {
                
                cmd.Parameters.AddWithValue("@wanteds", c.GetData("wanteds") + wanteds);

                cmd.ExecuteNonQuery();

                DatabaseAPI.API.GetInstance().FreeConnection(conn);

                c.SetData("wanteds", c.GetData("wanteds") + wanteds);
            } else
            {
                cmd.Parameters.AddWithValue("@wanteds", c.GetData("wanteds") - wanteds);

                cmd.ExecuteNonQuery();

                DatabaseAPI.API.GetInstance().FreeConnection(conn);

                c.SetData("wanteds", c.GetData("wanteds") - wanteds);
            }

            c.TriggerEvent("starhud", c.GetData("wanteds"));
        }

        public static void UpdatePlayerJailtime(Client c, int jailtime)
        {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();
            MySqlCommand cmd = new MySqlCommand("UPDATE characters SET jailtime = @jailtime WHERE id=@cid", conn);
            cmd.Parameters.AddWithValue("@cid", c.GetData("character_id"));
            cmd.Parameters.AddWithValue("@jailtime", jailtime);

            cmd.ExecuteNonQuery();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);

            c.SetData("jailtime", jailtime);

            if (jailtime == 0)
            {
                c.SendNotification("Benimm dich in Zukunft!");
                c.Position = new Vector3(1690.82, 2591.23, 45.91441);
                c.Rotation = new Vector3(0.9488825, 0, 0);
            } else
            {
                c.SendNotification($"Jailtime: {c.GetData("jailtime")} Minuten");
            }
        }

        public static void UpdateLicensePoints(Client c, int points)
        {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            c.SetData("autoscheinpunkte", c.GetData("autoscheinpunkte") + points);

            if(c.GetData("autoscheinpunkte") >= 10) {
                c.SendChatMessage("[~g~STAAT~w~]: ACHTUNG! Dein Führerschein ist nicht mehr gültig da Du mehr als 10 Punkte hast.");

                c.ResetData("führerschein");

                MySqlCommand cmd = new MySqlCommand("DELETE FROM licenses WHERE character_name=@cname", conn);
                cmd.Parameters.AddWithValue("@cname", c.Name);

                cmd.ExecuteNonQuery();

                DatabaseAPI.API.GetInstance().FreeConnection(conn);
            } else
            {
                MySqlCommand cmd = new MySqlCommand("UPDATE licenses SET punkte = @points WHERE character_name=@cname", conn);
                cmd.Parameters.AddWithValue("@cname", c.Name);
                cmd.Parameters.AddWithValue("@points", c.GetData("autoscheinpunkte") + points);

                cmd.ExecuteNonQuery();

                DatabaseAPI.API.GetInstance().FreeConnection(conn);
            }
        }

        public static void SyncPlayer(Client c)
        {
            #region Wanteds
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = new MySqlCommand("SELECT wanteds, jailtime FROM characters WHERE id = @character_id", conn);
            cmd.Parameters.AddWithValue("@character_id", c.GetData("character_id"));
            MySqlDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                c.SetData("wanteds", r.GetInt32("wanteds"));
                c.SetData("jailtime", r.GetInt32("jailtime"));

                if (r.GetInt32("jailtime") != 0)
                {
                    c.Position = new Vector3(1729.212, 2563.543, 45.56488);
                    c.Rotation = new Vector3(186.1379, 0, 0);
                }
            }
            r.Close();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);

            c.TriggerEvent("starhud", c.GetData("wanteds"));
            #endregion

            #region Führerscheine
            cmd = new MySqlCommand("SELECT * FROM licenses WHERE character_name = @cn", conn);
            cmd.Parameters.AddWithValue("@cn", c.Name);
            MySqlDataReader read = cmd.ExecuteReader();
            bool scCheck = read.Read();
            if (scCheck)
            {
                c.SetData("führerschein", 1);
                c.SetData("autoscheinpunkte", read.GetInt16("punkte"));
            }
            read.Close();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
            #endregion
        }
    }
}
