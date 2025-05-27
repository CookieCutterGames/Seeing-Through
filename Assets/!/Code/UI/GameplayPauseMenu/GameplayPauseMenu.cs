using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GameplayPauseMenu : MonoBehaviour
{
    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Start()
    {
        button.onClick.AddListener(
            delegate
            {
                UIManager.Instance.TurnOnPauseMenu();
            }
        );
    }
}
