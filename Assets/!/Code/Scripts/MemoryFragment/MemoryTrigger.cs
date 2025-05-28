using UnityEngine;
using UnityEngine.InputSystem;

public class MemoryTrigger : MonoBehaviour
{
    [SerializeField]
    private DialoguePayload memory;
    bool userInRange;
    bool memoryInUsage;

    void Start()
    {
        UserInput.Instance._interactAction.performed += Interact;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        memoryInUsage = true;
        UIManager.Instance.TurnOnDialogue(memory);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            userInRange = true;
            UIManager.Instance.TurnOnInteractionIndicator();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            userInRange = false;
            memoryInUsage = false;
            UIManager.Instance.TurnOffInteractionIndicator();
        }
    }
}
