using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecursiveUILerper : PercentageLerper
{
    private List<Image> childImageComponents;
    private List<Text> childTextComponents;

    public override float CurrentLerpPercentage => 0;

    protected override void UpdateLerpVariables(float lerpPercentage)
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        childImageComponents = new List<Image>(
            GetComponentsInChildren<Image>());
        childTextComponents = new List<Text>(
            GetComponentsInChildren<Text>());
    }


}
