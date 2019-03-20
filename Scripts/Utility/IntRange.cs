using UnityEngine;

[System.Serializable]
public class IntRange
{
    public int Min => min;
    public int Max => max;
    public int RandomRangeValue => Random.Range(min, max + 1);

    [SerializeField]
    private int min;
    [SerializeField]
    private int max;

    public IntRange(int minVal, int maxVal)
    {
        min = minVal;
        max = maxVal;
    }

    public static IntRange operator+ (IntRange a, IntRange b)
    {
        return new IntRange(a.min + b.min, a.max + b.max);
    }

    public static IntRange operator- (IntRange a, IntRange b)
    {
        return new IntRange(a.min - b.min, a.max - b.max);
    }
}
