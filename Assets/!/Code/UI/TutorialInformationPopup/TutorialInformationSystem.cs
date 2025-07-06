using System;
using UnityEngine;

public static class TutorialInformationSystem
{
    private static TutorialInformationPanel tutorialInformationPanel;
    public static bool isActive = false;

    public static void Show(string content)
    {
        if (tutorialInformationPanel == null)
            FindGameplayPauseMenuPanel();

        tutorialInformationPanel?.Show(content);
        isActive = true;
    }

    public static void Hide()
    {
        tutorialInformationPanel?.Hide();
        isActive = false;
    }

    private static void FindGameplayPauseMenuPanel()
    {
        tutorialInformationPanel = GameObject.FindAnyObjectByType<TutorialInformationPanel>(
            FindObjectsInactive.Include
        );

        if (tutorialInformationPanel == null)
        {
#if UNITY_EDITOR
            Debug.LogError("Cannot find DialogueUI on scene");
#endif
        }
    }
}
