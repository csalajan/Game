using UnityEngine;
using System.Collections;

public class StartMenuController : MonoBehaviour {

    public void StartGame()
    {
        Application.LoadLevel("Wind");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
