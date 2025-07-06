using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoriesList : MonoBehaviour
{
    public GameObject MemorySlot;
    public Transform memoriesContainer;

    public Sprite capturedMemorySprite;
    public Sprite defaultMemorySprite;

    public TextMeshProUGUI memoryText;

    void OnEnable()
    {
        PopulateMemories();
        ClearMemoryText();
        AudioManager.Instance.PlayChangePageEffect();
    }

    void PopulateMemories()
    {
        foreach (Transform child in memoriesContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var memoryEntry in PlayerMemoryController.capturedMemory)
        {
            GameObject slot = Instantiate(MemorySlot, memoriesContainer);

            var combinedContent = string.Join(" ", memoryEntry.Value.slides.Select(s => s.content));
            AddClickListener(slot, combinedContent);

            Image image = slot.GetComponentInChildren<Image>();
            if (image != null)
            {
                image.sprite = capturedMemorySprite;
            }
        }

        int uncapturedCount =
            PlayerMemoryController.totalMemories - PlayerMemoryController.capturedMemory.Count;
        for (int i = 0; i < uncapturedCount; i++)
        {
            GameObject slot = Instantiate(MemorySlot, memoriesContainer);

            TMP_Text text = slot.GetComponentInChildren<TMP_Text>();
            if (text != null)
            {
                text.text = "???";
            }

            Image image = slot.GetComponentInChildren<Image>();
            if (image != null)
            {
                image.sprite = defaultMemorySprite;
            }
            AddClickListener(slot, "");
        }
    }

    void AddClickListener(GameObject slot, string content)
    {
        Button btn = slot.GetComponent<Button>();
        if (btn == null)
        {
            btn = slot.AddComponent<Button>();
        }
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => SetMemoryText(content));
    }

    void SetMemoryText(string content)
    {
        memoryText.text = content;
    }

    void ClearMemoryText()
    {
        memoryText.text = "";
    }
}
