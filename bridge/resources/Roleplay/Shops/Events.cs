using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Roleplay.Shops
{
    class Events : Script
    {
        public Events()
        {
            //Fahrrad verleih
            NAPI.TextLabel.CreateTextLabel("Drücke '~g~E~w~' um dir für 35~g~$~w~ ein Fahrrad zu leihen!", new Vector3(-1151.364, -716.9759, 21.14194), 12, 1f, 4, new Color(255, 255, 255, 200));

            NAPI.TextLabel.CreateTextLabel("Drücke '~g~E~w~' um den Shop zu öffnen!", new Vector3(-594.2563, -1189.977, 17.04215), 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Drücke '~g~E~w~' um den Shop zu öffnen!", new Vector3(-34.12098, -1103.572, 26.42234), 12, 1f, 4, new Color(255, 255, 255, 200));
        }

        [RemoteEvent("Fahrradverleih")]
        public void Fahrradverleih(Client c)
        {
            if (c.Position.DistanceTo2D(new Vector3(-1151.364, -716.9759, 21.14194)) <= 3)
            {
                if (c.HasData("temp"))
                {
                    c.SendNotification("Du besitzt bereits ein Fahrzeug das du dir geliehen hast!");
                    return;
                }

                c.SendNotification("Nutze '/unrent' um das Fahrrad zurück zu geben.");
                c.SendNotification("~r~-~w~35~g~$");
                MoneyAPI.API.SubCash(c, 35);
                uint hash = NAPI.Util.GetHashKey("scorcher");
                Vehicle veh = NAPI.Vehicle.CreateVehicle(hash, c.Position, c.Rotation.Z, 0, 0);
                c.SetIntoVehicle(veh, -1);
                c.Vehicle.SetData("temp", 1);
                c.SetData("temp", c.Vehicle);
            }
        }

        [RemoteEvent("OpenShopMenu")]
        public void TestOpenShopMenu(Client c)
        {
            if (c.Position.DistanceTo2D(new Vector3(-594.2563, -1189.977, 17.04215)) <= 3)
            {
                c.TriggerEvent("OpenGebrauchtwagen");
            } else if (c.Position.DistanceTo2D(new Vector3(-34.12098, -1103.572, 26.42234)) <= 3)
            {
                c.TriggerEvent("OpenAutohausStein");
            }
        }

        #region Gebrauchtwagenhändler
        [RemoteEvent("BuySurferVehToServer")]
        public void BuySurferVeh(Client c)
        {
            if (c.GetData("vehicles") == 3)
            {
                c.SendNotification("Du hast keine Fahrzeugslots mehr frei!");
                return;
            }

            if (c.GetData("money_cash") < 8500)
            {
                c.SendNotification("[~r~Händler~w~]: Du hast nicht genügend Geld auf der Hand.");
                return;
            }

            MoneyAPI.API.SubCash(c, 8500);
            c.Position = new Vector3(-603.758, -1191.03, 15.7402);
            c.Rotation = new Vector3(0,0,309.089);
            Vehicles.Vehicles.Create(c, "surfer2");
            c.TriggerEvent("CloseAutohausMenu");
        }

        [RemoteEvent("BuyInjectionVehToServer")]
        public void BuyInjectionVeh(Client c)
        {
            if (c.GetData("vehicles") == 3)
            {
                c.SendNotification("Du hast keine Fahrzeugslots mehr frei!");
                return;
            }

            if (c.GetData("money_cash") < 10500)
            {
                c.SendNotification("[~r~Händler~w~]: Du hast nicht genügend Geld auf der Hand.");
                return;
            }

            MoneyAPI.API.SubCash(c, 8500);
            c.Position = new Vector3(-603.758, -1191.03, 15.7402);
            c.Rotation = new Vector3(0, 0, 309.089);
            Vehicles.Vehicles.Create(c, "bfinjection");
            c.TriggerEvent("CloseAutohausMenu");
        }

        [RemoteEvent("BuyEmperorVehToServer")]
        public void BuyEmperorVeh(Client c)
        {
            if (c.GetData("vehicles") == 3)
            {
                c.SendNotification("Du hast keine Fahrzeugslots mehr frei!");
                return;
            }

            if (c.GetData("money_cash") < 12500)
            {
                c.SendNotification("[~r~Händler~w~]: Du hast nicht genügend Geld auf der Hand.");
                return;
            }

            MoneyAPI.API.SubCash(c, 8500);
            c.Position = new Vector3(-603.758, -1191.03, 15.7402);
            c.Rotation = new Vector3(0, 0, 309.089);
            Vehicles.Vehicles.Create(c, "emperor2");
            c.TriggerEvent("CloseAutohausMenu");
        }

        [RemoteEvent("BuyBodhiVehToServer")]
        public void BuyBodhiVeh(Client c)
        {
            if (c.GetData("vehicles") == 3)
            {
                c.SendNotification("Du hast keine Fahrzeugslots mehr frei!");
                return;
            }

            if (c.GetData("money_cash") < 12500)
            {
                c.SendNotification("[~r~Händler~w~]: Du hast nicht genügend Geld auf der Hand.");
                return;
            }

            MoneyAPI.API.SubCash(c, 8500);
            c.Position = new Vector3(-603.758, -1191.03, 15.7402);
            c.Rotation = new Vector3(0, 0, 309.089);
            Vehicles.Vehicles.Create(c, "bodhi2");
            c.TriggerEvent("CloseAutohausMenu");
        }
        #endregion

        #region Fahrzeughändler
        [RemoteEvent("BuyNovakVehToServer")]
        public void BuyNovakVehToServer(Client c)
        {
            if (c.GetData("vehicles") == 3)
            {
                c.SendNotification("Du hast keine Fahrzeugslots mehr frei!");
                return;
            }

            if (c.GetData("money_cash") < 39500)
            {
                c.SendNotification("[~r~Händler~w~]: Du hast nicht genügend Geld auf der Hand.");
                return;
            }

            MoneyAPI.API.SubCash(c, 39500);
            c.Position = new Vector3(-31.18943, -1090.012, 26.13486);
            c.Rotation = new Vector3(0, 0, 330.0429);
            Vehicles.Vehicles.Create(c, "Novak");
            c.TriggerEvent("CloseAutohausMenu");
        }

        [RemoteEvent("BuyCaracaraVehToServer")]
        public void BuyCaracaraVehToServer(Client c)
        {
            if (c.GetData("vehicles") == 3)
            {
                c.SendNotification("Du hast keine Fahrzeugslots mehr frei!");
                return;
            }

            if (c.GetData("money_cash") < 37500)
            {
                c.SendNotification("[~r~Händler~w~]: Du hast nicht genügend Geld auf der Hand.");
                return;
            }

            MoneyAPI.API.SubCash(c, 37500);
            c.Position = new Vector3(-31.18943, -1090.012, 26.13486);
            c.Rotation = new Vector3(0, 0, 330.0429);
            Vehicles.Vehicles.Create(c, "Caracara2");
            c.TriggerEvent("CloseAutohausMenu");
        }

        [RemoteEvent("BuyDrafterVehToServer")]
        public void BuyDrafterVehToServer(Client c)
        {
            if (c.GetData("vehicles") == 3)
            {
                c.SendNotification("Du hast keine Fahrzeugslots mehr frei!");
                return;
            }

            if (c.GetData("money_cash") < 43000)
            {
                c.SendNotification("[~r~Händler~w~]: Du hast nicht genügend Geld auf der Hand.");
                return;
            }

            MoneyAPI.API.SubCash(c, 43000);
            c.Position = new Vector3(-31.18943, -1090.012, 26.13486);
            c.Rotation = new Vector3(0, 0, 330.0429);
            Vehicles.Vehicles.Create(c, "Drafter");
            c.TriggerEvent("CloseAutohausMenu");
        }

        [RemoteEvent("BuySchlagenVehToServer")]
        public void BuySchlagenVehToServer(Client c)
        {
            if (c.GetData("vehicles") == 3)
            {
                c.SendNotification("Du hast keine Fahrzeugslots mehr frei!");
                return;
            }

            if (c.GetData("money_cash") < 46000)
            {
                c.SendNotification("[~r~Händler~w~]: Du hast nicht genügend Geld auf der Hand.");
                return;
            }

            MoneyAPI.API.SubCash(c, 46000);
            c.Position = new Vector3(-31.18943, -1090.012, 26.13486);
            c.Rotation = new Vector3(0, 0, 330.0429);
            Vehicles.Vehicles.Create(c, "Schlagen");
            c.TriggerEvent("CloseAutohausMenu");
        }
        #endregion
    }
}
