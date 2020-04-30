using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Roleplay.Staat
{
    class Functions : Script
    {

        [RemoteEvent("Ekey")]
        public static void EKeyPressed(Client c)
        {
            if (c.Position.DistanceTo2D(new Vector3(-1016.432, -413.3861, 39.6161)) <= 2)
            {
                c.TriggerEvent("OeffneZulassungsstelle");
                return;
            }

            if (c.Position.DistanceTo2D(new Vector3(138.9402, -762.7764, 45.75203)) <= 2)
            {
                c.Position = new Vector3(136.1253, -761.7436, 242.152);
                c.Rotation = new Vector3(0, 0, 159.1514);
                return;
            }

            if (c.Position.DistanceTo2D(new Vector3(136.1253, -761.7436, 242.152)) <= 2)
            {
                c.Position = new Vector3(138.9402, -762.7764, 45.75203);
                c.Rotation = new Vector3(0, 0, 154.4504);
                return;
            }
        }

        [RemoteEvent("NewNumberplate")]
        public static void NewNumberplate(Client c, string numberplate)
        {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand ccmd = new MySqlCommand("SELECT character_id FROM numberplates WHERE character_id = @cid", conn);
            ccmd.Parameters.AddWithValue("@cid", c.GetData("character_id"));
            MySqlDataReader reader = ccmd.ExecuteReader();
            bool scCheck = reader.Read();
            reader.Close();
            if (scCheck)
            {
                c.SendNotification("Du besitzt bereits ein Nummernschild!");
                DatabaseAPI.API.GetInstance().FreeConnection(conn);
                return;
            }

            DatabaseAPI.API.GetInstance().FreeConnection(conn);

            if (numberplate.Length < 3)
            {
                c.SendNotification("Das Nummernschild benötigt min. 3 Zeichen!");
                return;
            }

            MySqlCommand cmd = new MySqlCommand("INSERT INTO numberplates (character_id, numberplate)VALUES(@cid, @numberplate)", conn);
            cmd.Parameters.AddWithValue("@cid", c.GetData("character_id"));
            cmd.Parameters.AddWithValue("@numberplate", numberplate);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    c.SendNotification("Dieses Nummernschild ist bereits vergeben~r~!");
                }

                DatabaseAPI.API.GetInstance().FreeConnection(conn);
                return;
            }

            DatabaseAPI.API.GetInstance().FreeConnection(conn);

            if (c.GetData("money_cash") >= 500)
            {
                MoneyAPI.API.SubCash(c, 500);
            } else if (c.GetData("money_bank") >= 500)
            {
                BankAPI.API.SubCash(c, 500);
            } else
            {
                c.SendNotification("[~g~STAAT~w~]: Du besitzt nicht genügend Geld für ein Nummerschild!");
                return;
            }

            c.SendNotification("[~g~STAAT~w~]: Du hast nun ein Nummernschild!");
        }
    }
}
