using UnityEngine;
using Assets.Scripts.Controllers;
using System.Linq;

namespace Assets.Scripts.Models.Soil
{
    using Plant;
    using Item;
    using System.Collections.Generic;
    using System;

    public class PlantCell : MonoBehaviour
    {
        public int Index;
        public bool IsLocked;
        public Color LockColor;
        [SerializeField] private EmptySoil emptySoil;
        [SerializeField] private List<Plant> Crop;
        [SerializeField] private int _plantPerCell; //default =1
        [SerializeField] private int _cropInCell;
        [SerializeField] private ToolManager tool;
        [SerializeField] private ItemManager item;
        [SerializeField] private CropManager mng;
        [SerializeField] private MoneyController moneyCtrl;
        [SerializeField] private NotificationManager notify;

        public int CropInCell => _cropInCell;
        public int PlantPerCell => _plantPerCell;
        public List<Plant> ListCrops => Crop;

        private void Awake()
        {
            Crop = new List<Plant>(); //All Crop used in this script code with 1 clant per cell and will be update soon
        }

        private void Start()
        {
            //Debug.Log(mng.Datas.plantCells[Index - 1].IsLocked);
            var data = mng.Datas.plantCells[Index - 1];
            IsLocked = data.IsLocked;         

            _plantPerCell = data.PlantPerCell;
            _cropInCell = data.CropInCell;

            for (int i = 0; i < CropInCell; i++)
            {
                PlantCrop(data.PrefabCropName, data.Crop[i].CurrentState, data.Crop[i].Timer);
            }

            emptySoil.InitialScene();
        }

        private void Update()
        {
            if (IsLocked) gameObject.GetComponent<SpriteRenderer>().color = LockColor;
            else gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }

        private void OnMouseDown()
        {
            if (!mng.manager.GameState.Peek().Equals("GameDisplay")) return;

            if (IsLocked)
            {
                var lockIndex = mng.Datas.plantCells.Count - mng.Datas.LockCell - 4;
                var money = DataLoader.PlantFieldUnLockPrice[lockIndex]; // 4 is number of Plant Field initiate because DataLoader just have 8 price of Field
                string message = $"Do you want to buy new Plant Field with {money.Coin} {money.Name}?";

                mng.AuMng.PlayAudio("ButtonClicked");
                notify.OpenDialog("Confirm", message, money, new Action[]{
                    () => 
                    {
                        if (!moneyCtrl.TrySubMoney(money)) //not enough money
                        {
                            mng.AuMng.PlayAudio("Warning");
                            notify.OpenDialog("Notification", "Sorry! You don't have enough money.");
                        }
                        else //enough
                        {
                            mng.UnlockNewField(lockIndex + 4); // 4 is number of Plant Field initiate
                            mng.AuMng.PlayAudio("GainItem");
                            notify.OpenDialog("Notification", $"Congratulations on your new Plant Field. Now you have {lockIndex + 4 + 1}"); // 4 is number of Plant Field initiate
                            //notify.OpenDialog("string");
                        }
                    },
                    () => { },//btn No have Nothing
                });
                return;
            }

            if (tool.Tool != null)
            {
                //Debug.Log(tool.Tool.name);
                switch (tool.Tool.name)
                {
                    case "Shovel":
                        if (CropInCell > 0)
                        {
                            mng.AuMng.PlayAudio("Warning");
                            notify.OpenDialog("Warning", "This action can't be undo. Are you sure about to dig this plant?", new Action[]
                            {
                                DigPlant, //"Do it!" button
                                () => { } //"Cancel" button
                            });
                        }
                        break;
                    case "Hoe":
                        if (!emptySoil.IsPlowed)
                        {
                            mng.AuMng.PlayAudio("Soiling");
                            emptySoil.PlowedSoil();
                        }
                        break;
                    default:
                        if (emptySoil.IsPlowed)
                        {
                            string name = tool.Tool.name.Substring(0, tool.Tool.name.Length - "Seed".Length) + "_Plant";

                            //Debug.Log(name);  
                            PlantCrop(name);
                        }
                        break;
                }
            }
            else if (Crop.Count > 0 && Crop[0].CanHarvest())
            {
                if (Crop[0].TryHarvest())
                {
                    //Debug.Log("Harvest " + Crop[0].Info.Name);
                    mng.AuMng.PlayAudio("Harvest");
                    item.Inventory.StoreItem(new Item<IItem>(Crop[0].Info.Product.name, typeof(Product)) { Amount = Crop[0].Info.HarvestAmount});
                }
            }
        }

        private void DigPlant()
        {
            mng.AuMng.PlayAudio("Digging");
            Crop.ForEach(p => Destroy(p.gameObject));
            Crop = new List<Plant>();
            _cropInCell = 0;
            emptySoil.DePlowedSoil();
        }

        private void PlantCrop(string name)
        {
            if (_cropInCell >= _plantPerCell)
            {
                Debug.Log(this.name + " is full space to plant");
                return;
            }

            mng.AuMng.PlayAudio("GrowPlant");
            item.Inventory.FindAll(tool.Tool.name).FirstOrDefault(i => ReferenceEquals(i, tool.Selected)).Amount -= 1;
            Instantiate(Resources.Load<GameObject>("Prefabs/" + name), gameObject.transform);
            Crop.Add(gameObject.GetComponentInChildren<Plant>());
            Crop[0].gameObject.transform.localPosition = new Vector3(0, 0, 0);
            Crop[0].Planted();

            _cropInCell++;
        }

        private void PlantCrop(string name, int currentState, float timer) //only for load
        {     
            var obj = Instantiate(Resources.Load<GameObject>("Prefabs/" + name), gameObject.transform);
            var script = obj.GetComponent<Plant>();

            Crop.Add(script);
            Crop[0].gameObject.transform.localPosition = new Vector3(0, 0, 0);
            Crop[0].Planted(currentState, timer);
        }
    }
}