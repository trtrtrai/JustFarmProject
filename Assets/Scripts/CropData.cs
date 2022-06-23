using System.Collections.Generic;

namespace Assets.Scripts
{
    using Models.Soil;

    [System.Serializable]
    public class CropData
    {
        public int LockCell;
        public List<PlantCellData> plantCells;

        public CropData() { }

        public CropData(List<PlantCell> list)
        {
            plantCells = new List<PlantCellData>();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsLocked) LockCell++;
                plantCells.Add(new PlantCellData(list[i]));
            }
        }
    }
}
