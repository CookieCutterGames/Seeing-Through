using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public static void PauseGame()
    {
        UserInput.Instance.DisableMovement();
        UserInput.Instance.DisableAbilites();
    }

    public static void ResumeGame()
    {
        UserInput.Instance.EnableMovement();
        UserInput.Instance.EnableAbilites();
    }

    public void StartLevel()
    {
        PauseGame();
    }

    public void EndLevel()
    {
        ResumeGame();
    }
}
