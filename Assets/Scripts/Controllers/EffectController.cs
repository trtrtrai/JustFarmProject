using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class EffectController : MonoBehaviour
    {
        public List<RectTransform> topToBottom;
        public List<Image> blurs;
        public float speed;
        public float blurSpeed;
        public Button BtnSkip;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(topToBottom[0].transform.localPosition.y);
            if (topToBottom[0].transform.localPosition.y > 0)
            {
                topToBottom[0].transform.localPosition = new Vector3(topToBottom[0].transform.localPosition.x, topToBottom[0].transform.localPosition.y - speed * Time.deltaTime);
                var color = blurs[0].color;
                if (color.a <= 0.2f) blurs[0].color = new Color(color.r, color.g, color.b, color.a + blurSpeed * Time.deltaTime);
            }
            else
            {
                SkipAnimation();
            }
        }

        public void SkipAnimation()
        {
            topToBottom[0].transform.localPosition = new Vector3(topToBottom[0].transform.localPosition.x, 0);
            var color = blurs[0].color;
            blurs[0].color = new Color(color.r, color.g, color.b, 0.2f);
            BtnSkip.gameObject.SetActive(false);
            enabled = false;
        }
    }
}