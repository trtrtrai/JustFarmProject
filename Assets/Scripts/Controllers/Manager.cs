using Assets.Scripts.Controllers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public Stack<string> GameState;
    public DontDestroyMng DntDesMng;

    private void Awake()
    {
        DntDesMng = GameObject.Find("DontDestroy").GetComponent<DontDestroyMng>();
        GameState = new Stack<string>();
        GameState.Push("GameDisplay");
    }

    public void SetGameState(string name)
    {
        GameState.Push(name);
    }

    public void PopGameState() => GameState.Pop();

    public void SwapScene(string name) => DntDesMng.SwapScene(name);

    public void QuitGame() => DntDesMng.QuitGame();
}
