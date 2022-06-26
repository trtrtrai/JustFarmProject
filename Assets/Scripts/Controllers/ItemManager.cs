using Assets.Scripts.Models.Item;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] NotificationManager notify;
        [SerializeField] MoneyController moneyCtrl;
        [SerializeField] AudioManager AuMng;
        [SerializeField] private Storage inventory;
        [SerializeField] private List<GameObject> listItemCell;
        [SerializeField] private string selectedPage;
        [SerializeField] GameObject allPage;
        [SerializeField] GameObject allItemContainer;
        [SerializeField] GameObject storageInfo;
        [SerializeField] ItemDetailsControllers detailsControllers;
        [SerializeField] public Item<IItem> Selected;     

        public Storage Inventory => inventory;

        private void Awake()
        {
            listItemCell = new List<GameObject>();
            inventory = gameObject.AddComponent<Storage>();
            inventory.InventoryChange += UpdateStorageUI;
        }

        private void Start()
        {
            inventory.Load();

            storageInfo.GetComponent<Text>().text = $"{inventory.Items.Count}/{inventory.Capacity}";
        }

        private void OnDestroy()
        {
            inventory.InventoryChange -= UpdateStorageUI;
            inventory.Save();
        }

        private void UpdateStorageUI(object sender, CapacityChangeEventArgs args)
        {
            if (args.NewValue > args.OldValue) //add itemCell
            {
                CreateItemCellUI(inventory.Items[args.NewValue - 1], allItemContainer.transform);
            }
          
            if (args.NewValue < args.OldValue) //destroy itemCell
            {
                var item = sender as Item<IItem>;
                var obj = listItemCell.First(i => ReferenceEquals(i.GetComponent<ItemCell>().Item, item));
                item.ItemChange -= obj.GetComponent<ItemCell>().ChangeText;
                DestroyItemCell(obj);
            }

            storageInfo.GetComponent<Text>().text = $"{inventory.Items.Count}/{inventory.Capacity}"; //update item
        }

        private void CreateItemCellUI(Item<IItem> item, Transform transform)
        {
            var obj = Instantiate(Resources.Load<GameObject>("Prefabs/ItemCellButton"), transform);
            var itemCell = obj.GetComponentInChildren<ItemCell>();
            itemCell.Item = item;
            listItemCell.Add(obj);

            itemCell.ItemImg.sprite = item.Model.Image;
            obj.name = item.Model.Name;
            itemCell.AmountTxt.text = item.Amount.ToString();
            itemCell.Type = item.Model.GetItemType().Name;
            item.ItemChange += itemCell.ChangeText;
            if (!selectedPage.Equals("All") && itemCell.Type != selectedPage) obj.SetActive(false);

            obj.GetComponent<Button>().onClick.AddListener(() => { AuMng.PlayAudio("ButtonClicked"); });
        }

        private void DestroyItemCell(GameObject obj)
        {
            obj.GetComponent<Button>().onClick.RemoveAllListeners();
            listItemCell.Remove(obj);
            Destroy(obj);
        }

        public void OnClick(string type)
        {
            selectedPage = type;
            if (type.Equals("All"))
            {
                listItemCell.ForEach((i) => i.SetActive(true));
                return;
            }

            listItemCell.ForEach((i) =>
            {
                var itemCell = i.GetComponent<ItemCell>();
                if (itemCell.Type != type) i.SetActive(false);
                else i.SetActive(true);
            });
        }

        public void SendItemDetails(string details, Sprite itemImg, Sprite moneyImg)
        {
            Selected.ItemChange += detailsControllers.SelectedChange;
            detailsControllers.DetailsDisplay(details, itemImg, moneyImg);
        }

        public void SellItemSelected(Slider slider)
        {
            var sellAmount = (int)slider.value;
            var cal = Selected.Amount - sellAmount;
            Selected.Amount = cal;
            if (cal <= 0) Selected.ItemChange -= detailsControllers.SelectedChange;
            //don't need audio because this is add money
            notify.OpenDialog("Sell Successfull", new Dictionary<Sprite, long>() { { Selected.Model.Price.CoinSprite, Selected.Model.Price.Coin * sellAmount} }, null);
            moneyCtrl.AddMoney(Selected, sellAmount);
        }

        public void SortItem() 
        {
            listItemCell.ForEach(g => Destroy(g));
            listItemCell = new List<GameObject>();
            inventory.SortItem(); 
        }
    }
}