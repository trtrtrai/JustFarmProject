using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.Controllers
{
    public class AudioManager : MonoBehaviour
    {
        private Dictionary<string, AudioSource> soundEffects;

        private void Awake()
        {
            soundEffects = new Dictionary<string, AudioSource>();
            gameObject.GetComponentsInChildren<AudioSource>().ToList().ForEach(a => { soundEffects.Add(a.gameObject.name, a); });
        }

        public bool PlayAudio(string name)
        {
            if (soundEffects.ContainsKey(name))
            {
                soundEffects[name].Play();
                return true;
            }

            return false;
        }

        public bool StopAudio(string name)
        {
            if (soundEffects.ContainsKey(name))
            {
                soundEffects[name].Stop();
                return true;
            }

            return false;
        }

        public bool Volume(string type, float vol)
        {
            switch (type)
            {
                case "Sound":
                    foreach (var item in soundEffects.Keys)
                    {
                        if (!item.ToLower().Contains("background")) soundEffects[item].volume = vol;
                    }
                    return true;
                case "Music":
                    foreach (var item in soundEffects.Keys)
                    {
                        if (item.ToLower().Contains("background")) soundEffects[item].volume = vol;
                    }
                    return true;
            }

            return false;
        }
    }
}