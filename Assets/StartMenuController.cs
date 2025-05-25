using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void OnStartClick()
    {
        SceneManager.LoadScene(1); // cyfa w nawiasie to do której sceny funkcja przekierowuje
    }
    public void OptionScene()
    {
        SceneManager.LoadScene(2);
    }
    public void MenuScene()
    {
        SceneManager.LoadScene(4);
    }
    public void ControlScene()
    {
        SceneManager.LoadScene(3);
    }
    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}