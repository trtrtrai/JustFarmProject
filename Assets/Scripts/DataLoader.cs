using Assets.Scripts.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public static class DataLoader
    {
        private static Dictionary<string, string> itemObjPrice; // "name=price-moneyName"
        private static Dictionary<string, string> itemObjDescription; // "name=description"
        private static Dictionary<string, int> itemMaxofType; // "type=max"
        public static readonly List<Money> PlantFieldUnLockPrice =
            new List<Money>()
            {
                new Money(500, "Coin"),
                new Money(1000, "Coin"),
                new Money(1500, "Coin"),
                new Money(2000, "Coin"),
                new Money(100, "Diamond"),
                new Money(300, "Diamond"),
                new Money(500, "Diamond"),
                new Money(800, "Diamond"),
            };

        public static bool GetItemPriceInfo(string name, out long price, out Sprite moneyImg)
        {
            if (!itemObjPrice.ContainsKey(name))
            {
                price = 0;
                moneyImg = null;
                return false;
            }

            var datas = itemObjPrice[name].Split('-');
            price = long.Parse(datas[0]);
            moneyImg = Resources.Load<Sprite>("Currency/" + datas[1]);
            return true;
        }

        public static bool GetItemDescription(string name, out string des)
        {
            if (!itemObjDescription.ContainsKey(name))
            {
                des = "";
                return false;
            }

            des = itemObjDescription[name];
            return true;
        }

        public static bool GetItemMax(string type, out int max)
        {
            if (!itemMaxofType.ContainsKey(type))
            {
                max = 0;
                return false;
            }

            max = itemMaxofType[type];
            return true;
        }

        public static bool LoadItemObjPrice()
        {
            itemObjPrice = new Dictionary<string, string>();
            var file = Application.dataPath + "/Data/ItemObject_Price.txt";
            if (!File.Exists(file))
            {
                return false;
            }

            JsonSerializer serializer = new JsonSerializer();
            using (StreamReader sReader = new StreamReader(file))
            using (JsonReader jReader = new JsonTextReader(sReader))
            {
                var datas = serializer.Deserialize<List<string>>(jReader);
                datas.ForEach(s => itemObjPrice.Add(s.Split('=')[0], s.Split('=')[1]));
            }

            return true;
        }

        public static bool LoadItemObjDes()
        {
            itemObjDescription = new Dictionary<string, string>();
            var file = Application.dataPath + "/Data/ItemObject_Description.txt";
            if (!File.Exists(file))
            {
                return false;
            }

            JsonSerializer serializer = new JsonSerializer();
            using (StreamReader sReader = new StreamReader(file))
            using (JsonReader jReader = new JsonTextReader(sReader))
            {
                var datas = serializer.Deserialize<List<string>>(jReader);
                datas.ForEach(s => itemObjDescription.Add(s.Split('=')[0], s.Split('=')[1]));
            }

            return true;
        }

        public static bool LoadItemMax()
        {
            itemMaxofType = new Dictionary<string, int>();
            var file = Application.dataPath + "/Data/Item_MaxofType.txt";
            if (!File.Exists(file))
            {
                return false;
            }

            JsonSerializer serializer = new JsonSerializer();
            using (StreamReader sReader = new StreamReader(file))
            using (JsonReader jReader = new JsonTextReader(sReader))
            {
                var datas = serializer.Deserialize<List<string>>(jReader);
                datas.ForEach(s => itemMaxofType.Add(s.Split('=')[0], int.Parse(s.Split('=')[1])));
            }

            return true;
        }
    }
}
