using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public DialogueSlide[] slides;
}

[System.Serializable]
public class DialogueSlide
{
    public string title;

    [TextArea(3, 10)]
    public string content;
    public Sprite image;
}
