using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class VisionMaskController : MonoBehaviour
{
    public Material maskMaterial;
    public float expandMultiplier = 2f;
    public float expandDuration = 0.5f;
    public float holdDuration = 0.5f;
    public float shrinkDuration = 0.25f;
    public float cooldownDuration = 1f;

    [SerializeField]
    private AudioClip usageAudio;

    private float originalRadius;
    private Coroutine anim;
    public bool canUse = true;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        if (maskMaterial != null)
            originalRadius = maskMaterial.GetFloat("_Radius");
        UserInput.Instance._attack2Action.performed += ExpandCircle;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        maskMaterial.SetFloat("_Radius", scene.name == EScenes.Menu.ToString() ? 0.001f : 0.1f);
        maskMaterial.SetFloat("_Softness", scene.name == EScenes.Menu.ToString() ? 0 : 0.15f);
    }

    public void ExpandCircle(InputAction.CallbackContext ctx)
    {
        if (EScenes.Menu.ToString() != SceneManager.GetActiveScene().name && !canUse)
            return;
        Object.FindAnyObjectByType<UIShockWave>()?.Triggered();
        canUse = false;
        UserInput.Instance.DisableMovement();

        if (anim != null)
            StopCoroutine(anim);

        anim = StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        AudioManager.Instance.PlaySound(usageAudio);
        yield return new WaitForSeconds(0.35f);
        float targetRadius = originalRadius * expandMultiplier;

        StartCoroutine(EnableMovementAfterDelay(expandDuration + (expandDuration / 2)));

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

        yield return new WaitForSeconds(cooldownDuration);
        canUse = true;
    }

    IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UserInput.Instance.EnableMovement();
    }
}
