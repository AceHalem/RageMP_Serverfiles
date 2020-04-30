using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roleplay.Fraktionssystem
{
    class Commands : Script
    {
        #region Leader
        [Command("invite")]
        public void fInvite(Client c, Client p)
        {
            if (!Fraktionssystem.API.HasLeaderRank(c))
                return;

            Fraktionssystem.API.fInvite(c, p);
        }

        [Command("uninvite")]
        public void fUninvite(Client c, Client p)
        {
            if (!Fraktionssystem.API.HasLeaderRank(c))
                return;

            Fraktionssystem.API.fUninvite(c, p);
        }

        [Command("setrank")]
        public void fSetRank(Client c, Client p, int rank)
        {
            if (!Fraktionssystem.API.HasLeaderRank(c))
                return;

            Fraktionssystem.API.fSetRank(c, p, rank);
        }

        [Command("fpark")]
        public void FractionVehPark(Client c)
        {
            if (!c.IsInVehicle)
            {
                c.SendNotification("Du sitzt in keinem Fahrzeug!");
                return;
            }

            if (c.GetData("fraktionrank") == 5)
            {
                if (Fraktionssystem.API.Frakranknames[(c.GetData("fraktion") > Fraktionssystem.API.Frakranknames.Length) ? 0 : c.GetData("fraktion")] == c.Vehicle.GetData("fraktion"))
                {
                    Vehicle v = c.Vehicle;

                    MySqlCommand cmd = new MySqlCommand("UPDATE fvehicles SET " +
                    "p_x = @p_x, p_y = @p_y, p_z = @p_z, r = @r " +
                    "WHERE id = @id");
                    cmd.Parameters.AddWithValue("@p_x", v.Position.X);
                    cmd.Parameters.AddWithValue("@p_y", v.Position.Y);
                    cmd.Parameters.AddWithValue("@p_z", v.Position.Z);
                    cmd.Parameters.AddWithValue("@r", v.Rotation.Z);

                    cmd.Parameters.AddWithValue("@id", v.GetData("fid"));

                    DatabaseAPI.API.executeNonQuery(cmd);

                    c.SendNotification("Fahrzeug wurde erfolgreich umgeparkt!");
                } else
                {
                    c.SendNotification("Dieses Fahrzeug gehört nicht zu deiner Fraktion!");
                }
            } else
            {
                c.SendNotification("Du bist kein Leader!");
            }
        }
        #endregion

        #region LSPD & FIB & LSSD
        [Command("ticket")]
        public void OeffneNewTicket(Client c, Client p)
        {
            if (Fraktionssystem.API.WhichFrak(c, 1) || Fraktionssystem.API.WhichFrak(c, 3))
            {
                if (!c.HasData("onduty"))
                {
                    c.SendNotification("du bist nicht im Dienst!");
                    return;
                }

                if (c.Position.DistanceTo2D(p.Position) <= 5)
                {
                    NAPI.Task.Run(() => {
                        c.TriggerEvent("OeffneNewTicket");
                        c.SetData("NewTicket", p);
                    },delayTime: 300);
                } else
                {
                    c.SendNotification("Spieler nicht in der Nähe!");
                }
            }
        }
        [Command("swat")]
        public void SwatCommand(Client c)
        {
            if (Fraktionssystem.API.WhichFrak(c, 3))
            {
                
            c.RemoveAllWeapons();
                NAPI.Player.SetPlayerArmor(c, 100);
                NAPI.Player.SetPlayerHealth(c, 100);
                NAPI.Player.SetPlayerClothes(c, 1, 52, 0);
                NAPI.Player.SetPlayerClothes(c, 4, 121, 0);
                NAPI.Player.SetPlayerClothes(c, 5, 0, 0);
                NAPI.Player.SetPlayerClothes(c, 6, 81, 0);
                NAPI.Player.SetPlayerClothes(c, 7, 0, 0);
                NAPI.Player.SetPlayerClothes(c, 9, 15, 0);
                NAPI.Player.SetPlayerClothes(c, 10, 0, 0);
                NAPI.Player.SetPlayerArmor(c, 100);
                if (Fraktionssystem.API.WhichfRank(c, 0) || Fraktionssystem.API.WhichfRank(c, 1) || Fraktionssystem.API.WhichfRank(c, 2))
                {
                    //SWAT ohne Kapfgewehr (0)
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Revolver, 0); // A130
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Revolver, 130); // A130

                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.AssaultSMG, 0); // B120
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.AssaultSMG, 120); // B120
                }
                else if (Fraktionssystem.API.WhichfRank(c, 3))
                {
                    //SWAT mit Kapfgewehr (90)
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Revolver, 0); // A130
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Revolver, 130); // A130

                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.AssaultSMG, 0); // B120
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.AssaultSMG, 120); // B120


                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.AdvancedRifle, 0); // D90
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.AdvancedRifle, 90); // D90
                }
                else if (Fraktionssystem.API.WhichfRank(c, 4))
                {
                    //SWAT mit Kapfgewehr (120)
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Revolver, 0); // A130
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Revolver, 130); // A130

                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.AssaultSMG, 0); // B150
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.AssaultSMG, 150); // B150


                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.AdvancedRifle, 0); // D120
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.AdvancedRifle, 120); // D120
                }
                else if (Fraktionssystem.API.WhichfRank(c, 5))
                {
                    //SWAT mit Kapfgewehr (150)
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Revolver, 0); // A130
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Revolver, 130); // A130

                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.AssaultSMG, 0); // B240
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.AssaultSMG, 240); // B240


                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.AdvancedRifle, 0); // D150
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.AdvancedRifle, 90); // D150
                }
            }
            else if (Fraktionssystem.API.WhichFrak(c, 1))
            {
                
            c.RemoveAllWeapons();
                if (Fraktionssystem.API.WhichfRank(c, 4))
                {
                    c.RemoveAllWeapons();
                    NAPI.Player.SetPlayerArmor(c, 100);
                    NAPI.Player.SetPlayerHealth(c, 100);
                    NAPI.Player.SetPlayerClothes(c, 1, 52, 0);
                    NAPI.Player.SetPlayerClothes(c, 4, 121, 0);
                    NAPI.Player.SetPlayerClothes(c, 5, 0, 0);
                    NAPI.Player.SetPlayerClothes(c, 6, 81, 0);
                    NAPI.Player.SetPlayerClothes(c, 7, 0, 0);
                    NAPI.Player.SetPlayerClothes(c, 9, 15, 0);
                    NAPI.Player.SetPlayerClothes(c, 10, 0, 0);

                    //SWAT mit Kapfgewehr (120)
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Revolver, 0); // A130
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Revolver, 130); // A130

                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.AssaultSMG, 0); // B150
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.AssaultSMG, 150); // B150


                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.AdvancedRifle, 0); // D120
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.AdvancedRifle, 120); // D120
                }
                else if (Fraktionssystem.API.WhichfRank(c, 5))
                {
                    c.RemoveAllWeapons();
                    NAPI.Player.SetPlayerArmor(c, 100);
                    NAPI.Player.SetPlayerHealth(c, 100);
                    NAPI.Player.SetPlayerClothes(c, 1, 52, 0);
                    NAPI.Player.SetPlayerClothes(c, 4, 121, 0);
                    NAPI.Player.SetPlayerClothes(c, 5, 0, 0);
                    NAPI.Player.SetPlayerClothes(c, 6, 81, 0);
                    NAPI.Player.SetPlayerClothes(c, 7, 0, 0);
                    NAPI.Player.SetPlayerClothes(c, 9, 15, 0);
                    NAPI.Player.SetPlayerClothes(c, 10, 0, 0);

                    //SWAT mit Kapfgewehr (150)
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Revolver, 0); // A130
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Revolver, 130); // A130

                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.AssaultSMG, 0); // B240
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.AssaultSMG, 240); // B240


                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.AdvancedRifle, 0); // D150
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.AdvancedRifle, 90); // D150
                }
            }
            else if(Fraktionssystem.API.WhichFrak(c, 4))
            {
                if (Fraktionssystem.API.WhichfRank(c, 4))
                {
                    c.RemoveAllWeapons();
                    NAPI.Player.SetPlayerArmor(c, 100);
                    NAPI.Player.SetPlayerHealth(c, 100);
                    NAPI.Player.SetPlayerClothes(c, 1, 52, 0);
                    NAPI.Player.SetPlayerClothes(c, 4, 121, 0);
                    NAPI.Player.SetPlayerClothes(c, 5, 0, 0);
                    NAPI.Player.SetPlayerClothes(c, 6, 81, 0);
                    NAPI.Player.SetPlayerClothes(c, 7, 0, 0);
                    NAPI.Player.SetPlayerClothes(c, 9, 15, 0);
                    NAPI.Player.SetPlayerClothes(c, 10, 0, 0);

                NAPI.Player.SetPlayerArmor(c, 100);

                    //SWAT mit Kapfgewehr (120)
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Revolver, 0); // A130
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Revolver, 130); // A130

                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.AssaultSMG, 0); // B150
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.AssaultSMG, 150); // B150


                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.AdvancedRifle, 0); // D120
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.AdvancedRifle, 120); // D120
                }
                else if (Fraktionssystem.API.WhichfRank(c, 5))
                {
                    c.RemoveAllWeapons();
                    NAPI.Player.SetPlayerHealth(c, 100);
                    NAPI.Player.SetPlayerArmor(c, 100);
                    NAPI.Player.SetPlayerClothes(c, 1, 52, 0);
                    NAPI.Player.SetPlayerClothes(c, 4, 121, 0);
                    NAPI.Player.SetPlayerClothes(c, 5, 0, 0);
                    NAPI.Player.SetPlayerClothes(c, 6, 81, 0);
                    NAPI.Player.SetPlayerClothes(c, 7, 0, 0);
                    NAPI.Player.SetPlayerClothes(c, 9, 15, 0);
                    NAPI.Player.SetPlayerClothes(c, 10, 0, 0);

                    //SWAT mit Kapfgewehr (150)
                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.Revolver, 0); // A130
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.Revolver, 130); // A130

                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.AssaultSMG, 0); // B240
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.AssaultSMG, 240); // B240


                    NAPI.Player.GivePlayerWeapon(c, WeaponHash.AdvancedRifle, 0); // D150
                    NAPI.Player.SetPlayerWeaponAmmo(c, WeaponHash.AdvancedRifle, 90); // D150
                }
            }
        }

        [Command("fpunkte")]
        public void FuehrerscheinPunkte(Client c, Client p, int punkte, string reason)
        {
            if (Fraktionssystem.API.WhichFrak(c, 1) || Fraktionssystem.API.WhichFrak(c, 3))
            {
                if (!c.HasData("onduty"))
                {
                    c.SendNotification("Du bist nicht im Dienst!");
                    return;
                }

                if (!p.HasData("führerschein"))
                {
                    c.SendNotification("Dieser Spieler besitzt keinen Führerschein!");

                    return;
                }

                c.SendChatMessage($"[~b~LSPD~w~]: Du hast dem Spieler {p.Name} erfolgreich {punkte} Punkte auf sein Führerschein gegeben.");
                p.SendChatMessage($"[~b~LSPD~w~]: Du bekamst von {c.Name} {punkte} Punkte auf dein Führerschein. Grund: {reason}");

                Player.PlayerUpdate.UpdateLicensePoints(p, punkte);
            }
        }


        [Command("setwanted")]
        public void GivePlayerWanteds(Client c, Client p, int wanteds, string reason)
        {
            if (Fraktionssystem.API.WhichFrak(c, 1) || Fraktionssystem.API.WhichFrak(c, 3))
            {
                if (!c.HasData("onduty"))
                {
                    c.SendNotification("Du bist nicht im Dienst!");
                    return;
                }

                c.SendChatMessage($"[~b~LSPD~w~]: Du hast dem Spieler {p.Name} erfolgreich {wanteds} aus dem Grund: {reason}, gegeben.");
                p.SendChatMessage($"[~b~LSPD~w~]: Du hast von {c.Name} {wanteds} Wanteds erhalten aus dem Grund: {reason}.");

                Player.PlayerUpdate.UpdatePlayerWanteds(p, true, wanteds);
            }
        }

        [Command("arrest")]
        public void PlayerArrest(Client c, Client p)
        {
            if (Fraktionssystem.API.WhichFrak(c, 1) || Fraktionssystem.API.WhichFrak(c, 3))
            {
                if (!c.HasData("onduty"))
                {
                    c.SendNotification("Du bist nicht im Dienst!");
                    return;
                }

                if (!c.IsInVehicle)
                {
                    c.SendNotification("Du bist in keinem Fahrzeug!");
                    return;
                }

                if (c.Vehicle != p.Vehicle)
                {
                    c.SendNotification("Der Spieler ist nicht in deinem Fahrzeug!");
                    return;
                }

                if (p.GetData("wanteds") == 0)
                {
                    c.SendNotification("Der Spieler wird aktuell nicht gesucht!");
                    return;
                }

                if (c.Position.DistanceTo2D(new Vector3(1690.748, 2608.409, 45.82283)) > 5)
                {
                    c.SendNotification("Du bist nicht in der Nähe.");
                }

                Player.PlayerUpdate.UpdatePlayerJailtime(p, p.GetData("wanteds") * 3);
                Player.PlayerUpdate.UpdatePlayerWanteds(p, false, p.GetData("wanteds"));

                p.SendNotification("Du wurdest verhaftet!");
                p.SendNotification($"Du bist nun für {p.GetData("jailtime")} Minuten im Gefängnis!");
                c.SendNotification($"Du hast den Spieler {p.Name} festgenommen!");

                p.Position = new Vector3(1729.212, 2563.543, 45.56488);
                p.Rotation = new Vector3(186.1379, 0, 0);
            }
        }

        [RemoteEvent("ShowPlayerDienstAusweis")]
        public void pMarke(Client c)
        {
            foreach (Client target in NAPI.Pools.GetAllPlayers())
            {
                if (c.Equals(target)) continue;
                if (Init.Init.IsInRangeOfPoint(c.Position, new Vector3(target.Position.X, target.Position.Y, target.Position.Z), 1f))
                {
                    if (target.Name == c.Name)
                        return;

                    target.TriggerEvent("dienstmarke", c);

                    NAPI.Task.Run(() =>
                    {
                        target.TriggerEvent("dienstmarked", c);
                    }, delayTime: 7000);
                }
            }
        }

        [Flags]
        public enum AnimationFlags
        {
            Loop = 1 << 0,
            StopOnLastFrame = 1 << 1,
            OnlyAnimateUpperBody = 1 << 4,
            AllowPlayerControl = 1 << 5,
            Cancellable = 1 << 7
        }

        [Command("cuff")]
        public void cuff(Client c, Client p)
        {
            if (Fraktionssystem.API.WhichFrak(c, 1) || Fraktionssystem.API.WhichFrak(c, 3))
            {
                if (!c.HasData("onduty"))
                {
                    c.SendNotification("Du bist nicht im Dienst!");
                    return;
                }

                if (c.Position.DistanceTo2D(p.Position) < 5)
                {
                    if (!p.HasData("cuffed"))
                    {
                        NAPI.Player.PlayPlayerAnimation(p, (int)(AnimationFlags.Loop | AnimationFlags.OnlyAnimateUpperBody | AnimationFlags.AllowPlayerControl), "mp_arresting", "idle");
                        p.RemoveAllWeapons();
                        p.SetData("cuffed", 1);
                        p.TriggerEvent("cuff");
                        c.SendNotification("Du hast " + p.Name + " festgenommen!");
                        p.SendNotification("Du wurdest von " + c.Name + " in Handschellen gelegt!");
                    }
                    else
                    {
                        p.StopAnimation();
                        p.ResetData("cuffed");
                        p.TriggerEvent("uncuff");
                        c.SendNotification("Du hast " + p.Name + " entfesselt!");
                        p.SendNotification("Du wurdest aus den Handschellen befreit!");
                    }
                }
                else
                {
                    c.SendNotification("Spieler ist nicht in Reichweite!");
                }
            }
        }

        [Command("rearm")]
        public void rearmCommand(Client c)
        {
            if (Fraktionssystem.API.WhichFrak(c, 1) || Fraktionssystem.API.WhichFrak(c, 4) || Fraktionssystem.API.WhichFrak(c, 3)) 
            {
                NAPI.Player.SetPlayerHealth(c, 100);
                NAPI.Player.SetPlayerArmor(c, 100);
            }
        }
        #endregion

                #region LSMS
        [Command("revive")]
        public void RevivePlayer(Client c)
        {
            if (!Fraktionssystem.API.WhichFrak(c, 2))
                return;

            if (!c.HasData("onduty"))
            {
                c.SendNotification("Du bist nicht im Dienst!");
                return;
            }

            foreach (Client target in NAPI.Pools.GetAllPlayers())
            {
                if (c.Equals(target)) continue;
                if (Init.Init.IsInRangeOfPoint(c.Position, new Vector3(target.Position.X, target.Position.Y, target.Position.Z), 2.5f))
                {

                    if (!target.HasData("death"))
                    {
                        c.SendNotification("Der Spieler muss nicht reanimiert werden!");
                        return;
                    }

                    NAPI.Player.SpawnPlayer(target, c.Position);
                    target.Health = 50;

                    target.TriggerEvent("DeathFalse");

                    target.ResetData("death");

                    if (target.HasData("deathblip"))
                    {
                        Blip PlayerDeathBlip = target.GetData("deathblip");
                        PlayerDeathBlip.Delete();
                        c.ResetData("deathblip");
                    }

                    target.SendNotification("[~r~LSMS~w~] Du wurdest wiederbelebt!");
                    c.SendNotification($"[~r~LSMS~w~] Du hast den Spieler {target.Name} wiederbelebt!");

                    MoneyAPI.API.SubCash(target, 150);
                    MoneyAPI.API.AddCash(c, 250);
                }
            }
        }

        [Command("heal")]
        public void HealPlayer(Client c, Client p)
        {
            if (!Fraktionssystem.API.WhichFrak(c, 2))
                return;

            if (!c.HasData("onduty"))
            {
                c.SendNotification("Du bist nicht im Dienst!");
                return;
            }

            if (!c.IsInVehicle || !p.IsInVehicle || c.Vehicle != p.Vehicle)
            {
                c.SendNotification("Die Veletzte Person und du müssen im RTW sitzen.");
                return;
            }

            if (c.Position.DistanceTo(p.Position) <= 5)
            {
                p.Health = 100;
                c.SendNotification($"Du hast den Spieler {p.Name} verarztet.");
                p.SendNotification($"Du wurdest von {c.Name} verarztet.");
            } else
            {
                c.SendNotification("Der Spieler ist zu weit entfernt!");
            }
        }
        #endregion

        #region Staatsfraktionen
        [Command("fc")]
        public void FraktionsChat(Client c)
        {
            if (c.GetData("fraktion") == 0)
            {
                c.SendNotification("Du bist in keiner Fraktion!");
                return;
            }

            if (!c.HasData("fc"))
            {
                if (c.HasData("sc"))
                {
                    c.ResetData("sc");
                }

                c.SendNotification("FC aktiviert!");
                c.SetData("fc", true);
            } else
            {
                c.SendNotification("FC deaktiviert!");
                c.ResetData("fc");
            }
        }

        [Command("sc")]
        public void StaatsChat(Client c)
        {
            if (c.GetData("fraktion") == 0)
            {
                c.SendNotification("Du bist in keiner Fraktion!");
                return;
            }

            if (!c.HasData("sc"))
            {
                if (c.HasData("fc"))
                {
                    c.ResetData("fc");
                }

                c.SendNotification("SC aktiviert!");
                c.SetData("sc", true);
            }
            else
            {
                c.SendNotification("SC deaktiviert!");
                c.ResetData("sc");
            }
        }
        #endregion
    }
}
