using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Roleplay.Login
{
    public class Commands : Script
    {
        [Command("uid")]
        public void HandleUid(Client c)
        {
            if (!c.HasData("uid"))
            {
                c.SendNotification("~r~Nicht eingeloggt!");
                return;
            }

            c.SendNotification("~g~UID: " + c.GetData("uid"));
        }

        [Command("udim")]
        public void Dimension(Client c)
        {
            c.SendNotification("Dimension: " + c.Dimension);
        }
    }
}
