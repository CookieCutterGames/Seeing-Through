using UnityEngine;

public class TabSelector : MonoBehaviour
{
    private int _currentIndex;

    [SerializeField]
    private int _defaultIndex;

    public Transform container;
    public bool IsInitialized;

    void Start()
    {
        ChangeTab(_defaultIndex);
    }

    void OnEnable()
    {
        if (IsInitialized)
            return;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void ChangeTab(int index)
    {
        container.GetChild(_currentIndex).gameObject.SetActive(false);
        _currentIndex = index;
        container.GetChild(_currentIndex).gameObject.SetActive(true);
    }
}
