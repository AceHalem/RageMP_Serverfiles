using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Roleplay.Housesystem
{
    public class House
    {
        public uint id { get; set; }
        public int status { get; set; }
        public string owner { get; set; }
        public int renter { get; set;}
        public int rentcost { get; set; }
        public int price { get; set; }
        public int interior { get; set; }
        public int locked { get; set; }
        public int platz { get; set; }
        public Vector3 position { get; set; }
        public TextLabel houseLabel { get; set; }
    }
}