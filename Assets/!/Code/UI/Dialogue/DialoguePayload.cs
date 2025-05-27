using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue")]
public class DialoguePayload : ScriptableObject
{
    [Serializable]
    public class SingleDialogue
    {
        public string header;
        public string content;
        public TextAlignmentOptions headerTextAlignment = TextAlignmentOptions.Left;
    }

    public List<SingleDialogue> payloads = new();
}
