using UnityEngine;

public class DualBlaster : Weapon
{
    public SingleBlaster LeftBlaster => leftBlaster;
    public SingleBlaster RightBlaster => rightBlaster;

    [SerializeField]
    private SingleBlaster leftBlaster;
    [SerializeField]
    private SingleBlaster rightBlaster;

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
