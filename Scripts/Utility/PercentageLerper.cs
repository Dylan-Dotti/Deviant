using System.Collections;
using UnityEngine;

public abstract class PercentageLerper : MonoBehaviour
{
    public abstract float CurrentLerpPercentage { get;}

    [SerializeField]
    private float maxLerpDuration = 1;

    private Coroutine lerpCR;

    public Coroutine LerpForward()
    {
        return LerpToPercentage(1);
    }

    public Coroutine LerpReverse()
    {
        return LerpToPercentage(0);
    }

    public Coroutine LerpToPercentage(float targetPercentage)
    {
        StopLerping();
        lerpCR = StartCoroutine(LerpCR(targetPercentage,
            GetAdjustedLerpDuration(targetPercentage, maxLerpDuration)));
        return lerpCR;
    }

    public Coroutine StartLerpPeriodic(float interval)
    {
        StopLerpingPeriodic();
        return StartCoroutine(LerpPeriodicCR(interval));
    }

    public Coroutine StartLerpPeriodic(FloatRange intervalRange)
    {
        StopLerpingPeriodic();
        return StartCoroutine(LerpPeriodicRangeCR(intervalRange));
    }

    public void StopLerping()
    {
        if (lerpCR != null)
        {
            StopCoroutine(lerpCR);
        }
    }

    public void StopLerpingPeriodic()
    {
        StopAllCoroutines();
    }

    protected float GetAdjustedLerpDuration(float targetPercentage, float duration)
    {
        return Mathf.Abs(CurrentLerpPercentage - targetPercentage) * duration;
    }

    protected abstract void UpdateLerpVariables(float lerpPercentage);

    private IEnumerator LerpCR(float targetPercent, float duration)
    {
        float startPercent = CurrentLerpPercentage;
        float percentDelta = targetPercent - startPercent;
        float startTime = Time.time;
        for (float elapsed = 0; elapsed < duration;
            elapsed = Time.time - startTime)
        {
            float lerpPercent = startPercent +
                percentDelta * elapsed / duration;
            UpdateLerpVariables(lerpPercent);
            yield return null;
        }
        UpdateLerpVariables(targetPercent);
        lerpCR = null;
    }

    private IEnumerator LerpPeriodicCR(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            yield return LerpForward();
            yield return new WaitForSeconds(interval);
            yield return LerpReverse();
        }
    }

    private IEnumerator LerpPeriodicRangeCR(FloatRange intervalRange)
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalRange.RandomRangeValue);
            yield return LerpForward();
            yield return new WaitForSeconds(intervalRange.RandomRangeValue);
            yield return LerpReverse();
        }
    }

    protected float Vector4InverseLerp(Vector4 a, Vector4 b, Vector4 value)
    {
        Vector4 AB = b - a;
        Vector4 AV = value - a;
        return Vector4.Dot(AV, AB) / Vector4.Dot(AB, AB);
    }
}
