using System;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Roleplay.MoneyAPI
{
    public class API : Script
    {
        [Command("cash")]
        public void HandleCash(Client c)
        {
            c.SendNotification("Cash: " + c.GetData("money_cash"), true);
            c.TriggerEvent("TestClientEvent");
        }

        [Command("scash")]
        public void HandleScash(Client c, int value)
        {
            if (!PermissionAPI.API.HasPermission(c, 3))
                return;

            AddCash(c, value);
            c.SendNotification("New Cash: " + c.GetData("money_cash"), true);
        }

        [Command("burncash")]
        public void BurnCash(Client c, int value) {
            SubCash(c, value);
        }

        #region EventGetMoney
        [RemoteEvent("getCash")]
        public void EventGetCash(Client c)
        {
            c.TriggerEvent("retrieveCash", c.GetData("money_cash"));
        }
        #endregion

        #region SyncCash
        /// <summary>
        /// Syncs cash from a db for a client.
        /// </summary>
        /// <param name="c">Client</param>
        /// <returns></returns>

        public static void SyncCash(Client c)
        {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = new MySqlCommand("SELECT cash FROM characters WHERE account_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", c.GetData("account_id"));
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read()) {
                c.SetData("money_cash", reader.GetInt32("cash"));
            }
            reader.Close();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);

            c.TriggerEvent("moneyhud", c.GetData("money_cash"));
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
            c.SetData("money_cash", value);

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = new MySqlCommand("UPDATE characters SET cash = @cash WHERE account_id = @id", conn);
            cmd.Parameters.AddWithValue("@cash", value);
            cmd.Parameters.AddWithValue("@id", c.GetData("account_id"));
            cmd.ExecuteNonQuery();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }
        #endregion

        #region GetCash
        /// <summary>
        /// Returns current cash for a client.
        /// </summary>
        /// <param name="c">Client</param>
        /// <returns>Current cash</returns>
        public static int GetCash(Client c)
        {
            return c.GetData("money_cash");
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
            SetCash(c, c.GetData("money_cash") + value);
            c.TriggerEvent("moneyhud", c.GetData("money_cash"));
            c.SendNotification("~g~+~w~" + value + "~g~$");
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
            SetCash(c, c.GetData("money_cash") - value);
            c.TriggerEvent("moneyhud", c.GetData("money_cash"));
            c.SendNotification("~r~-~w~" + value + "~g~$");
        }
        #endregion
    }
}
