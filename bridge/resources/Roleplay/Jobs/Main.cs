using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Roleplay.Jobs
{
    class Main : Script
    {
        public Main()
        {
            Blip Trucker = NAPI.Blip.CreateBlip(478, new Vector3(42.5559, -2633.977, 6.038727), 1.0f, 63);
            NAPI.Blip.SetBlipName(Trucker, "Trucker(JOB)"); NAPI.Blip.SetBlipShortRange(Trucker, true); NAPI.Blip.SetBlipScale(Trucker, 0.8f);
            NAPI.TextLabel.CreateTextLabel("Mit '~g~E~w~' kannst du als Trucker anfangen!", new Vector3(42.5559, -2633.977, 6.038727), 10, 1f, 4, new Color(255, 255, 255, 200));

            Blip TAXI = NAPI.Blip.CreateBlip(56, new Vector3(895.3112, -179.3955, 74.70036), 1.0f, 5);
            NAPI.Blip.SetBlipName(TAXI, "Taxi(JOB)"); NAPI.Blip.SetBlipShortRange(TAXI, true); NAPI.Blip.SetBlipScale(TAXI, 0.8f);
            NAPI.TextLabel.CreateTextLabel("Mit '~g~E~w~' kannst du als Taxifahrer anfangen!", new Vector3(895.3556, -179.3416, 74.70035), 10, 1f, 4, new Color(255, 255, 255, 200));

            Blip Müllabfuhr = NAPI.Blip.CreateBlip(318, new Vector3(-428.7398, -1728.215, 19.78386), 1.0f, 81);
            NAPI.Blip.SetBlipName(Müllabfuhr, "Müllabfuhr(JOB)"); NAPI.Blip.SetBlipShortRange(Müllabfuhr, true); NAPI.Blip.SetBlipScale(Müllabfuhr, 0.8f);
            NAPI.TextLabel.CreateTextLabel("Mit '~g~E~w~' kannst du als Müllmann/Müllfrau anfangen!", new Vector3(-428.9279, -1728.098, 19.78386), 10, 1f, 4, new Color(255, 255, 255, 200));

            Blip Gärtner = NAPI.Blip.CreateBlip(512, new Vector3(-949.2931, 332.9058, 71.33704), 1.0f, 52);
            NAPI.Blip.SetBlipName(Gärtner, "Gärtner(JOB)"); NAPI.Blip.SetBlipShortRange(Gärtner, true); NAPI.Blip.SetBlipScale(Gärtner, 0.8f);
            NAPI.TextLabel.CreateTextLabel("Mit '~g~E~w~' kannst du als Gärtner/in anfangen!", new Vector3(-949.2931, 332.9058, 71.33704), 10, 1f, 4, new Color(255, 255, 255, 200));

            Blip Busfahrer = NAPI.Blip.CreateBlip(513, new Vector3(437.8359, -622.9085, 28.7089), 1.0f, 25);
            NAPI.Blip.SetBlipName(Busfahrer, "Busfahrer(JOB)"); NAPI.Blip.SetBlipShortRange(Busfahrer, true); NAPI.Blip.SetBlipScale(Busfahrer, 0.8f);
            NAPI.TextLabel.CreateTextLabel("Mit '~g~E~w~' kannst du als Busfahrer/in anfangen!", new Vector3(437.8359, -622.9085, 28.7089), 10, 1f, 4, new Color(255, 255, 255, 200));

            Blip Hanf = NAPI.Blip.CreateBlip(496, new Vector3(2221.411, 5614.624, 54.90175), 1.0f, 2);
            NAPI.Blip.SetBlipName(Hanf, "Hanfsammeln(JOB)"); NAPI.Blip.SetBlipShortRange(Hanf, true); NAPI.Blip.SetBlipScale(Hanf, 0.8f);
            NAPI.TextLabel.CreateTextLabel("Mit '~g~E~w~' kannst du Hanf sammeln starten!", new Vector3(2221.411, 5614.624, 54.90175), 10, 1f, 4, new Color(255, 255, 255, 200));

        }

        [RemoteEvent("Jobs")]
        public void StartJob(Client c)
        {
            if (c.Position.DistanceTo2D(new Vector3(42.5559, -2633.977, 6.038727)) <= 2)
            {
                if (c.HasData("InJobTP") || c.HasData("InJob"))
                {
                    c.SendNotification("Du bist bereits in einem Job!");
                    return;
                }
                c.TriggerEvent("OpenTruckerMain");
            }

            if (c.Position.DistanceTo2D(new Vector3(895.3556, -179.3416, 74.70035)) <= 2)
            {
                if (c.HasData("InJobTP") || c.HasData("InJob"))
                {
                    c.SendNotification("Du bist bereits in einem Job!");
                    return;
                }
                Taxi.Main.TaxiJobStart(c);
            }

            if (c.Position.DistanceTo2D(new Vector3(-428.9279, -1728.098, 19.78386)) <= 2)
            {
                if (c.HasData("InJobTP") || c.HasData("InJob"))
                {
                    c.SendNotification("Du bist bereits in einem Job!");
                    return;
                }
                Müllabfuhr.Main.StartTrash(c);
            }

            if (c.Position.DistanceTo2D(new Vector3(-949.2931, 332.9058, 71.33704)) <= 2)
            {
                if (c.HasData("InJobTP") || c.HasData("InJob"))
                {
                    c.SendNotification("Du bist bereits in einem Job!");
                    return;
                }
                Gärtner.Gärtner.StartGärtner(c);
            }

            if (c.Position.DistanceTo2D(new Vector3(437.8359, -622.9085, 28.7089)) <= 2)
            {
                if (c.HasData("InJobTP") || c.HasData("InJob"))
                {
                    c.SendNotification("Du bist bereits in einem Job!");
                    return;
                }
                Busfahrer.Route.StartBusfahrer(c);
            }

            if (c.Position.DistanceTo2D(new Vector3(2221.411, 5614.624, 54.90175)) <= 2) {
                if (c.HasData("InJobTP") || c.HasData("InJob"))
                {
                    c.SendNotification("Du bist bereits in einem Job!");
                    return;
                }
                Hanfsammeln.Main.StartHanfSammeln(c);
            }
        }

        [Command("stopjob")]
        public void StopJob(Client c)
        {
            if (c.HasData("InJobCP"))
            {
                c.SendNotification("Bitte warte bis der Checkpoint fertig ist.");
                return;
            }

            if (c.HasData("InJob") || c.HasData("InJobTP") || c.HasData("InIllegalJob")) {
                if (c.HasData("InJob"))
                {
                    if (!c.IsInVehicle || c.Vehicle.GetData("owner") != c.GetData("character_id"))
                    {
                        c.SendNotification("Du bist nicht in dem Fahrzeug von deinem Job!");
                        return;
                    }
                    DeleteJobVehicle(c, c.Vehicle);
                    c.TriggerEvent("StopJobVehSpeedo");
                }

                if (c.HasData("InJobTP")) {
                    c.ResetData("InJobTP");
                    c.TriggerEvent("deleteTruckerLevelCP1");
                }

                if (c.HasData("InIllegalJob")) {
                    c.ResetData("InIllegalJob");
                    c.ResetData("HanfJob");
                    c.TriggerEvent("deleteJobCP6");
                }
            } else {
                c.SendNotification("Du machst gerade keinen Job.");
            }
        }

        public static void DeleteJobVehicle(Client c, Vehicle veh)
        {
            if (c.HasData("InFahrschule"))
            {
                Vehicle veh2 = c.GetData("InFahrschule");
                veh2.Delete();
                c.ResetData("InFahrschule");
                return;
            }

            if (c.HasData("InJob"))
            {
                if (c.GetData("InJob") == "Trucker")
                {
                    NAPI.Task.Run(() =>
                    {
                        c.TriggerEvent("StopJobEnableVehKeys");
                        c.TriggerEvent("StopJobVehSpeedo");
                    }, delayTime: 750);
                    c.Position = new Vector3(42.5559, -2633.977, 6.038727);
                    c.SendChatMessage("[~g~Trucker~w~]: Du hast den Job beendet.");
                    c.TriggerEvent("deleteJobCP2");
                    c.ResetData("TruckerLevel2");
                }

                if (c.GetData("InJob") == "Taxi")
                {
                    c.Position = new Vector3(895.3556, -179.3416, 74.70035);
                    c.SendChatMessage("[~y~Taxizentrale~w~]: Du hast den Job beendet.");
                }

                if (c.GetData("InJob") == "Trash")
                {
                    NAPI.Task.Run(() =>
                    {
                        c.TriggerEvent("StopJobEnableVehKeys");
                        c.TriggerEvent("StopJobVehSpeedo");
                    }, delayTime: 750);
                    c.Position = new Vector3(-428.9279, -1728.098, 19.78386);
                    c.SendChatMessage("[~o~Müllabfuhr~w~]: Du hast den Job beendet.");
                    c.TriggerEvent("deleteJobCP3");
                    c.ResetData("TrashJob");
                }

                if (c.GetData("InJob") == "Gärtner")
                {
                    NAPI.Task.Run(() =>
                    {
                        c.TriggerEvent("StopJobEnableVehKeys");
                        c.TriggerEvent("StopJobVehSpeedo");
                        c.TriggerEvent("deleteJobCP4");
                    }, delayTime: 750);
                    c.Position = new Vector3(-949.2931, 332.9058, 71.33704);
                    c.SendChatMessage("[~g~Gärtner~w~]: Du hast den Job beendet.");
                    c.ResetData("GärtnerJob");
                }

                if (c.GetData("InJob") == "Busfahrer")
                {
                    NAPI.Task.Run(() =>
                    {
                        c.TriggerEvent("StopJobEnableVehKeys");
                        c.TriggerEvent("StopJobVehSpeedo");
                    }, delayTime: 750);
                    c.Position = new Vector3(437.8359, -622.9085, 28.7089);
                    c.SendChatMessage("[~g~BUSZENTRALE~w~]: Du hast den Job beendet.");
                    c.TriggerEvent("deleteJobCP5");
                    c.ResetData("BusJob");
                }

                c.ResetData("InJob");

                if (c.HasData("TaxiAccept"))
                {
                    Client player = c.GetData("TaxiAccept");
                    player.SendNotification("[~y~Taxizentrale~w~]: Dein Fahrer hat den Auftrag nicht ausgeführt.");
                    player.ResetData("TaxiAccept");
                    c.ResetData("TaxiAccept");
                    c.TriggerEvent("deleteTaxiWP");
                }

                Vehicle veh2 = c.GetData("JobVehicle");
                veh2.Delete();
                c.ResetData("JobVehicle");
            }
        }
    }
}
