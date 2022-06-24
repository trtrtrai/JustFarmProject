using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Controllers
{
    using Models.Tool;
    using Other;
    using Models.Item;
    using UnityEngine.UI;
    using System.Linq;

    public class ToolManager : MonoBehaviour
    {
        [SerializeField] RectTransform toolTsf;
        [SerializeField] GameObject content;
        [SerializeField] private GameObject _tool; // tool choose by player
        [SerializeField] private ItemManager itemManage;
        [SerializeField] List<GameObject> seeds; //automated
        [SerializeField] List<GameObject> tools; //need to put in scene
        [SerializeField] GameObject showButton;
        [SerializeField] Vector2 minAnchor;
        [SerializeField] Vector2 maxAnchor;
        private string selectedPage;

        public GameObject MyHand;
        public GameObject Tool => _tool;
        public Item<IItem> Selected;

        public ItemManager ItemManage => itemManage;

        private void Awake()
        {
            selectedPage = "Hide";
            seeds = new List<GameObject>();
            itemManage.Inventory.InventoryChange += Inventory_InventoryChange;

            itemManage.Inventory.GetAll("Seed").ForEach(t => CreateToolObj(t));
        }

        private void Start()
        {
            OnClick(selectedPage);
        }

        private void OnDestroy()
        {
            itemManage.Inventory.InventoryChange -= Inventory_InventoryChange;
        }

        private void Inventory_InventoryChange(object sender, CapacityChangeEventArgs e)
        {
            if (e.NewValue > e.OldValue)
            {
                var i = sender as Item<IItem>;

                if (i.Model.GetItemType().Name == "Seed") CreateToolObj(i);
            }       
        }

        private void CreateToolObj(Item<IItem> i)
        {
            i.ItemChange += T_ItemChange;
            var obj = Instantiate(Resources.Load<GameObject>("Prefabs/SeedTool"), content.transform);
            seeds.Add(obj);
            obj.GetComponent<SeedTool>().item = i;

            if (!selectedPage.Equals("Seed")) obj.SetActive(false);
        }

        private void T_ItemChange(object sender, ItemChangeEventArgs e)
        {
            if (e.NewValue == 0)
            {
                var seed = sender as Item<IItem>;
                seeds.Remove(seeds.First(s => ReferenceEquals(s.GetComponent<SeedTool>().item, seed)));
                seed.ItemChange -= T_ItemChange;
                var tool = MyHand.GetComponentInChildren<MouseFollow>();
                if (tool != null) Destroy(tool.gameObject);
            }
        }

        public void Choose(Item<IItem> item) //seed
        {
            _tool = MyHand.GetComponentInChildren<MouseFollow>() == null ? CreateTool() : MyHand.GetComponentInChildren<MouseFollow>().gameObject;
            Selected = item;

            Cursor.SetCursor(Selected.Model.Image.texture, Vector2.zero, CursorMode.Auto);
            _tool.name = Selected.Model.Name;
        }

        public void Choose(string name)
        {
            _tool = MyHand.GetComponentInChildren<MouseFollow>() == null ? CreateTool() : MyHand.GetComponentInChildren<MouseFollow>().gameObject;
            var img = Resources.Load<Sprite>("Prefabs/" + name);

            Cursor.SetCursor(img.texture, Vector2.zero, CursorMode.Auto);
            _tool.name = name;
        }

        private GameObject CreateTool()
        {
            var tool = Instantiate(Resources.Load<GameObject>("Prefabs/Tool"), MyHand.transform);
            tool.transform.localPosition = new Vector3(0, 0);

            return tool;
        }

        public void OnClick(string type)
        {
            selectedPage = type;
            switch (type)
            {
                case "Seed":
                    seeds.ForEach(s => s.SetActive(true));
                    tools.ForEach(t => t.SetActive(false));
                    break;
                case "Tool":
                    seeds.ForEach(s => s.SetActive(false));
                    tools.ForEach(t => t.SetActive(true));
                    break;
                case "Hide":
                    toolTsf.anchorMin = new Vector2();
                    toolTsf.anchorMax = new Vector2();
                    showButton.SetActive(true);
                    break;
            }
        }

        public void ShowToolPage()
        {
            toolTsf.anchorMin = minAnchor;
            toolTsf.anchorMax = maxAnchor;
            showButton.SetActive(false);
        }

        #region detect double click
        int tapTimes;
        float resetTimer = 0.2f;

        IEnumerator ResetTapTime()
        {
            yield return new WaitForSeconds(resetTimer);
            tapTimes = 0;
        }

        private void Update()
        {
            if (_tool != null && _tool.GetComponentsInChildren<Transform>().Length != 0)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    //Debug.Log("Clicked");
                    StartCoroutine("ResetTapTime");
                    tapTimes++;
                }

                if (tapTimes == 2)
                {
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    Destroy(_tool);
                }
            }
        }
        #endregion
    }
}