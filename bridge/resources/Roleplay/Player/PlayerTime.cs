using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Roleplay.Player
{
    class PlayerTime : Script
    {
        public static int[] Gehalt = new int[]
        {
            0,
            750,
            950,
            1200,
            1350,
            1500
        };

        [Command("setpay")]
        public void SetPayDay(Client c, int payday)
        {
            c.SetData("PlayerPaydayTimer", payday);
            c.SendNotification("Neuer Payday: " + c.GetData("PlayerPaydayTimer"));
        }

        [Command("getpay")]
        public void SetPayDay(Client c)
        {
            c.GetData("PlayerPaydayTimer");
            c.SendNotification("Payday: " + c.GetData("PlayerPaydayTimer"));
        }

        #region PayDay
        public static void OnStartPayday(Client c) //Timer starten
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Task.Delay(1 * 60 * 1000).Wait();

                    Task PaydayTimer = Task.Run(() =>
                    {
                        OnPlayerPayday(c);
                    });

                    PaydayTimer.Wait();
                }
            });
        }

        public static void OnPlayerPayday(Client c)
        {
            if (c.GetData("jailtime") != 0)
            {
                PlayerUpdate.UpdatePlayerJailtime(c, c.GetData("jailtime") - 1);
            }

            if (c.GetData("PlayerPaydayTimer") < 59)
            {
                c.SetData("PlayerPaydayTimer", c.GetData("PlayerPaydayTimer") +1);
            } else if (c.GetData("PlayerPaydayTimer") == 59)
            {
                c.SendNotification("[~b~Payday~w~]");
                c.SendNotification("[~g~Lohn~w~]:");
                MoneyAPI.API.AddCash(c, 500);
                if (c.GetData("fraktion") != 0)
                {
                    c.SendNotification("[~g~Fraktionsbonus~w~]:");
                    MoneyAPI.API.AddCash(c, Gehalt[(c.GetData("fraktionrank") > Gehalt.Length) ? 0 : c.GetData("fraktionrank")]);
                }

                if (c.GetData("wanteds") != 0)
                {
                    PlayerUpdate.UpdatePlayerWanteds(c, false, 1);
                }

                c.SetData("PlayerPaydayTimer", 0);
            }
        }
        #endregion
    }
}
