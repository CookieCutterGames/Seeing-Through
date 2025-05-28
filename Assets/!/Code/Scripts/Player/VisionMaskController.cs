using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class VisionMaskController : MonoBehaviour
{
    public Material maskMaterial;
    public float expandMultiplier = 2f;
    public float expandDuration = 0.5f;
    public float holdDuration = 0.5f;
    public float shrinkDuration = 0.25f;

    private float originalRadius;
    private Coroutine anim;

    void Start()
    {
        if (maskMaterial != null)
            originalRadius = maskMaterial.GetFloat("_Radius");

        UserInput.Instance._attack2Action.performed += ExpandCircle;
    }

    public void ExpandCircle(InputAction.CallbackContext ctx)
    {
        if (anim != null)
            StopCoroutine(anim);
        anim = StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        float targetRadius = originalRadius * expandMultiplier;

        float t = 0f;
        while (t < expandDuration)
        {
            t += Time.deltaTime;
            float r = Mathf.Lerp(originalRadius, targetRadius, t / expandDuration);
            maskMaterial.SetFloat("_Radius", r);
            yield return null;
        }

        maskMaterial.SetFloat("_Radius", targetRadius);
        yield return new WaitForSeconds(holdDuration);

        t = 0f;
        while (t < shrinkDuration)
        {
            t += Time.deltaTime;
            float r = Mathf.Lerp(targetRadius, originalRadius, t / shrinkDuration);
            maskMaterial.SetFloat("_Radius", r);
            yield return null;
        }

        maskMaterial.SetFloat("_Radius", originalRadius);
        anim = null;
    }
}
