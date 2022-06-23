using Assets.Scripts.Models.Soil;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Models.Plant
{
    [RequireComponent(typeof(PlantInfo))]
    [RequireComponent(typeof(PlantState))]
    public class Plant : MonoBehaviour
    {
        public PlantInfo Info;
        public PlantState State;
        public PlantCell Cell; //??
        public SpriteRenderer Render;
        public int CurrentState = 1;
        private Harvest HarvestCrop;
        [SerializeField] private float _beginStateTime; // for save 

        public float BeginStateTime => _beginStateTime;

        private void Awake()
        {
            Info = gameObject.GetComponent<PlantInfo>();
            State = gameObject.GetComponent<PlantState>();
            Cell = gameObject.GetComponentInParent<PlantCell>();
            if (Cell == null) Destroy(gameObject);
            Render = gameObject.AddComponent<SpriteRenderer>();
            Render.sortingOrder = 1;
            name = Info.Name + "_Plant";
        }

        void Start()
        {
            Render.sprite = State.State[CurrentState - 1];
            HarvestCrop = HarvestProduct;
        }

        private IEnumerator Growing()
        {
            if (!CanHarvest())
            {
                if (!Info.IsRegrow && CurrentState == State.HarvestState + 1) { Debug.Log(name + " will not regrow"); StopAllCoroutines(); }

                yield return new WaitForSeconds(State.TimePerState[CurrentState - 1]);

                NextState();
            }
        }

        private IEnumerator Growing(float timer) //only Load
        {
            if (!CanHarvest())
            {
                if (!Info.IsRegrow && CurrentState == State.HarvestState + 1) { Debug.Log(name + " will not regrow"); StopAllCoroutines(); }

                yield return new WaitForSeconds(State.TimePerState[CurrentState - 1] - timer);

                NextState();
            }
        }

        public bool CanHarvest() => CurrentState == State.HarvestState;
        private float SetBeginStateTime() => _beginStateTime = Time.realtimeSinceStartup;

        private void NextState()
        {
            if (Info.IsRegrow && CurrentState == State.HarvestState + 1)
            {
                CurrentState = State.HarvestState;
                Render.sprite = State.State[CurrentState - 1];
            }
            else if (CurrentState != State.HarvestState + 1) Render.sprite = State.State[++CurrentState - 1];

            SetBeginStateTime();
            StartCoroutine(Growing());
        }

        public void Planted()
        {
            SetBeginStateTime();
            StartCoroutine(Growing());
        }

        public void Planted(int currentState, float timer) //only Load
        {
            StopAllCoroutines();
            CurrentState = currentState;
            if (CurrentState == State.HarvestState) return; //harvest state => stop
            if (!Info.IsRegrow && CurrentState == State.HarvestState + 1) return; //not regrow plant harvested

            SetBeginStateTime();
            StartCoroutine(Growing(timer));
        }

        private bool HarvestProduct()
        {
            if (CanHarvest())
            {
                NextState();
                return true;
            }
            else
            {
                Debug.Log("This tree don't ready to harvest.");
                return false;
            }
        }

        public bool TryHarvest() => HarvestCrop.Invoke();

        private delegate bool Harvest();
    }
}