using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorSystem : MonoBehaviour
{
    bool userInRange;

    public Sprite OpenedDoor;

    void Start()
    {
        UserInput.Instance._interactAction.performed += Interact;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (!userInRange)
            return;
        AudioManager.Instance.PlayDoorSound();
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = OpenedDoor;
        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = OpenedDoor;
        GetComponents<BoxCollider2D>().ToList().ForEach((c) => c.enabled = false);
        UserInput.Instance._interactAction.performed -= Interact;
        StartCoroutine(PauseMovementForInteraction(0.4f));
    }

    public IEnumerator PauseMovementForInteraction(float time)
    {
        UserInput.Instance.DisableMovement();
        yield return new WaitForSeconds(time);
        UserInput.Instance.EnableMovement();
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
