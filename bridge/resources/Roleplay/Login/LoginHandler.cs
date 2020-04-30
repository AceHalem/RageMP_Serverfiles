using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Roleplay.Login
{
    public class LoginHandler : Script
    {
        [RemoteEvent("RegisterAccount")]
        public static void RegisterAccount(Client c, string user, string pass)
        {
            if (c.HasData("login_time") && (((long)(DateTime.Now - (c.GetData("login_time"))).TotalMilliseconds) < 1000))
            {
                c.SendNotification("Nicht so schnell!");
                return;
            }

            if (user.Length < 3)
            {
                c.SendNotification("[~r~Fehler~w~]: Benutzername ist zu kurz! min. 3 Zeichen!");
                return;
            }

            if (pass.Length < 3)
            {
                c.SendNotification("[~r~Fehler~w~]: Passwort ist zu kurz! min. 3 Zeichen!");
                return;
            }

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = new MySqlCommand("SELECT name FROM accounts_socialclub WHERE name = @sc", conn);
            cmd.Parameters.AddWithValue("@sc", c.SocialClubName);
            MySqlDataReader reader = cmd.ExecuteReader();
            bool scCheck = reader.Read();
            reader.Close();
            if (scCheck)
            {
                c.SendNotification("~r~Dieser Social-Club-Account besitzt bereits einen Account. Bitte logge dich mit diesem ein.");
                DatabaseAPI.API.GetInstance().FreeConnection(conn);
                return;
            }

            cmd = new MySqlCommand("SELECT account_id FROM accounts_serial WHERE serial = @serial", conn);
            cmd.Parameters.AddWithValue("@serial", c.Serial);
            reader = cmd.ExecuteReader();
            bool hwCheck = reader.Read();
            reader.Close();
            if (hwCheck)
            {
                c.SendNotification("~r~Dieser Computer besitzt bereits einen Account. Bitte logge dich mit diesem ein.");
                DatabaseAPI.API.GetInstance().FreeConnection(conn);
                return;
            }

            string salt = Password.CreateSalt();
            string hash = Password.CreateHash(pass, salt, Password.PBKDF2_ITERATIONS);

            cmd = new MySqlCommand("INSERT INTO accounts (name, password_hash, password_salt, password_iterations, rank) VALUES (@user, @hash, @salt, @it, @rank)", conn);
            cmd.Parameters.AddWithValue("@user", user);
            cmd.Parameters.AddWithValue("@hash", hash);
            cmd.Parameters.AddWithValue("@salt", salt);
            cmd.Parameters.AddWithValue("@it", Password.PBKDF2_ITERATIONS);
            cmd.Parameters.AddWithValue("@rank", 0);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    c.SendNotification("~r~Name ist bereits vergeben!");
                }

                DatabaseAPI.API.GetInstance().FreeConnection(conn);
                return;
            }

            cmd = new MySqlCommand("SELECT LAST_INSERT_ID() AS id", conn);

            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int id = reader.GetInt32("id");
                int admin = 0;
                reader.Close();
                c.SendNotification("~g~Erfolgreich registriert!");
                Login(conn, c, id, admin);
            }
            else
            {
                reader.Close();
            }


            cmd = new MySqlCommand("INSERT INTO accounts_socialclub (account_id, name) VALUES (@a_id, @sc)", conn);
            cmd.Parameters.AddWithValue("@a_id", c.GetData("account_id"));
            cmd.Parameters.AddWithValue("@sc", c.SocialClubName);
            cmd.ExecuteNonQuery();

            cmd = new MySqlCommand("INSERT INTO accounts_serial (account_id, serial) VALUES (@a_id, @serial)", conn);
            cmd.Parameters.AddWithValue("@a_id", c.GetData("account_id"));
            cmd.Parameters.AddWithValue("@serial", c.Serial);
            cmd.ExecuteNonQuery();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }

        [RemoteEvent("LoginAccount")]
        public static void LoginAccount(Client c, string user, string pass)
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            if (c.HasData("login_time") && (((long)(DateTime.Now - c.GetData("login_time")).TotalMilliseconds) < 1000))
            {
                c.SendNotification("Nicht so schnell!");
                return;
            }

            if (c.HasData("account_id"))
            {
                c.SendNotification("Du bist bereits eingeloggt!");
                return;
            }


            c.SetData("login_time", DateTime.Now);


            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT socialclub FROM bans_socialclub WHERE socialclub = @sc";
            cmd.Parameters.AddWithValue("@sc", c.SocialClubName);
            MySqlDataReader read = cmd.ExecuteReader();
            bool scCheck = read.Read();
            read.Close();
            if (scCheck)
            {
                c.SendNotification("~r~Dein SocialClub wurde von unserem Server gebannt!");
                c.Kick();
                DatabaseAPI.API.GetInstance().FreeConnection(conn);
                return;
            }
            read.Close();

            cmd.CommandText = "SELECT id, password_hash, password_salt, password_iterations, rank, creation FROM accounts WHERE name = @user";
            cmd.Parameters.AddWithValue("@user", user);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (Password.Compare(pass, reader.GetString("password_hash"), reader.GetString("password_salt"), reader.GetInt32("password_iterations")))
                {
                    int id = reader.GetInt32("id");
                    int admin = reader.GetInt32("rank");
                    c.SetData("registriertseitdem", reader.GetString("creation"));
                    reader.Close();
                    Login(conn, c, id, admin);
                }
                else
                {
                    reader.Close();
                    Console.WriteLine("Passwort falsch");
                    c.SendNotification("~r~Passwort falsch");
                }

            }
            else
            {
                reader.Close();
                Console.WriteLine("Benutzername nicht gefunden");
                c.SendNotification("~r~Benutzername nicht gefunden");
            }

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }

        public struct CharacterObj
        {
            public int id;
            public string firstName;
            public string lastName;
        }

        public static void Login(MySqlConnection conn, Client c, int id, int admin)
        {
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, first_name, last_name FROM characters WHERE account_id = @a_id";
            cmd.Parameters.AddWithValue("@a_id", id);
            MySqlDataReader reader = cmd.ExecuteReader();

            //List< Tuple <int, string, string>> chars = new List <Tuple<int, string, string>> { };
            List<CharacterObj> chars = new List<CharacterObj> { };
            while (reader.Read())
            {
                CharacterObj characterObj = new CharacterObj();
                characterObj.id = reader.GetInt32("id");
                characterObj.firstName = reader.GetString("first_name");
                characterObj.lastName = reader.GetString("last_name");
                chars.Add(characterObj);
            }

            c.SetData("account_id", id);
            c.SetData("admin", admin);

            c.SendNotification("~g~Erfolgreich eingeloggt!");
            c.TriggerEvent("LoginSuccess", chars);
            reader.Close();
        }

        [RemoteEvent("testObj")]
        public static void LogintestObj2(Client c, string s)
        {
            List<CharacterObj> characterOb = JsonConvert.DeserializeObject<List<CharacterObj>>(s);
            Console.WriteLine(JsonConvert.SerializeObject(characterOb, Formatting.Indented));
        }
    }
}
