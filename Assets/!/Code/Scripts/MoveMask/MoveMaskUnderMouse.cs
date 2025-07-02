using UnityEngine;
using UnityEngine.InputSystem;

public class MoveMaskUnderMouse : MonoBehaviour
{
    void Update()
    {
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        Debug.Log(mouseScreenPosition.x + " na " + mouseScreenPosition.y);

        mouseScreenPosition.z = 0f;

        transform.position = mouseScreenPosition;
    }
}
