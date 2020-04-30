using GTANetworkAPI;

namespace Roleplay.Map
{
    public class DelObj : Script
    {
        [Command("createobj")]
        public void TestCreateObject(Client c, uint hash)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            NAPI.Object.CreateObject(hash, c.Position, c.Rotation);
        }

        [Command("deleteobj")]
        public void TestDeleteObject(Client c, uint hash)
        {
            if (!PermissionAPI.API.HasPermission(c, 1))
                return;

            NAPI.World.DeleteWorldProp(hash, c.Position, (1.0f));
        }

        public DelObj()
        {
            #region SellHouse
            NAPI.World.DeleteWorldProp(1295978393, new Vector3(-925.4514, 182.1665, 67.2203), (1.0f));
            #endregion

            #region LSPD
            //Pöller - Vorm LSPD die Große Treppe
            NAPI.World.DeleteWorldProp(-1510803822, new Vector3(416.7373, -975.8408, 29.3942), (1.0f));
            NAPI.World.DeleteWorldProp(-1510803822, new Vector3(416.7827, -973.0823, 29.4800), (1.0f));
            NAPI.World.DeleteWorldProp(-1510803822, new Vector3(416.8815, -977.3588, 29.2531), (1.0f));
            NAPI.World.DeleteWorldProp(-1510803822, new Vector3(416.8815, -980.3468, 29.1441), (1.0f));
            NAPI.World.DeleteWorldProp(-1510803822, new Vector3(416.8107, -983.0029, 29.4800), (1.0f));
            NAPI.World.DeleteWorldProp(-1510803822, new Vector3(416.8815, -984.3779, 29.2871), (1.0f));

            //Pöller - Vorm LSPD die kleine Treppe
            NAPI.World.DeleteWorldProp(-1510803822, new Vector3(423.9981, -966.0314, 29.3492), (1.0f)); //Zwei Stück auf einmal
            NAPI.World.DeleteWorldProp(-1510803822, new Vector3(426.8779, -965.8211, 29.3104), (1.0f)); //Zwei Stück auf einmal
            NAPI.World.DeleteWorldProp(-1510803822, new Vector3(429.6016, -965.8591, 29.2728), (1.0f)); //Zwei Stück auf einmal
            NAPI.World.DeleteWorldProp(-1510803822, new Vector3(432.3135, -965.8607, 29.2358), (1.0f)); //Zwei Stück auf einmal

            //Pöller - Der eine Pöller neben dem Busch
            NAPI.World.DeleteWorldProp(-1510803822, new Vector3(425.3451, -987.907, 30.7121), (1.0f)); //Zwei Stück auf einmal
            #endregion

            #region JOBS
            //Taxi - Mülltonne(Für das spawnen der Fahrzeuge)
            NAPI.World.DeleteWorldProp(-861197080, new Vector3(901.9013, -172.1681, 74.1418), (1.0f));
            #endregion
        }
    }
}
