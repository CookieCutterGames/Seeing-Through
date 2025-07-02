using UnityEngine;
using UnityEngine.InputSystem;

public class MemoryTrigger : MonoBehaviour
{
    [SerializeField]
    private DialogueData memory;

    [SerializeField]
    private string memoryID;

    bool userInRange;

    void Start()
    {
        if (PlayerMemoryController.capturedMemory.ContainsKey(memoryID))
        {
            gameObject.SetActive(false);
        }
        UserInput.Instance._interactAction.performed += Interact;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (!userInRange)
            return;
        DialogueSystem.Show(memory);
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
            UIManager.Instance.TurnOnInteractionIndicator();
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
