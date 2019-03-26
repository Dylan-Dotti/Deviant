using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuButton : Button
{
    public List<LerpWaveText> TextComponents { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        TextComponents = new List<LerpWaveText>();
        Transform textLayoutObject = 
            GetComponentInChildren<HorizontalLayoutGroup>().transform;
        for (int i = 0; i < textLayoutObject.childCount; i++)
        {
            TextComponents.Add(textLayoutObject.GetChild(i).
                GetComponent<LerpWaveText>());
        }
    }
}
