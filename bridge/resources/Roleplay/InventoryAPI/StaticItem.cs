using System;
using System.Collections.Generic;
using System.Text;

namespace Roleplay.InventoryAPI
{
    public class StaticItem
    {
        private int typeId;
        private string name;
        private string category;
        private string rarity;
        private int volume;
        private int weight;

        public StaticItem(int typeId, string name, string category, string rarity, int volume, int weight)
        {
            this.typeId = typeId;
            this.name = name;
            this.category = category;
            this.rarity = rarity;
            this.volume = volume;
            this.weight = weight;
        }

        public int GetTypeId()
        {
            return this.typeId;
        }

        public string GetName()
        {
            return this.name;
        }

        public string GetCategory()
        {
            return this.category;
        }

        public string GetRarity()
        {
            return this.rarity;
        }

        public int GetVolume()
        {
            return this.volume;
        }

        public int GetWeight()
        {
            return this.weight;
        }
    }
}
