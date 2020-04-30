using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Roleplay.PermissionAPI
{
    public class API
    {
        public static bool HasPermission(Client c, int rank)
        {
            if (!c.HasData("account_id"))
                return false;

            if (!c.HasData("character_id"))
                return false;

            if (c.GetData("admin") < rank)
            {
                c.SendNotification("Du bist dazu nicht befugt!");
                return false;
            }

            return true;
        }

        public static void SetRank(Client c, Client p, int rank)
        {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "UPDATE accounts SET rank = @rank WHERE id = @pid";
            cmd.Parameters.AddWithValue("@pid", p.GetData("account_id"));
            cmd.Parameters.AddWithValue("@rank", rank);
            cmd.ExecuteNonQuery();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
            if (rank < p.GetData("admin"))
            {
                c.SendNotification("Du hast dem Spieler " + p.Name + " die Rechte entzogen!");
                p.SendNotification("[~r~Server~w~]: Dir wurde der Adminrank entzogen! Neuer Adminrank: ~r~" + rank);
                p.SetData("admin", rank);
            } else
            {
                c.SendNotification("Du hast dem Spieler " + p.Name + " die Rechte " + rank + " gegeben!");
                p.SendNotification("[~r~Server~w~]: Dir wurde der Adminrank ~r~" + rank + "~w~ zugewiesen!");
                p.SetData("admin", rank);
            }
        }
    }
}
