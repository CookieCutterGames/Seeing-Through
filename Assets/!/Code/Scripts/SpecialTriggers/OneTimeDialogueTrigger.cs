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
        DialogueSystem.Show(
            content,
            isWelcomeDialogue
                ? () =>
                {
                    GameObject level1 = GameObject.Find("Level1");
                    if (level1 != null)
                    {
                        var triggers = level1.GetComponentsInChildren<TutorialPopupTrigger>(true);
                        foreach (var trigger in triggers)
                        {
                            trigger.gameObject.SetActive(true);
                        }
                    }
                }
                : () => { }
        );
        shown = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !shown)
        {
            Interact();
        }
    }
}
