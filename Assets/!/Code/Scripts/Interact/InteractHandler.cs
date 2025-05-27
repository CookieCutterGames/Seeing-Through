using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CircleCollider2D))]
public class InteractHandler : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private GameObject lastInteractableObject;

    private float _interactionRange = 3f;
    public float InteractionRange
    {
        get => _interactionRange;
        set
        {
            if (value > 0)
            {
                _interactionRange = value;
                gameObject.GetComponent<CircleCollider2D>().radius = value;
            }
        }
    }

    void Start()
    {
        gameObject.GetComponent<CircleCollider2D>().radius = _interactionRange;
        InputSystem.actions.FindAction("Interact").performed += TriggerInteraction;
    }

    void TriggerInteraction(InputAction.CallbackContext obj)
    {
        if (lastInteractableObject != null)
        {
            lastInteractableObject.GetComponent<IInteractable>().Interact(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable _))
        {
            lastInteractableObject = collision.gameObject;
            InteractLabel.Instance.TurnOn();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (lastInteractableObject != null && collision.gameObject == lastInteractableObject)
        {
            lastInteractableObject = null;
        }
        if (lastInteractableObject == null)
        {
            if (InteractLabel.Instance != null)
                InteractLabel.Instance.TurnOff();
        }
    }
}
