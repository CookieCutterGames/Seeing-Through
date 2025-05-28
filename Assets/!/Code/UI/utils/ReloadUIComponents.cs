using System.Collections.Generic;
using UnityEngine;

public class ReloadUIComponents : MonoBehaviour
{
    public bool ReloadChildrens;

    void Start()
    {
        if (ReloadChildrens)
        {
            List<(GameObject obj, bool wasActive)> childrenStates = new();

            foreach (Transform child in transform)
            {
                Debug.Log(child.name);
                GameObject childObj = child.gameObject;
                bool originalState = childObj.activeSelf;
                childrenStates.Add((childObj, originalState));

                childObj.SetActive(false);
                childObj.SetActive(true);
            }

            foreach (var (obj, wasActive) in childrenStates)
            {
                obj.SetActive(wasActive);
            }
        }
    }
}
