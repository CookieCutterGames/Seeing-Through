using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoryFoundAnimation : MonoBehaviour
{
    [Header("Settings")]
    public float fadeDuration = 1f;
    public float showDuration = 2.5f;
    public float scaleDuration = 0.4f;
    public float scaleUp = 1.1f;

    public void PlayAnimation()
    {
        Transform child = transform.GetChild(0);
        GameObject uiObject = child.gameObject;

        var text = child.GetComponentInChildren<TextMeshProUGUI>();
        var image = child.GetComponent<Image>();
        Transform targetTransform = text.transform;

        // Resetuj stan
        if (text != null)
        {
            text.alpha = 0;
        }

        if (image != null)
        {
            var color = image.color;
            color.a = 0;
            image.color = color;
        }

        targetTransform.localScale = Vector3.one;

        // Fade-in
        if (text != null)
            Tween.Alpha(text, 1f, fadeDuration);

        if (image != null)
            Tween.Color(
                image,
                new Color(image.color.r, image.color.g, image.color.b, 1f),
                fadeDuration
            );

        // Skalowanie: powiększ i wróć
        var scaleSequence = Sequence.Create();
        scaleSequence.Chain(Tween.Scale(targetTransform, Vector3.one * scaleUp, scaleDuration));
        scaleSequence.Chain(Tween.Scale(targetTransform, Vector3.one, scaleDuration));

        // Fade-out po czasie
        Tween.Delay(
            showDuration,
            () =>
            {
                if (text != null)
                    Tween.Alpha(text, 0f, fadeDuration);

                if (image != null)
                    Tween.Color(
                        image,
                        new Color(image.color.r, image.color.g, image.color.b, 0f),
                        fadeDuration
                    );
            }
        );
    }
}
