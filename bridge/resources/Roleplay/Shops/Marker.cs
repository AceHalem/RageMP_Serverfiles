using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Roleplay.Shops
{
    class Marker : Script
    {
        public static Vector3[] fuelstation = new Vector3[]
        {
            new Vector3(263.6518, 2606.583, 44.98414),
            new Vector3(-2555.247, 2334.371, 33.07914),
            new Vector3(-2098.742, -318.7727, 12.52344),
            new Vector3(-1436.104, -276.6026, 45.70399),
            new Vector3(621.0473, 268.3362, 102.6658),
            new Vector3(1181.185, -331.9545, 68.78868),
            new Vector3(1208.427, -1402.669, 34.80032),
            new Vector3(174.5453, -1563.314, 29.02609),
            new Vector3(-72.15495, -1760.97, 29.25477),
            new Vector3(2003.932, 3776.487, 31.93668),
            new Vector3(1703.198, 6418.478, 31.97831),
            new Vector3(181.8707, 6604.383, 31.19762),
            new Vector3(-94.39114, 6419.002, 30.82691),
            new Vector3(261.4218, -1260.924, 28.89927),
            new Vector3(-323.7411, -1472.909, 30.30348)
        };

        public static Vector3[] vehstore = new Vector3[]
        {
            new Vector3(53.97897, -880.0446, 30.10506),
            new Vector3(-339.7374, 288.5088, 85.24472),
            new Vector3(-1137.178, -860.3455, 13.23336),
            new Vector3(123.7082, 6620.354, 31.58156)
        };

        public Marker()
        {
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/fuel~w~' um dein Fahrzeug zu tanken!", fuelstation[0], 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/fuel~w~' um dein Fahrzeug zu tanken!", fuelstation[1], 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/fuel~w~' um dein Fahrzeug zu tanken!", fuelstation[2], 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/fuel~w~' um dein Fahrzeug zu tanken!", fuelstation[3], 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/fuel~w~' um dein Fahrzeug zu tanken!", fuelstation[4], 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/fuel~w~' um dein Fahrzeug zu tanken!", fuelstation[5], 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/fuel~w~' um dein Fahrzeug zu tanken!", fuelstation[6], 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/fuel~w~' um dein Fahrzeug zu tanken!", fuelstation[7], 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/fuel~w~' um dein Fahrzeug zu tanken!", fuelstation[8], 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/fuel~w~' um dein Fahrzeug zu tanken!", fuelstation[9], 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/fuel~w~' um dein Fahrzeug zu tanken!", fuelstation[10], 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/fuel~w~' um dein Fahrzeug zu tanken!", fuelstation[11], 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/fuel~w~' um dein Fahrzeug zu tanken!", fuelstation[12], 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/fuel~w~' um dein Fahrzeug zu tanken!", fuelstation[13], 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/fuel~w~' um dein Fahrzeug zu tanken!", fuelstation[14], 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/schrotten~w~' um dein Fahrzeug zu verkaufen!", new Vector3(485.187, -1308.765, 29.25349), 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/store~w~' um dein Fahrzeug einzuparken!", vehstore[0], 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/store~w~' um dein Fahrzeug einzuparken!", vehstore[1], 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/store~w~' um dein Fahrzeug einzuparken!", vehstore[2], 12, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/store~w~' um dein Fahrzeug einzuparken!", vehstore[3], 12, 1f, 4, new Color(255, 255, 255, 200));

            #region Shops
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um den Shop zu öffnen!", new Vector3(-1487.674, -378.5323, 40.16342), 8, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um den Shop zu öffnen!", new Vector3(-707.4033, -914.5958, 19.21559), 8, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um den Shop zu öffnen!", new Vector3(-48.32117, -1757.817, 29.42101), 8, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um den Shop zu öffnen!", new Vector3(1135.713, -982.8101, 46.41581), 8, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um den Shop zu öffnen!", new Vector3(1163.713, -323.9544, 69.20506), 8, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um den Shop zu öffnen!", new Vector3(374.2068, 327.7614, 103.5664), 8, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um den Shop zu öffnen!", new Vector3(-3041.015, 585.2131, 7.908929), 8, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um den Shop zu öffnen!", new Vector3(-3243.889, 1001.395, 12.83072), 8, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um den Shop zu öffnen!", new Vector3(1729.754, 6416.304, 35.03722), 8, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um den Shop zu öffnen!", new Vector3(1697.955, 4924.557, 42.06368), 8, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um den Shop zu öffnen!", new Vector3(1960.323, 3742.161, 32.34374), 8, 1f, 4, new Color(255, 255, 255, 200));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um den Shop zu öffnen!", new Vector3(2677.131, 3281.386, 55.24113), 8, 1f, 4, new Color(255, 255, 255, 200));
            #endregion

            #region Kleidungsgeschäft
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um den Shop zu öffnen!", new Vector3(425.5883, -806.1441, 29.49115), 8, 1f, 4, new Color(255, 255, 255, 200));
            #endregion
        }
    }
}
