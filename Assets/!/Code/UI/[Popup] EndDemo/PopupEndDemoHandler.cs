using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupEndDemoHandler : MonoBehaviour
{
    public string SceneName;

    public void ChangeScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
