using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptionsHandler : MonoBehaviour
{
    [SerializeField]
    private Transform Container;

    [SerializeField]
    private GameObject RecordPrefab;
    private bool IsInitialized = false;

    void OnEnable()
    {
        if (IsInitialized)
            return;
        Initialization();
    }

    void Initialization()
    {
        if (RecordPrefab == null)
        {
            Debug.LogError("Prefab not setted up - AudioOptionsHandler");
            return;
        }
        if (Container == null)
        {
            Container = transform;
        }
        CreateRecord(
            RecordPrefab,
            AudioManager.Instance.MainVolume,
            "głośność ogólna".ToUpper(),
            value => AudioManager.Instance.MainVolume = value
        );
        CreateRecord(
            RecordPrefab,
            AudioManager.Instance.MusicVolume,
            "głośność muzyki".ToUpper(),
            value => AudioManager.Instance.MusicVolume = value
        );
        IsInitialized = true;
    }

    void CreateRecord(
        GameObject prefab,
        float progressBarFill,
        string name,
        Action<float> onValueChanged
    )
    {
        var obj = Instantiate(prefab, Container);
        if (obj.TryGetComponent(out KVProgressBar component))
        {
            component.key.text = name;
            component.value.value = progressBarFill;
            component.value.onValueChanged.AddListener(value => onValueChanged(value));
        }
    }
}
