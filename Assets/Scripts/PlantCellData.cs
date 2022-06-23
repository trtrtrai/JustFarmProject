using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    using Models.Soil;

    [Serializable]
    public class PlantCellData
    {
        public bool IsLocked;
        public List<PlantData> Crop;
        public int PlantPerCell;
        public int CropInCell;
        public string PrefabCropName;
        /*public readonly float posX;
        public readonly float posY;
        public readonly float posZ;*/

        public PlantCellData() { }

        public PlantCellData(PlantCell cell)
        {
            IsLocked = cell.IsLocked;
            PlantPerCell = cell.PlantPerCell;
            CropInCell = cell.CropInCell;
            if (CropInCell > 0) PrefabCropName = cell.ListCrops[0].name;
            else PrefabCropName = "";
            /*var pos = cell.gameObject.transform.localPosition;
            posX = pos.x;
            posY = pos.y;
            posZ = pos.z;*/
            Crop = new List<PlantData>();

            for (int i = 0; i < CropInCell; i++)
            {
                Crop.Add(new PlantData(cell.ListCrops[i]));
            }
        }
    }
}
