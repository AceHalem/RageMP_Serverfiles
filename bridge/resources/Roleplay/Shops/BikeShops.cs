using GTANetworkAPI;

namespace Roleplay.Shops
{
    public class BikeShops : Script
    {            
        /*
        public BikeShops()
        {
            //Bike Shop
            Blip bike1 = NAPI.Blip.CreateBlip(661, new Vector3(260.632, -1155.983, 29.27629), 1.0f, 5);
            NAPI.Blip.SetBlipName(bike1, "Motorradhandel");NAPI.Blip.SetBlipShortRange(bike1, true); NAPI.Blip.SetBlipScale(bike1, 0.8f); NAPI.Blip.SetBlipColor(bike1, 0);

            uint mVeh1Hash = NAPI.Util.GetHashKey("akuma");
            Vector3 mVeh1Loc = new Vector3(262.4377, -1162.613, 28.64585);
            Vehicle mVeh1 = NAPI.Vehicle.CreateVehicle(mVeh1Hash, mVeh1Loc, 357.0724f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Akuma\nPreis: ~g~0$", mVeh1Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh1, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh1, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh1, "LYD");

            uint mVeh2Hash = NAPI.Util.GetHashKey("avarus");
            Vector3 mVeh2Loc = new Vector3(259.2657, -1162.615, 28.70268);
            Vehicle mVeh2 = NAPI.Vehicle.CreateVehicle(mVeh2Hash, mVeh2Loc, 357.0724f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Avarus\nPreis: ~g~0$", mVeh2Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh2, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh2, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh2, "LYD");

            uint mVeh3Hash = NAPI.Util.GetHashKey("bati");
            Vector3 mVeh3Loc = new Vector3(256.1473, -1162.709, 28.51831);
            Vehicle mVeh3 = NAPI.Vehicle.CreateVehicle(mVeh3Hash, mVeh3Loc, 357.0724f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Bati\nPreis: ~g~0$", mVeh3Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh3, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh3, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh3, "LYD");

            uint mVeh4Hash = NAPI.Util.GetHashKey("carbonrs");
            Vector3 mVeh4Loc = new Vector3(253.0343, -1162.821, 28.66362);
            Vehicle mVeh4 = NAPI.Vehicle.CreateVehicle(mVeh4Hash, mVeh4Loc, 357.0724f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Carbonrs\nPreis: ~g~0$", mVeh4Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh4, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh4, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh4, "LYD");

            uint mVeh5Hash = NAPI.Util.GetHashKey("deathbike");
            Vector3 mVeh5Loc = new Vector3(249.6809, -1162.953, 28.61941);
            Vehicle mVeh5 = NAPI.Vehicle.CreateVehicle(mVeh5Hash, mVeh5Loc, 357.0724f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Deathbike\nPreis: ~g~0$", mVeh5Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh5, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh5, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh5, "LYD");

            uint mVeh6Hash = NAPI.Util.GetHashKey("innovation");
            Vector3 mVeh6Loc = new Vector3(250.2189, -1149.425, 28.74404);
            Vehicle mVeh6 = NAPI.Vehicle.CreateVehicle(mVeh6Hash, mVeh6Loc, 180.375f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Innovation\nPreis: ~g~0$", mVeh6Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh6, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh6, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh6, "LYD");

            uint mVeh7Hash = NAPI.Util.GetHashKey("manchez");
            Vector3 mVeh7Loc = new Vector3(253.3151, -1149.492, 28.70732);
            Vehicle mVeh7 = NAPI.Vehicle.CreateVehicle(mVeh7Hash, mVeh7Loc, 180.375f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Manchez\nPreis: ~g~0$", mVeh7Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh7, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh7, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh7, "LYD");

            uint mVeh8Hash = NAPI.Util.GetHashKey("thrust");
            Vector3 mVeh8Loc = new Vector3(256.2813, -1149.546, 28.75526);
            Vehicle mVeh8 = NAPI.Vehicle.CreateVehicle(mVeh8Hash, mVeh8Loc, 180.375f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Thrust\nPreis: ~g~0$", mVeh8Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh8, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh8, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh8, "LYD");

            uint mVeh9Hash = NAPI.Util.GetHashKey("sovereign");
            Vector3 mVeh9Loc = new Vector3(259.727, -1149.477, 28.6343);
            Vehicle mVeh9 = NAPI.Vehicle.CreateVehicle(mVeh9Hash, mVeh9Loc, 180.375f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Sovereign\nPreis: ~g~0$", mVeh9Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh9, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh9, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh9, "LYD");

            uint mVeh10Hash = NAPI.Util.GetHashKey("wolfsbane");
            Vector3 mVeh10Loc = new Vector3(262.5512, -1149.427, 28.76591);
            Vehicle mVeh10 = NAPI.Vehicle.CreateVehicle(mVeh10Hash, mVeh10Loc, 180.375f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Wolfsbane\nPreis: ~g~0$", mVeh10Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh10, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh10, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh10, "LYD");

            uint mVeh11Hash = NAPI.Util.GetHashKey("blazer");
            Vector3 mVeh11Loc = new Vector3(265.7311, -1161.604, 28.71284);
            Vehicle mVeh11 = NAPI.Vehicle.CreateVehicle(mVeh11Hash, mVeh11Loc, 37.68903f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Blazer\nPreis: ~g~0$", mVeh11Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh11, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh11, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh11, "LYD");

            uint mVeh12Hash = NAPI.Util.GetHashKey("blazer4");
            Vector3 mVeh12Loc = new Vector3(265.6934, -1150.446, 28.57497);
            Vehicle mVeh12 = NAPI.Vehicle.CreateVehicle(mVeh12Hash, mVeh12Loc, 136.6203f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Blazer Sport\nPreis: ~g~0$", mVeh12Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh12, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh12, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh12, "LYD");

            //AirPort Start

            Blip airPort = NAPI.Blip.CreateBlip(661, new Vector3(-1001.662, -2758.575, 13.7568), 1.0f, 5);
            NAPI.Blip.SetBlipName(airPort, "Motorradhandel"); NAPI.Blip.SetBlipShortRange(airPort, true); NAPI.Blip.SetBlipScale(airPort, 0.8f); NAPI.Blip.SetBlipColor(airPort, 0);

            uint mVeh13Hash = NAPI.Util.GetHashKey("faggio");
            Vector3 mVeh13Loc = new Vector3(-999.0774, -2757.728, 13.23024);
            Vehicle mVeh13 = NAPI.Vehicle.CreateVehicle(mVeh13Hash, mVeh13Loc, 18.1987f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Faggio\nPreis: ~g~0$", mVeh13Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh13, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh13, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh13, "LYD");

            uint mVeh14Hash = NAPI.Util.GetHashKey("faggio2");
            Vector3 mVeh14Loc = new Vector3(-1002.808, -2756.099, 13.23327);
            Vehicle mVeh14 = NAPI.Vehicle.CreateVehicle(mVeh14Hash, mVeh14Loc, 299.2774f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Faggio2\nPreis: ~g~0$", mVeh14Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh14, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh14, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh14, "LYD");

            uint mVeh15Hash = NAPI.Util.GetHashKey("cruiser");
            Vector3 mVeh15Loc = new Vector3(-1000.103, -2759.355, 13.33448);
            Vehicle mVeh15 = NAPI.Vehicle.CreateVehicle(mVeh15Hash, mVeh15Loc, 18.1987f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Cruiser\nPreis: ~g~0$", mVeh15Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh15, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh15, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh15, "LYD");

            uint mVeh16Hash = NAPI.Util.GetHashKey("fixter");
            Vector3 mVeh16Loc = new Vector3(-1003.716, -2757.811, 13.36251);
            Vehicle mVeh16 = NAPI.Vehicle.CreateVehicle(mVeh16Hash, mVeh16Loc, 299.2774f, 0, 0);
            NAPI.TextLabel.CreateTextLabel("Fahrzeug: Fixter\nPreis: ~g~0$", mVeh16Loc, 12, 1f, 4, new Color(255, 255, 255, 100));
            NAPI.Vehicle.SetVehicleEngineStatus(mVeh16, false);
            NAPI.Vehicle.SetVehicleLocked(mVeh16, true);
            NAPI.Vehicle.SetVehicleNumberPlate(mVeh16, "LYD");
        }*/
    }
}
