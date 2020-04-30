using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Roleplay.BankAPI
{
    class API : Script
    {

        [Command("sbank")]
        public void HandleSbank(Client c, int value)
        {
            if (!PermissionAPI.API.HasPermission(c, 3))
                return;

            AddCash(c, value);
        }

        [Command("burnbank")]
        public void BurnBank(Client c, int value)
        {
            SubCash(c, value);
        }

        #region SyncCash
        /// <summary>
        /// Syncs cash from a db for a client.
        /// </summary>
        /// <param name="c">Client</param>
        /// <returns></returns>

        public static void SyncCash(Client c)
        {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM bank_accounts WHERE character_id = @charid", conn);
            cmd.Parameters.AddWithValue("@charid", c.GetData("character_id"));
            MySqlDataReader read = cmd.ExecuteReader();
            bool scCheck = read.Read();
            if (scCheck)
            {
                c.SetData("money_bank", read.GetInt32("money"));
                c.SetData("bankpin", read.GetInt16("pin"));
            } else
            {
                c.SetData("money_bank", 0);
            }
            read.Close();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);

            c.TriggerEvent("bankhud", c.GetData("money_bank"));
        }
        #endregion

        #region SetCash
        /// <summary>
        /// Sets cash on a client.
        /// </summary>
        /// <param name="c">Client</param>
        /// <param name="value">New money value</param>
        /// <returns></returns>
        public static void SetCash(Client c, int value)
        {
            c.SetData("money_bank", value);

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = new MySqlCommand("UPDATE bank_accounts SET money = @money WHERE character_id = @cid", conn);
            cmd.Parameters.AddWithValue("@money", value);
            cmd.Parameters.AddWithValue("@cid", c.GetData("character_id"));
            cmd.ExecuteNonQuery();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }
        #endregion

        #region AddCash
        /// <summary>
        /// Adds cash on a client.
        /// </summary>
        /// <param name="c">Client</param>
        /// <param name="value">Money value to add</param>
        /// <returns></returns>
        public static void AddCash(Client c, int value)
        {
            SetCash(c, c.GetData("money_bank") + value);
            c.TriggerEvent("bankhud", c.GetData("money_bank"));
            c.SendNotification("[FL~g~EE~w~CA]: ~g~+~w~" + value + "~g~$");
        }
        #endregion

        #region SubCash
        /// <summary>
        /// Subs cash on a client.
        /// </summary>
        /// <param name="c">Client</param>
        /// <param name="value">Money value to add</param>
        /// <returns></returns>
        public static void SubCash(Client c, int value)
        {
            SetCash(c, c.GetData("money_bank") - value);
            c.TriggerEvent("bankhud", c.GetData("money_bank"));
            c.SendNotification("[FL~g~EE~w~CA]: ~r~-~w~" + value + "~g~$");
        }
        #endregion
    }
}
