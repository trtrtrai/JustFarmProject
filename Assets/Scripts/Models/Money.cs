using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Money
    {
        private string _name;
        private long _coin;

        public string Name { get => _name;}
        public long Coin 
        { 
            get => _coin;
            set 
            {
                if (value < 0) return;

                _coin = value;
            } 
        }

        public Sprite CoinSprite;
        public Money() { }
        public Money(long num, Sprite img) //price
        {
            _name = img.name;
            Coin = num;
            CoinSprite = img;
        }
        public Money(long num, string name) //load my money
        {
            _name = name;
            Coin = num;
            CoinSprite = Resources.Load<Sprite>("Currency/" + Name);
        }

        public string StrCoin => Coin.ToString();
    }
}