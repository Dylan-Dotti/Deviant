using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSliderBar : MonoBehaviour
{
    [SerializeField]
    private Image background;
    [SerializeField]
    private Image foreground;

    [Header("Colors")]
    [SerializeField]
    private Color highColor;
    [SerializeField]
    private Color midColor;
    [SerializeField]
    private Color lowColor;

    [Header("Color Lerp Percentages")]
    [SerializeField]
    [Range(0f, 1f)]
    private float midLerpStartPercentage = 0.85f;
    [SerializeField]
    [Range(0f, 1f)]
    private float midLerpEndPercentage = 0.5f;
    [SerializeField]
    [Range(0f, 1f)]
    private float lowLerpStartPercentage = 0.5f;
    [SerializeField]
    [Range(0f, 1f)]
    private float lowLerpEndPercentage = 0.15f;


    public void SetPercentage(float percentage)
    {
        foreground.fillAmount = percentage;

        //change color
        if (percentage > midLerpStartPercentage)
        {
            foreground.color = highColor;
        }
        else if (percentage >= midLerpEndPercentage && percentage <= midLerpStartPercentage)
        {
            float lerpPercentage = (midLerpStartPercentage - percentage) /
                (midLerpStartPercentage - midLerpEndPercentage);
            foreground.color = Vector4.Lerp(highColor, midColor, lerpPercentage);
        }
        else if (percentage >= lowLerpEndPercentage && percentage <= lowLerpStartPercentage)
        {
            float lerpPercentage = (lowLerpStartPercentage - percentage) /
                (lowLerpStartPercentage - lowLerpEndPercentage);
            foreground.color = Vector4.Lerp(midColor, lowColor, lerpPercentage);
        }
        else if (percentage < lowLerpEndPercentage)
        {
            foreground.color = lowColor;
        }
    }
}
