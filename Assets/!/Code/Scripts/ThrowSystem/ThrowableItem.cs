using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowableItem : MonoBehaviour
{
    [SerializeField]
    bool userInRange;

    void Start()
    {
        UserInput.Instance._interactAction.performed += Interact;
    }

    void Interact(InputAction.CallbackContext ctx)
    {
        if (!userInRange)
            return;
        if (Player.Instance.isHoldingMug)
            return;

        Player.Instance.isHoldingMug = true;
        gameObject.SetActive(false);
        UIManager.Instance.TurnOffInteractionIndicator();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIManager.Instance.TurnOnInteractionIndicator();
            userInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIManager.Instance.TurnOffInteractionIndicator();
            userInRange = false;
        }
    }
}
