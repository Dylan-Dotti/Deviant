using System;

public static class FloatRounder
{
    public static float Round(float value, int digits)
    {
        return (float)Math.Round(value, digits);
    }
}
