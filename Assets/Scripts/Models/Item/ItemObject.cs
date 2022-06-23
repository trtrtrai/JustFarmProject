using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Models.Item
{
    public abstract class ItemObject : IItem
    {
        protected string _name;
        protected Money _price;
        protected string _description;

        public string Name => _name;
        public Money Price => _price;
        public Sprite Image { get; set; }
        public string Description { get => _description; }

        protected ItemObject(string name, string path)
        {
            _name = name;

            Image = Resources.LoadAll<Sprite>(path).FirstOrDefault(s => s.name == name);

            // Load Price and Description from database
            DataLoader.GetItemPriceInfo(Name, out long price, out Sprite moneyImg);
            _price = new Money(price, moneyImg);
            DataLoader.GetItemDescription(Name, out _description);
        }

        public abstract string GetName();
        public abstract Type GetItemType();
        public string GetIItemDetails() => $"{Price.StrCoin}-{Description}";
    }
}