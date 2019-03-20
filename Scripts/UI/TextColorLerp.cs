using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextColorLerp : Lerper
{
    [SerializeField]
    private Color mainLerpColor;
    [SerializeField]
    private Color outlineLerpColor;

    private Color mainOriginalColor;
    private Color outlineOriginalColor;

    private Text text;
    private Outline outline;

    private void Awake()
    {
        text = GetComponent<Text>();
        outline = GetComponent<Outline>();
        mainOriginalColor = text.color;
        outlineOriginalColor = outline.effectColor;
    }

    protected override IEnumerator LerpCR(bool forward, float duration)
    {
        Color lerpMainStartColor = text.color;
        Color lerpOutlineStartColor = outline.effectColor;
        Color lerpMainEndColor = forward ? mainLerpColor : mainOriginalColor;
        Color lerpOutlineEndColor = forward ? outlineLerpColor : outlineOriginalColor;
        float startTime = Time.time;
        for (float elapsed = 0; elapsed < duration;
            elapsed = Time.time - startTime)
        {
            float lerpPercentage = elapsed / duration;
            text.color = Vector4.Lerp(lerpMainStartColor,
                lerpMainEndColor, lerpPercentage);
            outline.effectColor = Vector4.Lerp(lerpOutlineStartColor,
                lerpOutlineEndColor, lerpPercentage);
            yield return null;
        }
        text.color = lerpMainEndColor;
        outline.effectColor = lerpOutlineEndColor;
        yield return StartCoroutine(base.LerpCR(forward, duration));
    }
}
