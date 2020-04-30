using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Roleplay.Jobs.Hanfsammeln
{
    class Main : Script
    {
        public enum AnimationFlags
        {
            Loop = 1 << 0,
            StopOnLastFrame = 1 << 1,
            OnlyAnimateUpperBody = 1 << 4,
            AllowPlayerControl = 1 << 5,
            Cancellable = 1 << 7
        }

        private static List<Vector3> HANF_CPS = new List<Vector3>()
        {
            new Vector3(2217.84, 5579.697, 52.97567),
            new Vector3(2230.471, 5574.688, 52.88255),
            new Vector3(2233.477, 5578.726, 53.12169),
            new Vector3(2217.779, 5575.129, 52.72498),
            new Vector3(2227.282, 5576.812, 52.87181),
            new Vector3(2224.485, 5579.135, 52.92464),
            new Vector3(2233.282, 5574.336, 52.99109)
        };

        public static void StartHanfSammeln(Client c)
        {
            c.SetData("InIllegalJob", "Hanfsammeln");
            c.SendChatMessage("[~r~ANONYM~w~]: Mit '/stopjob' kannst du den Job beenden.");
            c.SetData("HanfJob", 0);
            StartHanfSammelnCP(c);
        }

        public static void StartHanfSammelnCP(Client c)
        {
            int checkPoint = c.GetData("HanfJob");
            c.TriggerEvent("ShowCP6", HANF_CPS[checkPoint]);
            c.TriggerEvent("ShowBlip6", HANF_CPS[checkPoint]);
            c.TriggerEvent("ShowOBJ6", HANF_CPS[checkPoint]);
        }

        [RemoteEvent("StartHanfSammeln")]
        public void HanfsammelnCheckpoint(Client c)
        {
            if (c.HasData("HanfJob"))
            {
                int checkPoint = c.GetData("HanfJob");
                if (c.Position.DistanceTo2D(HANF_CPS[checkPoint]) <= 2) {
                    if (checkPoint < HANF_CPS.Count - 1)
                    {
                        c.TriggerEvent("deleteJobCP6");
                        c.SendNotification("Hanf wird eingesammelt...");
                        c.SetData("InJobCP", 1);
                        NAPI.Player.PlayPlayerAnimation(c, (int)(AnimationFlags.Loop | AnimationFlags.AllowPlayerControl), "missexile3", "ex03_dingy_search_case_base_michael");
                        c.TriggerEvent("freezeplayer");
                        NAPI.Task.Run(() =>
                        {
                            c.SetData("HanfJob", checkPoint + 1);
                            c.TriggerEvent("ShowBlip6", HANF_CPS[checkPoint + 1]);
                            c.TriggerEvent("ShowCP6", HANF_CPS[checkPoint + 1]);
                            c.TriggerEvent("ShowOBJ6", HANF_CPS[checkPoint + 1]);
                            c.TriggerEvent("unfreezeplayer");
                            c.ResetData("InJobCP");

                            c.StopAnimation();

                            Random rand = new Random();
                            int hanf = rand.Next(0, 3);

                            if (hanf == 0) {
                                c.SendNotification("Keine Hanfknollen gefunden!");
                            } else {
                                InventoryAPI.API.AddItem(c, 4, hanf);
                                c.SendNotification("Hanfknollen gefunden: " + hanf);
                            }

                        }, delayTime: 8000);
                    }
                    else
                    {
                        c.SendNotification("Hanf wird eingesammelt...");
                        c.SetData("InJobCP", 1);
                        NAPI.Player.PlayPlayerAnimation(c, (int)(AnimationFlags.Loop | AnimationFlags.AllowPlayerControl), "missexile3", "ex03_dingy_search_case_base_michael");
                        c.TriggerEvent("freezeplayer");
                        c.TriggerEvent("deleteJobCP6");
                        NAPI.Task.Run(() =>
                        {
                        Random rand2 = new Random();
                        int hanf = rand2.Next(0, 3);

                        if (hanf == 0) {
                            c.SendNotification("Keine Hanfknollen gefunden!");
                        } else {
                            InventoryAPI.API.AddItem(c, 4, hanf);
                            c.SendNotification("Hanfknollen gefunden: " + hanf);
                        }

                        Random rand = new Random();
                        int Money = rand.Next(75, 121);

                        MoneyAPI.API.AddCash(c, Money);

                        c.TriggerEvent("unfreezeplayer");
                        c.SendChatMessage("[~r~ANONYM~w~]: Du hast den Job fleißig beendet, nimm dafür dieses Geld als Belohnung.");
                        c.ResetData("InJobCP");
                        c.StopAnimation();

                        c.ResetData("HanfJob");
                        c.ResetData("InIllegalJob");

                        }, delayTime: 8000);
                    }
                }
            }
        }
    }
}
