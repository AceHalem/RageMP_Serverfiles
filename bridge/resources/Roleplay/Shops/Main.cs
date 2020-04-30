using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Roleplay.Shops
{
    public class Main : Script
    {
        public static void BuyItem(Client c, int item, int amount, int cost)
        {
            if (c.GetData("money_cash") < cost)
            {
                c.SendNotification("[~g~Verkäufer~w~]: Du hast nicht genügend Geld!");
                return;
            }

            InventoryAPI.API.AddItem(c, item, amount);
            MoneyAPI.API.SubCash(c, cost);
        }
    }
}
