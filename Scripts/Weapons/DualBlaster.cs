using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualBlaster : Weapon
{
    [SerializeField]
    private SingleBlaster leftBlaster;
    [SerializeField]
    private SingleBlaster rightBlaster;

    private bool firedLeftLast = false;

    protected override void Awake()
    {
        fireSound = null;
    }

    public override void FireWeapon()
    {
        base.FireWeapon();
        if (firedLeftLast)
        {
            rightBlaster.FireWeapon();
        }
        else
        {
            leftBlaster.FireWeapon();
        }
        firedLeftLast = !firedLeftLast;
    }

    public override void TurnToFace(Vector3 targetPos)
    {
        throw new NotImplementedException();
    }
}
