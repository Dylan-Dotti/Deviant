using System;
using UnityEngine;

public class DualWeapon : Weapon
{
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
