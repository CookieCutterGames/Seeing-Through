using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("LevelCombined");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
