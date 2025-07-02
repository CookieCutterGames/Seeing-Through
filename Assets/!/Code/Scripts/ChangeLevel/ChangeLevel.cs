using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeLevel : MonoBehaviour
{
    public static ChangeLevel Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool usedChangedScene;

    public Image fadePanel;

    [SerializeField]
    float minTimeEffect = 1f;

    enum FadeSide
    {
        IN,
        OUT,
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!usedChangedScene)
            return;
        usedChangedScene = false;
        fadePanel = FadePanel.Instance.GetComponentInChildren<Image>();

        if (fadePanel != null)
        {
            var color = fadePanel.color;
            color.a = 255;
            fadePanel.color = color;
            StartCoroutine(FadeIn());
        }
        GameplayManager.Instance?.StartLevel();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ChangeScene(EScenes scene)
    {
        usedChangedScene = true;
        StartCoroutine(FadeAndChangeScene(scene.ToString()));
    }

    IEnumerator FadeAndChangeScene(string sceneName)
    {
        UserInput.Instance.DisableMovement();
        yield return StartCoroutine(FadeEffect(FadeSide.OUT));
        yield return SceneManager.LoadSceneAsync(sceneName);
    }

    IEnumerator FadeIn()
    {
        UserInput.Instance.DisableMovement();
        yield return StartCoroutine(FadeEffect(FadeSide.IN));
        UserInput.Instance.EnableMovement();
    }

    IEnumerator FadeEffect(FadeSide side)
    {
        float elapsed = 0f;
        Color color = fadePanel.color;

        float startAlpha = (side == FadeSide.OUT) ? 0f : 1f;
        float endAlpha = (side == FadeSide.OUT) ? 1f : 0f;

        while (elapsed < minTimeEffect)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / minTimeEffect);
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadePanel.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadePanel.color = color;
    }
}
