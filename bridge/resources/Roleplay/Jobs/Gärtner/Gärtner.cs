using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Roleplay.Jobs.Gärtner
{
    class Gärtner : Script
    {
        private static List<Vector3> GÄRTNER_CPS = new List<Vector3>()
        {
            new Vector3(-956.2892, 299.4449, 68.76437),
            new Vector3(-939.6992, 282.1823, 68.81203),
            new Vector3(-977.4921, 285.0718, 67.01888),
            new Vector3(-1000.539, 319.4752, 67.59653),
            new Vector3(-1013.916, 329.3642, 67.78719),
            new Vector3(-984.1941, 342.2758, 70.1946),
            new Vector3(-969.9884, 335.2211, 69.77515)
        };

        public static void StartGärtner(Client c)
        {
            c.SetData("InJob", "Gärtner");
            c.Position = new Vector3(-948.2819, 321.5802, 70.78724);
            c.Rotation = new Vector3(0, 0, 157.4344);
            Vehicles.Vehicles.Createjv(c, "mower");
            c.SetData("JobVehicle", c.Vehicle);
            c.SendChatMessage("[~g~Gärtner~w~]: Mit '/stopjob' kannst du den Job beenden.");
            c.SetData("GärtnerJob", 0);
            StartGärtnerCP(c);
        }

        public static void StartGärtnerCP(Client c)
        {
            int checkPoint = c.GetData("GärtnerJob");
            c.TriggerEvent("ShowCP4", GÄRTNER_CPS[checkPoint]);
            c.TriggerEvent("ShowBlip4", GÄRTNER_CPS[checkPoint]);
            c.TriggerEvent("ShowOBJ4", GÄRTNER_CPS[checkPoint]);
        }

        [RemoteEvent("OnPlayerEnterJobCP3")]
        public void MüllabfuhrCheckpoint(Client c)
        {
            if (c.HasData("GärtnerJob"))
            {
                int checkPoint = c.GetData("GärtnerJob");
                if (checkPoint < GÄRTNER_CPS.Count - 1)
                {
                    c.TriggerEvent("deleteJobCP4");
                    c.SendNotification("Rasen wird gemäht...");
                    c.SetData("InJobCP", 1);
                    c.TriggerEvent("DisableVehKeys");
                    NAPI.Task.Run(() =>
                    {
                        c.SetData("GärtnerJob", checkPoint + 1);
                        c.TriggerEvent("ShowBlip4", GÄRTNER_CPS[checkPoint + 1]);
                        c.TriggerEvent("ShowCP4", GÄRTNER_CPS[checkPoint + 1]);
                        c.TriggerEvent("ShowOBJ4", GÄRTNER_CPS[checkPoint + 1]);
                        c.TriggerEvent("EnableVehKeys");
                        c.ResetData("InJobCP");

                        Random rand = new Random();
                        int Money = rand.Next(13, 26);

                        MoneyAPI.API.AddCash(c, Money);

                    }, delayTime: 5000);
                }
                else
                {
                    c.SendNotification("Rasen wird gemäht...");
                    c.SetData("InJobCP", 1);
                    c.TriggerEvent("DisableVehKeys");
                    //c.TriggerEvent("deleteJobCP4");
                    NAPI.Task.Run(() =>
                    {
                        Random rand = new Random();
                        int Money = rand.Next(112, 225);

                        MoneyAPI.API.AddCash(c, Money);

                        c.SendChatMessage("[~g~Gärtner~w~]: Du hast den Job fleißig beendet, nimm dafür dieses Geld als Belohnung.");
                        c.ResetData("InJobCP");

                        c.ResetData("GärtnerJob");

                        Jobs.Main.DeleteJobVehicle(c, c.Vehicle);
                    }, delayTime: 5000);
                }
            }
        }
    }
}
