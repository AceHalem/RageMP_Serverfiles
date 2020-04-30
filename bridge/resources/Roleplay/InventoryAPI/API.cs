using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

namespace Roleplay.InventoryAPI
{
    public class API : Script
    {
        private static List<StaticItem> itemList;
        private static string[] rarityNames = new string[] {
            "Common",
            "Uncommon",
            "Epic",
            "Legendary",
            "Special"
        };

        public API()
        {
            itemList = new List<StaticItem>();

            new Timer(SyncConfigItemsFromDb, null, 0, 900000);
        }

        [Command("setitemamount")]
        public void HandleSetItemAmount(Client c, int typeId, int amount = 1)
        {
            SetItemAmount(c, typeId, amount);
        }

        [Command("additem")]
        public void HandlAddItem(Client c, int typeId, int amount = 1)
        {
            AddItem(c, typeId, amount);
        }

        [Command("remitem")]
        public void HandlRemoveItem(Client c, int typeId, int amount = 1)
        {
            RemoveItem(c, typeId, amount);
        }

        [Command("items")]
        public void HandleItems(Client c)
        {
            List<Item> items = c.GetData("inventory_items");
            if (items != null && items.Count >= 1)
            {
                c.SendChatMessage("======== ITEMS ========");
                foreach (Item item in items)
                {
                    c.SendChatMessage(item.GetName() + ":");
                    c.SendChatMessage("- Anzahl: " + item.GetAmount());
                    c.SendChatMessage("- Volumen: " + item.GetVolume());
                    c.SendChatMessage("- Gewicht: " + item.GetWeight());
                }
            }
            else
            {
                c.SendChatMessage("Keine Items :(");
            }
        }

        [RemoteEvent("getItems")]
        public void HandleGetItems(Client c)
        {
            if (!c.HasData("inventory_items"))
                return;

            NAPI.Task.Run(() =>
            {
                List<Item> items = c.GetData("inventory_items");

                List<BasicItem> basicItems = new List<BasicItem>();
                if (items.Count >= 1)
                {
                    foreach (Item item in items)
                    {
                        basicItems.Add(new BasicItem
                        {
                            typeId = item.GetTypeId(),
                            name = item.GetName(),
                            category = item.GetCategory(),
                            rarity = item.GetRarity(),
                            volume = item.GetVolume(),
                            weight = item.GetWeight(),
                            amount = item.GetAmount()
                        });
                    }
                }

                c.TriggerEvent("recieveItems", basicItems);
            }, delayTime: 250);
        }

        public static void SetItemAmount(Client c, int typeId, int amount = 1)
        {
            List<Item> items = c.GetData("inventory_items");

            int invIndex = GetInventoryIndex(c, typeId);
            if (invIndex != -1)
            {
                if (amount >= 1)
                {
                    Item stack = items[invIndex];
                    stack.SetAmount(amount);
                    UpdateItemInDb(c, typeId, amount);
                }
                else
                {
                    items.RemoveAt(invIndex);
                    RemoveItemFromDb(c, typeId);
                }
            }
            else
            {
                StaticItem itemData = GetItemData(typeId);
                if (itemData != null)
                {
                    items.Add(new Item(typeId, itemData.GetName(), itemData.GetCategory(), itemData.GetRarity(), itemData.GetVolume(), itemData.GetWeight(), amount));
                    AddItemToDb(c, typeId, amount);

                }
            }
        }

        public static void AddItem(Client c, int typeId, int amount = 1)
        {
            List<Item> items = c.GetData("inventory_items");

            int invIndex = GetInventoryIndex(c, typeId);
            if (invIndex != -1)
            {
                Item stack = items[invIndex];
                stack.SetAmount(stack.GetAmount() + amount);
                UpdateItemInDb(c, typeId, stack.GetAmount() + amount);
            }
            else
            {
                if (items.Count == 5)
                {
                    c.SendNotification("Du hast die Max. Anzahl von Gegenständen im Inventar erreicht!");
                    return;
                }

                StaticItem itemData = GetItemData(typeId);
                if (itemData != null)
                {
                    items.Add(new Item(typeId, itemData.GetName(), itemData.GetCategory(), itemData.GetRarity(), itemData.GetVolume(), itemData.GetWeight(), amount));
                    AddItemToDb(c, typeId, amount);
                }
            }
        }

        public static void RemoveItem(Client c, int typeId, int amount = 1)
        {
            List<Item> items = c.GetData("inventory_items");

            int invIndex = GetInventoryIndex(c, typeId);
            if (invIndex != -1)
            {
                Item stack = items[invIndex];
                int newAmount = stack.GetAmount() - amount;
                if (newAmount < 1)
                {
                    items.RemoveAt(invIndex);
                    RemoveItemFromDb(c, typeId);
                }
                else
                {
                    stack.SetAmount(newAmount);
                    UpdateItemInDb(c, typeId, amount);
                }
            }
        }

        public static void RemoveAllItems(Client c, int typeId)
        {
            List<Item> items = c.GetData("inventory_items");

            int invIndex = GetInventoryIndex(c, typeId);
            if (invIndex != -1)
            {
                items.RemoveAt(invIndex);
                RemoveItemFromDb(c, typeId);
            }
        }

        public static bool HasItem(Client c, int typeId)
        {
            return GetItemStack(c, typeId) != null;
        }

        private static StaticItem GetItemData(int typeId)
        {
            foreach (StaticItem staticItem in itemList)
            {
                if (staticItem.GetTypeId() == typeId)
                    return staticItem;
            }
            return null;
        }

        private static int GetInventoryIndex(Client c, int typeId)
        {
            List<Item> items = c.GetData("inventory_items");
            if (items != null && items.Count >= 1)
            {
                int i = 0;
                foreach (Item item in items)
                {
                    if (typeId == item.GetTypeId())
                        return i;
                    i++;
                }
            }
            return -1;
        }

        private static Item GetItemStack(Client c, int typeId)
        {
            foreach (Item item in c.GetData("inventory_items"))
            {
                if (typeId == item.GetTypeId())
                    return item;
            }
            return null;
        }

        public static int GetItemAmount(Client c, int typeId)
        {
            return 0;
        }

        private static void AddItemToDb(Client c, int typeId, int amount)
        {
            DatabaseAPI.API db = DatabaseAPI.API.GetInstance();

            MySqlConnection conn = db.GetConnection();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO items (character_id, item_id, amount) VALUES (@character_id, @item_id, @amount)", conn);
            cmd.Parameters.AddWithValue("@character_id", c.GetData("character_id"));
            cmd.Parameters.AddWithValue("@item_id", typeId);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.ExecuteNonQuery();

            db.FreeConnection(conn);
        }

        private static void RemoveItemFromDb(Client c, int typeId)
        {
            DatabaseAPI.API db = DatabaseAPI.API.GetInstance();

            MySqlConnection conn = db.GetConnection();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM items WHERE character_id = @character_id AND item_id = @item_id", conn);
            cmd.Parameters.AddWithValue("@character_id", c.GetData("character_id"));
            cmd.Parameters.AddWithValue("@item_id", typeId);
            cmd.ExecuteNonQuery();

            db.FreeConnection(conn);
        }

        private static void UpdateItemInDb(Client c, int typeId, int amount)
        {
            DatabaseAPI.API db = DatabaseAPI.API.GetInstance();

            MySqlConnection conn = db.GetConnection();
            MySqlCommand cmd = new MySqlCommand("UPDATE items SET amount = @amount WHERE character_id = @character_id AND item_id = @item_id", conn);
            cmd.Parameters.AddWithValue("@character_id", c.GetData("character_id"));
            cmd.Parameters.AddWithValue("@item_id", typeId);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.ExecuteNonQuery();

            db.FreeConnection(conn);
        }

        public static void SyncItems(Client c)
        {
            List<Item> inventory = new List<Item>();

            DatabaseAPI.API db = DatabaseAPI.API.GetInstance();
            MySqlConnection conn = db.GetConnection();

            Dictionary<int, int> dbItems = new Dictionary<int, int>();
            MySqlCommand cmd = new MySqlCommand("SELECT item_id, amount FROM items WHERE character_id = @character_id", conn);
            cmd.Parameters.AddWithValue("@character_id", c.GetData("character_id"));
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dbItems.Add(reader.GetInt32("item_id"), reader.GetInt32("amount"));
            }
            reader.Close();
            db.FreeConnection(conn);

            foreach (KeyValuePair<int, int> entry in dbItems)
            {
                
                foreach (StaticItem itemData in itemList)
                {
                    if (itemData.GetTypeId() == entry.Key)
                    {
                        inventory.Add(new Item(entry.Key, itemData.GetName(), itemData.GetCategory(), itemData.GetRarity(), itemData.GetVolume(), itemData.GetWeight(), entry.Value));
                        break;
                    }
                }
            }

            c.SetData("inventory_items", inventory);
        }

        private void SyncConfigItemsFromDb(object state)
        {
            itemList.Clear();

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM cfg_items", conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                itemList.Add(new StaticItem(
                    reader.GetInt32("id"),
                    reader.GetString("name"),
                    reader.GetString("category"),
                    rarityNames[(reader.GetInt32("rarity") > rarityNames.Length) ? 0 : reader.GetInt32("rarity")],
                    reader.GetInt32("volume"),
                    reader.GetInt32("weight")
                ));
            }

            reader.Close();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);
        }
    }
}
