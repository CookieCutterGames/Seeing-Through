using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class OneTimeDialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private DialogueData content;
    public static bool shown = false;

    public bool isWelcomeDialogue;

    private void Interact()
    {
        Action onCompleteCallback = isWelcomeDialogue ? EnableTutorialPopupTriggers : () => { };
        DialogueSystem.Show(content, onCompleteCallback);
        shown = true;
    }

    private void EnableTutorialPopupTriggers()
    {
        try
        {
            var triggers = FindObjectsByType<TutorialPopupTrigger>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

            foreach (var trigger in triggers)
            {
                if (trigger != null)
                {
                    trigger.gameObject.SetActive(true);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to enable tutorial popup triggers: {e.Message}");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !shown)
        {
            Interact();
        }
    }
}
