using System;
using UnityEngine;

public static class DialogueSystem
{
    private static DialogueUI currentDialogue;
    public static bool shown = false;

    public static void Show(DialogueData data, Action onComplete = null)
    {
        shown = true;
        if (currentDialogue == null)
            FindDialogueUI();

        if (currentDialogue != null)
        {
            GameplayManager.PauseGame();
            UserInput.Instance.DisableMovement();
            GameObject.FindAnyObjectByType<GhostAI>().freezed = true;
            currentDialogue.Show(data, onComplete);
        }
    }

    public static void Hide()
    {
        shown = false;
        GameObject.FindAnyObjectByType<GhostAI>().freezed = false;
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

    internal static void Show(DialogueData content, object value)
    {
        throw new NotImplementedException();
    }
}
