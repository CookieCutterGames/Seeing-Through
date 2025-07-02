using UnityEngine;

public class UIManager : OneInstanceSingleton<UIManager>
{
    void Start()
    {
        UserInput.Instance._menuOpenCloseAction.performed += (v) =>
        {
            if (GameplayPauseMenuSystem.isActive)
                GameplayPauseMenuSystem.Hide();
            else
                GameplayPauseMenuSystem.Show();
        };
    }

    public InteractionIndicator interactionIndicator;

    public void TurnOnInteractionIndicator()
    {
        interactionIndicator.gameObject.SetActive(true);
    }

    public void TurnOffInteractionIndicator()
    {
        interactionIndicator.gameObject.SetActive(false);
    }
}
