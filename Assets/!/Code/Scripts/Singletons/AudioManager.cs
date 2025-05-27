using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public float MainVolume = 75f;
    public float MusicVolume = 75f;

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
    }
    #endregion
}
