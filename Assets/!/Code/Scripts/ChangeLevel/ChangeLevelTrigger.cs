using UnityEngine;

public class ChangeLevelTrigger : MonoBehaviour
{
    [SerializeField]
    EScenes sceneName;

    public void DoChangeScene()
    {
        ChangeLevel.Instance.ChangeScene(sceneName);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            DoChangeScene();
    }
}
