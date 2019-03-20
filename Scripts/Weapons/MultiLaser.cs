﻿using System.Collections.Generic;
using UnityEngine;

public class MultiLaser : ToggleWeapon
{
    public List<SingleLaser> Lasers => lasers;

    public float LaserStartWidth
    {
        set => lasers.ForEach(l => l.LaserStartWidth = value);
    }
    public float LaserEndWidth
    {
        set => lasers.ForEach(l => l.LaserEndWidth = value);
    }

    [SerializeField]
    private List<SingleLaser> lasers;

    public override void FireWeapon()
    {
        lasers.ForEach(l => l.FireWeapon());
    }

    public override void AttemptFireWeapon()
    {
        lasers.ForEach(l => l.AttemptFireWeapon());
    }

    public override void CancelFireWeapon()
    {
        lasers.ForEach(l => l.CancelFireWeapon());
    }

    public override void TurnToFace(Vector3 targetPos)
    {
        lasers.ForEach(l => l.TurnToFace(targetPos));
    }
}
