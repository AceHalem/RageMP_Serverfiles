using System;
using System.Collections.Generic;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System.Threading;

namespace Roleplay.AdminTools
{
    public class Commands : Script
    {
        static readonly string[] ranknames = new string[] {
            "Bürger",
            "Supporter",
            "Admin",
            "HeadAdmin",
            "Projektleiter"
        };

        [Command("gethere")]
        public void GetPlayerHere(Client c, Client p)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            p.Position = c.Position;
            p.SendNotification("Spieler ~r~" + c.Name + " hat dich zu sich teleportiert.");
            p.SendNotification("Du hast ~r~" + c.Name + " zu dich teleportiert.");
        }

        [Command("goto")]
        public void GoToPlayer(Client c, Client p)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            c.Position = p.Position;
        }

        [Command("kick")]
        public void KickPlayer(Client c, Client p, string grund)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            NAPI.Chat.SendChatMessageToAll($"[~r~{c.Name}~w~]: Spieler {p.Name} wurde wegen ~y~{grund}~w~ vom Server gekickt.");

            p.Kick();
        }

        [Command("tveh")]
        public void TestVehSpawn(Client c, string fahrzeug_model)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            uint hash = NAPI.Util.GetHashKey(fahrzeug_model);
            Vehicle veh = NAPI.Vehicle.CreateVehicle(hash, c.Position, c.Rotation.Z, 0, 0, "ADMINVEH");
            c.SetIntoVehicle(veh, -1);
            c.Vehicle.SetData("temp", 1);
        }

        [Command("ban")]
        public void BanPlayer(Client c, Client p, string grund)
        {
            if (!PermissionAPI.API.HasPermission(c, 2))
                return;

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO bans_account (account_id) VALUES (@aid)", conn);
            cmd.Parameters.AddWithValue("@aid", p.GetData("account_id"));
            cmd.ExecuteNonQuery();

            MySqlCommand cmd2 = new MySqlCommand("INSERT INTO bans_serial (serial) VALUES (@serial)", conn);
            cmd2.Parameters.AddWithValue("@serial", p.Serial);
            cmd2.ExecuteNonQuery();

            MySqlCommand cmd3 = new MySqlCommand("INSERT INTO bans_socialclub (socialclub) VALUES (@sc)", conn);
            cmd3.Parameters.AddWithValue("@sc", p.SocialClubName);
            cmd3.ExecuteNonQuery();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);

            NAPI.Chat.SendChatMessageToAll($"[~r~{c.Name}~w~]: Spieler {p.Name} wurde wegen ~y~{grund}~w~ vom Server gebannt.");

            p.Kick();
        }

        [Command("deletev")]
        public void DeleteVehicle(Client c)
        {
            if (!PermissionAPI.API.HasPermission(c, 3))
                return;

            if (!c.IsInVehicle)
            {
                c.SendNotification("Du sitzt in keinem Fahrzeug!");
                return;
            }

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM vehicles WHERE id = @vid", conn);
            cmd.Parameters.AddWithValue("@vid", c.Vehicle.GetData("id"));
            cmd.ExecuteNonQuery();
            DatabaseAPI.API.GetInstance().FreeConnection(conn);

            c.Vehicle.Delete();
        }
        
        [Command("setcloth")]
        public void TestSetCloth(Client c, int slot, int drawable, int color)
        {
            NAPI.Player.SetPlayerClothes(c, slot, drawable, color);
        }

        [Command("createfv")]
        public void CreateFrakVehicle(Client c, string fraktion, string vehicle)
        {
            if (!PermissionAPI.API.HasPermission(c, 3))
                return;

            Vehicles.Vehicles.Createfv(c, fraktion, vehicle);
        }

        [Command("setleader")]
        public void SetLeader(Client c, Client p, int Fraktion)
        {
            if (!p.HasData("character_id") || !c.HasData("character_id"))
                return;

            if (!PermissionAPI.API.HasPermission(c, 3))
                return;

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "UPDATE characters SET fraktionrank = @lead, fraktion = @frak WHERE id = @pid";
            cmd.Parameters.AddWithValue("@pid", p.GetData("character_id"));
            if (Fraktion == 0)
            {
                cmd.Parameters.AddWithValue("@lead", 0);

            } else
            {
                cmd.Parameters.AddWithValue("@lead", 5);
            }
            cmd.Parameters.AddWithValue("@frak", Fraktion);
            cmd.ExecuteNonQuery();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);

            if (Fraktion == 0)
            {
                p.SetData("fraktionrank", 0);
                p.SetData("fraktion", 0);

                c.SendNotification($"~g~Spieler ~w~{p.Name}~g~ wurde als Leader entlassen!");
                p.SendNotification($"~g~Du wurdest als Leader entlassen und bist absofort wieder ein Bürger!");

                if (p.HasData("fc") || p.HasData("sc"))
                {
                    p.ResetData("fc");
                    p.ResetData("sc");
                }
                return;
            }

            p.SetData("fraktionrank", 5);
            p.SetData("fraktion", Fraktion);

            c.SendNotification($"~g~Spieler ~w~{p.Name}~g~ wurde nun zum Leader von " + Fraktionssystem.API.Frakranknames[(Fraktion > Fraktionssystem.API.Frakranknames.Length) ? 0 : Fraktion] + " ernannt!");
            p.SendNotification($"~g~Du wurdest zum Leader von " + Fraktionssystem.API.Frakranknames[(Fraktion > Fraktionssystem.API.Frakranknames.Length) ? 0 : Fraktion] + " ernannt!");
        }

        [Command("rank")]
        public void ShowAdminRank(Client c)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            c.SendChatMessage(ranknames[(c.GetData("admin") > ranknames.Length) ? 0 : c.GetData("admin")]);

        }

        [Command("setadmin")]
        public void SetAdminRank(Client c, Client p, int rank)
        {
            if (!PermissionAPI.API.HasPermission(c, 4))
                return;

            PermissionAPI.API.SetRank(c, p, rank);

        }

        [Command("spawn")]
        public void HandleSpawn(Client c, int id)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            Vehicles.Garage.SpawnVehicles(c, id);
        }


        [Command("allveh")]
        public void HandleAllVeh(Client c)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            int i = 0;
            foreach (Vehicle v in NAPI.Pools.GetAllVehicles())
            {
                NAPI.Task.Run(() =>
                {
                    Console.WriteLine("Veh: " + v.Class + "|" + v.ClassName + "|" + v.DisplayName);
                    c.Position = v.Position;
                }, delayTime: i * 5000);
                i++;
            }

        }

        [Command("select")]
        public void HandleSelect(Client c, int id)
        {
            if (!PermissionAPI.API.HasPermission(c, 2))
                return;

            Login.Character.SelectCharacter(c, id);
        }

        [Command("1")]
        public void HandlePos(Client c, string text)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            Vector3 pos = c.Vehicle.Position;
            Vector3 rot = c.Vehicle.Rotation;

            Console.WriteLine(text);
            Console.WriteLine("Pos: " + pos.X + "| " + pos.Y + "| " + pos.Z);
            Console.WriteLine("Rotation");
            Console.WriteLine("Z: " + rot.Z);

            Console.WriteLine("---------------");
            c.SendChatMessage("---------------");

            c.SendChatMessage(text);
            c.SendChatMessage("Pos: " + pos.X + "| " + pos.Y + "| " + pos.Z);
            c.SendChatMessage("Rotation");
            c.SendChatMessage("Z: " + rot.Z);
        }


        [Command("p")]
        public void Pos(Client c, string text)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            Vector3 pos = c.Position;
            Vector3 rot = c.Rotation;
            Console.WriteLine(text);
            Console.WriteLine(pos.X + "| " + pos.Y + "| " + pos.Z);
            c.SendChatMessage(text);
            c.SendChatMessage(pos.X + "| " + pos.Y + "| " + pos.Z);
        }

        [Command("pos")]
        public void HandlePos(Client c)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            Vector3 pos = c.Position;
            Vector3 rot = c.Rotation;

            Console.WriteLine("Diese Positionsdaten wurden von " + c.Name + " angefordert:");
            Console.WriteLine("Position");
            Console.WriteLine("Pos: " + pos.X + "| " + pos.Y + "| " + pos.Z);
            Console.WriteLine("Rotation");
            Console.WriteLine("Z: " + rot.Z);

            Console.WriteLine("---------------");
            c.SendChatMessage("---------------");

            c.SendChatMessage("Position");
            c.SendChatMessage("Pos: " + pos.X + "| " + pos.Y + "| " + pos.Z);
            c.SendChatMessage("Rotation");
            c.SendChatMessage("Z: " + rot.Z);
        }

        [Command("posveh")]
        public void HandlePosveh(Client c)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            Vector3 pos = c.Vehicle.Position;
            Vector3 rot = c.Vehicle.Rotation;

            Console.WriteLine("Diese Positionsdaten wurden von " + c.Name + " angefordert:");
            Console.WriteLine("Position");
            Console.WriteLine("X: " + pos.X);
            Console.WriteLine("Y: " + pos.Y);
            Console.WriteLine("Z: " + pos.Z);
            Console.WriteLine("Rotation");
            Console.WriteLine("X: " + rot.X);
            Console.WriteLine("Y: " + rot.Y);
            Console.WriteLine("Z: " + rot.Z);

            Console.WriteLine("---------------");
            c.SendChatMessage("---------------");

            c.SendChatMessage("Position");
            c.SendChatMessage("X: " + pos.X);
            c.SendChatMessage("Y: " + pos.Y);
            c.SendChatMessage("Z: " + pos.Z);
            c.SendChatMessage("Rotation");
            c.SendChatMessage("X: " + rot.X);
            c.SendChatMessage("Y: " + rot.Y);
            c.SendChatMessage("Z: " + rot.Z);
        }


        [Command("vccolor")]
        public void HandleVCColor(Client c, byte pR, byte pG, byte pB, byte sR, byte sG, byte sB)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            c.Vehicle.CustomPrimaryColor = new Color(pR, pG, pB);
            c.Vehicle.CustomSecondaryColor = new Color(sR, sG, sB);
        }

        [Command("vcolor")]
        public void HandleVColor(Client c, int primaryColor, int secondaryColor)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            c.Vehicle.PrimaryColor = primaryColor;
            c.Vehicle.SecondaryColor = secondaryColor;
        }

        [Command("vtrans")]
        public void HandleVTrans(Client c, int transparency)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            c.Vehicle.Transparency = transparency;
        }

        [Command("getcolor")]
        public void HandleGetColor(Client c)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            Vehicle v = c.Vehicle;
            Color c1 = v.CustomPrimaryColor;
            Color c2 = v.CustomPrimaryColor;
            Console.WriteLine("r: " + c1.Red + "g: " + c1.Green + "b: " + c1.Blue + "a: " + c1.Alpha);
            Console.WriteLine("r2: " + c2.Red + "g2: " + c2.Green + "b2: " + c2.Blue + "a2: " + c2.Alpha);
        }

        [Command("getcolor2")]
        public void HandleGetColor2(Client c)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            Color cpCol = c.Vehicle.CustomPrimaryColor;
            Color csCol = c.Vehicle.CustomSecondaryColor;
            c.SendChatMessage("Normal | 1st color: " + c.Vehicle.PrimaryColor + " | 2nd color: " + c.Vehicle.SecondaryColor);
            c.SendChatMessage("Custom | R: " + cpCol.Red + " G: " + cpCol.Green + " B: " + cpCol.Blue + " | R: " + csCol.Red + " G: " + csCol.Green + " B: " + csCol.Blue);
        }


        [Command("serial")]
        public void HandleSerial(Client c)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            Console.WriteLine("Player Serial: |" + c.Serial + "|");
        }

        [Command("ped")]
        public void HandlePad(Client c)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            c.SetSkin(PedHash.Zombie01);
        }

        [Command("creator")]
        public void HandleCreator(Client c)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            c.ResetData("character_id");
            c.ResetData("jailtime");
            c.ResetData("PlayerPaydayTimer");
            c.ResetData("createdc");
            c.ResetData("vehicles");
            c.ResetData("fraktion");
            c.ResetData("fraktionrank");
            c.ResetData("h_key");
            c.ResetData("dimension");
            c.ResetData("houseid");
            c.ResetData("money_bank");
            c.ResetData("money_cash");
            c.ResetData("wanteds");
            c.ResetData("führerschein");
            c.ResetData("autoscheinpunkte");

            NAPI.Player.SetPlayerDefaultClothes(c);

            Login.Character.CharacterCreator(c);
        }

        [Command("veh", GreedyArg = true)]
        public void HandleVeh(Client c, string vehName)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            Vehicles.Vehicles.Create(c, vehName);
        }

        [Command("god")]
        public void HandleGod(Client c)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            bool state = false;
            if (c.HasData("godmode"))
                state = c.GetData("godmode");

            c.SetData("godmode", !state);
            c.Invincible = !state;

            if (!state)
            {
                c.SendChatMessage("aktiviert");
            }
            else
            {
                c.SendChatMessage("deaktiviert");
            }
        }

        [Command("epm")]
        public void HandleEpm(Client c, int amount)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            c.TriggerEvent("setEpm", amount);
            c.SendChatMessage("Engine Power Mult: " + amount);
        }

        /*[Command("etm")]
        public void HandleEtm(Client c, int amount)
        {
            c.Vehicle.SetSharedData("etm", amount);
            c.SendChatMessage("Engine Torque Mult: " + amount);
        }*/

        [Command("dim")]
        public void HandleTp(Client c, uint d)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            c.Dimension =  d;
        }

        [Command("tp")]
        public void HandleTp(Client c, double x, double y, double z)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            c.Position = new Vector3(x, y, z);
        }

        [Command("tpp", GreedyArg = true)]
        public void HandleTpp(Client c, string targetName)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            Client target = NAPI.Player.GetPlayerFromName(targetName);
            c.SendChatMessage("Dein Name: " + c.Name);
            if (target.IsNull)
            {
                c.SendChatMessage("Spieler nicht gefunden");
            }
            else
            {
                c.Position = target.Position;
            }
        }

        [Command("tpv")]
        public void HandleTpv(Client c, int id)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            foreach (Vehicle v in NAPI.Pools.GetAllVehicles())
            {
                if (v.HasData("id") && v.GetData("id") == id)
                {
                    Vehicles.Vehicles.MoveIn(c, v);
                }

            }
        }

        [Command("tpvv")]
        public void HandleTpvv(Client c, int id)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            foreach (Vehicle v in NAPI.Pools.GetAllVehicles())
            {
                if (v.HasData("id") && v.GetData("id") == id)
                {
                    v.Position = c.Position;
                    Vehicles.Vehicles.MoveIn(c, v);
                }
            }
        }

        [Command("weapon")]
        public void WeaponCommand(Client c, WeaponHash hash)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            NAPI.Player.GivePlayerWeapon(c, hash, 500);
        }

        [Command("upthash")]
        public void HandleUptHash(Client c, string vehName)
        {
            if (!PermissionAPI.API.HasPermission(c, 4))
                return;

            uint hash = NAPI.Util.GetHashKey(vehName);

            MySqlConnection connWrite = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmdWrite = new MySqlCommand("INSERT INTO cfg_vehicles (name, hash) VALUES (@name, @hash)", connWrite);
            cmdWrite.Parameters.AddWithValue("@hash", hash);
            cmdWrite.Parameters.AddWithValue("@name", vehName);
            cmdWrite.ExecuteNonQuery();

            DatabaseAPI.API.GetInstance().FreeConnection(connWrite);
        }

        [Command("hash")]
        public void HandleHash(Client c, string vehName)
        {
            if (!PermissionAPI.API.HasPermission(c, 4))
                return;

            uint hash = NAPI.Util.GetHashKey(vehName);
            c.SendChatMessage(vehName + " hash: " + hash);
        }
    }
}
