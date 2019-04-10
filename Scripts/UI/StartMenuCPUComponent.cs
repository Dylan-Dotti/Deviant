using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuCPUComponent : MonoBehaviour
{
    public Color MainColor
    {
        get => components[0].material.GetColor(mainColorProperty);
        set => components.ForEach(c => c.material.SetColor(
            mainColorProperty, value));
    }

    public Color FresnelColor
    {
        get => components[0].material.GetColor(fresnelColorProperty);
        set => components.ForEach(c => c.material.SetColor(
            fresnelColorProperty, value));
    }

    public float FresnelStrength
    {
        get => components[0].material.GetFloat(fresnelStrengthProperty);
        set => components.ForEach(c => c.material.SetFloat(
            fresnelStrengthProperty, value));
    }

    [SerializeField][ColorUsage(true, true)]
    Color mainLerpColor;
    [SerializeField][ColorUsage(true, true)]
    Color fresnelLerpColor;

    private Color mainOriginalColor;
    private Color fresnelOriginalColor;

    [SerializeField]
    private List<Renderer> components;

    private readonly string mainColorProperty = "Color_4B842E6E";
    private readonly string fresnelColorProperty = "Color_AF7F0660";
    private readonly string fresnelStrengthProperty = "Vector1_7BA6298F";

    private void Awake()
    {
        if (components.Count == 0)
        {
            components = new List<Renderer>(
                GetComponentsInChildren<Renderer>());
            Material newMaterial = new Material(components[0].material);
            components.ForEach(c => c.material = newMaterial);
        }
        mainOriginalColor = MainColor;
        fresnelOriginalColor = FresnelColor;
    }

    public Coroutine LerpColorBlue(float duration)
    {
        //CancelLerping();
        return StartCoroutine(LerpColorCR(mainLerpColor,
            fresnelLerpColor, duration));
    }

    public Coroutine LerpColorRed(float duration)
    {
        //CancelLerping();
        return StartCoroutine(LerpColorCR(mainOriginalColor,
            fresnelOriginalColor, duration));
    }

    public Coroutine LerpColorPeriodic(FloatRange r2bDurationRange,
        FloatRange r2bIntervalRange, FloatRange b2rDurationRange,
        FloatRange b2rIntervalRange)
    {
        //CancelLerping();
        return StartCoroutine(LerpColorPeriodicCR(r2bDurationRange,
            r2bIntervalRange, b2rDurationRange, b2rIntervalRange));
    }

    public Coroutine LerpFresnelStrengthPeriodic(float direction,
        FloatRange durationRange, FloatRange intervalRange)
    {
        //CancelLerping();
        return StartCoroutine(LerpFresnelStrengthPeriodicCR(direction,
            durationRange, intervalRange));
    }

    public void CancelLerping()
    {
        StopAllCoroutines();
    }

    private IEnumerator LerpColorCR(Color mainEndColor, 
        Color fresnelEndColor, float duration)
    {
        Color mainStartColor = MainColor;
        Color fresnelStartColor = FresnelColor;
        float lerpStartTime = Time.time;
        for (float elapsed = 0; elapsed < duration; 
             elapsed = Time.time - lerpStartTime)
        {
            float lerpPercentage = Mathf.Sin(Mathf.PI / 2 * elapsed / duration);
            MainColor = Color.Lerp(mainStartColor, mainEndColor, lerpPercentage);
            FresnelColor = Color.Lerp(fresnelStartColor, fresnelEndColor, lerpPercentage);
            yield return null;
        }
        MainColor = mainEndColor;
        FresnelColor = fresnelEndColor;
    }

    private IEnumerator LerpColorPeriodicCR(FloatRange r2bDurationRange,
        FloatRange r2bIntervalRange, FloatRange b2rDurationRange, 
        FloatRange b2rIntervalRange)
    {
        Color mainColor = mainLerpColor;
        Color fresnelColor = fresnelLerpColor;
        while (true)
        {
            yield return new WaitForSeconds(r2bIntervalRange.RandomRangeValue);
            yield return StartCoroutine(LerpColorCR(mainLerpColor, fresnelLerpColor,
                r2bDurationRange.RandomRangeValue));
            yield return new WaitForSeconds(b2rIntervalRange.RandomRangeValue);
            yield return StartCoroutine(LerpColorCR(mainOriginalColor,
                fresnelOriginalColor, b2rDurationRange.RandomRangeValue));
        }
    }

    private IEnumerator LerpFresnelStrengthPeriodicCR(float direction, 
        FloatRange durationRange, FloatRange intervalRange)
    {
        //int direction = -3;
        while (true)
        {
            yield return new WaitForSeconds(intervalRange.RandomRangeValue);
            float originalStrength = FresnelStrength;
            float lerpStrength = originalStrength + direction;
            float lerpDuration = durationRange.RandomRangeValue;
            float lerpStartTime = Time.time;
            for (float elapsed = 0; elapsed < lerpDuration;
                 elapsed = Time.time - lerpStartTime)
            {
                float lerpPercent = elapsed / lerpDuration;
                FresnelStrength = Mathf.Lerp(originalStrength,
                    lerpStrength, lerpPercent);
                yield return null;
            }
            FresnelStrength = lerpStrength;
            direction *= -1;
        }
    }
}
