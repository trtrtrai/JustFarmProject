using System;

namespace Assets.Scripts.Models.Item
{
    [Serializable]
    public class Seed : ItemObject
    {
        private static string _path = "Seeds";

        public Seed(string name) : base(name, _path) { }

        public override Type GetItemType()
        {
            return typeof(Seed);
        }

        public override string GetName()
        {
            return Name.Substring(0, Name.IndexOf("Seed")) + " Seed";
        }
    }
}