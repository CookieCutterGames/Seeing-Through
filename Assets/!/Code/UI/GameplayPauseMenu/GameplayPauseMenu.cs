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
        UIManager.Instance.gameplayPauseMenu = this;
        button.onClick.AddListener(
            delegate
            {
                UIManager.Instance.TurnOnPauseMenu();
            }
        );
    }
}
