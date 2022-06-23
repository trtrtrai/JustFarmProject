using UnityEngine;

namespace Assets.Scripts.Models.Plant
{
    [RequireComponent(typeof(PlantInfo))]
    public class PlantState : MonoBehaviour
    {
        [SerializeField] private Sprite[] _state;
        [SerializeField] float[] _timePerState;
        [SerializeField] int _harvestState;

        public Sprite[] State => _state;
        public float[] TimePerState => _timePerState;
        public int HarvestState => _harvestState;
    }
}