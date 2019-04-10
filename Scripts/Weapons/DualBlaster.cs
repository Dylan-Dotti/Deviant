using UnityEngine;

public class DualBlaster : BlasterWeapon
{
    public BlasterWeapon LeftBlaster => leftBlaster;
    public BlasterWeapon RightBlaster => rightBlaster;

    public override IntRange ProjectileDmgRange
    {
        get { return leftBlaster.ProjectileDmgRange; }
        set { leftBlaster.ProjectileDmgRange = value;
              rightBlaster.ProjectileDmgRange = value; }
    }

    [SerializeField]
    private BlasterWeapon leftBlaster;
    [SerializeField]
    private BlasterWeapon rightBlaster;

    private bool firedLeftLast = false;

    public override void FireWeapon()
    {
        base.FireWeapon();
        if (firedLeftLast)
        {
            rightBlaster.AttemptFireWeapon();
        }
        else
        {
            leftBlaster.AttemptFireWeapon();
        }
        firedLeftLast = !firedLeftLast;
    }
}
