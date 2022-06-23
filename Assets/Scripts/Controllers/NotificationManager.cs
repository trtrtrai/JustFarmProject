using UnityEngine;
using System;

namespace Assets.Scripts.Controllers
{
    using Views;
    using Models;
    using System.Collections.Generic;
    using UnityEngine.UI;

    public class NotificationManager : MonoBehaviour
    {
        public GameObject BG;
        public List<GameObject> NotifyContainers;
        public Manager Mng;
        public AudioManager AuMng;
        public MoneyController MoneyCtrl;

        private void Awake()
        {
            NotifyContainers = new List<GameObject>();
        }

        private void Update()
        {
            if (NotifyContainers.Count > 0) BG.SetActive(true);
            else BG.SetActive(false);
        }

        public void OpenDialog(string title, string message, Money money, Action[] actions) //ask do you want to buy new Fieldplant
        {
            Mng.GameState.Push("Notify");
            var obj = Instantiate(Resources.Load<GameObject>("Prefabs/Messages/ContainerNotification"), gameObject.transform);
            var script = obj.AddComponent<Notification>();
            Instantiate(Resources.Load<GameObject>("Prefabs/Messages/MessageBox"), script.Content.transform);

            var btnYes = Instantiate(Resources.Load<GameObject>("Prefabs/Messages/NavButton_MinusMoney"), script.NavObj.transform);
            var btnNo = Instantiate(Resources.Load<GameObject>("Prefabs/Messages/NavButton"), script.NavObj.transform);
            script.NavButtons.Add(btnYes.GetComponent<Button>(), "Yes");
            script.NavButtons.Add(btnNo.GetComponent<Button>(), "No");

            NotifyContainers.Add(obj);
            script.Render(title, message, money, actions);
        }

        public void OpenDialog(string title, string message)
        {
            Mng.GameState.Push("Notify");
            var obj = Instantiate(Resources.Load<GameObject>("Prefabs/Messages/ContainerNotification"), gameObject.transform);
            var script = obj.AddComponent<Notification>();
            Instantiate(Resources.Load<GameObject>("Prefabs/Messages/MessageBox"), script.Content.transform);

            var btn = Instantiate(Resources.Load<GameObject>("Prefabs/Messages/NavButton"), script.NavObj.transform);
            script.NavButtons.Add(btn.GetComponent<Button>(), "Ok");

            NotifyContainers.Add(obj);
            script.Render(title, message);
        }

        public void OpenDialog(string title, Dictionary<Sprite, long> items, Action[] actions, bool cancelButton = false) //notify you get a prize, loss item,.. cancel button for ask loss item
        {
            Mng.GameState.Push("Notify");
            var obj = Instantiate(Resources.Load<GameObject>("Prefabs/Messages/ContainerNotification"), gameObject.transform);
            var script = obj.AddComponent<Notification>();
            Instantiate(Resources.Load<GameObject>("Prefabs/Messages/HorizonListBox"), script.Content.transform);

            var btn = Instantiate(Resources.Load<GameObject>("Prefabs/Messages/NavButton"), script.NavObj.transform);
            script.NavButtons.Add(btn.GetComponent<Button>(), "Nice");

            NotifyContainers.Add(obj);
            if (!cancelButton) script.Render(title, items);
        }
        
        public void OpenDialog(string title, string message, Action[] actions) //for confirm decision
        {
            Mng.GameState.Push("Notify");
            var obj = Instantiate(Resources.Load<GameObject>("Prefabs/Messages/ContainerNotification"), gameObject.transform);
            var script = obj.AddComponent<Notification>();
            Instantiate(Resources.Load<GameObject>("Prefabs/Messages/MessageBox"), script.Content.transform);

            var btn1 = Instantiate(Resources.Load<GameObject>("Prefabs/Messages/NavButton"), script.NavObj.transform);
            script.NavButtons.Add(btn1.GetComponent<Button>(), "Do it!");
            var btn2 = Instantiate(Resources.Load<GameObject>("Prefabs/Messages/NavButton"), script.NavObj.transform);
            script.NavButtons.Add(btn2.GetComponent<Button>(), "Cancel");

            NotifyContainers.Add(obj);
            script.Render(title, message, actions);
        }
    }
}
