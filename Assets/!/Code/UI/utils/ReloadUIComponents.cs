using UnityEngine;

public class ReloadUIComponents : MonoBehaviour
{
    public bool ReloadChildrens;

    void Start()
    {
        if (ReloadChildrens)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
                child.gameObject.SetActive(false);
            }
        }
    }
}
