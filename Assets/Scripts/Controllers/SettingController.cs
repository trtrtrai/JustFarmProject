using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class SettingController : MonoBehaviour
    {
        public Manager Mng;
        public Slider Music;
        public Slider Sound;

        private void Start()
        {
            Music.value = Mng.DntDesMng.MusicVolume;
            Sound.value = Mng.DntDesMng.SoundVolume;
            gameObject.SetActive(false);
        }

        public void MusicChange()
        {
            Mng.DntDesMng.MusicVolume = Music.value;
        }

        public void SoundChange()
        {
            Mng.DntDesMng.SoundVolume = Sound.value;
        }
    }
}