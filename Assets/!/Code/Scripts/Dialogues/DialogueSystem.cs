using System;
using UnityEngine;

public static class DialogueSystem
{
    private static DialogueUI currentDialogue;

    public static void Show(DialogueData data, Action onComplete = null)
    {
        if (currentDialogue == null)
            FindDialogueUI();

        if (currentDialogue != null)
        {
            currentDialogue.Show(data, onComplete);
            GameplayManager.PauseGame();
        }
    }

    public static void Hide()
    {
        if (currentDialogue != null)
        {
            currentDialogue.Hide();
            GameplayManager.ResumeGame();
        }
    }

    private static void FindDialogueUI()
    {
        currentDialogue = GameObject.FindFirstObjectByType<DialogueUI>(FindObjectsInactive.Include);
#if UNITY_EDITOR
        if (currentDialogue == null)
            Debug.LogError("DialogueUI not found in the scene.");
#endif
    }
}
