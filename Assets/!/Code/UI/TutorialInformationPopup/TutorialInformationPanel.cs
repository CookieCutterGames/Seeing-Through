using System.Collections;
using System.Linq;
using System.Text;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialInformationPanel : MonoBehaviour
{
    public TextMeshProUGUI content;
    public Image dialoguePanel;
    public Image smallestPanel;
    public Image mediumPanel;

    private Coroutine typingCoroutine;

    private bool isAnimating = false;

    public void Show(string newContent)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        content.text = "";

        isAnimating = true; // blokujemy wejście na czas animacji

        smallestPanel.gameObject.SetActive(true);
        mediumPanel.gameObject.SetActive(false);
        dialoguePanel.gameObject.SetActive(false);

        smallestPanel.rectTransform.localScale = Vector3.zero;
        mediumPanel.rectTransform.localScale = Vector3.zero;
        dialoguePanel.rectTransform.localScale = Vector3.zero;

        Sequence
            .Create()
            .Group(Tween.Scale(smallestPanel.rectTransform, Vector3.one, 0.3f, Ease.OutBack))
            .ChainCallback(() =>
            {
                mediumPanel.gameObject.SetActive(true);
            })
            .Chain(Tween.Scale(mediumPanel.rectTransform, Vector3.one, 0.3f, Ease.OutBack))
            .ChainCallback(() =>
            {
                dialoguePanel.gameObject.SetActive(true);
            })
            .Chain(Tween.Scale(dialoguePanel.rectTransform, Vector3.one, 0.3f, Ease.OutBack))
            .ChainCallback(() =>
            {
                isAnimating = false; // animacja skończona, odblokuj
                typingCoroutine = StartCoroutine(TypeText(newContent));
            });
        Invoke(nameof(Hide), 5f);
    }

    public void Hide()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        isAnimating = true; // blokujemy wejście na czas animacji zanikania

        Sequence
            .Create()
            .Group(Tween.Scale(dialoguePanel.rectTransform, Vector3.zero, 0.3f, Ease.InBack))
            .ChainCallback(() =>
            {
                dialoguePanel.gameObject.SetActive(false);
                mediumPanel.gameObject.SetActive(false);

                content.text = "";
                content.ForceMeshUpdate();
            })
            .Chain(Tween.Scale(mediumPanel.rectTransform, Vector3.zero, 0.3f, Ease.InBack))
            .ChainCallback(() =>
            {
                mediumPanel.gameObject.SetActive(false);
                smallestPanel.gameObject.SetActive(false);
            })
            .Chain(Tween.Scale(smallestPanel.rectTransform, Vector3.zero, 0.3f, Ease.InBack))
            .ChainCallback(() =>
            {
                isAnimating = false;
                smallestPanel.gameObject.SetActive(false);
            });
        UserInput.Instance.EnableMovement();
    }

    private string TranslateBindingName(string binding)
    {
        binding = binding.ToLowerInvariant().Trim();

        if (binding.Contains("right") && binding.Contains("mouse"))
            return "prawy przycisk myszy";
        if (binding.Contains("left") && binding.Contains("mouse"))
            return "lewy przycisk myszy";
        if (binding.Contains("middle") && binding.Contains("mouse"))
            return "środkowy przycisk myszy";

        switch (binding)
        {
            case "rmb":
                return "prawy przycisk myszy";
            case "lmb":
                return "lewy przycisk myszy";
            case "mmb":
                return "środkowy przycisk myszy";
            case "space":
            case "spacebar":
                return "spacja";
            case "esc":
            case "escape":
                return "escape";
            case "enter":
                return "enter";
            case "tab":
                return "tabulator";
            case "shift":
            case "left shift":
            case "right shift":
                return "shift";
            default:
                return binding;
        }
    }

    private string ParseText(string text)
    {
        StringBuilder result = new StringBuilder();

        for (int i = 0; i < text.Length; )
        {
            if (text[i] == '{')
            {
                int end = text.IndexOf('}', i);
                if (end != -1)
                {
                    string keyName = text.Substring(i + 1, end - i - 1);
                    string rawBinding = UserInput
                        .Instance.GetActionByName(keyName)
                        .GetBindingDisplayString()
                        .Split(" ")[0];

                    string translated = TranslateBindingName(rawBinding);
                    result.Append(translated);
                    i = end + 1;
                    continue;
                }
            }

            result.Append(text[i]);
            i++;
        }

        return result.ToString();
    }

    private IEnumerator TypeText(string fullText)
    {
        string parsedText = ParseText(fullText);

        content.text = "";
        bool inRichText = false;

        foreach (char c in parsedText)
        {
            content.text += c;

            if (c == '<')
                inRichText = true;
            if (c == '>')
                inRichText = false;

            if (!inRichText)
                yield return new WaitForSeconds(Random.Range(0.01f, 0.04f));
        }
    }
}
