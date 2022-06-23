using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using Newtonsoft.Json;

namespace Assets.Scripts.Models.Item
{
    [Serializable]
    public class Storage : MonoBehaviour, ISaveable //a singleton?
    {
        [SerializeField] private int _capacity = 20;
        [SerializeField] private List<Item<IItem>> _items;
        private string path;
        private string fileName;

        public int Capacity => _capacity;
        public List<Item<IItem>> Items => _items; 
        public event EventHandler<CapacityChangeEventArgs> InventoryChange;

        private void Awake()
        {
            _items = new List<Item<IItem>>();
            path = Application.dataPath + "/Data/"; //Class Path ??
            fileName = "storage.txt";
        }

        public List<Item<IItem>> GetAll(string itemType) => Items.Where(i => i.Model.GetItemType().Name.Equals(itemType)).ToList();
        public Item<IItem> Find(string name) => Items.FirstOrDefault(i => i.Model.Name == name);
        public List<Item<IItem>> FindAll(string name) => Items.FindAll(i => i.Model.Name == name);

        public void SortItem() //i will sort Seed > Product > ... And compile same items doesn't max amount 
        {
            SaveSort();
            Load();
        }

        public bool StoreItem(Item<IItem> item)
        {
            var e = Find(item.Model.Name);
            if (!ReferenceEquals(e, null))
            {
                var i = FindAll(item.Model.Name).FirstOrDefault(i => i.Amount < i.Max);
                if (ReferenceEquals(i, null)) //store new Item
                {
                    return AddNewItem(item);
                }
                // i.Amount can store more
                var amount = i.Amount + item.Amount;
                var max = i.Max;
                i.Amount = amount;
                if (amount > max) // amount > max: store new Item
                {
                    item.Amount = amount - max;
                    StoreItem(item);
                }
              
                return true;
            }

            return AddNewItem(item); // if e is null it can go here
        }

        private bool AddNewItem(Item<IItem> item)
        {
            if (Items.Count == Capacity)
            {
                //Debug.Log("Storage is full.");
                return false;
            }

            _items.Add(item);
            item.ItemChange += ItemAmountChange;
            InventoryChange?.Invoke(item, new CapacityChangeEventArgs(Items.Count - 1, Items.Count));
            return true;
        }

        private void ItemAmountChange(object sender, ItemChangeEventArgs args)
        {
            if (args.NewValue == 0) //Destroy item in _items (real)
            {
                var item = sender as Item<IItem>;
                _items.Remove(item);
                InventoryChange?.Invoke(item, new CapacityChangeEventArgs(Items.Count, Items.Count - 1)); //send to destroy GameObject in Scene (UI)
                item.ItemChange -= ItemAmountChange;
            }
        }

        private void SaveSort()
        {
            List<string> data = new List<string>();

            foreach (var i in _items) if (i.Model.GetItemType().Name.Equals("Seed")) data.Add($"{i.Model.Name}-{i.Model.GetItemType().Name}-{i.Amount}");
            foreach (var i in _items) if (i.Model.GetItemType().Name.Equals("Product")) data.Add($"{i.Model.Name}-{i.Model.GetItemType().Name}-{i.Amount}");

            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter streamWriter = new StreamWriter(path + fileName))
            using (JsonWriter writer = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(writer, data);
            }
        }

        public void Save()
        {
            List<string> data = new List<string>();

            foreach (var i in Items)
            {
                data.Add($"{i.Model.Name}-{i.Model.GetItemType().Name}-{i.Amount}");
            }

            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter streamWriter = new StreamWriter(path + fileName))
            using (JsonWriter writer = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(writer, data);
            }
        }

        public void Load()
        {
            if (!File.Exists(path + fileName))
            {
                Save();
                return;
            }

            JsonSerializer serializer = new JsonSerializer();
            using (StreamReader sReader = new StreamReader(path + fileName))
            using (JsonReader jReader = new JsonTextReader(sReader))
            {
                var datas = serializer.Deserialize<List<string>>(jReader);
                _items = new List<Item<IItem>>();
                for (int i = 0; i < datas.Count; i++)
                {
                    var fields = datas[i].Split('-');
                    StoreItem(new Item<IItem>(fields[0], Type.GetType("Assets.Scripts.Models.Item." + fields[1])) { Amount = int.Parse(fields[2])});
                }
            }
        }
    }

    public class CapacityChangeEventArgs : EventArgs
    {
        public readonly int OldValue;
        public readonly int NewValue;

        public CapacityChangeEventArgs(int oldValue, int newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}