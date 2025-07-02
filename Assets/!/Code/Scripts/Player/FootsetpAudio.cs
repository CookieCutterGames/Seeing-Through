using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepAudio : MonoBehaviour
{
    public LayerMask surfaceLayerMask; // Warstwy, które mogą mieć różne powierzchnie
    public float footstepInterval = 0.4f;

    [System.Serializable]
    public class SurfaceAudio
    {
        public string surfaceTag; // Np. "Grass", "Wood"
        public AudioClip[] footstepClips;
    }

    public List<SurfaceAudio> surfaceAudios;

    private AudioSource audioSource;
    private Rigidbody2D rb;
    private float stepTimer;
    private Dictionary<string, AudioClip[]> surfaceClipDict;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        surfaceClipDict = new Dictionary<string, AudioClip[]>();
        foreach (var s in surfaceAudios)
        {
            surfaceClipDict[s.surfaceTag] = s.footstepClips;
        }
    }

    void Update()
    {
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = footstepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void PlayFootstep()
    {
        StartCoroutine(PlayFootstepDelayed(0.1f));
    }

    IEnumerator PlayFootstepDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        string surfaceTag = DetectSurfaceTag();
        if (surfaceClipDict.TryGetValue(surfaceTag, out AudioClip[] clips) && clips.Length > 0)
        {
            var clip = clips[Random.Range(0, clips.Length)];
            AudioManager.Instance.PlaySoundAtPosition(clip, transform.position);
        }
    }

    string DetectSurfaceTag()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            0.1f,
            surfaceLayerMask
        );
        if (hit.collider != null)
        {
            return hit.collider.tag; // Upewnij się, że tagi są ustawione na kolizjach powierzchni
        }
        return "Default"; // lub inny fallback
    }
}
