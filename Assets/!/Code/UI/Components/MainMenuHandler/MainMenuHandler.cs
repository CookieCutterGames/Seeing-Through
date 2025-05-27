using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level1UpStairs");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
