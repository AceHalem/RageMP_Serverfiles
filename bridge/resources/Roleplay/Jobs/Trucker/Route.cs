using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Roleplay.Jobs.Trucker
{
    class Route : Script
    {
        private static List<Vector3> TRUCKER_LEVEL1_CPS = new List<Vector3>()
        {
            new Vector3(35.41171, -2691.211, 5.006472),
            new Vector3(45.55693, -2695.340, 5.062666),
            new Vector3(48.62197, -2715.065, 5.004552),
            new Vector3(47.70337, -2732.775, 5.003381),
            new Vector3(18.93903, -2736.105, 5.005797),
            new Vector3(19.65575, -2723.587, 5.006742),
            new Vector3(23.28773, -2710.505, 5.009985),
            new Vector3(19.81340, -2694.546, 5.009777),
            new Vector3(10.58998, -2694.681, 5.008894),
            new Vector3(27.64610, -2680.565, 11.23541),
            new Vector3(48.16381, -2720.014, 11.20660),
            new Vector3(58.92599, -2680.118, 5.009071),
            new Vector3(45.12034, -2650.275, 5.004588),
            new Vector3(29.93791, -2637.761, 5.057109)
        };

        private static List<Vector3> TRUCKER_LEVEL2_CPS = new List<Vector3>()
        {
            new Vector3(1011.549, -2510.397, 27.40836),
            new Vector3(854.8928, -2308.668, 29.44975),
            new Vector3(1013.222, -1846.83, 30.35005),
            new Vector3(924.0259, -1537.273, 30.00484),
            new Vector3(726.5836, -980.2916, 23.282),
        };

        public enum AnimationFlags
        {
            Loop = 1 << 0,
            StopOnLastFrame = 1 << 1,
            OnlyAnimateUpperBody = 1 << 4,
            AllowPlayerControl = 1 << 5,
            Cancellable = 1 << 7
        }

        public static void TruckerJobStart(Client c)
        {
            if (c.HasData("InJobTP"))
            {
                int checkPoint = c.GetData("TruckerLevel1");

                c.TriggerEvent("ShowTruckerCP1", TRUCKER_LEVEL1_CPS[checkPoint]);
                c.TriggerEvent("ShowTruckerBlip1", TRUCKER_LEVEL1_CPS[checkPoint]);
            } else
            {
                int checkPoint = c.GetData("TruckerLevel2");

                c.TriggerEvent("ShowCP2", TRUCKER_LEVEL2_CPS[checkPoint]);
                c.TriggerEvent("ShowBlip2", TRUCKER_LEVEL2_CPS[checkPoint]);
            }

        }

        [RemoteEvent("OnTruckerLevel1")]
        public static void TruckerPraktikant(Client c)
        {
            c.TriggerEvent("DeleteTruckerBrowser");

            c.SetData("InJobTP", 1);
            c.SetData("TruckerLevel1", 0);
            TruckerJobStart(c);
            c.SendChatMessage("[~g~Trucker~w~]: Mit '/stopjob' kannst du den Job beenden.");
        }

        [RemoteEvent("OnTruckerLevel2")]
        public static void TruckerLieferant(Client c)
        {
            c.TriggerEvent("DeleteTruckerBrowser");

            c.Position = new Vector3(33.97424, -2663.726, 6.007133);
            c.Rotation = new Vector3(0, 0, 356.609);
            c.SetData("TruckerLevel2", 0);
            TruckerJobStart(c);
            c.SendChatMessage("[~g~Trucker~w~]: Mit '/stopjob' kannst du den Job beenden.");
            Vehicles.Vehicles.Createjv(c, "pounder2");
            c.SetData("JobVehicle", c.Vehicle);
            c.SetData("InJob", "Trucker");
            //c.SendChatMessage("[~y~WARNUNG~w~]: Sobald du das Fahrzeug verlässt wird in wenigen Sekunden dein Job beendet!.");
        }

        [RemoteEvent("OnPlayerEnterTruckerLevel")]
        public static void TruckerCheckpoint(Client c)
        {
            if (c.HasData("TruckerLevel1"))
            {
                int checkPoint = c.GetData("TruckerLevel1");
                    if (checkPoint < TRUCKER_LEVEL1_CPS.Count - 1)
                    {
                        NAPI.Player.PlayPlayerAnimation(c, (int)(AnimationFlags.Loop | AnimationFlags.AllowPlayerControl), "missexile3", "ex03_dingy_search_case_base_michael");
                        c.TriggerEvent("deleteTruckerLevelCP1");
                        c.TriggerEvent("freezeplayer");
                        c.SetData("InJobCP", 1);
                        NAPI.Task.Run(() =>
                        {
                            c.SetData("TruckerLevel1", checkPoint + 1);
                            c.TriggerEvent("ShowTruckerBlip1", TRUCKER_LEVEL1_CPS[checkPoint + 1]);
                            c.TriggerEvent("ShowTruckerCP1", TRUCKER_LEVEL1_CPS[checkPoint + 1]);

                            Random rand = new Random();
                            Random rand2 = new Random();
                            int Money = rand.Next(8, 21);
                            int Exp = rand2.Next(1, 8);

                            MoneyAPI.API.AddCash(c, Money);

                            c.StopAnimation();
                            c.TriggerEvent("unfreezeplayer");
                            c.ResetData("InJobCP");
                        }, delayTime: 5000);
                    }
                    else
                    {
                        NAPI.Player.PlayPlayerAnimation(c, (int)(AnimationFlags.Loop | AnimationFlags.AllowPlayerControl), "missexile3", "ex03_dingy_search_case_base_michael");
                        c.TriggerEvent("deleteTruckerLevelCP1");
                        c.TriggerEvent("freezeplayer");
                        c.SetData("InJobCP", 1);
                        NAPI.Task.Run(() =>
                        {
                            Random rand = new Random();
                            Random rand2 = new Random();
                            int Money = rand.Next(95, 230);

                            MoneyAPI.API.AddCash(c, Money);

                            c.SendChatMessage("[~g~Trucker~w~]: Du hast den Job fleißig beendet, nimm dafür dieses Geld als Belohnung.");

                            c.ResetData("TruckerLevel1");
                            c.ResetData("InJobTP");

                            c.StopAnimation();
                            c.TriggerEvent("unfreezeplayer");
                            c.ResetData("InJobCP");
                        }, delayTime: 5000);
                    }
            }
        }

        [RemoteEvent("OnPlayerEnterJobCP")]
        public void TruckerCheckpointLevel2(Client c)
        {
            if (c.HasData("TruckerLevel2"))
            {
                int checkPoint = c.GetData("TruckerLevel2");
                    if (checkPoint < TRUCKER_LEVEL2_CPS.Count - 1)
                    {
                        c.TriggerEvent("deleteJobCP2");
                        c.SendNotification("Gegenstände werden ausgeladen...");
                        c.TriggerEvent("DisableVehKeys");
                        c.SetData("InJobCP", 1);
                        NAPI.Task.Run(() =>
                        {
                            c.SetData("TruckerLevel2", checkPoint + 1);
                            c.TriggerEvent("ShowBlip2", TRUCKER_LEVEL2_CPS[checkPoint + 1]);
                            c.TriggerEvent("ShowCP2", TRUCKER_LEVEL2_CPS[checkPoint + 1]);
                            c.TriggerEvent("EnableVehKeys");
                            c.ResetData("InJobCP");

                            Random rand = new Random();
                            int Money = rand.Next(17, 31);

                            MoneyAPI.API.AddCash(c, Money);

                        }, delayTime: 5000);
                    }
                    else
                    {
                        c.SendNotification("Gegenstände werden ausgeladen...");
                        c.TriggerEvent("DisableVehKeys");
                        c.SetData("InJobCP", 1);
                        NAPI.Task.Run(() =>
                        {
                            Random rand = new Random();
                            int Money = rand.Next(145, 325);

                            MoneyAPI.API.AddCash(c, Money);

                            c.SendChatMessage("[~g~Trucker~w~]: Du hast den Job fleißig beendet, nimm dafür dieses Geld als Belohnung.");
                            c.ResetData("InJobCP");

                            c.ResetData("TruckerLevel2");

                            Jobs.Main.DeleteJobVehicle(c, c.Vehicle);
                        }, delayTime: 5000);
                    }
            }
        }
    }
}
