using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private RectTransform healthBarTransform;
    [SerializeField]
    private Image healthBar;
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
    [SerializeField]
    [Range(0f, 1f)]
    private float yellowLerpStartPercentage = 1f;
    [SerializeField]
    [Range(0f, 1f)]
    private float yellowLerpEndPercentage = 0.66f;
    [SerializeField]
    [Range(0f, 1f)]
    private float redLerpStartPercentage = 0.66f;
    [SerializeField]
    [Range(0f, 1f)]
    private float redLerpEndPercentage = 0.33f;

    [Header("Fade Effect")]
    [SerializeField]
    private float fadeSpeed = 1f;
    [SerializeField]
    private float fadeDelay = 6f;

    private float timeSinceLastChange;
    private float backgroundStartAlpha;

    private void Start()
    {
        timeSinceLastChange = fadeDelay;
        backgroundStartAlpha = healthBarBackground.color.a;
    }

    private void LateUpdate()
    {
        timeSinceLastChange += Time.deltaTime;
        /*if (timeSinceLastChange < fadeDelay)
        {
            float newAlpha = Mathf.Min(healthBar.color.a + fadeSpeed, 255f);
            healthBar.color = new Color(healthBar.color.r, healthBar.color.g,
                healthBar.color.b, newAlpha);
            newAlpha = Mathf.Min(healthBarBackground.color.a + fadeSpeed, 145f);
            healthBarBackground.color = new Color(healthBarBackground.color.r, 
                healthBarBackground.color.g, healthBarBackground.color.b, newAlpha);
            //continue
        }
        else
        {
            float newAlpha = Mathf.Max(healthBar.color.a - fadeSpeed, 0f);
            healthBar.color = new Color(healthBar.color.r, healthBar.color.g,
                healthBar.color.b, newAlpha);
            newAlpha = Mathf.Max(healthBarBackground.color.a + fadeSpeed, 0f);
            healthBarBackground.color = new Color(healthBarBackground.color.r, 
                healthBarBackground.color.g, healthBarBackground.color.b, newAlpha);
        }*/
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
            healthBar.color = Vector4.Lerp(greenColor, yellowColor, lerpPercentage);
        }
        else if (percentage >= redLerpEndPercentage && percentage <= redLerpStartPercentage)
        {
            float lerpPercentage = (redLerpStartPercentage - percentage) / 
                (redLerpStartPercentage - redLerpEndPercentage);
            healthBar.color = Vector4.Lerp(yellowColor, redColor, lerpPercentage);
        }
        timeSinceLastChange = 0f;
    }
}
