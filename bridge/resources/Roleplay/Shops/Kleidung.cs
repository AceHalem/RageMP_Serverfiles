using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Roleplay.Shops
{
    class Kleidung : Script
    {
        #region Hose
        [RemoteEvent("Hose1Buy")]
        public static void Hose1(Client c)
        {
            if (c.GetData("isMale") == true)
            {
                c.SetClothes(4, 9, 0);
                c.SetData("templegs", 9);
            } else
            {
                c.SetClothes(4, 78, 0);
                c.SetData("templegs", 78);
            }
        }

        [RemoteEvent("Hose2Buy")]
        public static void Hose2(Client c)
        {
            if (c.GetData("isMale") == true)
            {
                c.SetClothes(4, 86, 0);
                c.SetData("templegs", 86);
            }
            else
            {
                c.SetClothes(4, 43, 0);
                c.SetData("templegs", 43);
            }
        }

        [RemoteEvent("Hose3Buy")]
        public static void Hose3(Client c)
        {
            if (c.GetData("isMale") == true)
            {
                c.SetClothes(4, 102, 0);
                c.SetData("templegs", 102);
            }
            else
            {
                c.SetClothes(4, 44, 0);
                c.SetData("templegs", 44);
            }
        }
        #endregion

        #region Oberteile
        [RemoteEvent("Oberteil1Buy")]
        public static void Oberteil1Buy(Client c)
        {
            if (c.GetData("isMale") == true)
            {
                c.SetClothes(11, 9, 0);
                c.SetData("temptops", 9);
                c.SetClothes(3, 0, 0);
                c.SetData("temptorsos", 0);
            }
            else
            {
                c.SetClothes(11, 1, 0);
                c.SetData("temptops", 1);
                c.SetClothes(3, 64, 0);
                c.SetData("temptorsos", 64);
                c.SetClothes(8, 16, 0);
                c.SetData("tempundershirts", 16);
            }
        }

        [RemoteEvent("Oberteil2Buy")]
        public static void Oberteil2Buy(Client c)
        {
            if (c.GetData("isMale") == true)
            {
                c.SetClothes(11, 33, 0);
                c.SetData("temptops", 33);
                c.SetClothes(3, 0, 0);
                c.SetData("temptorsos", 0);
            }
            else
            {
                c.SetClothes(11, 27, 0);
                c.SetData("temptops", 27);
                c.SetClothes(3, 0, 0);
                c.SetData("temptorsos", 0);
                c.SetClothes(8, 15, 0);
                c.SetData("tempundershirts", 15);
            }
        }

        [RemoteEvent("Oberteil3Buy")]
        public static void Oberteil3Buy(Client c)
        {
            if (c.GetData("isMale") == true)
            {
                c.SetClothes(11, 41, 0);
                c.SetData("temptops", 41);
                c.SetClothes(3, 1, 0);
                c.SetData("temptorsos", 1);
            }
            else
            {
                c.SetClothes(11, 2, 0);
                c.SetData("temptops", 2);
                c.SetClothes(3, 2, 0);
                c.SetData("temptorsos", 2);
                c.SetClothes(8, 15, 0);
                c.SetData("tempundershirts", 15);
            }
        }
        #endregion

        #region Oberteile
        [RemoteEvent("Schuhe1Buy")]
        public static void Schuhe1Buy(Client c)
        {
            if (c.GetData("isMale") == true)
            {
                c.SetClothes(6, 4, 0);
                c.SetData("tempshoes", 4);
            }
            else
            {
                c.SetClothes(6, 8, 0);
                c.SetData("tempshoes", 8);
            }
        }

        [RemoteEvent("Schuhe2Buy")]
        public static void Schuhe2Buy(Client c)
        {
            if (c.GetData("isMale") == true)
            {
                c.SetClothes(6, 31, 0);
                c.SetData("tempshoes", 31);
            }
            else
            {
                c.SetClothes(6, 49, 0);
                c.SetData("tempshoes", 49);
            }
        }

        [RemoteEvent("Schuhe3Buy")]
        public static void Schuhe3Buy(Client c)
        {
            if (c.GetData("isMale") == true)
            {
                c.SetClothes(6, 65, 0);
                c.SetData("tempshoes", 65);
            }
            else
            {

            }
            c.SetClothes(6, 11, 0);
            c.SetData("tempshoes", 11);
        }
        #endregion
    }
}
