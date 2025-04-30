using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayPauseMenu : MonoBehaviour
{
    void Awake()
    {
        InputSystem.actions.FindAction("TurnPauseMenu").performed += OnTurnAction;
        gameObject.SetActive(false);
    }

    void OnTurnAction(InputAction.CallbackContext obj)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
