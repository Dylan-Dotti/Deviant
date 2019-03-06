using UnityEngine;

public class DualWeapon : Weapon
{
    public Weapon LeftWeapon { get { return leftWeapon; } }
    public Weapon RightWeapon { get { return rightWeapon; } }

    [SerializeField]
    private Weapon leftWeapon;
    [SerializeField]
    private Weapon rightWeapon;

    private bool firedLeftLast = false;

    public override void FireWeapon()
    {
        base.FireWeapon();
        if (firedLeftLast)
        {
            rightWeapon.FireWeapon();
        }
        else
        {
            leftWeapon.FireWeapon();
        }
        firedLeftLast = !firedLeftLast;
    }
}
