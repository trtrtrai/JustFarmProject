using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers.Others
{
    public class ClickToSkip : MonoBehaviour
    {
        private void Start()
        {
            
        }

        public void OnClick()
        {
            gameObject.GetComponentInParent<EffectController>().SkipAnimation();
            enabled = false;
        }
    }
}