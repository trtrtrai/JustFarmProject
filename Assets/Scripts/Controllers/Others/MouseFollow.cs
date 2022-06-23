using UnityEngine;

namespace Assets.Scripts.Controllers.Other
{
    public class MouseFollow : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 10;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.GetComponentInParent<Transform>().position = new Vector3(position.x, position.y);
        }
    }
}

