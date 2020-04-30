using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Roleplay.Fahrschule
{
    class Main : Script
    {
        private static List<Vector3> SCHOOL_CPS = new List<Vector3>()
        {
            new Vector3(-703.8685, -1247.016, 9.477857),
            new Vector3(-647.4716, -1293.593, 9.85239),
            new Vector3(-608.9464, -1278.429, 10.03066),
            new Vector3(-539.1052, -983.3345, 22.54159),
            new Vector3(-558.5583, -955.2985, 22.61626),
            new Vector3(-660.7577, -953.3079, 20.55861),
            new Vector3(-772.3116, -964.9909, 14.91346),
            new Vector3(-823.0359, -1035.803, 12.37168),
            new Vector3(-770.1521, -1139.296, 9.77116),
            new Vector3(-699.1017, -1231.923, 9.79788),
            new Vector3(-755.9940, -1284.162, 4.181840),
            new Vector3(-854.6119, -1257.317, 4.183186)
        };

        public Main()
        {
            NAPI.TextLabel.CreateTextLabel("Drücke '~g~E~w~' um die Fahrschule zu öffnen!", new Vector3(-867.2078, -1275.194, 5.150179), 8, 1f, 4, new Color(255, 255, 255, 200));
            Blip Fahrschule = NAPI.Blip.CreateBlip(88, new Vector3(-867.2078, -1275.194, 5.150179), 1.0f, 4);
            NAPI.Blip.SetBlipName(Fahrschule, "Fahrschule"); NAPI.Blip.SetBlipShortRange(Fahrschule, true); NAPI.Blip.SetBlipScale(Fahrschule, 0.8f); NAPI.Blip.SetBlipColor(Fahrschule, 4);

            NAPI.TextLabel.CreateTextLabel("Bitte für die Fahrschule freihalten!", new Vector3(-852.5863, -1258.475, 5.000177), 8, 1f, 4, new Color(255, 255, 255, 200));
        }

        [RemoteEvent("StartLicenses")]
        public void StartLicenses(Client c)
        {
            if (c.Position.DistanceTo2D(new Vector3(-867.2078, -1275.194, 5.150179)) <= 3)
            {
                c.TriggerEvent("StartFahrschulBrowser");
            }
        }

        [RemoteEvent("Carlicense")]
        public void Carlicense(Client c)
        {
            if (c.HasData("führerschein"))
            {
                c.SendNotification("~r~Du besitzt bereits einen Führerschein!");
                return;
            }

            if (c.GetData("money_cash") < 1500)
            {
                c.SendNotification("~r~Du hast nicht genügend Geld bei dir!");
                return;
            }

            MoneyAPI.API.SubCash(c, 1500);

            c.Position = new Vector3(-852.5863, -1258.475, 5.000177);
            c.Rotation = new Vector3(0, 0, 230.8702);
            Vehicles.Vehicles.Createjv(c, "minivan");
            c.Vehicle.SetData("FahrschulFahrzeug", 1);
            c.SetData("InFahrschule", c.Vehicle);

            c.SetData("FahrschulPraxis", 0);

            int checkPoint = c.GetData("FahrschulPraxis");
            c.TriggerEvent("ShowFahrschulBlip", SCHOOL_CPS[checkPoint]);
            c.TriggerEvent("ShowFahrschulCP", SCHOOL_CPS[checkPoint]);

            c.SendChatMessage("[~b~Fahrlehrer~w~]: Willkommen bei der Fahrschule!");
            c.SendChatMessage("[~b~Fahrlehrer~w~]: Mit '~b~X~w~' kannst du den Motor starten!");
            c.SendChatMessage("[~b~Fahrlehrer~w~]: Max. erlaubte Geschwindigkeit in der Stadt beträgt 75Km/h + 10Km/h tolleranz!");
            c.SendChatMessage("[~b~Fahrlehrer~w~]: Fährst du zu schnell wird die Prüfung abgebrochen!");
            c.SendChatMessage("[~b~Fahrlehrer~w~]: Ampeln können ignoriert werden, ansonsten gilt die übliche StVo!");
        }

        public void GivePlayerLicense(Client p, int license)
        {
            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "INSERT INTO licenses (character_name, license) VALUES (@char, @license)";
            cmd.Parameters.AddWithValue("@char", p.Name);
            cmd.Parameters.AddWithValue("@license", license);
            cmd.ExecuteNonQuery();

            p.SendNotification("~g~Führerschein erhalten!");
            p.SetData("führerschein", 1);
            p.SetData("autoscheinpunkte", 0);

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }

        [RemoteEvent("OnPlayerEnterFahrschulCP")]
        public void OnPlayerEnterSchoolCP(Client c)
        {
            if (!c.IsInVehicle)
                return;
            
            if (c.HasData("InFahrschule"))
            {
                int checkPoint = c.GetData("FahrschulPraxis");
                if (checkPoint < SCHOOL_CPS.Count - 1)
                {
                    c.TriggerEvent("deleteFahrschulCP");
                    NAPI.Task.Run(() =>
                    {
                        c.SetData("FahrschulPraxis", checkPoint + 1);
                        c.TriggerEvent("ShowFahrschulBlip", SCHOOL_CPS[checkPoint + 1]);
                        c.TriggerEvent("ShowFahrschulCP", SCHOOL_CPS[checkPoint + 1]);

                    }, delayTime: 10);
                }
                else
                {
                    c.TriggerEvent("deleteFahrschulCP");
                    NAPI.Task.Run(() =>
                    {
                        c.ResetData("FahrschulPraxis");
                        c.SendNotification("[~b~Fahrlehrer~w~]: Du hast den Führerschein erfolgreich bestanden!");
                        c.TriggerEvent("StopJobVehSpeedo");

                        Jobs.Main.DeleteJobVehicle(c, c.GetData("InFahrschule"));
                        GivePlayerLicense(c, 1);

                        c.Position = new Vector3(-867.2078, -1275.194, 5.150179);

                    }, delayTime: 10);
                }
            }
        }

        [ServerEvent(Event.Update)]
        public void OnUpdate()
        {
            foreach (Client c in NAPI.Pools.GetAllPlayers())
            {
                if (c.HasData("InFahrschule"))
                {
                    Vehicle fveh = c.GetData("InFahrschule");
                    if (c.IsInVehicle && c.VehicleSeat == (int)VehicleSeat.Driver)
                    {
                        Vector3 velocity = NAPI.Entity.GetEntityVelocity(fveh);
                        double speed = Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y + velocity.Z * velocity.Z);
                        if (Math.Round(speed * 3.6f) > 85)
                        {
                            c.TriggerEvent("deleteFahrschulCP");

                            c.ResetData("FahrschulPraxis");
                            c.SendNotification("[~b~Fahrlehrer~w~]: Du bist zu schnell gefahren und hast die Prüfung NICHT bestanden!");
                            c.TriggerEvent("StopJobVehSpeedo");

                            Jobs.Main.DeleteJobVehicle(c, c.GetData("InFahrschule"));

                            c.Position = new Vector3(-867.2078, -1275.194, 5.150179);
                        }
                    }
                }
            }
        }
    }
}
