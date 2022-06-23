using Assets.Scripts.Controllers;
using Assets.Scripts.Models.Item;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Models.Tool
{
    public class SeedTool : MonoBehaviour
    {
        [SerializeField] Text label;
        [SerializeField] Image Img;
        public Item<IItem> item;

        private void Awake()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            name = item.Model.Name;
            item.ItemChange += Item_ItemChange;
            label.text = item.GetNameAmountForm(item.Model.GetName());
            Img.sprite = item.Model.Image;
        }

        private void Item_ItemChange(object sender, ItemChangeEventArgs e)
        {
            if (e.NewValue == 0) Destroy(gameObject);
            else label.text = item.GetNameAmountForm(item.Model.GetName()); //sender == item
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Clicked()
        {
            gameObject.GetComponentInParent<ToolManager>().Choose(item);
        }

        private void OnDestroy()
        {
            item.ItemChange -= Item_ItemChange;
        }
    }
}