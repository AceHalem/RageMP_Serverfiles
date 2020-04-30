using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Roleplay.BankAPI
{
    class BlipsMarker : Script
    {
        public BlipsMarker() //108 
        {
            Blip FLEECA = NAPI.Blip.CreateBlip(374, new Vector3(149.9948, -1040.474, 29.37407), 1.0f, 25);
            NAPI.Blip.SetBlipName(FLEECA, "FLEECA"); NAPI.Blip.SetBlipShortRange(FLEECA, true); NAPI.Blip.SetBlipScale(FLEECA, 0.8f);
            NAPI.TextLabel.CreateTextLabel("Erstelle mit '~g~E~w~' ein Konto!", new Vector3(149.9948, -1040.474, 29.37407), 6, 1f, 4, new Color(255, 255, 255, 200));

            /* Blip Atm10 = NAPI.Blip.CreateBlip(108, new Vector3(), 1.0f, 25);
             NAPI.Blip.SetBlipName(Atm10, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm10, true); NAPI.Blip.SetBlipScale(Atm1, 0.6f);*/
            Blip Atm1 = NAPI.Blip.CreateBlip(108, new Vector3(155, 6642, 31), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm1, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm1, true); NAPI.Blip.SetBlipScale(Atm1, 0.6f);

            Blip Atm2 = NAPI.Blip.CreateBlip(108, new Vector3(132, 6366, 31), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm2, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm2, true); NAPI.Blip.SetBlipScale(Atm2, 0.6f);

            Blip Atm3 = NAPI.Blip.CreateBlip(108, new Vector3(-282, 6225, 31), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm3, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm3, true); NAPI.Blip.SetBlipScale(Atm3, 0.6f);

            Blip Atm4 = NAPI.Blip.CreateBlip(108, new Vector3(-386, 6045, 31), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm4, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm4, true); NAPI.Blip.SetBlipScale(Atm4, 0.6f);

            Blip Atm5 = NAPI.Blip.CreateBlip(108, new Vector3(1701, 6426, 32), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm5, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm5, true); NAPI.Blip.SetBlipScale(Atm5, 0.6f);

            Blip Atm6 = NAPI.Blip.CreateBlip(108, new Vector3(1735, 6410, 35), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm6, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm6, true); NAPI.Blip.SetBlipScale(Atm6, 0.6f);

            Blip Atm7 = NAPI.Blip.CreateBlip(108, new Vector3(1703, 4933, 42), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm7, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm7, true); NAPI.Blip.SetBlipScale(Atm7, 0.6f);

            Blip Atm8 = NAPI.Blip.CreateBlip(108, new Vector3(1686, 4816, 42), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm8, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm8, true); NAPI.Blip.SetBlipScale(Atm8, 0.6f);

            Blip Atm9 = NAPI.Blip.CreateBlip(108, new Vector3(1822, 3683, 34), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm9, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm9, true); NAPI.Blip.SetBlipScale(Atm9, 0.6f);

            Blip Atm10 = NAPI.Blip.CreateBlip(108, new Vector3(1968, 3743, 32), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm10, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm10, true); NAPI.Blip.SetBlipScale(Atm10, 0.6f);

            Blip Atm11 = NAPI.Blip.CreateBlip(108, new Vector3(-258, -723, 33), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm11, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm11, true); NAPI.Blip.SetBlipScale(Atm11, 0.6f);

            Blip Atm12 = NAPI.Blip.CreateBlip(108, new Vector3(-256, -715, 33), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm12, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm12, true); NAPI.Blip.SetBlipScale(Atm12, 0.6f);

            /*Blip Atm13 = NAPI.Blip.CreateBlip(108, new Vector3(-254, 692, 33), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm13, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm13, true); NAPI.Blip.SetBlipScale(Atm13, 0.6f);*/

            Blip Atm14 = NAPI.Blip.CreateBlip(108, new Vector3(-28, -723, 44), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm14, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm14, true); NAPI.Blip.SetBlipScale(Atm14, 0.6f);

            Blip Atm15 = NAPI.Blip.CreateBlip(108, new Vector3(1078, -776, 57), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm15, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm15, true); NAPI.Blip.SetBlipScale(Atm15, 0.6f);

            Blip Atm16 = NAPI.Blip.CreateBlip(108, new Vector3(1138, -468, 66), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm16, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm16, true); NAPI.Blip.SetBlipScale(Atm16, 0.6f);

            Blip Atm17 = NAPI.Blip.CreateBlip(108, new Vector3(1166, -456, 66), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm17, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm17, true); NAPI.Blip.SetBlipScale(Atm17, 0.6f);

            Blip Atm18 = NAPI.Blip.CreateBlip(108, new Vector3(1153, -326, 69), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm18, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm18, true); NAPI.Blip.SetBlipScale(Atm18, 0.6f);

            Blip Atm19 = NAPI.Blip.CreateBlip(108, new Vector3(285, 143, 104), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm19, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm19, true); NAPI.Blip.SetBlipScale(Atm19, 0.6f);

            Blip Atm20 = NAPI.Blip.CreateBlip(108, new Vector3(89, 2, 67), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm20, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm20, true); NAPI.Blip.SetBlipScale(Atm20, 0.6f);

            Blip Atm21 = NAPI.Blip.CreateBlip(108, new Vector3(-56, -1752, 29), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm21, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm21, true); NAPI.Blip.SetBlipScale(Atm21, 0.6f);

            Blip Atm22 = NAPI.Blip.CreateBlip(108, new Vector3(33, -1348, 29), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm22, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm22, true); NAPI.Blip.SetBlipScale(Atm22, 0.6f);

            Blip Atm23 = NAPI.Blip.CreateBlip(108, new Vector3(288, -1282, 29), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm23, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm23, true); NAPI.Blip.SetBlipScale(Atm23, 0.6f);

            Blip Atm24 = NAPI.Blip.CreateBlip(108, new Vector3(289, -1256, 29), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm24, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm24, true); NAPI.Blip.SetBlipScale(Atm24, 0.6f);

            Blip Atm25 = NAPI.Blip.CreateBlip(108, new Vector3(146, -1035, 29), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm25, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm25, true); NAPI.Blip.SetBlipScale(Atm25, 0.6f);

            Blip Atm26 = NAPI.Blip.CreateBlip(108, new Vector3(199, -883, 31), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm26, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm26, true); NAPI.Blip.SetBlipScale(Atm26, 0.6f);

            Blip Atm27 = NAPI.Blip.CreateBlip(108, new Vector3(112, -775, 31), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm27, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm27, true); NAPI.Blip.SetBlipScale(Atm27, 0.6f);

            Blip Atm28 = NAPI.Blip.CreateBlip(108, new Vector3(112, -819, 31), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm28, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm28, true); NAPI.Blip.SetBlipScale(Atm28, 0.6f);

            Blip Atm29 = NAPI.Blip.CreateBlip(108, new Vector3(296, -895, 29), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm29, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm29, true); NAPI.Blip.SetBlipScale(Atm29, 0.6f);

            Blip Atm30 = NAPI.Blip.CreateBlip(108, new Vector3(-1827, 784, 138), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm30, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm30, true); NAPI.Blip.SetBlipScale(Atm30, 0.6f);

            Blip Atm31 = NAPI.Blip.CreateBlip(108, new Vector3(-1410, -99, 52), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm31, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm31, true); NAPI.Blip.SetBlipScale(Atm31, 0.6f);

            /*Blip Atm32 = NAPI.Blip.CreateBlip(108, new Vector3(1570, -546, 34), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm32, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm32, true); NAPI.Blip.SetBlipScale(Atm32, 0.6f);*/

            Blip Atm33 = NAPI.Blip.CreateBlip(108, new Vector3(2683, 3286, 55), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm33, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm33, true); NAPI.Blip.SetBlipScale(Atm33, 0.6f);

            Blip Atm34 = NAPI.Blip.CreateBlip(108, new Vector3(2564, 2584, 38), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm34, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm34, true); NAPI.Blip.SetBlipScale(Atm34, 0.6f);

            Blip Atm35 = NAPI.Blip.CreateBlip(108, new Vector3(1171, 2702, 38), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm35, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm35, true); NAPI.Blip.SetBlipScale(Atm35, 0.6f);

            Blip Atm36 = NAPI.Blip.CreateBlip(108, new Vector3(-1091, 2708, 18), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm36, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm36, true); NAPI.Blip.SetBlipScale(Atm36, 0.6f);

            Blip Atm37 = NAPI.Blip.CreateBlip(108, new Vector3(-1827, 784, 138), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm37, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm37, true); NAPI.Blip.SetBlipScale(Atm37, 0.6f);

            Blip Atm38 = NAPI.Blip.CreateBlip(108, new Vector3(-1410, -99, 52), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm38, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm38, true); NAPI.Blip.SetBlipScale(Atm38, 0.6f);

            Blip Atm39 = NAPI.Blip.CreateBlip(108, new Vector3(-1540, -546, 34), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm39, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm39, true); NAPI.Blip.SetBlipScale(Atm39, 0.6f);

            Blip Atm40 = NAPI.Blip.CreateBlip(108, new Vector3(-254, -692, 33), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm40, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm40, true); NAPI.Blip.SetBlipScale(Atm40, 0.6f);

            Blip Atm41 = NAPI.Blip.CreateBlip(108, new Vector3(-302, -829, 32), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm41, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm41, true); NAPI.Blip.SetBlipScale(Atm41, 0.6f);

            Blip Atm42 = NAPI.Blip.CreateBlip(108, new Vector3(-526, -1222, 18), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm42, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm42, true); NAPI.Blip.SetBlipScale(Atm42, 0.6f);

            Blip Atm43 = NAPI.Blip.CreateBlip(108, new Vector3(-537, -854, 29), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm43, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm43, true); NAPI.Blip.SetBlipScale(Atm43, 0.6f);

            Blip Atm44 = NAPI.Blip.CreateBlip(108, new Vector3(-613, -704, 31), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm44, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm44, true); NAPI.Blip.SetBlipScale(Atm44, 0.6f);

            Blip Atm45 = NAPI.Blip.CreateBlip(108, new Vector3(-660, -854, 24), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm45, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm45, true); NAPI.Blip.SetBlipScale(Atm45, 0.6f);

            Blip Atm46 = NAPI.Blip.CreateBlip(108, new Vector3(-711, -818, 23), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm46, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm46, true); NAPI.Blip.SetBlipScale(Atm46, 0.6f);

            Blip Atm47 = NAPI.Blip.CreateBlip(108, new Vector3(-717, -915, 19), 1.0f, 25);
            NAPI.Blip.SetBlipName(Atm47, "Bankautomat"); NAPI.Blip.SetBlipShortRange(Atm47, true); NAPI.Blip.SetBlipScale(Atm47, 0.6f);
        }
    }
}
