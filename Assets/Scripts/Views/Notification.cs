using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

namespace Assets.Scripts.Views
{
    using Assets.Scripts.Models;
    using Controllers;
    public class Notification : MonoBehaviour
    {
        public string Title;
        public Dictionary<Button, string> NavButtons; //button - label
        public NotificationManager NotifyManager;
        public GameObject TitleObj;
        public GameObject Content;
        public GameObject NavObj;

        private void Awake()
        {
            NotifyManager = gameObject.GetComponentInParent<NotificationManager>();
            TitleObj = gameObject.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject;
            Content = gameObject.transform.GetChild(1).gameObject;
            NavObj = gameObject.transform.GetChild(2).gameObject;
            NavButtons = new Dictionary<Button, string>();
        }

        private void DestroyThis()
        {
            NotifyManager.NotifyContainers.Remove(gameObject);
            NotifyManager.Mng.GameState.Pop();
            Destroy(gameObject);
        }

        public void Render(string title, string message, Money money, Action[] actions) //for buy Plant Field
        {
            Title = title;
            TitleObj.GetComponentInChildren<Text>().text = title.ToUpper();
            Content.GetComponentInChildren<Text>().text = message;

            var listBtn = NavButtons.Keys.ToList();
            var mn = listBtn[0].gameObject.transform.GetChild(1).gameObject;
            mn.GetComponent<Text>().text = "-" + money.StrCoin;
            mn.GetComponentInChildren<Image>().sprite = money.CoinSprite;

            foreach (var item in NavButtons)
            {
                item.Key.gameObject.transform.GetChild(0).GetComponent<Text>().text = item.Value.ToUpper();
            }

            listBtn[0].onClick.AddListener(() =>
            {
                NotifyManager.AuMng.PlayAudio("ButtonClicked");
                actions[0]?.Invoke();
                listBtn[0].onClick.RemoveAllListeners();
                DestroyThis();
            });

            listBtn[1].onClick.AddListener(() =>
            {
                NotifyManager.AuMng.PlayAudio("ButtonClicked");
                actions[1]?.Invoke();
                listBtn[1].onClick.RemoveAllListeners();
                DestroyThis();
            });

            if (listBtn.Count < 3) return;
            listBtn[2].onClick.AddListener(() =>
            {
                NotifyManager.AuMng.PlayAudio("ButtonClicked");
                actions[2]?.Invoke();
                listBtn[2].onClick.RemoveAllListeners();
                DestroyThis();
            });
        }

        public void Render(string title, string message) //for infomation 
        {
            Title = title;
            TitleObj.GetComponentInChildren<Text>().text = title.ToUpper();
            Content.GetComponentInChildren<Text>().text = message;

            foreach (var item in NavButtons)
            {
                item.Key.gameObject.transform.GetChild(0).GetComponent<Text>().text = item.Value.ToUpper();
                item.Key.onClick.AddListener(() =>
                {
                    NotifyManager.AuMng.PlayAudio("ButtonClicked");
                    item.Key.onClick.RemoveAllListeners();
                    DestroyThis();
                });
                
            }
        }

        public void Render(string title, Dictionary<Sprite, long> items)
        {
            Title = title;
            TitleObj.GetComponentInChildren<Text>().text = title.ToUpper();
            var listItemContainer = Content.GetComponentInChildren<CanvasGridControllers>();

            foreach (var item in items)
            {
                var i = Instantiate(Resources.Load<GameObject>("Prefabs/Messages/ItemUI"), listItemContainer.gameObject.transform);
                i.GetComponentInChildren<Image>().sprite = item.Key;
                i.GetComponentInChildren<Text>().text = " +" + item.Value.ToString();
            }

            var btn = NavButtons.Keys.ToList()[0];
            var label = NavButtons[btn];
            btn.GetComponentInChildren<Text>().text = label.ToUpper();
            btn.onClick.AddListener(() => 
            {
                NotifyManager.AuMng.PlayAudio("ButtonClicked");
                btn.onClick.RemoveAllListeners();
                DestroyThis();
            });
        }

        public void Render(string title, string message, Action[] actions) //for confirm decision => 2 button
        {
            Title = title;
            TitleObj.GetComponentInChildren<Text>().text = title.ToUpper();
            Content.GetComponentInChildren<Text>().text = message;

            foreach (var item in NavButtons)
            {
                item.Key.gameObject.transform.GetChild(0).GetComponent<Text>().text = item.Value.ToUpper();
            }

            var listBtn = NavButtons.Keys.ToList();
            listBtn[0].onClick.AddListener(() =>
            {
                NotifyManager.AuMng.PlayAudio("ButtonClicked");
                actions[0]?.Invoke();
                listBtn[0].onClick.RemoveAllListeners();
                DestroyThis();
            });

            listBtn[1].onClick.AddListener(() =>
            {
                NotifyManager.AuMng.PlayAudio("ButtonClicked");
                actions[1]?.Invoke();
                listBtn[1].onClick.RemoveAllListeners();
                DestroyThis();
            });
        }
    }
}
