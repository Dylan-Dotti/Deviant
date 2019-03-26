using UnityEngine;
using UnityEngine.UI;

public class ImageColorLerp : PercentageLerper
{
    [SerializeField]
    private Color lerpColor;

    private Image image;
    private Color originalColor;

    public override float CurrentLerpPercentage
    {
        get => Vector4InverseLerp(originalColor,
            lerpColor, image.color);
    }

    protected override void UpdateLerpVariables(float lerpPercentage)
    {
        image.color = Color.Lerp(originalColor, 
            lerpColor, lerpPercentage);
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
    }
}
