using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Bar")]
    [SerializeField]
    private RectTransform healthBarTransform;
    [SerializeField]
    private Image healthBarForeground;
    [SerializeField]
    private Image healthBarBackground;

    [Header("Colors")]
    [SerializeField]
    private Color greenColor;
    [SerializeField]
    private Color yellowColor;
    [SerializeField]
    private Color redColor;

    [Header("Color Lerp Percentages")]
    [SerializeField][Range(0f, 1f)]
    private float yellowLerpStartPercentage = 0.85f;
    [SerializeField][Range(0f, 1f)]
    private float yellowLerpEndPercentage = 0.5f;
    [SerializeField][Range(0f, 1f)]
    private float redLerpStartPercentage = 0.5f;
    [SerializeField][Range(0f, 1f)]
    private float redLerpEndPercentage = 0.15f;

    [Header("Fade Effect")]
    [SerializeField]
    private bool fadeEnabled = true;
    [SerializeField]
    private float fadeDuration = 2f;
    [SerializeField]
    private float fadeDelay = 5f;

    private float timeSinceLastChange;
    private float foregroundStartAlpha;
    private float backgroundStartAlpha;

    private void Start()
    {
        timeSinceLastChange = fadeDelay;
        foregroundStartAlpha = healthBarForeground.color.a;
        backgroundStartAlpha = healthBarBackground.color.a;
        if (fadeEnabled)
        {
            healthBarForeground.color = new Color(healthBarForeground.color.r, healthBarForeground.color.g,
                healthBarForeground.color.b, 0);
            healthBarBackground.color = new Color(healthBarBackground.color.r, healthBarBackground.color.g,
                healthBarBackground.color.b, 0);
        }
        enabled = false;
    }

    private void LateUpdate()
    {
        if (timeSinceLastChange > fadeDelay)
        {
            FadeOut();
            enabled = false;
        }
        timeSinceLastChange += Time.deltaTime;
    }

    public void SetHealthPercentage(float percentage)
    {
        healthBarTransform.localScale = new Vector3(Mathf.Clamp01(percentage), 
            healthBarTransform.localScale.y, healthBarTransform.localScale.z);

        //change color
        if (percentage >= yellowLerpEndPercentage && percentage <= yellowLerpStartPercentage)
        {
            float lerpPercentage = (yellowLerpStartPercentage - percentage) /
                (yellowLerpStartPercentage - yellowLerpEndPercentage);
            healthBarForeground.color = Vector4.Lerp(greenColor, yellowColor, lerpPercentage);
        }
        else if (percentage >= redLerpEndPercentage && percentage <= redLerpStartPercentage)
        {
            float lerpPercentage = (redLerpStartPercentage - percentage) / 
                (redLerpStartPercentage - redLerpEndPercentage);
            healthBarForeground.color = Vector4.Lerp(yellowColor, redColor, lerpPercentage);
        }
        else if (percentage < redLerpEndPercentage)
        {
            healthBarForeground.color = redColor;
        }
        //fade out
        if (fadeEnabled)
        {
            timeSinceLastChange = 0f;
            FadeIn();
            enabled = true;
        }
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        float duration = fadeDuration * (1f - healthBarForeground.color.a / foregroundStartAlpha);
        if (duration != 0)
        {
            StartCoroutine(LerpAlpha(healthBarBackground, backgroundStartAlpha, duration));
            StartCoroutine(LerpAlpha(healthBarForeground, foregroundStartAlpha, duration));
        }
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        float duration = fadeDuration * (healthBarForeground.color.a / foregroundStartAlpha);
        if (duration != 0)
        {
            StartCoroutine(LerpAlpha(healthBarBackground, 0, duration));
            StartCoroutine(LerpAlpha(healthBarForeground, 0, duration));
        }
    }

    private IEnumerator LerpAlpha(Image lerpImage, float newAlpha, float duration)
    {
        float startAlpha = lerpImage.color.a;
        float lerpStartTime = Time.time;
        while (Time.time - lerpStartTime < fadeDuration)
        {
            float lerpPercentage = (Time.time - lerpStartTime) / duration;
            float lerpAlpha = Mathf.Lerp(startAlpha, newAlpha, lerpPercentage);
            lerpImage.color = new Color(lerpImage.color.r, lerpImage.color.g,
                lerpImage.color.b, lerpAlpha);
            greenColor = new Color(greenColor.r, greenColor.g, greenColor.b, lerpAlpha);
            yellowColor = new Color(yellowColor.r, yellowColor.g, yellowColor.b, lerpAlpha);
            redColor = new Color(redColor.r, redColor.g, redColor.b, lerpAlpha);
            yield return null;
        }
        lerpImage.color = new Color(lerpImage.color.r, lerpImage.color.g,
            lerpImage.color.b, newAlpha);
        greenColor = new Color(greenColor.r, greenColor.g, greenColor.b, newAlpha);
        yellowColor = new Color(yellowColor.r, yellowColor.g, yellowColor.b, newAlpha);
        redColor = new Color(redColor.r, redColor.g, redColor.b, newAlpha);
    }
}
