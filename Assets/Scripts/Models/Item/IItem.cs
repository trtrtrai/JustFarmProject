using System;
using UnityEngine;

namespace Assets.Scripts.Models.Item
{
    public interface IItem
    {
        public string Name { get; }
        public Money Price { get; }
        public Sprite Image { get; set; }
        public string Description { get;}

        string GetName();
        Type GetItemType();
        string GetIItemDetails();
    }
}
