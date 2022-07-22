using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.Scripts.Controllers
{
    using Assets.Scripts.Models;
    using Models.Item;
    using Newtonsoft.Json;
    using System.IO;
    using UnityEngine.UI;

    public class ShopManager : MonoBehaviour, ISaveable
    {
        private string path;
        private string fileName;
        private Dictionary<string, List<string>> shopType; // {"type":"name"}
        private Dictionary<string, Dictionary<string, int>> shopDatas; // {name:{"money":"limit"}}
        private Dictionary<string, Dictionary<string, long>> shopPrice; // {name:{"money":"price"}}
        private Item<IItem> selected;
        private string selectedPage;
        [SerializeField] NotificationManager notify;

        public AudioManager AuMng;

        #region UI define
        [SerializeField] GameObject content;
        [SerializeField] GameObject buyCointainer;
        [SerializeField] Button buyBtn;
        [SerializeField] MoneyController moneyCtrl;
        [SerializeField] ItemManager itemMang;
        [SerializeField] Slider slider;
        [SerializeField] Image moneyBuyImg;
        [SerializeField] Text priceBuyTxt;
        [SerializeField] Text limitBuyTxt;
        [SerializeField] InputField inputField;
        #endregion

        public ShopAmountChange AmountChange;

        private void Awake()
        {
            shopType = new Dictionary<string, List<string>>();
            shopDatas = new Dictionary<string, Dictionary<string, int>>();
            shopPrice = new Dictionary<string, Dictionary<string, long>>();
        }

        private void Start()
        {
            Load();
        }

        public void Load()
        {
            if (!File.Exists(Application.streamingAssetsPath + "/Data/itemShop_Type.txt") && !File.Exists(Application.streamingAssetsPath + "/Data/itemShop_Data.txt") && !File.Exists(Application.streamingAssetsPath + "/Data/itemShop_Price.txt"))
            {
                Save();
                return;
            }

            JsonSerializer serializer = new JsonSerializer();
            using (StreamReader sReader = new StreamReader(Application.streamingAssetsPath + "/Data/itemShop_Type.txt"))
            using(StreamReader sReader2 = new StreamReader(Application.streamingAssetsPath + "/Data/itemShop_Data.txt"))
            using(StreamReader sReader3 = new StreamReader(Application.streamingAssetsPath + "/Data/itemShop_Price.txt"))
            using (JsonReader jReader = new JsonTextReader(sReader))
            using (JsonReader jReader2 = new JsonTextReader(sReader2))
            using (JsonReader jReader3 = new JsonTextReader(sReader3))
            {
                shopType = serializer.Deserialize<Dictionary<string, List<string>>>(jReader);
                shopDatas = serializer.Deserialize<Dictionary<string, Dictionary<string, int>>>(jReader2);
                shopPrice = serializer.Deserialize<Dictionary<string, Dictionary<string, long>>>(jReader3);
            }

            foreach (var item in shopType)//get type
            {
                foreach (var name in item.Value)//get name
                {
                    int i = 1;
                    foreach (var money in shopDatas[name])//get money
                    {
                        var obj = Instantiate(Resources.Load<GameObject>("Prefabs/ItemShop"), content.transform);
                        var script = obj.GetComponent<ItemShopCell>();
                        obj.name = $"{name}_{i++}";
                        script.Name = name;
                        script.Type = item.Key;
                        script.Money = money.Key;
                        AmountChange += script.UpdateUI;
                    }
                }
            }
        }

        public bool GetLimitAndAmount(string name, string money, out int limit, out long price)
        {
            limit = shopDatas[name][money];
            price = shopPrice[name][money];
            //Debug.Log(limit + " " + price);
            return true;
        }  

        public void Save()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter streamWriter = new StreamWriter(Application.streamingAssetsPath + "/Data/itemShop_Type.txt"))
            using (JsonWriter writer = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(writer, shopType);
            }

            using (StreamWriter streamWriter = new StreamWriter(Application.streamingAssetsPath + "/Data/itemShop_Data.txt"))
            using (JsonWriter writer = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(writer, shopDatas);
            }

            using (StreamWriter streamWriter = new StreamWriter(Application.streamingAssetsPath + "/Data/itemShop_Price.txt"))
            using (JsonWriter writer = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(writer, shopPrice);
            }
        }

        private void OnDestroy()
        {
            Save();
        }

        public void OnClick(string type) //Selected page
        {
            selectedPage = type;
            var scripts = new List<ItemShopCell>(gameObject.GetComponentsInChildren<ItemShopCell>(true));
            if (type.Equals("All"))
            {
                scripts.ForEach((i) => i.gameObject.SetActive(true));
                return;
            }

            scripts.ForEach((i) =>
            {
                if (i.Type != type) i.gameObject.SetActive(false);
                else i.gameObject.SetActive(true);
            });
        }

        public void BuyItemSelected(Slider slider)
        {
            var clearData = false;
            var buyAmount = (int)slider.value;
            var name = selected.Model.Name;
            var money = moneyBuyImg.sprite.name;
            var cal = Mathf.Clamp(shopDatas[name][money] - buyAmount, 0, int.MaxValue);

            selected.Amount = buyAmount;
            if (!itemMang.Inventory.StoreItem(selected)) //check inventory full
            {
                AuMng.PlayAudio("Warning");
                notify.OpenDialog("Notification", "Oops! Your inventory is full. Please get more slot.");
                return;
            }

            if (shopDatas[name][money] != -1)
            {
                AmountChange?.Invoke(name, money, shopDatas[name][money], cal);

                if (!ReferenceEquals(selected, null) && cal == 0)
                {
                    shopDatas[name][money] = 0;
                    buyCointainer.SetActive(false);
                    clearData = true;                         
                }
                else shopDatas[name][money] = cal; //update amount
            }

            if (moneyCtrl.TrySubMoney(new Money(shopPrice[name][money] * buyAmount, moneyBuyImg.sprite)))
            {
                AuMng.PlayAudio("GainItem");
                notify.OpenDialog("Congrats", new Dictionary<Sprite, long>() { { selected.Model.Image, buyAmount } }, null);
            }
            else
            {
                AuMng.PlayAudio("Warning");
                notify.OpenDialog("Notification", "Sorry! You don't enough money"); // accoding to logic code, this case will be never happens...
            }

            if (clearData)
            {
                var nameCount = shopDatas[name].Count;
                if (nameCount == 1) //if 1 item type name in shop -> remove all data
                {
                    shopType[selected.Model.GetItemType().Name].Remove(name);
                    shopDatas.Remove(name);
                    shopPrice.Remove(name);
                    Debug.Log("all");
                }
                else //remove part data don't nessessary
                {
                    shopDatas[name].Remove(money);
                    shopPrice[name].Remove(money);
                    Debug.Log("part");
                }
            }
            else DisplaySelectedItemShop(selected.Model.GetItemType().Name, name, moneyBuyImg.sprite);
        }

        #region Display UI Func
        public void DisplaySelectedItemShop(string type, string name, Sprite imgMoney)
        {
            selected = new Item<IItem>(name, Type.GetType("Assets.Scripts.Models.Item." + type));
            GetLimitAndAmount(name, imgMoney.name, out int amount, out long price);

            moneyBuyImg.sprite = imgMoney;
            slider.value = 0;
            slider.minValue = 0;
            int limit = amount < 0 ? 99 : amount;
            long limitBuy = moneyCtrl.GetPrice(imgMoney.name) / price;
            var limitRs = Mathf.Clamp(limitBuy, 0, limit);
            slider.maxValue = limitRs;
            limitBuyTxt.text = "/" + limitRs.ToString();
            priceBuyTxt.text = "-" + ((int)slider.value * price).ToString();

            buyCointainer.SetActive(true);
        }

        public void OnSliderValueChange()
        {
            priceBuyTxt.text = "-" + (slider.value * shopPrice[selected.Model.Name][moneyBuyImg.sprite.name]).ToString();
            inputField.text = slider.value.ToString();

            if (slider.maxValue == 0 || slider.value == 0) buyBtn.interactable = false;
            else buyBtn.interactable = true;
        }

        public void ClampNumberInput()
        {
            var rs = int.TryParse(inputField.text, out int value);
            if (!rs) return;
            var txt = Mathf.Clamp(value, (int)slider.minValue, (int)slider.maxValue);
            if (slider.value != txt) slider.value = txt;
        }

        public void AddAmount()
        {
            slider.value += 1;
        }

        public void SubAmount()
        {
            slider.value -= 1;
        }
        #endregion
    }

    public delegate void ShopAmountChange(string name, string money, int oldValue, int newValue);
}