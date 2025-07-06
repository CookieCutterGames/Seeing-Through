using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuLabelBasedOnMask : MonoBehaviour
{
    [SerializeField]
    private VisionMaskController visionMaskController;

    [SerializeField]
    private TMP_Text labelText;

    [SerializeField]
    private float fadeInDuration = 2f;

    private bool isShown = false;
    private bool isAnimated = false;

    void Start()
    {
        if (visionMaskController == null)
        {
            Debug.LogError("VisionMaskController reference is missing!");
        }

        if (labelText == null)
        {
            Debug.LogError("TMP_Text reference is missing!");
        }

        if (
            UserInput.Instance == null
            || UserInput.Instance._attack2Action == null
            || UserInput.Instance._attack2Action.bindings.Count == 0
        )
        {
            Debug.LogError("Attack2Action bindings not properly set in UserInput.");
            return;
        }

        var readable = InputControlPath.ToHumanReadableString(
            UserInput.Instance._attack2Action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );

        labelText.text =
            $"<color=#ffffff>Naciśnij <color=#C91419>{TutorialInformationPanel.TranslateBindingName(readable)}</color> aby użyć echa</color>";
        SetAlpha(0f);
    }

    void Update()
    {
        if (visionMaskController == null || visionMaskController.maskMaterial == null)
            return;

        float radius = visionMaskController.maskMaterial.GetFloat("_Radius");

        if (radius <= 0.01f && !isAnimated && !isShown)
        {
            isAnimated = true;
            StartCoroutine(FadeInAfterDelay(2f));
        }
        // Hide label when radius grows again
        else if (radius > 0.01f && isShown)
        {
            isShown = false;
            isAnimated = false;
            SetAlpha(0f);
        }
    }

    IEnumerator FadeInAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            if (!visionMaskController.canUse)
            {
                isShown = false;
                SetAlpha(0f);
                isAnimated = false;
                yield break;
            }
            if (visionMaskController.maskMaterial.GetFloat("_Radius") >= 0.1f)
            {
                break;
            }

            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeInDuration);
            SetAlpha(alpha);
            yield return null;
        }
        if (visionMaskController.maskMaterial.GetFloat("_Radius") <= 0.1f)
            SetAlpha(1f);
        isShown = true;
        isAnimated = false;
    }

    private void SetAlpha(float alpha)
    {
        if (labelText != null)
        {
            Color c = labelText.color;
            c.a = alpha;
            labelText.color = c;
        }
    }
}
