using GTANetworkAPI;

namespace Roleplay.Shops
{
    public class VanShops : Script
    {
        /*
        public VanShops()
        {
            //Van Shop

            Blip van1 = NAPI.Blip.CreateBlip(669, new Vector3(1956.665, 3771.535, 32.20424), 1.0f, 5);
            NAPI.Blip.SetBlipName(van1, "Fahrzeughandel");NAPI.Blip.SetBlipShortRange(van1, true); NAPI.Blip.SetBlipScale(van1, 0.8f); NAPI.Blip.SetBlipColor(van1, 0);

            uint mVeh1Hash = NAPI.Util.GetHashKey("youga");
            Vector3 mVeh1Loc = new Vector3(1964.126, 3780.692, 31.68771);
            Vehicle mVeh1 = NAPI.Vehicle.CreateVehicle(mVeh1Hash, mVeh1Loc, 118.3646f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Youga\nPreis: ~g~0$", mVeh1Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh1, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh1, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh1, "LYD");

            uint mVeh2Hash = NAPI.Util.GetHashKey("speedo");
            Vector3 mVeh2Loc = new Vector3(1965.663, 3777.924, 31.93813);
            Vehicle mVeh2 = NAPI.Vehicle.CreateVehicle(mVeh2Hash, mVeh2Loc, 119.8005f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Speedo\nPreis: ~g~0$", mVeh2Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh2, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh2, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh2, "LYD");

            uint mVeh3Hash = NAPI.Util.GetHashKey("paradise");
            Vector3 mVeh3Loc = new Vector3(1967.318, 3775.121, 32.22064);
            Vehicle mVeh3 = NAPI.Vehicle.CreateVehicle(mVeh3Hash, mVeh3Loc, 118.3646f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Paradise\nPreis: ~g~0$", mVeh3Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh3, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh3, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh3, "LYD");

            uint mVeh4Hash = NAPI.Util.GetHashKey("pony");
            Vector3 mVeh4Loc = new Vector3(1968.786, 3772.384, 31.98429);
            Vehicle mVeh4 = NAPI.Vehicle.CreateVehicle(mVeh4Hash, mVeh4Loc, 118.3646f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Pony\nPreis: ~g~0$", mVeh4Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh4, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh4, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh4, "LYD");

            uint mVeh5Hash = NAPI.Util.GetHashKey("gburrito2");
            Vector3 mVeh5Loc = new Vector3(1963.114, 3766.292, 32.00911);
            Vehicle mVeh5 = NAPI.Vehicle.CreateVehicle(mVeh5Hash, mVeh5Loc, 31.93329f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Gburrito 2\nPreis: ~g~0$", mVeh5Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh5, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh5, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh5, "LYD");

            uint mVeh6Hash = NAPI.Util.GetHashKey("pony2");
            Vector3 mVeh6Loc = new Vector3(1959.792, 3764.43, 31.98958);
            Vehicle mVeh6 = NAPI.Vehicle.CreateVehicle(mVeh6Hash, mVeh6Loc, 31.93329f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Pony 2\nPreis: ~g~0$", mVeh6Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh6, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh6, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh6, "LYD");

            uint mVeh7Hash = NAPI.Util.GetHashKey("burrito4");
            Vector3 mVeh7Loc = new Vector3(1953.159, 3760.678, 32.41143);
            Vehicle mVeh7 = NAPI.Vehicle.CreateVehicle(mVeh7Hash, mVeh7Loc, 31.93329f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Burrito 4\nPreis: ~g~0$", mVeh7Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh7, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh7, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh7, "LYD");

            uint mVeh8Hash = NAPI.Util.GetHashKey("rumpo3");
            Vector3 mVeh8Loc = new Vector3(1956.302, 3762.368, 32.01946);
            Vehicle mVeh8 = NAPI.Vehicle.CreateVehicle(mVeh8Hash, mVeh8Loc, 31.93329f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Rumpo 3\nPreis: ~g~0$", mVeh8Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh8, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh8, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh8, "LYD");
        }*/
    }
}
