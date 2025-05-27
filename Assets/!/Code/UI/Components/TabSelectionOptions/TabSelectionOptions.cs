using TMPro;
using UnityEngine;

public class TabSelectionOptions : MonoBehaviour
{
    private int _currentIndex;

    [SerializeField]
    private int _defaultIndex;

    public Transform container;

    public Transform headerContainer;

    void Start()
    {
        ChangeTab(_defaultIndex);
    }

    public void ChangeTab(int index)
    {
        container.GetChild(_currentIndex).gameObject.SetActive(false);
        if (ColorUtility.TryParseHtmlString("#FFFFFF", out Color deactivatedColor))
        {
            headerContainer.GetChild(_currentIndex).GetComponent<TextMeshProUGUI>().color =
                deactivatedColor;
            headerContainer
                .GetChild(_currentIndex)
                .GetComponent<TextMeshProUGUI>()
                .ForceMeshUpdate();
        }
        container.GetChild(index).gameObject.SetActive(true);
        if (ColorUtility.TryParseHtmlString("#C91419", out Color activatedColor))
        {
            headerContainer.GetChild(index).GetComponent<TextMeshProUGUI>().color = activatedColor;
            headerContainer.GetChild(index).GetComponent<TextMeshProUGUI>().ForceMeshUpdate();
        }
        _currentIndex = index;
    }
}
