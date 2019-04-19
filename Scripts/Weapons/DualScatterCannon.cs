using UnityEngine;

public class DualScatterCannon : Weapon
{
    public IntRange DamageRange
    {
        get
        {
            return leftCannon.DamageRange;
        }
        set
        {
            leftCannon.DamageRange = value;
            rightCannon.DamageRange = value;
        }
    }

    public float Range
    {
        get => leftCannon.Range;
        set { leftCannon.Range = value; rightCannon.Range = value; }
    }
    public float Spread
    {
        get => leftCannon.Spread;
        set { leftCannon.Spread = value; rightCannon.Spread = value; }
    }

    public FloatRange DamagePerSecond => new FloatRange(
        DamageRange.Min / FireRate, DamageRange.Max / FireRate);

    public SingleScatterCannon LeftCannon => leftCannon;
    public SingleScatterCannon RightCannon => rightCannon;

    [SerializeField]
    private SingleScatterCannon leftCannon;
    [SerializeField]
    private SingleScatterCannon rightCannon;

    private bool firedLeftLast = false;

    public override void FireWeapon()
    {
        base.FireWeapon();
        if (firedLeftLast)
        {
            rightCannon.AttemptFireWeapon();
        }
        else
        {
            leftCannon.AttemptFireWeapon();
        }
        firedLeftLast = !firedLeftLast;
    }
}
