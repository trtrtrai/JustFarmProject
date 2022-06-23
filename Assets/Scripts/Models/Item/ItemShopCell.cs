using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Models.Item
{
    using Controllers;
    using System.Linq;

    public class ItemShopCell : MonoBehaviour
    {
        [SerializeField] Image itemImg;
        [SerializeField] Text nameLabel;
        [SerializeField] Text limitTxt;
        [SerializeField] Text priceTxt;
        [SerializeField] Image moneyImg;
        [SerializeField] Money price;
        [SerializeField] int amount;

        private ShopManager manager;
        private Button btn;

        public string Type;
        public string Name;
        public string Money;

        private void Awake()
        {
            btn = gameObject.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() => { manager.AuMng.PlayAudio("ButtonClicked"); });
        }

        // Start is called before the first frame update
        void Start()
        {
            manager = GameObject.Find("Shop").GetComponent<ShopManager>();
            manager.GetLimitAndAmount(Name, Money, out amount, out long price);
            this.price = new Money(price, Money);
            //Debug.Log(amount + " " + price);
            DisplayUI();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void DisplayUI()
        {
            if (Type == "Seed")
                nameLabel.text = Name.Substring(0, Name.IndexOf("Seed")) + " Seed";
            else nameLabel.text = Name;

            limitTxt.text = amount < 0 ? "99" : amount.ToString();
            priceTxt.text = price.StrCoin;
            moneyImg.sprite = price.CoinSprite;
            itemImg.sprite = Resources.LoadAll<Sprite>(Type + "s").FirstOrDefault(s => s.name == Name);
        }

        public void UpdateUI(string name, string money, int oldValue, int newValue)
        {
            if (Name.Equals(name) && price.Name.Equals(money))
            {
                if (newValue == 0)
                {
                    Destroy(gameObject);
                    //Debug.Log("Still run while destroyed"); //it's really "still run"
                    return;
                }
                else
                {
                    amount = newValue;
                    limitTxt.text = amount < 0 ? "99" : amount.ToString();
                }
            }
        }

        public void OnClick()
        {
            manager.DisplaySelectedItemShop(Type, Name, price.CoinSprite);
        }

        private void OnDestroy()
        {
            manager.AmountChange -= UpdateUI;
            btn.onClick.RemoveAllListeners();
        }
    }
}