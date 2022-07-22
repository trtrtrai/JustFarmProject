using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    using Models;
    using Newtonsoft.Json;
    using System.IO;
    using Models.Item;

    public class MoneyController : MonoBehaviour, ISaveable
    {
        public Money Coin;
        public Money Diamond;
        public List<GameObject> listMoney;

        [SerializeField] AudioManager AuMng;
        private string path;
        private string fileName;

        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            Load();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            Save();
        }

        public void Save()
        {
            List<string> data = new List<string>();

            data.Add($"{Coin.Name}={Coin.Coin}");
            data.Add($"{Diamond.Name}={Diamond.Coin}");

            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter streamWriter = new StreamWriter(Application.streamingAssetsPath + "/Data/currency.txt"))
            using (JsonWriter writer = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(writer, data);
            }
        }

        public void Load()
        {
            var file = Application.streamingAssetsPath + "/Data/currency.txt";
            if (!File.Exists(file))
            {
                Save();
                return;
            }

            JsonSerializer serializer = new JsonSerializer();
            using (StreamReader sReader = new StreamReader(file))
            using (JsonReader jReader = new JsonTextReader(sReader))
            {
                var datas = serializer.Deserialize<List<string>>(jReader);

                var money = datas[0].Split('=');
                Coin = new Money(long.Parse(money[1]), money[0]);
                listMoney[0].GetComponent<Text>().text = Coin.StrCoin;

                money = datas[1].Split('=');
                Diamond = new Money(long.Parse(money[1]), money[0]);
                listMoney[1].GetComponent<Text>().text = Diamond.StrCoin;
            }
        }

        public void AddMoney(Item<IItem> item, int amount)
        {
            AuMng.PlayAudio("AddMoney");
            var price = item.Model.Price;
            switch (price.Name)
            {
                case "Coin":
                    CoinUpdate(amount* price.Coin);
                    break;
                case "Diamond":
                    DiamondUpdate(amount* price.Coin);
                    break;
            }
        }

        /*public void SubMoney(Money price, int amount) //for buy amount item
        {
            switch (price.Name)
            {
                case "Coin":
                    CoinUpdate(-amount * price.Coin);
                    break;
                case "Diamond":
                    DiamondUpdate(-amount * price.Coin);
                    break;
            }
        }*/

        public bool TrySubMoney(Money price)
        {
            switch (price.Name)
            {
                case "Coin":
                    if (Coin.Coin >= price.Coin) CoinUpdate(-price.Coin);
                    else return false;
                    break;
                case "Diamond":
                    if (Diamond.Coin >= price.Coin) DiamondUpdate(-price.Coin);
                    else return false;
                    break;
                default: return false;
            }

            AuMng.PlayAudio("SubMoney");
            return true;
        }

        public long GetPrice(string money)
        {
            switch (money)
            {
                case "Coin":
                    return Coin.Coin;
                case "Diamond":
                    return Diamond.Coin;
            }

            return 0;
        }

        private void CoinUpdate(long price)
        {
            Coin.Coin = Coin.Coin + price;
            listMoney[0].GetComponent<Text>().text = Coin.StrCoin;
        }

        private void DiamondUpdate(long price)
        {
            Diamond.Coin = Diamond.Coin + price;
            listMoney[1].GetComponent<Text>().text = Diamond.StrCoin;
        }
    }
}