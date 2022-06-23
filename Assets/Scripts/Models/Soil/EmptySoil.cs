using UnityEngine;

namespace Assets.Scripts.Models.Soil
{
    public class EmptySoil : MonoBehaviour
    {
        [SerializeField] private PlantCell parent;
        [SerializeField] SpriteRenderer lockSprite;
        private bool isPlowed;

        public bool IsPlowed => isPlowed;

        private void Awake()
        {
            parent = gameObject.GetComponentInParent<PlantCell>();
        }

        public void InitialScene()
        {
            if (parent.IsLocked)
            {
                gameObject.GetComponent<SpriteRenderer>().color = parent.LockColor;
                DePlowedSoil();
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                lockSprite.sortingOrder = -1;
                if (parent.CropInCell > 0) PlowedSoil();
                else DePlowedSoil();
            }
        }

        public void DePlowedSoil()
        {
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            isPlowed = false;
        }

        public void PlowedSoil()
        {
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
            isPlowed = true;
        }
    }
}