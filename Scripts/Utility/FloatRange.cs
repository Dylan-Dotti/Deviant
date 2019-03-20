using UnityEngine;

[System.Serializable]
public class FloatRange
{
    public float Min => min;
    public float Max => max;
    public float RandomRangeValue => Random.Range(min, max);

    [SerializeField]
    private float min;
    [SerializeField]
    private float max;

    public FloatRange(float minVal, float maxVal)
    {
        min = minVal;
        max = maxVal;
    }

    public static FloatRange operator+ (FloatRange a, FloatRange b)
    {
        return new FloatRange(a.min + b.min, a.max + b.max);
    }

    public static FloatRange operator- (FloatRange a, FloatRange b)
    {
        return new FloatRange(a.min - b.min, a.max - b.max);
    }
}
