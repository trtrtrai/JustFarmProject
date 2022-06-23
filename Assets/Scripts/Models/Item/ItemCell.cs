using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Models.Item
{
    using Scripts.Controllers;
    public class ItemCell : MonoBehaviour
    {
        public Image ItemImg;
        public Text AmountTxt;
        public string Type;
        public Item<IItem> Item;

        public void ChangeText(object sender, ItemChangeEventArgs args)
        {
            AmountTxt.text = args.NewValue.ToString();
        }

        public void SendItemDetails()
        {
            var mng = GameObject.Find("Storage").GetComponent<ItemManager>();
            mng.Selected = Item;
            mng.SendItemDetails($"{Item.Model.GetName()}-{Item.Model.GetIItemDetails()}-{Item.Amount}", ItemImg.sprite, Item.Model.Price.CoinSprite);
        }
    }
}
