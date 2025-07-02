using UnityEngine;

public static class InGameInformationSystem
{
    private static InGameInformation inGameInformation;
    public static bool isActive = false;

    public static void Show(string title, string content)
    {
        if (inGameInformation == null)
            Find();
        inGameInformation?.Show(title, content);
        GameplayManager.PauseGame();
        isActive = true;
    }

    public static void Hide()
    {
        inGameInformation?.Hide();
        GameplayManager.ResumeGame();
        isActive = false;
    }

    private static void Find()
    {
        inGameInformation = GameObject.FindAnyObjectByType<InGameInformation>(
            FindObjectsInactive.Include
        );
        if (inGameInformation == null)
        {
#if UNITY_EDITOR
            Debug.LogError("Cannot find DialogueUI on scene");
#endif
        }
    }
}
