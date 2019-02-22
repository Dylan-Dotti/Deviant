using UnityEngine;

[System.Serializable]
public class IntRange
{
    public int Min { get { return min; } }
    public int Max { get { return max; } }

    [SerializeField]
    private int min;
    [SerializeField]
    private int max;

    public IntRange(int minVal, int maxVal)
    {
        min = minVal;
        max = maxVal;
    }
}
