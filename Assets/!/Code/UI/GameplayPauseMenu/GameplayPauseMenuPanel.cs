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

    float timeOfExpand = 1f;

    void OnEnable()
    {
        transform.localScale.Set(0, 1, 1);
        expandPanel();
    }

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

    void OnDisable()
    {
        transform.localScale.Set(0, 1, 1);
    }

    void Start()
    {
        resume.onClick.AddListener(
            delegate
            {
                gameObject.SetActive(false);
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
                UIManager.Instance.TurnOffPauseMenu();
            }
        );
    }
}
