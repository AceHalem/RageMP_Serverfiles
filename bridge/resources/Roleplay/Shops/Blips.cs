using GTANetworkAPI;

namespace Roleplay.Shops
{
    public class FuelStations : Script
    {                                                                                                                                                       //shortRange standard false
        public Blip CreateMarker(uint sprite, Vector3 position, float scale, byte color, string name = "", byte alpha = 255, float drawDistance = 0, bool shortRange = true, short rotation = 0, uint dimension = uint.MaxValue)
        {
            Blip blip = NAPI.Blip.CreateBlip(sprite, position, scale, color, name, alpha, drawDistance, shortRange, rotation, dimension);
            return blip;
        }

        public FuelStations()
        {
            CreateMarker(361, new Vector3(263.6518, 2606.583, 44.98414), 0.8f, 37, "Tankstelle");
            CreateMarker(361, new Vector3(-2555.247, 2334.371, 33.07914), 0.8f, 37, "Tankstelle");
            CreateMarker(361, new Vector3(-2098.742, -318.7727, 12.52344), 0.8f, 37, "Tankstelle");
            CreateMarker(361, new Vector3(-1436.104, -276.6026, 45.70399), 0.8f, 37, "Tankstelle");
            CreateMarker(361, new Vector3(621.0473, 268.3362, 102.6658), 0.8f, 37, "Tankstelle");
            CreateMarker(361, new Vector3(1181.185, -331.9545, 68.78868), 0.8f, 37, "Tankstelle");
            CreateMarker(361, new Vector3(1208.427, -1402.669, 34.80032), 0.8f, 37, "Tankstelle");
            CreateMarker(361, new Vector3(174.5453, -1563.314, 29.02609), 0.8f, 37, "Tankstelle");
            CreateMarker(361, new Vector3(-72.15495, -1760.97, 29.25477), 0.8f, 37, "Tankstelle");
            CreateMarker(361, new Vector3(2003.932, 3776.487, 31.93668), 0.8f, 37, "Tankstelle");
            CreateMarker(361, new Vector3(1703.198, 6418.478, 31.97831), 0.8f, 37, "Tankstelle");
            CreateMarker(361, new Vector3(181.8707, 6604.383, 31.19762), 0.8f, 37, "Tankstelle");
            CreateMarker(361, new Vector3(-94.39114, 6419.002, 30.82691), 0.8f, 37, "Tankstelle");
            CreateMarker(361, new Vector3(261.4218, -1260.924, 28.89927), 0.8f, 37, "Tankstelle");
            CreateMarker(361, new Vector3(-323.7411, -1472.909, 30.30348), 0.8f, 37, "Tankstelle");

            CreateMarker(61, new Vector3(1840.412, 3670.306, 33.7011), 0.9f, 37, "Krankenhaus");
            CreateMarker(61, new Vector3(304.7992, -1435.226, 29.56051), 0.9f, 37, "Krankenhaus");
            CreateMarker(61, new Vector3(367.322, -590.4484, 28.4659), 0.9f, 37, "Krankenhaus");
            CreateMarker(61, new Vector3(-468.6577, -333.4573, 34.12247), 0.9f, 37, "Krankenhaus");
            CreateMarker(61, new Vector3(1152.221, -1527.989, 34.8434), 0.9f, 37, "LSMS");

            CreateMarker(60, new Vector3(429.6514, -981.28, 30.71037), 0.9f, 37, "LSPD");
            CreateMarker(60, new Vector3(-445, 6015, 31), 0.9f, 37, "LSSD");
            CreateMarker(60, new Vector3(104.7777, -744.4713, 45.75475), 0.9f, 37, "FIB");
            CreateMarker(188, new Vector3(1690.748, 2608.409, 45.82283), 0.9f, 37, "Knast");

            CreateMarker(313, new Vector3(16.88035, -1116.377, 29.79117), 0.9f, 37, "Waffenladen");

            CreateMarker(446, new Vector3(732.7979, -1088.876, 21.68531), 0.9f, 37, "Werkstatt");
            CreateMarker(446, new Vector3(111.7056, 6625.828, 31.40783), 0.9f, 37, "Werkstatt");
            CreateMarker(446, new Vector3(-965.0243, -3024.913, 13.56587), 0.9f, 37, "Werkstatt");

            CreateMarker(68, new Vector3(403.7291, -1635.22, 29.04765), 0.9f, 37, "Abschlepphof");
            CreateMarker(380, new Vector3(485.187, -1308.765, 29.25349), 0.9f, 37, "Schrottplatz");

            CreateMarker(473, new Vector3(53.97897, -880.0446, 30.10506), 0.9f, 37, "Garage");
            CreateMarker(473, new Vector3(-339.7374, 288.5088, 85.24472), 0.9f, 37, "Garage");
            CreateMarker(473, new Vector3(-1137.178, -860.3455, 13.23336), 0.9f, 37, "Garage");
            CreateMarker(473, new Vector3(123.7082, 6620.354, 31.58156), 0.9f, 37, "Garage");

            CreateMarker(52, new Vector3(-1487.674, -378.5323, 40.16342), 0.9f, 37, "Shop");
            CreateMarker(52, new Vector3(-707.4033, -914.5958, 19.21559), 0.9f, 37, "Shop");
            CreateMarker(52, new Vector3(-48.32117, -1757.817, 29.42101), 0.9f, 37, "Shop");
            CreateMarker(52, new Vector3(1135.713, -982.8101, 46.41581), 0.9f, 37, "Shop");
            CreateMarker(52, new Vector3(1163.713, -323.9544, 69.20506), 0.9f, 37, "Shop");
            CreateMarker(52, new Vector3(374.2068, 327.7614, 103.5664), 0.9f, 37, "Shop");
            CreateMarker(52, new Vector3(-3041.015, 585.2131, 7.908929), 0.9f, 37, "Shop");
            CreateMarker(52, new Vector3(-3243.889, 1001.395, 12.83072), 0.9f, 37, "Shop");
            CreateMarker(52, new Vector3(1729.754, 6416.304, 35.03722), 0.9f, 37, "Shop");
            CreateMarker(52, new Vector3(1697.955, 4924.557, 42.06368), 0.9f, 37, "Shop");
            CreateMarker(52, new Vector3(1960.323, 3742.161, 32.34374), 0.9f, 37, "Shop");
            CreateMarker(52, new Vector3(2677.131, 3281.386, 55.24113), 0.9f, 37, "Shop");

            CreateMarker(73, new Vector3(418.0296, -807.5797, 29.39654), 0.9f, 37, "Kleidungsgeschaeft");
        }
    }
}

