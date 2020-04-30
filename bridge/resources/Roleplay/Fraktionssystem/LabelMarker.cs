using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roleplay.Fraktionssystem
{
    public class LabelMarker : Script
    {
        public LabelMarker()
        {

            NAPI.TextLabel.CreateTextLabel("Benutze '~b~E~w~' um den Computer zu verwenden.", new Vector3(440.7106, -975.6343, 30.68961), 8, 1f, 4, new Color(255, 255, 255, 255));

            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um in den Dienst zu gehen.", new Vector3(461.416, -981.1726, 30.6896), 8, 1f, 4, new Color(255, 255, 255, 255));

            //Prison
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~/arrest~w~' um jemanden einzusperren.", new Vector3(1690.748, 2608.409, 45.82283), 8, 1f, 4, new Color(255, 255, 255, 255));

            //FIB
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um den Aufzug zu benutzen.", new Vector3(138.9402, -762.7764, 45.75203), 8, 1f, 4, new Color(255, 255, 255, 255));
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um den Aufzug zu benutzen.", new Vector3(136.1253, -761.7436, 242.152), 8, 1f, 4, new Color(255, 255, 255, 255));

            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um in den Dienst zu gehen.", new Vector3(118.8053, -729.1281, 242.1519), 8, 1f, 4, new Color(255, 255, 255, 255));

            //LSMS
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um in den Dienst zu gehen.", new Vector3(1124.566, -1523.731, 34.84324), 8, 1f, 4, new Color(255, 255, 255, 255));

            //LSSD
            NAPI.TextLabel.CreateTextLabel("Benutze '~g~E~w~' um in den Dienst zu gehen.", new Vector3(-440.3005, 6019.714, 31.49011), 8, 1f, 4, new Color(255, 255, 255, 255));
        }
    }
}
