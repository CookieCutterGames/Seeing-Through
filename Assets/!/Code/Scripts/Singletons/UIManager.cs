using UnityEngine;

public class UIManager : OneInstanceSingleton<UIManager>
{
    public GameplayPauseMenu gameplayPauseMenu;
    public GameObject gameplayPauseMenuPanel;
    public DialogueHandler dialogueHandler;
    public InteractionIndicator interactionIndicator;

    public void TurnOnPauseMenu()
    {
        UserInput.Instance.DisableMovement();
        gameplayPauseMenuPanel.SetActive(true);
    }

    public void TurnOffPauseMenu()
    {
        UserInput.Instance.EnableMovement();
        gameplayPauseMenuPanel.SetActive(false);
    }

    public void TurnOnDialogue(DialoguePayload payload)
    {
        UserInput.Instance.DisableMovement();
        dialogueHandler.TurnOn(payload);
    }

    public void TurnOffDialogue()
    {
        UserInput.Instance.EnableMovement();
        dialogueHandler.gameObject.SetActive(false);
    }

    public void TurnOnInteractionIndicator()
    {
        interactionIndicator.gameObject.SetActive(true);
    }

    public void TurnOffInteractionIndicator()
    {
        interactionIndicator.gameObject.SetActive(false);
    }
}
