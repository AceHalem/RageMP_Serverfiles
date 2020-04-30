using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roleplay.Fraktionssystem
{
    class API : Script
    {
        public static readonly string[] Frakranknames = new string[] {
            "Bürger",
            "LSPD",
            "LSMS",
            "FIB",
            "LSSD"
        };

        public static bool HasLeaderRank(Client c)
        {
            if (!c.HasData("account_id"))
                return false;

            if (!c.HasData("character_id"))
                return false;

            if (c.GetData("fraktionrank") < 5)
            {
                c.SendNotification("Du bist dazu nicht befugt!");
                return false;
            }

            return true;
        }

        public static void fInvite(Client c, Client p)
        {
            if (!p.HasData("account_id") || !c.HasData("account_id"))
                return;

            if (!p.HasData("character_id") || !c.HasData("character_id"))
                return;

            if (c.GetData("fraktionrank") < 5)
            {
                c.SendNotification("Du bist dazu nicht befugt!");
                return;
            }

            if (p.GetData("fraktion") != 0)
            {
                c.SendNotification("Spieler ist bereits in einer Fraktion!");
                return;
            }

            if (c.HasData("finvite"))
            {
                c.SendNotification("Spieler hat bereits eine Einladung!");
                return;
            }

            p.SetData("finvite", c);

            c.SendNotification($"~g~Spieler ~w~{p.Name}~g~ wurde in die Fraktion eingeladen!");
            p.SendNotification($"~g~Du wurdest in die Fraktion " + Frakranknames[(c.GetData("fraktion") > Frakranknames.Length) ? 0 : c.GetData("fraktion")] + " eingeladen!");
            p.SendNotification("Nutze '~g~/accept~w~' um die Einladung anzunehmen.");

            NAPI.Task.Run(() =>
            {
                if (p.HasData("finvite"))
                {
                    c.SendNotification("Spieler hat die Einladung ~r~NICHT~w~ angenommen!");
                    p.SendNotification("Du hast die Einladung ~r~NICHT~w~ angenommen!");
                    p.ResetData("finvite");
                    return;
                }
            }, delayTime: 10000);
        }

        public static void fUninvite(Client c, Client p)
        {
            if (!p.HasData("account_id") || !c.HasData("account_id") || !p.HasData("character_id") || !c.HasData("character_id"))
                return;

            if (c.GetData("fraktion") != p.GetData("fraktion")) {
                c.SendNotification("Ihr befindet euch nicht in der selben Fraktion!");
                return;
            }

            if (c.GetData("fraktionrank") < 5)
            {
                c.SendNotification("Du bist dazu nicht befugt!");
                return;
            }

            p.SetData("fraktion", 0);
            p.SetData("fraktionrank", 0);

            if (p.HasData("fc") || p.HasData("sc"))
            {
                p.ResetData("fc");
                p.ResetData("sc");
            }

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "UPDATE characters SET fraktion = @frak WHERE id = @pid";
            cmd.Parameters.AddWithValue("@pid", p.GetData("character_id"));
            cmd.Parameters.AddWithValue("@frak", 0);
            cmd.ExecuteNonQuery();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }

        public static void fSetRank(Client c, Client p, int rank)
        {
            if (!p.HasData("account_id") || !c.HasData("account_id") || !p.HasData("character_id") || !c.HasData("character_id"))
                return;

            if (c.GetData("fraktion") != p.GetData("fraktion"))
            {
                c.SendNotification("Ihr befindet euch nicht in der selben Fraktion!");
                return;
            }

            if (c.GetData("fraktionrank") < 5)
            {
                c.SendNotification("Du bist dazu nicht befugt!");
                return;
            }

            if (rank >= 5)
            {
                c.SendNotification("Du bist dazu nicht befugt!");
                return;
            }

            if (rank < p.GetData("fraktionrank"))
            {
                p.SendNotification("Der Leader hat dich degradiert! neuer Rank: ~g~" +  rank);
                c.SendNotification($"Spieler ~g~{p.Name}~w~ wurde auf Rank ~g~{rank}~w~ gesetzt");
            }

            if (rank > p.GetData("fraktionrank"))
            {
                p.SendNotification("Der Leader hat dich befördert! neuer Rank: ~g~" + rank);
                c.SendNotification($"Spieler ~g~{p.Name}~w~ wurde auf Rank ~g~{rank}~w~ gesetzt");
            }

            p.SetData("fraktionrank", rank);

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "UPDATE characters SET fraktionrank = @rank WHERE id = @pid";
            cmd.Parameters.AddWithValue("@pid", p.GetData("character_id"));
            cmd.Parameters.AddWithValue("@rank", rank);
            cmd.ExecuteNonQuery();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }

        public static bool WhichFrak(Client c, int rank)
        {
            if (!c.HasData("account_id") || !c.HasData("character_id"))
                return false;

            int prank = c.GetData("fraktion");
            if (prank != rank)
                return false;

            return true;
        }

        public static bool WhichfRank(Client c, int rank)
        {
            if (!c.HasData("account_id") || !c.HasData("character_id"))
                return false;

            int prank = c.GetData("fraktionrank");
            if (prank != rank)
            {
                c.SendNotification("Du kannst diesen Befehl nicht ausführen!");
                return false;
            }

            return true;
        }

        #region Duty/OffDuty

        #region LSPD
        [RemoteEvent("OpenLSPDGate")]
        public void LSPDGateOpen(Client c)
        {
            if (c.GetData("fraktion") == 1 || c.GetData("fraktion") == 3)
            {
                if (c.Position.DistanceTo2D(new Vector3(411.67, -1023.172, 28.4064846)) < 7)
                {
                    if (Init.Init.LSPDGateLock == 1)
                    {
                        c.SendNotification("Das Tor ist bereits offen!");
                        return;
                    }

                    foreach (Client target in NAPI.Pools.GetAllPlayers())
                    {
                        target.TriggerEvent("LSPDGateOpen");
                        if (Init.Init.LSPDGateLock == 0)
                        {
                            Init.Init.LSPDGateLock = 1;
                            c.SendNotification("In 5 Sekunden schließt sich das Tor wieder!");
                        }
                        NAPI.Task.Run(() =>
                        {
                            target.TriggerEvent("LSPDGateClose");
                            Init.Init.LSPDGateLock = 0;
                        }, 5000);
                    }
                }
            }
        }

        [RemoteEvent("OpenLSPDDoor")]
        public void LSPDDoorOpen(Client c)
        {
            if (c.GetData("fraktion") == 1 || c.GetData("fraktion") == 3)
            {
                if (c.Position.DistanceTo2D(new Vector3(453.0793, -983.1895, 30.83926)) < 5)
                {
                    if (Init.Init.LSPDDoorLock == 1)
                    {
                        c.SendNotification("Das Tor ist bereits offen!");
                        return;
                    }

                    foreach (Client target in NAPI.Pools.GetAllPlayers())
                    {
                        target.TriggerEvent("LSPDWeaponDoorOpen");
                        if (Init.Init.LSPDDoorLock == 0)
                        {
                            Init.Init.LSPDDoorLock = 1;
                            c.SendNotification("In 5 Sekunden schließt sich die Tür wieder!");
                        }
                        NAPI.Task.Run(() =>
                        {
                            target.TriggerEvent("LSPDWeaponDoorClose");
                            Init.Init.LSPDDoorLock = 0;
                        }, 5000);
                    }
                }
            }
        }

        public static void LSPDDuty(Client c)
        {
            if (!c.HasData("character_id"))
                return;

            if (c.HasData("onduty"))
            {
                OffDuty(c);
                return;
            }


            
            c.RemoveAllWeapons();
                NAPI.Player.SetPlayerHealth(c, 100);
                NAPI.Player.SetPlayerArmor(c, 100);

            if (c.GetData("isMale") == true)
            {
                if (Fraktionssystem.API.WhichfRank(c, 0))
                {
                    c.SetClothes(1, 0, 0);
                    c.SetClothes(3, 0, 0);
                    c.SetClothes(4, 35, 0);
                    c.SetClothes(5, 0, 0);
                    c.SetClothes(6, 10, 0);
                    c.SetClothes(7, 0, 0);
                    c.SetClothes(8, 122, 0);
                    c.SetClothes(9, 0, 0);
                    c.SetClothes(10, 0, 0);
                    c.SetClothes(11, 55, 0);

                    
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.PumpShotgun, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.PumpShotgun, 36);
                }
                else if (Fraktionssystem.API.WhichfRank(c, 1))
                {
                    c.SetClothes(1, 0, 0);
                    c.SetClothes(3, 0, 0);
                    c.SetClothes(4, 35, 0);
                    c.SetClothes(5, 0, 0);
                    c.SetClothes(6, 10, 0);
                    c.SetClothes(7, 0, 0);
                    c.SetClothes(8, 122, 0);
                    c.SetClothes(9, 0, 0);
                    c.SetClothes(10, 0, 0);
                    c.SetClothes(11, 55, 0);

                    
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.PumpShotgun, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.PumpShotgun, 36);
                }
                else if (Fraktionssystem.API.WhichfRank(c, 2))
                {
                    c.SetClothes(1, 0, 0);
                    c.SetClothes(3, 0, 0);
                    c.SetClothes(4, 35, 0);
                    c.SetClothes(5, 0, 0);
                    c.SetClothes(6, 10, 0);
                    c.SetClothes(7, 0, 0);
                    c.SetClothes(8, 122, 0);
                    c.SetClothes(9, 0, 0);
                    c.SetClothes(10, 0, 0);
                    c.SetClothes(11, 55, 0);

                    
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.PumpShotgun, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.PumpShotgun, 36);
                }
                else if (Fraktionssystem.API.WhichfRank(c, 3))
                {
                    c.SetClothes(1, 0, 0);
                    c.SetClothes(3, 0, 0);
                    c.SetClothes(4, 35, 0);
                    c.SetClothes(5, 0, 0);
                    c.SetClothes(6, 10, 0);
                    c.SetClothes(7, 0, 0);
                    c.SetClothes(8, 122, 0);
                    c.SetClothes(9, 0, 0);
                    c.SetClothes(10, 0, 0);
                    c.SetClothes(11, 55, 0);

                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.PumpShotgun, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.PumpShotgun, 56);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.SMG, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.SMG, 60);
                }
                else if (Fraktionssystem.API.WhichfRank(c, 4))
                {
                    c.SetClothes(1, 0, 0);
                    c.SetClothes(3, 0, 0);
                    c.SetClothes(4, 35, 0);
                    c.SetClothes(5, 0, 0);
                    c.SetClothes(6, 10, 0);
                    c.SetClothes(7, 0, 0);
                    c.SetClothes(8, 122, 0);
                    c.SetClothes(9, 0, 0);
                    c.SetClothes(10, 0, 0);
                    c.SetClothes(11, 55, 0);

                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.PumpShotgun, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.PumpShotgun, 56);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.SMG, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.SMG, 120);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.CarbineRifle, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.CarbineRifle, 120);
                }
                else if (Fraktionssystem.API.WhichfRank(c, 5))
                {
                    c.SetClothes(1, 0, 0);
                    c.SetClothes(3, 0, 0);
                    c.SetClothes(4, 35, 0);
                    c.SetClothes(5, 0, 0);
                    c.SetClothes(6, 10, 0);
                    c.SetClothes(7, 0, 0);
                    c.SetClothes(8, 122, 0);
                    c.SetClothes(9, 0, 0);
                    c.SetClothes(10, 0, 0);
                    c.SetClothes(11, 13, 0);

                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.PumpShotgun, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.PumpShotgun, 56);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.SMG, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.SMG, 210);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.CarbineRifle, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.CarbineRifle, 150);
                }
            } else
            {
                c.SetClothes(8, 35, 0);
                c.SetClothes(11, 48, 0);
                c.SetClothes(4, 34, 0);
                c.SetClothes(6, 25, 0);
                c.SetClothes(7, 0, 0);
                c.SetClothes(3, 0, 0);
            }


            c.SendNotification("Du bist nun im Dienst!");
            c.SetData("onduty", 1);
        }
        #endregion

        #region FIB
        public static void FIBDuty(Client c)
        {
            if (!c.HasData("character_id"))
                return;

            if (c.HasData("onduty"))
            {
                OffDuty(c);
                return;
            }

            if (c.GetData("isMale") == true)
            {
                c.SetClothes(11, 26, 4);
                c.SetClothes(7, 0, 0);
                c.SetClothes(4, 25, 0);
                c.SetClothes(3, 11, 0);
                c.SetClothes(6, 10, 0);
                c.SetClothes(8, 122, 0);
                c.SetClothes(9, 54, 0);
            }
            else
            {
                c.SetClothes(11, 26, 1);
                c.SetClothes(7, 125, 0);
                c.SetClothes(4, 126, 0);
                c.SetClothes(3, 11, 0);
                c.SetClothes(6, 10, 0);
            }


            c.RemoveAllWeapons();
            NAPI.Player.SetPlayerArmor(c, 100);
            NAPI.Player.SetPlayerHealth(c, 100);

            NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);

            NAPI.Player.GivePlayerWeapon(c, WeaponHash.Pistol50, 0);
            NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Pistol50, 150);

            NAPI.Player.GivePlayerWeapon(c, WeaponHash.SMG, 0);
            NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.SMG, 120);


            c.SendNotification("Nun tretest du den Dienst an!");
            c.SetData("onduty", 1);
        }
        #endregion

        #region LSSD
        public static void LSSDDuty(Client c)
        {
            if (!c.HasData("character_id"))
                return;

            if (c.HasData("onduty"))
            {
                OffDuty(c);
                return;
            }

            c.RemoveAllWeapons();
                NAPI.Player.SetPlayerHealth(c, 100);
                NAPI.Player.SetPlayerArmor(c, 100);

            if (c.GetData("isMale") == true)
            {
                if (Fraktionssystem.API.WhichfRank(c, 0))
                {
                    c.SetClothes(1, 0, 0);
                    c.SetClothes(3, 11, 0);
                    c.SetClothes(4, 37, 1);
                    c.SetClothes(5, 0, 0);
                    c.SetClothes(6, 10, 0);
                    c.SetClothes(7, 0, 0);
                    c.SetClothes(8, 130, 0);
                    c.SetClothes(9, 0, 0);
                    c.SetClothes(10, 0, 0);
                    c.SetClothes(11, 55, 0);

                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.CombatPistol, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.CombatPistol, 100);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.PumpShotgun, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.PumpShotgun, 36);

                }
                else if (Fraktionssystem.API.WhichfRank(c, 1))
                {
                    c.SetClothes(1, 0, 0);
                    c.SetClothes(3, 11, 0);
                    c.SetClothes(4, 37, 1);
                    c.SetClothes(5, 0, 0);
                    c.SetClothes(6, 10, 0);
                    c.SetClothes(7, 0, 0);
                    c.SetClothes(8, 122, 0);
                    c.SetClothes(9, 0, 0);
                    c.SetClothes(10, 0, 0);
                    c.SetClothes(11, 55, 0);

                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.PumpShotgun, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.PumpShotgun, 36);
                }
                else if (Fraktionssystem.API.WhichfRank(c, 2))
                {
                    c.SetClothes(1, 0, 0);
                    c.SetClothes(3, 11, 0);
                    c.SetClothes(4, 37, 1);
                    c.SetClothes(5, 0, 0);
                    c.SetClothes(6, 10, 0);
                    c.SetClothes(7, 0, 0);
                    c.SetClothes(8, 58, 0);
                    c.SetClothes(9, 0, 0);
                    c.SetClothes(10, 0, 0);
                    c.SetClothes(11, 55, 0);
                    
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.PumpShotgun, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.PumpShotgun, 36);
                }
                else if (Fraktionssystem.API.WhichfRank(c, 3))
                {
                    c.SetClothes(1, 0, 0);
                    c.SetClothes(3, 11, 0);
                    c.SetClothes(4, 37, 1);
                    c.SetClothes(5, 0, 0);
                    c.SetClothes(6, 10, 0);
                    c.SetClothes(7, 0, 0);
                    c.SetClothes(8, 58, 0);
                    c.SetClothes(9, 0, 0);
                    c.SetClothes(10, 0, 0);
                    c.SetClothes(11, 13, 2);
                    
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.PumpShotgun, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.PumpShotgun, 56);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.SMG, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.SMG, 60);
                }
                else if (Fraktionssystem.API.WhichfRank(c, 4))
                {
                    c.SetClothes(1, 0, 0);
                    c.SetClothes(3, 11, 0);
                    c.SetClothes(4, 37, 1);
                    c.SetClothes(5, 0, 0);
                    c.SetClothes(6, 10, 0);
                    c.SetClothes(7, 38, 0);
                    c.SetClothes(8, 122, 0);
                    c.SetClothes(9, 0, 0);
                    c.SetClothes(10, 0, 0);
                    c.SetClothes(11, 13, 1);
                    
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.PumpShotgun, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.PumpShotgun, 72);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.SMG, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.SMG, 180);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.CarbineRifle, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.CarbineRifle, 120);
                }
                else if (Fraktionssystem.API.WhichfRank(c, 5))
                {
                    c.SetClothes(1, 0, 0);
                    c.SetClothes(3, 11, 0);
                    c.SetClothes(4, 37, 1);
                    c.SetClothes(5, 0, 0);
                    c.SetClothes(6, 10, 0);
                    c.SetClothes(7, 38, 0);
                    c.SetClothes(8, 122, 0);
                    c.SetClothes(9, 0, 0);
                    c.SetClothes(10, 0, 0);
                    c.SetClothes(11, 13, 0);
                    
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.PumpShotgun, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.PumpShotgun, 72);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.SMG, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.SMG, 210);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.CarbineRifle, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.CarbineRifle, 150);
                }
            }
            else
            {
                if (Fraktionssystem.API.WhichfRank(c, 0))
                {
                    c.SetClothes(1, 0, 0);
                    c.SetClothes(3, 11, 0);
                    c.SetClothes(4, 37, 1);
                    c.SetClothes(5, 0, 0);
                    c.SetClothes(6, 10, 0);
                    c.SetClothes(7, 0, 0);
                    c.SetClothes(8, 130, 0);
                    c.SetClothes(9, 0, 0);
                    c.SetClothes(10, 0, 0);
                    c.SetClothes(11, 55, 0);

                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.CombatPistol, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.CombatPistol, 100);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.PumpShotgun, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.PumpShotgun, 36);

                }
                else if (Fraktionssystem.API.WhichfRank(c, 1))
                {
                    c.SetClothes(1, 0, 0);
                    c.SetClothes(3, 11, 0);
                    c.SetClothes(4, 37, 1);
                    c.SetClothes(5, 0, 0);
                    c.SetClothes(6, 10, 0);
                    c.SetClothes(7, 0, 0);
                    c.SetClothes(8, 122, 0);
                    c.SetClothes(9, 0, 0);
                    c.SetClothes(10, 0, 0);
                    c.SetClothes(11, 55, 0);

                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.PumpShotgun, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.PumpShotgun, 36);
                }
                else if (Fraktionssystem.API.WhichfRank(c, 2))
                {
                    c.SetClothes(1, 0, 0);
                    c.SetClothes(3, 11, 0);
                    c.SetClothes(4, 37, 1);
                    c.SetClothes(5, 0, 0);
                    c.SetClothes(6, 10, 0);
                    c.SetClothes(7, 0, 0);
                    c.SetClothes(8, 58, 0);
                    c.SetClothes(9, 0, 0);
                    c.SetClothes(10, 0, 0);
                    c.SetClothes(11, 55, 0);
                    
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.PumpShotgun, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.PumpShotgun, 36);
                }
                else if (Fraktionssystem.API.WhichfRank(c, 3))
                {
                    c.SetClothes(1, 0, 0);
                    c.SetClothes(3, 11, 0);
                    c.SetClothes(4, 37, 1);
                    c.SetClothes(5, 0, 0);
                    c.SetClothes(6, 10, 0);
                    c.SetClothes(7, 0, 0);
                    c.SetClothes(8, 58, 0);
                    c.SetClothes(9, 0, 0);
                    c.SetClothes(10, 0, 0);
                    c.SetClothes(11, 13, 2);
                    
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.PumpShotgun, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.PumpShotgun, 56);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.SMG, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.SMG, 60);
                }
                else if (Fraktionssystem.API.WhichfRank(c, 4))
                {
                    c.SetClothes(1, 0, 0);
                    c.SetClothes(3, 11, 0);
                    c.SetClothes(4, 37, 1);
                    c.SetClothes(5, 0, 0);
                    c.SetClothes(6, 10, 0);
                    c.SetClothes(7, 38, 0);
                    c.SetClothes(8, 122, 0);
                    c.SetClothes(9, 0, 0);
                    c.SetClothes(10, 0, 0);
                    c.SetClothes(11, 13, 1);
                    
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.PumpShotgun, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.PumpShotgun, 72);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.SMG, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.SMG, 180);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.CarbineRifle, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.CarbineRifle, 120);
                }
                else if (Fraktionssystem.API.WhichfRank(c, 5))
                {
                    c.SetClothes(1, 0, 0);
                    c.SetClothes(3, 11, 0);
                    c.SetClothes(4, 37, 1);
                    c.SetClothes(5, 0, 0);
                    c.SetClothes(6, 10, 0);
                    c.SetClothes(7, 38, 0);
                    c.SetClothes(8, 122, 0);
                    c.SetClothes(9, 0, 0);
                    c.SetClothes(10, 0, 0);
                    c.SetClothes(11, 13, 0);
                    
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.StunGun, 0);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Pistol50, 150);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.PumpShotgun, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.PumpShotgun, 72);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.SMG, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.SMG, 210);
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.CarbineRifle, 0);
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.CarbineRifle, 150);
                }
            }


            c.SendNotification("Nun tretest du den Dienst an!");
            c.SetData("onduty", 1);
        }
        #endregion

        #region LSMS
        public static void LSMSDuty (Client c)
        {
            if (!c.HasData("character_id"))
                return;

            if (c.HasData("onduty"))
            {
                OffDuty(c);
                return;
            }

            if (c.GetData("isMale") == true)
            {
                c.SetClothes(8, 129, 0);
                c.SetClothes(11, 250, 0);
                c.SetClothes(4, 98, 19);
                c.SetClothes(6, 25, 0);
                c.SetClothes(7, 127, 0);
                c.SetClothes(3, 85, 0);
            }
            else
            {
                c.SetClothes(8, 159, 0);
                c.SetClothes(11, 27, 0);
                c.SetClothes(4, 0, 0);
                c.SetClothes(6, 24, 0);
                c.SetClothes(7, 97, 0);
            }

            c.SendNotification("Du bist nun im Dienst!");
            c.SetData("onduty", 2);
        }
        #endregion

        public static void OffDuty(Client c)
        {

            NAPI.Player.SetPlayerClothes(c, 7, 0, 0);

            c.SetClothes(6, c.GetData("shoes"), 0);
            c.SetClothes(4, c.GetData("legs"), 0);
            c.SetClothes(11, c.GetData("tops"), 0);
            c.SetClothes(3, c.GetData("torsos"), 0);
            c.SetClothes(8, c.GetData("undershirts"), 0);
            c.SetClothes(9, 0, 0);

            c.RemoveAllWeapons();
            c.SendNotification("Du bist nun nicht mehr im Dienst!");
            c.ResetData("onduty");
        }
        #endregion
    }
}
