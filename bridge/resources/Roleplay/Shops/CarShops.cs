using GTANetworkAPI;

namespace Roleplay.Shops
{
    public class CarShops : Script
    {
        public CarShops()
        {

            #region Gebrauchtwagen
            Blip kleinwagen = NAPI.Blip.CreateBlip(669, new Vector3(-594.3101, -1178.47, 17.13922), 1.0f, 5);
            NAPI.Blip.SetBlipName(kleinwagen, "Gebrauchtwagenhandel");NAPI.Blip.SetBlipShortRange(kleinwagen, true); NAPI.Blip.SetBlipScale(kleinwagen, 0.8f); NAPI.Blip.SetBlipColor(kleinwagen, 0);

            Blip Fahrzeughandel = NAPI.Blip.CreateBlip(669, new Vector3(-61.28416, -1093.512, 26.49005), 1.0f, 5);
            NAPI.Blip.SetBlipName(Fahrzeughandel, "Fahrzeughandel"); NAPI.Blip.SetBlipShortRange(Fahrzeughandel, true); NAPI.Blip.SetBlipScale(Fahrzeughandel, 0.8f); NAPI.Blip.SetBlipColor(Fahrzeughandel, 0);

            uint mVeh17Hash = NAPI.Util.GetHashKey("bodhi2");
            Vector3 mVeh17Loc = new Vector3(-594.3478, -1179.097, 17.09003);
            Vehicle mVeh17 = NAPI.Vehicle.CreateVehicle(mVeh17Hash, mVeh17Loc, 181.9045f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Bodhi\nPreis: ~g~12.500$", mVeh17Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh17, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh17, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh17, "Gebrauchtwagenhandel");

            uint mVeh18Hash = NAPI.Util.GetHashKey("bfinjection");
            Vector3 mVeh18Loc = new Vector3(-590.7121, -1178.891, 17.03508);
            Vehicle mVeh18 = NAPI.Vehicle.CreateVehicle(mVeh18Hash, mVeh18Loc, 181.9045f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Injection\nPreis: ~g~10.500$", mVeh18Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh18, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh18, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh18, "Gebrauchtwagenhandel");

            uint mVeh19Hash = NAPI.Util.GetHashKey("surfer2");
            Vector3 mVeh19Loc = new Vector3(-586.8423, -1178.971, 17.44989);
            Vehicle mVeh19 = NAPI.Vehicle.CreateVehicle(mVeh19Hash, mVeh19Loc, 181.9045f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Surfer\nPreis: ~g~8.500$", mVeh19Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh19, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh19, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh19, "Gebrauchtwagenhandel");

            uint mVeh20Hash = NAPI.Util.GetHashKey("emperor2");
            Vector3 mVeh20Loc = new Vector3(-598.0319, -1179.223, 16.3766);
            Vehicle mVeh20 = NAPI.Vehicle.CreateVehicle(mVeh20Hash, mVeh20Loc, 181.9045f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Emperor\nPreis: ~g~12.500$", mVeh20Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh20, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh20, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh20, "Gebrauchtwagen");
            #endregion

            #region Fahrzeughandel
            uint mVeh21Hash = NAPI.Util.GetHashKey("caracara2");
            Vector3 mVeh21Loc = new Vector3(-48.38884, -1101.052, 26.13589);
            Vehicle mVeh21 = NAPI.Vehicle.CreateVehicle(mVeh21Hash, mVeh21Loc, 341.0058f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: caracara2\nPreis: ~g~37.500$", mVeh21Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh21, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh21, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh21, "Fahrzeughandel");

            uint mVeh22Hash = NAPI.Util.GetHashKey("novak");
            Vector3 mVeh22Loc = new Vector3(-48.45838, -1092.314, 25.93192);
            Vehicle mVeh22 = NAPI.Vehicle.CreateVehicle(mVeh22Hash, mVeh22Loc, 159.8555f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: novak\nPreis: ~g~39.500$", mVeh22Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh22, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh22, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh22, "Fahrzeughandel");

            uint mVeh23Hash = NAPI.Util.GetHashKey("drafter");
            Vector3 mVeh23Loc = new Vector3(-57.11064, -1097.489, 25.93905);
            Vehicle mVeh23 = NAPI.Vehicle.CreateVehicle(mVeh23Hash, mVeh23Loc, 297.347f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: drafter\nPreis: ~g~43.000$~w~\n[~y~NUR DIESEN MONAT!~w~]", mVeh23Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh23, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh23, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh23, "Fahrzeughandel");

            uint mVeh24Hash = NAPI.Util.GetHashKey("schlagen");
            Vector3 mVeh24Loc = new Vector3(-43.0004, -1093.897, 25.71457);
            Vehicle mVeh24 = NAPI.Vehicle.CreateVehicle(mVeh24Hash, mVeh24Loc, 159.8555f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: schlagen\nPreis: ~g~46.000$", mVeh24Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh24, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh24, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh24, "Fahrzeughandel");
            #endregion
        }
    }
}
