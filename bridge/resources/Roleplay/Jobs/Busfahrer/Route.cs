using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Roleplay.Jobs.Busfahrer
{
    class Route : Script
    {
        private static List<Vector3> BUSFAHRER_CPS = new List<Vector3>()
        {
            new Vector3(306.2343, -770.4838, 27.60483),
            new Vector3(359.1451, -1064.585, 28.37372),
            new Vector3(789.3958, -1375.308, 25.19206),
            new Vector3(825.9601, -1644.771, 28.44638),
            new Vector3(807.1461, -1347.623, 24.69172),
            new Vector3(472.0353, -603.1791, 27.49533)
        };

        public static void StartBusfahrer(Client c)
        {
            c.SetData("InJob", "Busfahrer");
            c.Position = new Vector3(455.4108, -564.3416, 28.54221);
            c.Rotation = new Vector3(0, 0, 174.7369);
            Vehicles.Vehicles.Createjv(c, "bus");
            c.SetData("JobVehicle", c.Vehicle);
            c.Vehicle.SetData("JobVeh", "bus");
            c.Vehicle.PrimaryColor = 55;
            c.SendChatMessage("[~g~BUSZENTRALE~w~]: Mit '/stopjob' kannst du den Job beenden.");
            c.SetData("BusJob", 0);
            StartBusfahrerCP(c);
        }

        public static void StartBusfahrerCP(Client c)
        {
            int checkPoint = c.GetData("BusJob");
            c.TriggerEvent("ShowCP5", BUSFAHRER_CPS[checkPoint]);
            c.TriggerEvent("ShowBlip5", BUSFAHRER_CPS[checkPoint]);
        }

        [RemoteEvent("OnPlayerEnterJobCP4")]
        public void MüllabfuhrCheckpoint(Client c)
        {
            if (c.HasData("BusJob"))
            {
                int checkPoint = c.GetData("BusJob");
                if (checkPoint < BUSFAHRER_CPS.Count - 1)
                {
                    c.TriggerEvent("deleteJobCP5");
                    c.SendNotification("Warte auf Fahrgäste...");
                    c.SetData("InJobCP", 1);
                    c.TriggerEvent("DisableVehKeys");
                    NAPI.Task.Run(() =>
                    {
                        c.SetData("BusJob", checkPoint + 1);
                        c.TriggerEvent("ShowBlip5", BUSFAHRER_CPS[checkPoint + 1]);
                        c.TriggerEvent("ShowCP5", BUSFAHRER_CPS[checkPoint + 1]);
                        c.TriggerEvent("EnableVehKeys");
                        c.ResetData("InJobCP");

                        Random rand = new Random();
                        int Money = rand.Next(17, 31);

                        MoneyAPI.API.AddCash(c, Money);

                    }, delayTime: 5000);
                }
                else
                {
                    c.SendNotification("Jobende wird angekündigt...");
                    c.TriggerEvent("DisableVehKeys");
                    c.SetData("InJobCP", 1);
                    NAPI.Task.Run(() =>
                    {
                        Random rand = new Random();
                        int Money = rand.Next(145, 325);

                        MoneyAPI.API.AddCash(c, Money);

                        c.SendChatMessage("[~g~BUSZENTRALE~w~]: Du hast den Job fleißig beendet, nimm dafür dieses Geld als Belohnung.");
                        c.ResetData("InJobCP");

                        c.ResetData("BusJob");

                        Jobs.Main.DeleteJobVehicle(c, c.Vehicle);
                    }, delayTime: 5000);
                }
            }
        }
    }
}
