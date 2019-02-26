using UnityEngine;

[System.Serializable]
public class FloatRange
{
    public float Min { get { return min; } }
    public float Max { get { return max; } }

    [SerializeField]
    private float min;
    [SerializeField]
    private float max;

    public FloatRange(float minVal, float maxVal)
    {
        min = minVal;
        max = maxVal;
    }
}
