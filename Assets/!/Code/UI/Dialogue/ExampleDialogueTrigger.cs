using UnityEngine;

public class ExampleDialogueTrigger : MonoBehaviour
{
    public DialoguePayload dialogueToShow;

    public void Update()
    {
        if (Input.GetKey(KeyCode.U))
        {
            DialogueHandler.Instance.TurnOn(dialogueToShow);
        }
    }
}
