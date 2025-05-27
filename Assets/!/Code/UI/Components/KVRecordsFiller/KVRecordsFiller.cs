using System.Collections.Generic;
using UnityEngine;

public class KVRecordsFiller : MonoBehaviour
{
    [System.Serializable]
    public class KeyValuePair
    {
        public string key;
        public string value;
    }

    [SerializeField]
    private List<KeyValuePair> dictionaryList = new();
    private Dictionary<string, string> runtimeDictionary;

    private void Awake()
    {
        runtimeDictionary = new Dictionary<string, string>();
        foreach (var pair in dictionaryList)
        {
            if (!runtimeDictionary.ContainsKey(pair.key))
            {
                runtimeDictionary.Add(pair.key, pair.value);
            }
        }
    }
}
