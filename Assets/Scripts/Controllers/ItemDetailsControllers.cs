using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    using Models.Item;
    public class ItemDetailsControllers : MonoBehaviour
    {
        [SerializeField] public GameObject header;
        [SerializeField] public GameObject content;
        [SerializeField] Image itemImage;
        [SerializeField] Text itemNameLabel;
        [SerializeField] Text itemDescriptionField;
        [SerializeField] Slider slider;
        [SerializeField] InputField inputField;
        [SerializeField] Text ItemAmountTxt;
        [SerializeField] Text PriceSum;
        [SerializeField] Image PriceImage;
        [SerializeField] int pricePerOnce;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SelectedChange(object sender, ItemChangeEventArgs args)
        {
            if (args.NewValue == 0)
            {
                header.SetActive(false);
                content.SetActive(false);
                return;
            } 
            slider.maxValue = args.NewValue;
            ItemAmountTxt.text = "/" + args.NewValue.ToString();
            slider.value = 1;
        }

        public void DetailsDisplay(string details, Sprite itemImg, Sprite moneyImg)
        {
            var fields = details.Split('-'); // name-pricePerOnce-description-amount

            itemImage.sprite = itemImg;
            itemNameLabel.text = fields[0];
            itemDescriptionField.text = fields[2];
            slider.minValue = 1;
            slider.maxValue = int.Parse(fields[3]);
            slider.value = 1;
            inputField.text = slider.value.ToString();
            ItemAmountTxt.text = "/" + fields[3];
            pricePerOnce = int.Parse(fields[1]);
            PriceSum.text = (slider.value * pricePerOnce).ToString();
            PriceImage.sprite = moneyImg;

            if (!header.activeInHierarchy)
            {
                header.SetActive(true);
                content.SetActive(true);
            }
        }

        public void OnSliderValueChange()
        {
            PriceSum.text = (slider.value * pricePerOnce).ToString();
            inputField.text = slider.value.ToString();
        }

        public void ClampNumberInput()
        {
            var rs = int.TryParse(inputField.text, out int value);
            if (!rs) return;
            var txt = Mathf.Clamp(value, (int)slider.minValue, (int)slider.maxValue);
            if (slider.value != txt) slider.value = txt;
        }

        public void AddAmount()
        {
            slider.value += 1;
        }

        public void SubAmount()
        {
            slider.value -= 1;
        }
    }
}