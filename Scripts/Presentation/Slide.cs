using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    public int NumComponentsRemaining
    {
        get { return slideComponents.Count; }
    }

    [SerializeField]
    private List<GameObject> slideComponents;

    public void ActivateNextComponent()
    {
        if (NumComponentsRemaining > 0)
        {
            GameObject component = slideComponents[0];
            slideComponents.RemoveAt(0);
            component.SetActive(true);
        }
    }
}
