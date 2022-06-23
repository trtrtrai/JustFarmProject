using UnityEngine;

namespace Assets.Scripts
{
    using Models.Plant;
    [System.Serializable]
    public class PlantData
    { 
        public int CurrentState;
        public float Timer;

        public PlantData() { }

        public PlantData(Plant plant)
        {
            CurrentState = plant.CurrentState;
            Timer = Time.realtimeSinceStartup - plant.BeginStateTime;
        }
    }
}
