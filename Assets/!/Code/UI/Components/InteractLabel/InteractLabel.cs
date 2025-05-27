using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TextMeshProUGUI))]
public class InteractLabel : MonoBehaviour
{
    public static InteractLabel Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text =
            $"Press {InputControlPath.ToHumanReadableString(InputSystem.actions.FindAction("Interact").bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice)} to interact";
    }

    public void TurnOn()
    {
        gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
    }
}
