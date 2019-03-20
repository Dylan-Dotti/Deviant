using System.Collections;
using UnityEngine;

public abstract class Lerper : MonoBehaviour
{
    [SerializeField]
    protected float maxLerpDuration = 1.0f;

    private Coroutine lerpCR;
    private float lerpStartTime;
    private float lastLerpDuration;
    private float lerpPercentage;
    private bool lerpedForwardLast = false;

    public Coroutine LerpForward()
    {
        StopLerpCR();
        float adjustedDuration = maxLerpDuration * (1f - GetCurrentLerpPercentage());
        lerpStartTime = Time.time;
        lerpCR = StartCoroutine(LerpCR(true, adjustedDuration));
        lerpedForwardLast = true;
        return lerpCR;
    }

    public Coroutine LerpReverse()
    {
        StopLerpCR();
        float adjustedDuration = maxLerpDuration * GetCurrentLerpPercentage();
        lerpStartTime = Time.time;
        lerpCR = StartCoroutine(LerpCR(false, adjustedDuration));
        lerpedForwardLast = false;
        return lerpCR;
    }

    public Coroutine StartLerpPeriodic(float interval)
    {
        StopLerpCR();
        StopAllCoroutines();
        return StartCoroutine(LerpPeriodicCR(interval));
    }

    public void StopLerpPeriodic()
    {
        StopLerpCR();
        StopAllCoroutines();
    }

    protected float GetCurrentLerpPercentage()
    {
        return lerpPercentage;
    }

    private void StopLerpCR()
    {
        if (lerpCR != null)
        {
            lastLerpDuration = Time.time - lerpStartTime;
            float lastLerpPercentage = Mathf.Clamp01(
                lastLerpDuration / maxLerpDuration);
            lerpPercentage += lerpedForwardLast ?
                lastLerpPercentage : -lastLerpPercentage;
            StopCoroutine(lerpCR);
        }
    }

    private IEnumerator LerpPeriodicCR(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            LerpForward();
            yield return new WaitForSeconds(interval);
            LerpReverse();
        }
    }

    //must be called after override code
    protected virtual IEnumerator LerpCR(bool forward, float duration)
    {
        lerpCR = null;
        lastLerpDuration = maxLerpDuration;
        lerpPercentage = forward ? 1 : 0;
        yield return null;
    }
}
