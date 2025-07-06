using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameplayPauseMenuPanel : MonoBehaviour
{
    [SerializeField]
    Button resume;

    [SerializeField]
    Button mainMenu;

    [SerializeField]
    Button closeBtn;

    [SerializeField]
    Button optionsBtn;

    [SerializeField]
    Button memoriesListBtn;

    [SerializeField]
    Button hudButton;

    [SerializeField]
    GameObject memoriesListPanel;

    [SerializeField]
    GameObject optionsPanel;

    float timeOfExpand = 1f;

    IEnumerator expandPanel()
    {
        float startScale = 0f;
        float endScale = 1;
        float elapsed = 0f;
        while (elapsed < timeOfExpand)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / timeOfExpand);
            float currentScale = Mathf.Lerp(startScale, endScale, t);
            transform.localScale.Set(currentScale, 1, 1);
            yield return null;
        }
    }

    public void Show()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        transform.localScale.Set(0, 1, 1);
        expandPanel();
        GameplayManager.PauseGame();
        AudioManager.Instance.PlayChangePageEffect();
    }

    public void Hide()
    {
        transform.localScale.Set(0, 1, 1);
        transform.GetChild(1).gameObject.SetActive(false);
        GameplayManager.ResumeGame();
        optionsPanel.SetActive(false);
        memoriesListPanel.SetActive(false);
        AudioManager.Instance.PlayChangePageEffect();
    }

    void Start()
    {
        resume.onClick.AddListener(
            delegate
            {
                GameplayPauseMenuSystem.Hide();
            }
        );
        mainMenu.onClick.AddListener(
            delegate
            {
                ChangeLevel.Instance.ChangeScene(EScenes.Menu);
            }
        );
        closeBtn.onClick.AddListener(
            delegate
            {
                GameplayPauseMenuSystem.Hide();
            }
        );
        optionsBtn.onClick.AddListener(
            delegate
            {
                optionsPanel.SetActive(true);
            }
        );
        memoriesListBtn.onClick.AddListener(
            delegate
            {
                memoriesListPanel.SetActive(true);
            }
        );
        if (hudButton == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning(
                "GameplayPauseMenuPanel - Hud button is not initialized, finding by script"
            );
#endif
            hudButton = GameObject.Find("[HUD] PauseMenu").GetComponent<Button>();
        }
        hudButton.onClick.AddListener(
            delegate
            {
                GameplayPauseMenuSystem.Show();
            }
        );
    }
}
