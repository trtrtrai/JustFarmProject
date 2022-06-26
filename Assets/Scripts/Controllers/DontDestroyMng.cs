using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class DontDestroyMng : MonoBehaviour
    {
        //Need to save into data
        public float MusicVolume;
        public float SoundVolume;
        public string ThisScene;

        // Start is called before the first frame update
        void Start()
        {
            //Don't destroy on load script
            var objs = GameObject.FindGameObjectsWithTag("DontDestroy"); //only right when have 1 obj don't destroy

            if (objs.Length > 1)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            //this is Create data in static class
            DataLoader.LoadItemObjPrice();
            DataLoader.LoadItemObjDes();
            DataLoader.LoadItemMax();
        }

        public void SwapScene(string name)
        {
            ThisScene = name;
            SceneManager.LoadScene(name);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}