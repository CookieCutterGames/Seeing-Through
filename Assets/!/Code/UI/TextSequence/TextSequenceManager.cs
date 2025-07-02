using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSequenceManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image fadePanel;
    public TMP_Text dialogueText;
    public Button nextButton;

    [Header("Settings")]
    public float minTimeEffect = 1f;

    [TextArea(3, 10)]
    public List<string> texts;

    private int currentTextIndex = 0;
    private Coroutine typingCoroutine;

    private enum FadeSide
    {
        IN,
        OUT,
    }

    void Start()
    {
        nextButton.onClick.AddListener(OnNextClicked);
        dialogueText.text = "";
        nextButton.interactable = false;

        StartTyping(texts[currentTextIndex]);
    }

    void OnNextClicked()
    {
        nextButton.interactable = false;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = texts[currentTextIndex];
            typingCoroutine = null;
            nextButton.interactable = true;
            return;
        }

        currentTextIndex++;

        if (currentTextIndex < texts.Count)
        {
            dialogueText.text = "";
            StartTyping(texts[currentTextIndex]);
        }
        else
        {
            StartCoroutine(EndSequence());
        }
    }

    void StartTyping(string text)
    {
        typingCoroutine = StartCoroutine(TypeTextRoutine(text));
    }

    IEnumerator TypeTextRoutine(string text)
    {
        dialogueText.text = "";
        bool inRichText = false;
        foreach (char c in text)
        {
            if (c == '<')
                inRichText = true;
            if (c == '>')
                inRichText = false;

            dialogueText.text += c;

            if (!inRichText)
                yield return new WaitForSeconds(Random.Range(0.0f, 0.0f));
        }

        typingCoroutine = null;
        nextButton.interactable = true;
    }

    IEnumerator EndSequence()
    {
        Coroutine textAndButtonFade = StartCoroutine(FadeTextAndButton(FadeSide.OUT));

        yield return new WaitForSeconds(minTimeEffect / 2f);

        Coroutine panelFade = StartCoroutine(FadeGraphic(fadePanel, FadeSide.OUT, minTimeEffect));

        yield return textAndButtonFade;
        yield return panelFade;
        yield return null;

        gameObject.SetActive(false);
    }

    IEnumerator FadeTextAndButton(FadeSide side)
    {
        TMP_Text buttonText = nextButton.GetComponentInChildren<TMP_Text>();

        yield return StartCoroutine(FadeGraphic(dialogueText, side, minTimeEffect / 2f));
        yield return StartCoroutine(FadeGraphic(nextButton.image, side, minTimeEffect / 2f));
        yield return StartCoroutine(FadeGraphic(buttonText, side, minTimeEffect / 2f));
    }

    IEnumerator FadeGraphic(Graphic graphic, FadeSide side, float duration)
    {
        float elapsed = 0f;
        Color color = graphic.color;

        float startAlpha = (side == FadeSide.OUT) ? 1f : 0f;
        float endAlpha = (side == FadeSide.OUT) ? 0f : 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            graphic.color = color;
            yield return null;
        }

        color.a = endAlpha;
        graphic.color = color;
    }
}
