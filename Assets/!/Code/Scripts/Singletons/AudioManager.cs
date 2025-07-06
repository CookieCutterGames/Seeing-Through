using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private float _mainVolume = 0.75f;
    public float MainVolume
    {
        get => _mainVolume;
        set
        {
            _mainVolume = value;
            UpdateMusicVolume();
        }
    }

    [SerializeField]
    private float _musicVolume = 0.75f;
    public float MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = value;
            UpdateMusicVolume();
        }
    }

    private void UpdateMusicVolume()
    {
        Debug.Log("updated music volume: " + (_musicVolume * MainVolume));
        if (musicAudioSource != null)
            musicAudioSource.volume = (_musicVolume * MainVolume);
    }

    [Header("General Audio")]
    [HideInInspector]
    public AudioSource globalAudioSource;

    [HideInInspector]
    public AudioSource worldAudioPrefab;

    [HideInInspector]
    public AudioSource musicAudioSource;

    [SerializeField]
    private AudioClip backgroundMusic;

    [SerializeField]
    private AudioClip memoryFragmentMusic;

    [SerializeField]
    private AudioClip[] doorSoundsEffect;
    public AudioClip pickupSoundEffect;

    public AudioClip changePageSoundEffect;
    private bool playingMemory;
    #region SingletonRegion
    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        SetupGlobalAudioSource();
        SetupWorldAudioPrefab();
        SetupMusicAudioSource();
    }

    void Start()
    {
        PlayMusic(backgroundMusic, MusicVolume);
    }

    private void SetupGlobalAudioSource()
    {
        GameObject globalAudioGO = new("GlobalAudioSource");
        globalAudioGO.transform.parent = transform;

        globalAudioSource = globalAudioGO.AddComponent<AudioSource>();
        globalAudioSource.playOnAwake = false;
        globalAudioSource.spatialBlend = 0f;
    }

    private void SetupWorldAudioPrefab()
    {
        GameObject prefabGO = new("WorldAudioPrefab");
        AudioSource audioSource = prefabGO.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;

        prefabGO.hideFlags = HideFlags.HideAndDontSave;
        worldAudioPrefab = audioSource;
    }

    private void SetupMusicAudioSource()
    {
        GameObject musicGO = new("MusicAudioSource");
        musicGO.transform.parent = this.transform;

        musicAudioSource = musicGO.AddComponent<AudioSource>();
        musicAudioSource.playOnAwake = false;
        musicAudioSource.loop = true;
        musicAudioSource.spatialBlend = 0f;
    }
    #endregion

    public void PlayMusic(AudioClip musicClip, float volume = 1f)
    {
        if (musicClip == null)
            return;

        musicAudioSource.clip = musicClip;
        musicAudioSource.volume = (_musicVolume * MainVolume);
        musicAudioSource.Play();
    }

    public void PlayChangePageEffect()
    {
        PlaySound(changePageSoundEffect);
    }

    public void PlayMemoryFragment()
    {
        StartCoroutine(PlayMemoryFragmentRoutine());
    }

    public void StopMemoryFragment()
    {
        StartCoroutine(StopMemoryFragmentRoutine());
    }

    private IEnumerator StopMemoryFragmentRoutine()
    {
        if (playingMemory)
        {
            yield return StartCoroutine(FadeMusicVolume(0f, 1f));
            musicAudioSource.clip = backgroundMusic;
            musicAudioSource.loop = true;
            musicAudioSource.Play();

            yield return StartCoroutine(FadeMusicVolume((_musicVolume * MainVolume), 0.4f));
            playingMemory = false;
        }
    }

    private IEnumerator PlayMemoryFragmentRoutine()
    {
        yield return StartCoroutine(FadeMusicVolume(0f, 0.1f));
        playingMemory = true;
        musicAudioSource.clip = memoryFragmentMusic;
        musicAudioSource.loop = true;
        musicAudioSource.volume = MusicVolume;
        musicAudioSource.Play();
    }

    private IEnumerator FadeMusicVolume(float targetVolume, float duration)
    {
        float startVolume = musicAudioSource.volume;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(startVolume, targetVolume, t / duration);
            yield return null;
        }

        musicAudioSource.volume = targetVolume;
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            globalAudioSource.PlayOneShot(clip);
    }

    public void PlaySoundAtPosition(AudioClip clip, Vector3 position)
    {
        if (clip == null)
            return;

        if (worldAudioPrefab == null)
        {
            SetupWorldAudioPrefab();
            if (worldAudioPrefab == null)
            {
                return;
            }
        }

        AudioSource source = Instantiate(worldAudioPrefab, position, Quaternion.identity);
        source.clip = clip;
        source.volume = _musicVolume * MainVolume;
        source.Play();
        Destroy(source.gameObject, clip.length);
    }

    internal void PlayDoorSound()
    {
        PlaySound(doorSoundsEffect[UnityEngine.Random.Range(0, doorSoundsEffect.Length - 1)]);
    }
}
