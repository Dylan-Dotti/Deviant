using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/* Manages health bar percentage and changes color 
 * based on health percentage.
 */
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
    
    private void Start()
    {
        timeSinceLastChange = fadeDelay;
        if (fadeEnabled)
        {
            healthBarForeground.color = new Color(healthBarForeground.color.r, 
                healthBarForeground.color.g, healthBarForeground.color.b, 0);
            healthBarBackground.color = new Color(healthBarBackground.color.r,
                healthBarBackground.color.g, healthBarBackground.color.b, 0);
            greenColor = new Color(greenColor.r, greenColor.g, greenColor.b, 0);
            yellowColor = new Color(yellowColor.r, yellowColor.g, yellowColor.b, 0);
            redColor = new Color(redColor.r, redColor.g, redColor.b, 0);
        }
    }

    private void LateUpdate()
    {
        if (timeSinceLastChange > fadeDelay && fadeEnabled)
        {
            FadeOut();
        }
        timeSinceLastChange += Time.deltaTime;
    }

    public void SetHealthPercentage(float percentage)
    {
        healthBarTransform.localScale = new Vector3(Mathf.Clamp01(percentage), 
            healthBarTransform.localScale.y, healthBarTransform.localScale.z);

        //change color
        if (percentage > yellowLerpStartPercentage)
        {
            healthBarForeground.color = greenColor;
        }
        else if (percentage >= yellowLerpEndPercentage && percentage <= yellowLerpStartPercentage)
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
        //fade in
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
        float duration = fadeDuration * (1f - healthBarForeground.color.a);
        if (duration != 0)
        {
            StartCoroutine(LerpAlpha(healthBarBackground, 0.56863f, duration));
            StartCoroutine(LerpAlpha(healthBarForeground, 1, duration));
        }
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        float duration = fadeDuration * (healthBarForeground.color.a);
        if (duration != 0)
        {
            StartCoroutine(LerpAlpha(healthBarBackground, 0, duration));
            StartCoroutine(LerpAlpha(healthBarForeground, 0, duration));
        }
    }

    private IEnumerator LerpAlpha(Image healthBar, float newAlpha, float duration)
    {
        float startAlpha = healthBar.color.a;
        float lerpStartTime = Time.time;
        while (Time.time - lerpStartTime < fadeDuration)
        {
            float lerpPercentage = (Time.time - lerpStartTime) / duration;
            float lerpAlpha = Mathf.Lerp(startAlpha, newAlpha, lerpPercentage);
            healthBar.color = new Color(healthBar.color.r, healthBar.color.g,
                healthBar.color.b, lerpAlpha);
            greenColor = new Color(greenColor.r, greenColor.g, greenColor.b, lerpAlpha);
            yellowColor = new Color(yellowColor.r, yellowColor.g, yellowColor.b, lerpAlpha);
            redColor = new Color(redColor.r, redColor.g, redColor.b, lerpAlpha);
            yield return null;
        }
        healthBar.color = new Color(healthBar.color.r, healthBar.color.g,
            healthBar.color.b, newAlpha);
        greenColor = new Color(greenColor.r, greenColor.g, greenColor.b, newAlpha);
        yellowColor = new Color(yellowColor.r, yellowColor.g, yellowColor.b, newAlpha);
        redColor = new Color(redColor.r, redColor.g, redColor.b, newAlpha);
    }
}
