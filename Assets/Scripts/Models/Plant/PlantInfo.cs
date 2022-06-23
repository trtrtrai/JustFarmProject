using UnityEngine;

namespace Assets.Scripts.Models.Plant
{
    public class PlantInfo : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _product;
        [SerializeField] private int _harvestAmount;
        [SerializeField] private bool _isRegrow;

        public string Name => _name;
        public Sprite Product => _product;
        public int HarvestAmount => _harvestAmount;
        public bool IsRegrow => _isRegrow;
    }
}