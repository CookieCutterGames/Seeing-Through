using UnityEngine;
using UnityEngine.InputSystem;

public class MemoryTrigger : MonoBehaviour
{
    [SerializeField]
    private DialogueData memory;

    [SerializeField]
    private string memoryID;

    bool userInRange;

    [SerializeField]
    bool shouldCount = true;

    [SerializeField]
    bool isInteractable = true;

    void Start()
    {
        if (PlayerMemoryController.capturedMemory.ContainsKey(memoryID))
        {
            gameObject.SetActive(false);
        }
        if (isInteractable)
            UserInput.Instance._interactAction.performed += InteractPerformed;
    }

    private void InteractPerformed(InputAction.CallbackContext context)
    {
        Interact();
    }

    private void Interact()
    {
        if (!userInRange)
            return;
        DialogueSystem.Show(memory);
        if (shouldCount)
            PlayerMemoryController.capturedMemory.Add(memoryID, memory);
        GameObject.Find("DefaultUI").GetComponentInChildren<MemoryFoundAnimation>().PlayAnimation();
        AudioManager.Instance.PlayMemoryFragment();
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            userInRange = true;
            if (!isInteractable)
            {
                Interact();
            }
            else
            {
                UIManager.Instance.TurnOnInteractionIndicator();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            userInRange = false;
            UIManager.Instance.TurnOffInteractionIndicator();
        }
    }
}
