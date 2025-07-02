using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TeleporterSystem : MonoBehaviour
{
    public Image fadePanel;

    public Transform spawnPoint;

    [SerializeField]
    float minTimeEffect = 1f;

    enum FadeSide
    {
        IN,
        OUT,
    }

    void Start()
    {
        fadePanel = Object.FindAnyObjectByType<FadePanel>().GetComponentInChildren<Image>();
    }

    private void Interact()
    {
        StartCoroutine(FadeAndTeleportPlayer());
    }

    void OnTeleporterEnded()
    {
        GameplayManager.Instance?.StartLevel();
    }

    IEnumerator FadeAndTeleportPlayer()
    {
        UserInput.Instance.DisableMovement();

        yield return StartCoroutine(FadeEffect(FadeSide.OUT));
        yield return new WaitForSeconds(0.25f);
        Object.FindAnyObjectByType<Player>().transform.position = spawnPoint.position;
        yield return StartCoroutine(FadeIn());
        OnTeleporterEnded();
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Interact();
        }
    }
}
