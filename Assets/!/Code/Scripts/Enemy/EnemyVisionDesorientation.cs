using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EnemyVisionDesorientation : MonoBehaviour
{
    [SerializeField]
    private float maxDistortion = 0.3f;

    [SerializeField]
    private float distortionDistance = 5f;

    [SerializeField]
    private Transform ghost;

    private Material peripheralMaterial;
    private float currentDistortion;

    void Start()
    {
        var volume = GetComponent<Volume>();
        if (volume.profile.TryGet(out LensDistortion lensDistortion))
        {
            peripheralMaterial = new Material(Shader.Find("Custom/PeripheralShadow"));
        }
    }

    void Update()
    {
        if (ghost == null)
            return;

        float distanceToGhost = Vector3.Distance(transform.position, ghost.position);
        currentDistortion = Mathf.Lerp(maxDistortion, 0, distanceToGhost / distortionDistance);

        peripheralMaterial.SetFloat("_GhostDistance", distanceToGhost);
        peripheralMaterial.SetFloat("_ShadowIntensity", Mathf.PingPong(Time.time * 0.5f, 0.3f));
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, peripheralMaterial);
    }
}
