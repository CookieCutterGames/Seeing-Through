using TMPro;
using UnityEngine;

public static class GameplayPauseMenuSystem
{
    private static GameplayPauseMenuPanel gameplayPauseMenuPanel;
    public static bool isActive = false;

    public static void Show()
    {
        if (gameplayPauseMenuPanel == null)
            FindGameplayPauseMenuPanel();
        gameplayPauseMenuPanel?.Show();
        GameplayManager.PauseGame();
        isActive = true;
    }

    public static void Hide()
    {
        gameplayPauseMenuPanel?.Hide();
        GameplayManager.ResumeGame();
        isActive = false;
    }

    private static void FindGameplayPauseMenuPanel()
    {
        gameplayPauseMenuPanel = GameObject.FindAnyObjectByType<GameplayPauseMenuPanel>(
            FindObjectsInactive.Include
        );
        if (gameplayPauseMenuPanel == null)
        {
#if UNITY_EDITOR
            Debug.LogError("Cannot find DialogueUI on scene");
#endif
        }
    }
}
