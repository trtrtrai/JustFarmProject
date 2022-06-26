using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    using Models.Soil;
    using Newtonsoft.Json;
    using System.IO;

    public class CropManager : MonoBehaviour, ISaveable
    {
        [SerializeField] List<PlantCell> scripts;
        [SerializeField] CropData datas;

        public AudioManager AuMng;
        public Manager manager;
        public CropData Datas => datas;
        public int NumberOfYourField;

        public void Load()
        {
            if(!File.Exists(Application.dataPath + "/Data/cropDatas.txt"))
            {
                Save();
                return;
            }

            JsonSerializer serializer = new JsonSerializer();
            using (StreamReader sReader = new StreamReader(Application.dataPath + "/Data/cropDatas.txt"))
            using (JsonReader jReader = new JsonTextReader(sReader))
            {
                datas = serializer.Deserialize<CropData>(jReader);
            }
        }

        public void Save()
        {
            datas = new CropData(scripts);

            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter streamWriter = new StreamWriter(Application.dataPath + "/Data/cropDatas.txt"))
            using (JsonWriter writer = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(writer, datas);
            }
        }

        private void Awake()
        {
            scripts = new List<PlantCell>(gameObject.GetComponentsInChildren<PlantCell>());
        }

        private void Start()
        {
            Load();

            scripts.ForEach((s) => s.LoadData());
        }

        private void OnDestroy()
        {
            Save(); 
        }

        public void UnlockNewField(int index)
        {
            scripts[index].IsLocked = false;
            datas.LockCell--;
            datas.plantCells[index].IsLocked = false;
        }
    }
}
