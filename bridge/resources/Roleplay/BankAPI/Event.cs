using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Roleplay.BankAPI
{
    class Event : Script
    {
        public static Vector3[] bankPos = new Vector3[] {
        new Vector3(155,6642,31), new Vector3(132,6366,31), new Vector3(-282,6225,31), new Vector3(-386,6045,31), new Vector3(1701,6426,32), //
	    new Vector3(1735,6410,35), new Vector3(1703,4933,42), new Vector3(1686,4816,42), new Vector3(1822,3683,34), new Vector3(1968,3743,32), //
	    new Vector3(-258,-723,33), new Vector3(-256,-715,33), new Vector3(-254,692,33), new Vector3(-28,-723,44), new Vector3(1078,-776,57), //
	    new Vector3(1138,-468,66), new Vector3(1166,-456,66), new Vector3(1153,-326,69), new Vector3(285,143,104), new Vector3(89,2,67), //
	    new Vector3(-56,-1752,29), new Vector3(33,-1348,29), new Vector3(288,-1282,29), new Vector3(289,-1256,29), new Vector3(146,-1035,29), //
	    new Vector3(199,-883,31), new Vector3(112,-775,31), new Vector3(112,-819,31), new Vector3(296,-895,29), new Vector3(-1827,784,138), //
	    new Vector3(-1410,-99,52), new Vector3(1570,-546,34), new Vector3(2683,3286,55), new Vector3(2564,2584,38), new Vector3(1171,2702,38), //
	    new Vector3(-1091,2708,18), new Vector3(-1827,784,138), new Vector3(-1410,-99,52), new Vector3(-1540,-546,34), new Vector3(-254,-692,33), //
	    new Vector3(-302,-829,32), new Vector3(-526,-1222,18), new Vector3(-537,-854,29), new Vector3(-613,-704,31), new Vector3(-660,-854,24),
        new Vector3(-711,-818,23), new Vector3(-717,-915,19)
        };

        [RemoteEvent("StartATM")]
        public void StartEnterBankPin(Client c)
        {
            for (int i = 0, max = bankPos.Length; i < max; i++)
            {
                if (c.Position.DistanceTo2D(bankPos[i]) < 3)
                {
                    c.TriggerEvent("StartEnterBankPin");
                }
            }

            if (c.Position.DistanceTo2D(new Vector3(149.9948, -1040.474, 29.37407)) < 3)
            {
                c.TriggerEvent("StartCreateBankPin");
            }
        }

        [RemoteEvent("OnPlayerBankRegister")]
        public void OnPlayerBankRegister(Client c, int pin, int pin2)
        {
            MySqlConnection cconn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand ccmd = new MySqlCommand("SELECT character_id FROM bank_accounts WHERE character_id = @cid", cconn);
            ccmd.Parameters.AddWithValue("@cid", c.GetData("character_id"));
            MySqlDataReader reader = ccmd.ExecuteReader();
            bool scCheck = reader.Read();
            reader.Close();
            if (scCheck)
            {
                c.SendNotification("[FL~g~EE~w~CA]: ~r~Du besitzt bei uns bereits ein Konto.");
                DatabaseAPI.API.GetInstance().FreeConnection(cconn);
                return;
            }

            DatabaseAPI.API.GetInstance().FreeConnection(cconn);

            if (pin < 4)
            {
                c.SendNotification("[FL~g~EE~w~CA]: ~r~Mind. 4 Zahlen benötigt.");
                return;
            }

            if (pin != pin2)
            {
                c.SendNotification("[FL~g~EE~w~CA]: ~r~Die PIN's stimmen nicht überein!");
                return;
            }

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO bank_accounts (character_id, pin, money)VALUES(@cid, @pin, @money)", conn);
            cmd.Parameters.AddWithValue("@cid", c.GetData("character_id"));
            cmd.Parameters.AddWithValue("@pin", pin);
            cmd.Parameters.AddWithValue("@money", 0);
            cmd.ExecuteNonQuery();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);

            c.SendNotification("[FL~g~EE~w~CA]: ~g~Konto erfolgreich erstellt!");
            c.SendNotification("[FL~g~EE~w~CA]: Neuer Pin lautet " + pin);
            c.SetData("bankpin", pin);
        }

        [RemoteEvent("OnPlayerBankLogin")]
        public void OnPlayerBankLogin(Client c, int pin)
        {
            if (!c.HasData("bankpin"))
            {
                c.SendNotification("[FL~g~EE~w~CA]: ~r~Du besitzt bei uns noch kein Konto.");
                return;
            }

            if (pin < 4)
            {
                c.SendNotification("[FL~g~EE~w~CA]: Min. 4 Zahlen.");
                return;
            }

            if (pin == c.GetData("bankpin"))
            {
                c.SendNotification("[FL~g~EE~w~CA]: ~g~Erfolgreich eingeloggt.");
                c.TriggerEvent("ShowBankTerminal");
                c.TriggerEvent("bankterminalmoney", c.GetData("money_bank"));
            } else
            {
                c.SendNotification("[FL~g~EE~w~CA]: ~r~Falscher PIN.");
                return;
            }
        }

        [RemoteEvent("OnPlayerBankEinzahlen")]
        public void OnPlayerBankEinzahlen(Client c, int summe)
        {
            if (c.GetData("money_cash") < summe)
            {
                c.SendNotification("[FL~g~EE~w~CA]: ~r~Dieses Geld besitzt du nicht!");
                return;
            }

            MoneyAPI.API.SubCash(c, summe);
            BankAPI.API.AddCash(c, summe);
        }

        [RemoteEvent("OnPlayerBankAuszahlen")]
        public void OnPlayerBankAuszahlen(Client c, int summe)
        {
            if (c.GetData("money_bank") < summe)
            {
                c.SendNotification("[FL~g~EE~w~CA]: ~r~Dieses Geld besitzt du nicht!");
                return;
            }

            MoneyAPI.API.AddCash(c, summe);
            BankAPI.API.SubCash(c, summe);
        }

        [RemoteEvent("OnPlayerBankUeberweisen")]
        public void OnPlayerBankUeberweisen(Client c, string name, int summe)
        {
            if (c.GetData("money_bank") < summe)
            {
                c.SendNotification("[FL~g~EE~w~CA]: ~r~Dieses Geld besitzt du nicht auf der Bank!");
                return;
            }

            Client p = NAPI.Pools.GetAllPlayers().Find(x => x.Name == name);

            if (p == null)
            {
                c.SendNotification("[FL~g~EE~w~CA]: ~r~Dieser Spieler ist nicht Online!");
                return;
            }

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM bank_accounts WHERE character_id = @charid", conn);
            cmd.Parameters.AddWithValue("@charid", p.GetData("character_id"));
            MySqlDataReader read = cmd.ExecuteReader();
            bool scCheck = read.Read();
            if (scCheck)
            {
                BankAPI.API.SubCash(c, summe);
                BankAPI.API.AddCash(p, summe);
                p.SendNotification("[FL~g~EE~w~CA]: " + c.Name + " hat dir soeben Geld überwiesen!");
            }
            else
            {
                c.SendNotification("[FL~g~EE~w~CA]: " + p.Name + " besitzt kein Konto!");
            }
            read.Close();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }
    }
}
