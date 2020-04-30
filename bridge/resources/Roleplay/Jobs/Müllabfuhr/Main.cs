using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Roleplay.Jobs.Müllabfuhr
{
    class Main : Script
    {
        private static List<Vector3> MÜLLABFUHR_CPS = new List<Vector3>()
        {
            new Vector3(-171.4677, -1411.795, 30.67194),
            new Vector3(-93.38874, -1328.694, 28.94548),
            new Vector3(-91.28317, -1279.874, 28.87386),
            new Vector3(-510.1389, -865.6319, 29.32015),
            new Vector3(-671.5712, -952.5666, 20.83414),
            new Vector3(-1148.139, -1377.351, 4.680765),
            new Vector3(-467.2278, -1677.823, 18.74924)
        };

        public static void StartTrash(Client c)
        {
            c.SetData("InJob", "Trash");
            c.Position = new Vector3(-426.7617, -1689.674, 18.74228);
            c.Rotation = new Vector3(0, 0, 157.4344);
            Vehicles.Vehicles.Createjv(c, "trash2");
            c.SetData("JobVehicle", c.Vehicle);
            c.Vehicle.SetData("JobVeh", "trash");
            c.Vehicle.PrimaryColor = 88;
            c.SendChatMessage("[~o~Müllabfuhr~w~]: Mit '/stopjob' kannst du den Job beenden.");
            c.SetData("TrashJob", 0);
            StartTrashCP(c);
        }

        public static void StartTrashCP(Client c)
        {
            int checkPoint = c.GetData("TrashJob");
            c.TriggerEvent("ShowCP3", MÜLLABFUHR_CPS[checkPoint]);
            c.TriggerEvent("ShowBlip3", MÜLLABFUHR_CPS[checkPoint]);
        }

        [RemoteEvent("OnPlayerEnterJobCP2")]
        public void MüllabfuhrCheckpoint(Client c)
        {
            if (c.HasData("TrashJob"))
            {
                int checkPoint = c.GetData("TrashJob");
                if (checkPoint < MÜLLABFUHR_CPS.Count - 1)
                {
                    c.TriggerEvent("deleteJobCP3");
                    c.SendNotification("Müll wird eingeladen...");
                    c.SetData("InJobCP", 1);
                    c.TriggerEvent("DisableVehKeys");
                    NAPI.Task.Run(() =>
                    {
                        c.SetData("TrashJob", checkPoint + 1);
                        c.TriggerEvent("ShowBlip3", MÜLLABFUHR_CPS[checkPoint + 1]);
                        c.TriggerEvent("ShowCP3", MÜLLABFUHR_CPS[checkPoint + 1]);
                        c.TriggerEvent("EnableVehKeys");
                        c.ResetData("InJobCP");

                        Random rand = new Random();
                        int Money = rand.Next(17, 31);

                        MoneyAPI.API.AddCash(c, Money);

                    }, delayTime: 5000);
                }
                else
                {
                    c.SendNotification("Müll wird eingeladen...");
                    c.SetData("InJobCP", 1);
                    c.TriggerEvent("DisableVehKeys");
                    NAPI.Task.Run(() =>
                    {
                        Random rand = new Random();
                        int Money = rand.Next(145, 325);

                        MoneyAPI.API.AddCash(c, Money);

                        c.SendChatMessage("[~o~Müllabfuhr~w~]: Du hast den Job fleißig beendet, nimm dafür dieses Geld als Belohnung.");
                        c.ResetData("InJobCP");

                        c.ResetData("TrashJob");

                        Jobs.Main.DeleteJobVehicle(c, c.Vehicle);
                    }, delayTime: 5000);
                }
            }
        }
    }
}
