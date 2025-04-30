using UnityEngine;

public class ExampleInteractTrigger : MonoBehaviour, IInteractable
{
    public void Interact(GameObject trigger)
    {
        Debug.Log("Hello World");
    }
}
