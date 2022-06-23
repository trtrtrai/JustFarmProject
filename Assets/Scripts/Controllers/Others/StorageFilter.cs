using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers.Others
{
    public class StorageFilter : MonoBehaviour
    {
        [SerializeField] private Button[] listFilterButton;
        
        public void OnClick(int index)
        {
            listFilterButton[index].gameObject.GetComponent<Image>().color = listFilterButton[index].colors.selectedColor;

            foreach (var item in listFilterButton.Where(i=>!ReferenceEquals(i, listFilterButton[index])))
            {
                item.gameObject.GetComponent<Image>().color = item.colors.normalColor;
            }
        }
    }
}