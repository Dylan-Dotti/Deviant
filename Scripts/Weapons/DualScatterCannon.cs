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
