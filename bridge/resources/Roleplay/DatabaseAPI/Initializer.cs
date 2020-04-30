using System;
using GTANetworkAPI;

namespace Roleplay.DatabaseAPI
{
    public class Initializer : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            DatabaseAPI.API.GetInstance();
        }
    }
}
