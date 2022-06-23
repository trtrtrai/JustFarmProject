using System;

namespace Assets.Scripts.Models.Item
{
    [Serializable]
    public class Product : ItemObject
    {
        private static string _path = "Products";

        public Product(string name) : base(name, _path) { }

        public override Type GetItemType()
        {
            return typeof(Product);
        }

        public override string GetName()
        {
            return Name;
        }
    }
}
