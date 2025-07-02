using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameInformation : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI title;

    [SerializeField]
    TextMeshProUGUI content;

    void OnEnable() { }

    public void Show(string title, string content)
    {
        UserInput.Instance.DisableMovement();
        this.title.text = title;
        this.content.text = content;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Hide()
    {
        UserInput.Instance.EnableMovement();
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
