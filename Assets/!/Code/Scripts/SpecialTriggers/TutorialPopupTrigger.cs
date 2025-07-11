using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialPopupTrigger : MonoBehaviour
{
    [SerializeField]
    private string content;

    [SerializeField]
    public bool shown = false;

    [SerializeField]
    private string inputActionName = "Interact";

    InputAction inputAction;

    private bool isActive = false;

    private void Interact()
    {
        if (isActive || TutorialInformationSystem.isActive)
            return;

        isActive = true;

        TutorialInformationSystem.Show(content);

        shown = true;

        inputAction = UserInput.Instance.GetActionByName(inputActionName);
        inputAction.performed += HideTutorial;
    }

    void HideTutorial(InputAction.CallbackContext ctx)
    {
        TutorialInformationSystem.Hide();

        if (inputAction != null)
        {
            inputAction.performed -= HideTutorial;
            inputAction = null;
        }

        isActive = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !shown)
        {
            Interact();
        }
    }
}
