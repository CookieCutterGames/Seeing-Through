using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionIndicator : MonoBehaviour
{
    TextMeshProUGUI content;

    void Awake()
    {
        content = GetComponent<TextMeshProUGUI>();
        UIManager.Instance.interactionIndicator = this;
    }

    void OnEnable()
    {
        var readable = InputControlPath.ToHumanReadableString(
            UserInput.Instance._interactAction.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );

        content.text = $"<color=#ffffff>Naciśnij <color=#C91419>{readable}<color=#ffffff> aby użyć";
    }
}
