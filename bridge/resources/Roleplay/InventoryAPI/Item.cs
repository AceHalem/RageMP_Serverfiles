using System;
using System.Collections.Generic;
using System.Text;

namespace Roleplay.InventoryAPI
{
    public class Item
    {
        private int typeId;
        private string name;
        private string category;
        private string rarity;
        private int volume;
        private int weight;
        private int amount;

        public Item(int typeId, string name, string category, string rarity, int volume, int weight, int amount)
        {
            this.typeId = typeId;
            this.name = name;
            this.category = category;
            this.rarity = rarity;
            this.volume = volume;
            this.weight = weight;
            this.amount = amount;
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

        public int GetWeight()
        {
            return this.weight * amount;
        }

        public int GetVolume()
        {
            return this.volume * amount;
        }

        public int GetAmount()
        {
            return this.amount;
        }

        public void SetAmount(int amount)
        {
            this.amount = amount;
        }
    }
}
