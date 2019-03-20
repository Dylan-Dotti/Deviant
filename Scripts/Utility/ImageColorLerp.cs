using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorLerp : Lerper
{
    [SerializeField]
    private Color lerpColor;

    private Image image;
    private Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
    }

    protected override IEnumerator LerpCR(bool forward, float duration)
    {
        //Debug.Log(duration);
        Color startColor = image.color;
        Color endColor = forward ? lerpColor : originalColor;
        float startTime = Time.time;
        for (float elapsed = 0; elapsed < duration;
             elapsed = Time.time - startTime)
        {
            image.color = Color.Lerp(startColor, endColor, elapsed / duration);
            yield return null;
        }
        image.color = endColor;
        yield return StartCoroutine(base.LerpCR(forward, duration));
    }
}
