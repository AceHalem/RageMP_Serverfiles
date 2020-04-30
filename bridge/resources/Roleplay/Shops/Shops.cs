using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Roleplay.Shops
{
    class Shops : Script
    {
        new Vector3[] OpenShopPositions = new Vector3[]
        {
            new Vector3(-1487.674, -378.5323, 40.16342),
            new Vector3(-707.4033, -914.5958, 19.21559),
            new Vector3(-48.32117, -1757.817, 29.42101),
            new Vector3(1135.713, -982.8101, 46.41581),
            new Vector3(1163.713, -323.9544, 69.20506),
            new Vector3(374.2068, 327.7614, 103.5664),
            new Vector3(-3041.015, 585.2131, 7.908929),
            new Vector3(-3243.889, 1001.395, 12.83072),
            new Vector3(1729.754, 6416.304, 35.03722),
            new Vector3(1697.955, 4924.557, 42.06368),
            new Vector3(1960.323, 3742.161, 32.34374),
            new Vector3(2677.131, 3281.386, 55.24113)
        };

        [RemoteEvent("BuyBenzinkanisterToServer")]
        public void BuyBenzinkanisterToServer(Client c)
        {
            Main.BuyItem(c, 5, 1, 150);
            c.SendNotification("[~g~Verkäufer~w~]: Du hast ein Benzinkanister gekauft!");
        }

        [RemoteEvent("OpenShopBrowser")]
        public void OpenShop(Client c)
        {
            for(int i = 0, max = OpenShopPositions.Length; i < max; i++)
            {
                if (c.Position.DistanceTo2D(OpenShopPositions[i]) <= 2)
                {
                    c.TriggerEvent("OpenShop");
                }
            }
        }

        [RemoteEvent("OpenKleidung")]
        public void Kleidungsgeschäft(Client c)
        {
            if (c.Position.DistanceTo2D(new Vector3(425.5883, -806.1441, 29.49115)) <= 3)
            {
                c.Position = new Vector3(429.7159, -811.9921, 29.49114);
                c.Rotation = new Vector3(0, 0, 355.1579);
                c.TriggerEvent("OpenKleidungsgeschäft");
            }
        }

        [RemoteEvent("KleidungBuy")]
        public void BuyClothes(Client c)
        {
            int preis = 0;

            if (c.HasData("temptops"))
            {
                preis = preis + 100;
                c.SetData("tops", c.GetData("temptops"));
                c.ResetData("temptops");
            }

            if (c.HasData("templegs"))
            {
                preis = preis + 100;
                c.SetData("legs", c.GetData("templegs"));
                c.ResetData("templegs");
            }

            if (c.HasData("tempshoes"))
            {
                preis = preis + 100;
                c.SetData("shoes", c.GetData("tempshoes"));
                c.ResetData("tempshoes");
            }

            if (c.HasData("temptorsos"))
            {
                c.SetData("torsos", c.GetData("temptorsos"));
                c.ResetData("temptorsos");
            }

            if (c.HasData("tempundershirts"))
            {
                c.SetData("undershirts", c.GetData("tempundershirts"));
                c.ResetData("tempundershirts");
            }

            if (c.GetData("money_cash") < preis)
            {
                c.SendNotification("[~g~Verkäufer~w~]: Du hast nicht genügend Geld bei dir!");
                return;
            } else
            {
                MoneyAPI.API.SubCash(c, preis);
                c.SendNotification("[~g~Verkäufer~w~]: Danke für Ihren Einkauf!");
            }

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();
            MySqlCommand cmd = new MySqlCommand("UPDATE characters_clothes SET tops = @tops, legs = @legs, shoes = @shoes, torsos = @torsos, undershirts = @us WHERE character_id=@cid", conn);
            cmd.Parameters.AddWithValue("@cid", c.GetData("character_id"));

            cmd.Parameters.AddWithValue("@tops", c.GetData("tops"));
            cmd.Parameters.AddWithValue("@legs", c.GetData("legs"));
            cmd.Parameters.AddWithValue("@shoes", c.GetData("shoes"));
            cmd.Parameters.AddWithValue("@torsos", c.GetData("torsos"));
            cmd.Parameters.AddWithValue("@us", c.GetData("undershirts"));

            cmd.ExecuteNonQuery();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }

        [RemoteEvent("KleidungSchliessen")]
        public void KleidungSchliessen(Client c)
        {
            if (c.HasData("temptops") || c.HasData("templegs") || c.HasData("tempshoes") || c.HasData("temptorsos"))
            {
                c.SetClothes(6, c.GetData("shoes"), 0);
                c.SetClothes(4, c.GetData("legs"), 0);
                c.SetClothes(11, c.GetData("tops"), 0);
                c.SetClothes(3, c.GetData("torsos"), 0);
                c.SetClothes(8, c.GetData("undershirts"), 0);
                c.ResetData("temptops");
                c.ResetData("templegs");
                c.ResetData("tempshoes");
                c.ResetData("temptorsos");
                c.ResetData("tempundershirts");
            }
        }
    }
}
