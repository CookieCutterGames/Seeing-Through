using UnityEngine;
using UnityEngine.InputSystem;

public class Hideout : MonoBehaviour
{
    [SerializeField]
    private bool userInRange;

    private void Start()
    {
        UserInput.Instance._interactAction.performed += Interact;
    }

    private void OnDestroy()
    {
        UserInput.Instance._interactAction.performed -= Interact;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (!userInRange)
            return;

        SpriteRenderer spriteRenderer =
            Player.Instance.gameObject.GetComponentInChildren<SpriteRenderer>();

        if (Player.Instance.isHiding)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            Player.Instance.isHiding = false;
            UserInput.Instance.EnableAbilites();
            UserInput.Instance.EnableMovement();
        }
        else
        {
            Debug.Log("hiding");
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            Debug.Log("hiding1");
            Player.Instance.isHiding = true;
            Debug.Log("hiding2");
            UserInput.Instance.DisableAbilites();
            Debug.Log("Disabled");
            UserInput.Instance.DisableMovement();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            userInRange = true;
            UIManager.Instance.TurnOnInteractionIndicator();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            userInRange = false;
            UIManager.Instance.TurnOffInteractionIndicator();
        }
    }
}
