using System;
using UnityEngine;

public class UIManager : OneInstanceSingleton<UIManager>
{
    public GameplayPauseMenu gameplayPauseMenu;
    public GameObject gameplayPauseMenuPanel;

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
}
