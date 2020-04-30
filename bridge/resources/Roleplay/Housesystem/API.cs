using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Roleplay.Housesystem
{
    public class API : Script
    {

        public static Blip CreateMarker(uint sprite, Vector3 position, float scale, byte color, string name = "", byte alpha = 255, float drawDistance = 0, bool shortRange = true, short rotation = 0, uint dimension = uint.MaxValue)
        {
            Blip blip = NAPI.Blip.CreateBlip(sprite, position, scale, color, name, alpha, drawDistance, shortRange, rotation, dimension);
            return blip;
        }

        public static List<House> houseList;

        private static Vector3[] InteriorList = new Vector3[]
        {
            new Vector3(-614.86, 40.6783, 97.60007), //Hochhaus Gehoben - 0
            new Vector3(152.2605, -1004.471, -98.99999), //Low Low End Apartment - 1
            new Vector3(261.4586, -998.8196, -99.00863), //Low End Apartment - 2
            new Vector3(347.2686, -999.2955, -99.19622), //Medium End Apartment - 3
            new Vector3(-1477.14, -538.7499, 55.5264), //Hochhaus Sehr Gehoben - 4
            new Vector3(-169.286, 486.4938, 137.4436), // Sehr Gehoben Hills 1 - 5
            new Vector3(340.9412, 437.1798, 149.3925), // Sehr Gehoben Hills 2 - 6
            new Vector3(373.023, 416.105, 145.7006), // Sehr Gehoben Hills 3 - 7
            new Vector3(-676.127, 588.612, 145.1698), // Sehr Gehoben Hills 4 - 8
            new Vector3(-763.107, 615.906, 144.1401), // Sehr Gehoben Hills 5 - 9
            new Vector3(-857.798, 682.563, 152.6529), // Sehr Gehoben Hills 6 - 10
            new Vector3(-1288, 440.748, 97.69459), // Sehr Gehoben Hills 7 - 11
            new Vector3(1397.072, 1142.011, 114.3335) // Farm Ultra Luxus - 12
        };

        #region BuyHouse/SellHouse
        public static void SellHouse(Client c)
        {
            foreach (House houseModel in houseList)
            {
                while (c.Position.DistanceTo2D(houseModel.position) < 5 || c.Dimension == houseModel.id)
                {
                    if (houseModel.owner == c.Name)
                    {
                        FinishSellHouse(c, houseModel.id);

                        houseModel.owner = "STAAT";
                        houseModel.status = 2;
                        houseModel.locked = 0;
                        houseModel.houseLabel.Text = GetHouseLabelText(houseModel);

                        c.SendChatMessage("Haus verkauft!");
                        Log.WriteS($"Spieler {c.Name} hat sein Haus mit folgender ID verkauft: {houseModel.id}");
                    }
                    else
                    {
                        c.SendChatMessage("~r~Das Haus gehört nicht dir!");
                    }
                    break;
                }
            }
        }

        public static void BuyHouse(Client c)
        {

            foreach (House houseModel in houseList)
            {
                while (c.Position.DistanceTo2D(houseModel.position) < 5 || c.Dimension == houseModel.id)
                {
                    if (houseModel.status == 2)
                    {
                        if (c.GetData("h_key") != 0)
                        {
                            c.SendNotification("Du besitzt bereits ein Haus!");
                            return;
                        }

                        if (c.GetData("money_cash") <= houseModel.price)
                        {
                            c.SendNotification("Dein Geld reicht dafür nicht aus!");
                            return;
                        }

                        FinishBuyHouse(c, houseModel.id);

                        houseModel.owner = c.Name;
                        houseModel.status = 0;
                        houseModel.houseLabel.Text = GetHouseLabelText(houseModel);

                        MoneyAPI.API.SubCash(c, houseModel.price);
                        c.SendChatMessage("Haus gekauft!");
                        Log.WriteS($"Spieler {c.Name} hat das Haus mit folgender ID gekauft: {houseModel.id}");
                    }
                    else
                    {
                        c.SendChatMessage("~r~Das Haus steht nicht zum vekauf!");
                    }
                    break;
                }
            }
        }

        public static void FinishSellHouse(Client c, uint houseid)
        {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "UPDATE house SET owner=@owner, status=@status, locked=@locked WHERE id=@houseid";
            cmd.Parameters.AddWithValue("@houseid", houseid);
            cmd.Parameters.AddWithValue("@owner", "STAAT");
            cmd.Parameters.AddWithValue("@status", 2);
            cmd.Parameters.AddWithValue("@locked", 0);
            cmd.ExecuteNonQuery();

            cmd.CommandText = "UPDATE characters SET h_key=@hkey WHERE account_id=@aid";
            cmd.Parameters.AddWithValue("@aid", c.GetData("account_id"));
            cmd.Parameters.AddWithValue("@hkey", 0);
            cmd.ExecuteNonQuery();

            c.SetData("h_key", 0);

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }

        public static void FinishBuyHouse(Client c, uint houseid)
        {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "UPDATE house SET status=@status, owner=@owner WHERE id=@houseid";
            cmd.Parameters.AddWithValue("@houseid", houseid);
            cmd.Parameters.AddWithValue("@status", 0);
            cmd.Parameters.AddWithValue("@owner", c.Name);
            cmd.ExecuteNonQuery();

            cmd.CommandText = "UPDATE characters SET h_key=@hkey WHERE account_id=@aid";
            cmd.Parameters.AddWithValue("@aid", c.GetData("account_id"));
            cmd.Parameters.AddWithValue("@hkey", houseid);
            cmd.ExecuteNonQuery();

            c.SetData("h_key", houseid);

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }
        #endregion

        #region LockHouse
        [RemoteEvent("LockThatHouse")]
        public static void LockHouse(Client c)
        {
            if (c.IsInVehicle) {
                c.SendNotification("~r~Du musst dafür aussteigen!");
                return;
            }

            foreach (House houseModel in houseList)
            {
                if (c.Position.DistanceTo(houseModel.position) < 5 || c.Dimension == houseModel.id)
                {
                    if (c.GetData("h_key") == houseModel.id)
                    {
                        if (houseModel.locked == 0)
                        {
                            LockedHouse(0, houseModel.id);  
                            c.SendChatMessage("~r~Haus abgeschlossen!");
                        } else
                        {
                            LockedHouse(1, houseModel.id);
                            c.SendChatMessage("~g~Haus aufgeschlossen!");
                        }
                    } else
                    {
                        c.SendChatMessage($"Du besitzt keinen Schlüssel für dieses Haus!");
                    }
                    break;
                }
            }
        }

        public static void LockedHouse(int locked, uint houseid)
        {

            House house = GetHouseById(houseid);

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = new MySqlCommand("UPDATE house SET locked=@locked WHERE id=@houseid", conn);
            cmd.Parameters.AddWithValue("@houseid", houseid);

            if (locked == 0)
            {
                cmd.Parameters.AddWithValue("@locked", 1);
                cmd.ExecuteNonQuery();

                house.locked = 1;
            } else if (locked == 1)
            {
                cmd.Parameters.AddWithValue("@locked", 0);
                cmd.ExecuteNonQuery();

                house.locked = 0;
            }

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }
        #endregion

        #region EnterHouse/ExitHouse
        [RemoteEvent("EnterThatHouse")]
        public static void EnterHouse(Client c)
        {
            if (c.IsInVehicle) {
                c.SendNotification("~r~Du musst dafür aussteigen!");
                return;
            }

            foreach (House houseModel in houseList)
            {
                while (c.Position.DistanceTo(houseModel.position) < 2)
                {
                    if (houseModel.locked == 0)
                    {
                        c.Dimension = houseModel.id;
                        c.Position = InteriorList[(houseModel.interior > InteriorList.Length) ? 0 : houseModel.interior];
                        c.SetData("houseid", houseModel.id);
                        c.SendNotification("Dimension:" + c.Dimension);
                    }
                    else
                    {
                        c.SendChatMessage("~r~Das Haus ist abgeschlossen!");
                    }
                    break;
                }
            }
        }
        
        [RemoteEvent("ExitThatHouse")]
        public static void ExitHouse(Client c)
        {
            foreach (House houseModel in houseList)
            {
                while (c.Dimension == houseModel.id)
                {
                    c.Dimension = 0;
                    c.Position = houseModel.position;
                    c.ResetData("houseid");
                    break;
                }
            }
        }
        #endregion

        #region CreateHouse
        public static void CreateHouse(Client c, int platz, int interior, int cost)
        {

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = new MySqlCommand("INSERT INTO house (status, owner, interior, x, y, z, locked, cost, renter, rentcost, platz) VALUES (@status, @account, @interior, @x, @y, @z, @locked, @cost, @renter, @rentcost, @platz)", conn);
            cmd.Parameters.AddWithValue("@status", 2);
            cmd.Parameters.AddWithValue("@account", "STAAT");
            cmd.Parameters.AddWithValue("@interior", interior);
            cmd.Parameters.AddWithValue("@x", c.Position.X);
            cmd.Parameters.AddWithValue("@y", c.Position.Y);
            cmd.Parameters.AddWithValue("@z", c.Position.Z);
            cmd.Parameters.AddWithValue("@locked", 0);
            cmd.Parameters.AddWithValue("@cost", cost);
            cmd.Parameters.AddWithValue("@renter", 0);
            cmd.Parameters.AddWithValue("@rentcost", 0);
            cmd.Parameters.AddWithValue("@platz", platz);
            cmd.ExecuteNonQuery();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);

            MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM house WHERE id = LAST_INSERT_ID();", conn);
            MySqlDataReader reader = cmd2.ExecuteReader();

            if (reader.Read())
            {
                //c.SetData("lasthouse", reader.GetInt32("id")); Für die Garage geplant der jeweiligen Häuser

                    House house = new House();
                    float posX = reader.GetFloat("x");
                    float posY = reader.GetFloat("y");
                    float posZ = reader.GetFloat("z");

                    house.id = reader.GetUInt32("id");
                    house.status = reader.GetInt32("status");
                    house.position = new Vector3(posX, posY, posZ);
                    house.price = reader.GetInt32("cost");
                    house.owner = reader.GetString("owner");
                    house.renter = reader.GetInt32("renter");
                    house.rentcost = reader.GetInt32("rentcost");
                    house.locked = reader.GetInt32("locked");
                    house.interior = reader.GetInt32("interior");
                    house.platz = reader.GetInt32("platz");

                    houseList.Add(house);

                    string houseLabelText = GetHouseLabelText(house);
                    house.houseLabel = NAPI.TextLabel.CreateTextLabel(houseLabelText, house.position, 20.0f, 0.75f, 4, new Color(255, 255, 255), false, 0);
                    CreateMarker(40, house.position, 0.8f, 2, "Haus");
            }
            reader.Close();
        }
        #endregion

        #region Rent
        public static void DelRentHouse(Client c) {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT * FROM house";

            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (c.Dimension == reader.GetInt32("id"))
                    {
                        if (reader.GetString("owner") == c.Name)
                        {
                            uint houseId = c.GetData("houseid");
                            House house = GetHouseById(houseId);

                            house.status = 0;
                            house.renter = 0;

                            house.houseLabel.Text = GetHouseLabelText(house);
                            c.SendChatMessage($"Haus wird nun nicht mehr vermietet!");
                        }
                        else
                        {
                            c.SendChatMessage("~r~Das Haus gehört nicht dir!");
                            return;
                        }
                        break;
                    }
                }
            }

            reader.Close();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);

            MySqlConnection conn2 = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd2 = conn2.CreateCommand();
            cmd2.CommandText = "UPDATE house SET status=@status, renter=@renter WHERE owner=@owner";
            cmd2.Parameters.AddWithValue("@status", 0);
            cmd2.Parameters.AddWithValue("@renter", 0);
            cmd2.Parameters.AddWithValue("@owner", c.Name);
            cmd2.ExecuteNonQuery();

            DatabaseAPI.API.GetInstance().FreeConnection(conn2);
        }

        public static void SetRentHouse(Client c, int cost) {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT * FROM house";

            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (c.Dimension == reader.GetInt32("id"))
                    {
                        if (reader.GetString("owner") == c.Name)
                        {
                            uint houseId = c.GetData("houseid");
                            House house = GetHouseById(houseId);

                            house.status = 1;
                            house.rentcost = cost;

                            house.houseLabel.Text = GetHouseLabelText(house);
                            c.SendChatMessage($"Haus wird nun für {cost}~g~$~w~ vermietet!");
                        }
                        else
                        {
                            c.SendChatMessage("~r~Das Haus gehört nicht dir!");
                            return;
                        }
                        break;
                    }
                }
            }

            reader.Close();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);

            MySqlConnection conn2 = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd2 = conn2.CreateCommand();
            cmd2.CommandText = "UPDATE house SET status=@status, rentcost=@rentcost, WHERE owner=@owner";
            cmd2.Parameters.AddWithValue("@status", 1);
            cmd2.Parameters.AddWithValue("@rentcost", cost);
            cmd2.Parameters.AddWithValue("@owner", c.Name);
            cmd2.ExecuteNonQuery();

            DatabaseAPI.API.GetInstance().FreeConnection(conn2);
        }
        #endregion

        #region LoadHouse
        public static List<House> LoadAllHouses()
        {
            List<House> houseList = new List<House>();

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT * FROM house";

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    House house = new House();
                    float posX = reader.GetFloat("x");
                    float posY = reader.GetFloat("y");
                    float posZ = reader.GetFloat("z");

                    house.id = reader.GetUInt32("id");
                    house.status = reader.GetInt32("status");
                    house.position = new Vector3(posX, posY, posZ);
                    house.price = reader.GetInt32("cost");
                    house.owner = reader.GetString("owner");
                    house.renter = reader.GetInt32("renter");
                    house.rentcost = reader.GetInt32("rentcost");
                    house.locked = reader.GetInt32("locked");
                    house.interior = reader.GetInt32("interior");
                    house.platz = reader.GetInt32("platz");

                    houseList.Add(house);
                    
                }
            }

            return houseList;
        }
      
        public static void LoadDatabaseHouses()
        {
            houseList = LoadAllHouses();
            foreach (House houseModel in houseList)
            {
                string houseLabelText = GetHouseLabelText(houseModel);
                houseModel.houseLabel = NAPI.TextLabel.CreateTextLabel(houseLabelText, houseModel.position, 20.0f, 0.75f, 4, new Color(255, 255, 255), false, 0);
                CreateMarker(40, houseModel.position, 0.8f, 2, "Haus");
            }
        }

        public static string GetHouseLabelText(House house)
        {
            string label = string.Empty;

            switch (house.status)
            {
                case 0:
                    label = $"[~g~Haus von~w~]:" + house.owner;
                    break;
                case 1:
                    label = $"[~g~Haus von~w~]:" + house.owner +  "\n[~g~Mietkosten~w~]: " + house.rentcost + "~g~$";
                    break;
                case 2:
                    label = "[~y~Haus wird verkauft!~w~]\n[~g~Kosten~w~]: "+ house.price + "~g~$~w~\n[~g~Max. Inventar~w~]: " + house.platz + "~g~kg~w~";
                    break;
            }
            return label;
        }

        public static House GetHouseById(uint id)
        {
            House house = null;
            foreach (House houseModel in houseList)
            {
                if (houseModel.id == id)
                {
                    house = houseModel;
                    break;
                }
            }
        return house;
        }
        #endregion
    }
}
