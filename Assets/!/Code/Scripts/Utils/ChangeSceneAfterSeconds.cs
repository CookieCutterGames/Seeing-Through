using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneAfterSeconds : MonoBehaviour
{
    public string TargetScene;

    public float time;

    void Update()
    {
        time -= Time.deltaTime;

        if (time < 0)
        {
            SceneManager.LoadScene(TargetScene);
        }
    }
}
