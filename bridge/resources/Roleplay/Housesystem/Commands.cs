using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Roleplay.Housesystem
{
    public class Commands : Script
    {
        //Zum speichern der Ausgeparkten Fahrzeuge in einer Garage
          //Garagen id,     vehicle slot id
        Dictionary<int, Dictionary<int, Vehicle>> activeGarages = new Dictionary<int, Dictionary<int, Vehicle>>();


        [RemoteEvent("HouseMenuOpen")]
        public void TestOpenHouseMenu(Client c) {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT x, y, z, locked FROM house";

            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (c.Position.DistanceTo(new Vector3(reader.GetInt32("x"), reader.GetInt32("y"), reader.GetInt32("z"))) < 2)
                    {
                        int lockedhouse = reader.GetInt32("locked");
                        c.TriggerEvent("StartHouseMenu", lockedhouse, c);            
                        break;
                    }
                } 
            }

            reader.Close();
            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }

        [RemoteEvent("ExitHouseMenuOpen")]
        public void TestExitHouseMenu(Client c) {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT locked FROM house WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", c.GetData("houseid"));

            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                if (c.HasData("houseid")) {
                    int locked = reader.GetInt32("locked");
                    c.TriggerEvent("StartExitHouseMenu", c, locked);
                }
            }

            reader.Close();
            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }

        [Command("delrenthouse")]
        public void DelRentHouse(Client c) {
            Housesystem.API.DelRentHouse(c);
        }

        [Command("setrenthouse")]
        public void SetRentHouse(Client c, int kosten) {
            Housesystem.API.SetRentHouse(c, kosten);
        }

        [Command("sellhouse")]
        public void SellHouse(Client c)
        {
            Housesystem.API.SellHouse(c);
        }

        [Command("buyhouse")]
        public void BuyHouse(Client c)
        {
            Housesystem.API.BuyHouse(c);
        }

        [Command("createhouse")]
        public void CreateHouse(Client c, int platz, int interior, int cost)
        {
            Housesystem.API.CreateHouse(c, platz, interior, cost);
        }

        [Command("rhouse")]
        public void RentHouse(Client c)
        {
            
        }

        [Command("urhouse")]
        public void UnrentHouse(Client c)
        {

        }
    }
}
