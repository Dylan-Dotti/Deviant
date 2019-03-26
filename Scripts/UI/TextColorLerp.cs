using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextColorLerp : PercentageLerper
{
    public override float CurrentLerpPercentage
    {
        get => Vector4InverseLerp(mainOriginalColor,
            mainLerpColor, text.color);
    }

    [SerializeField]
    private Color mainLerpColor;
    //[SerializeField]
    //private List<Color> mainLerpColors;
    [SerializeField]
    private Color outlineLerpColor;
    //[SerializeField]
    //private Color outlineLerpColors;

    private Color mainOriginalColor;
    private Color outlineOriginalColor;

    private Text text;
    private Outline outline;
    private float mainPercentPerColor;
    private float outlinePercentPerColor;

    private void Awake()
    {
        text = GetComponent<Text>();
        outline = GetComponent<Outline>();
        mainOriginalColor = text.color;
        outlineOriginalColor = outline.effectColor;
        //mainPercentPerColor = mainLerpColors.Count == 1 ?
          //  1 : 1 / (mainLerpColors.Count - 1);
    }

    protected override void UpdateLerpVariables(float lerpPercentage)
    {

        text.color = Vector4.Lerp(mainOriginalColor,
            mainLerpColor, lerpPercentage);
        outline.effectColor = Vector4.Lerp(outlineOriginalColor,
            outlineLerpColor, lerpPercentage);
    }
}
