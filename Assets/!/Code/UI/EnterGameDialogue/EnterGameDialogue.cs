using UnityEngine;

public class EnterGameDialogue : OneInstanceSingleton<EnterGameDialogue>
{
    public DialoguePayload content;

    bool shownDialogue = false;

    void Update()
    {
        if (!shownDialogue && UIManager.Instance.dialogueHandler != null)
        {
            UIManager.Instance.TurnOnDialogue(content);
            shownDialogue = true;
        }
    }
}
