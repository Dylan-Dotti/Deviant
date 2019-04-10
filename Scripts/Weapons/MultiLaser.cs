using System.Collections.Generic;
using UnityEngine;

public class MultiLaser : ToggleWeapon
{
    public List<SingleLaser> Lasers => lasers;

    public IntRange LaserDamagePerTick
    {
        get => lasers.Count > 0 ?
            lasers[0].DamagePerTick : new IntRange(0, 0);
        set => lasers.ForEach(l => l.DamagePerTick = value);
    }
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
        base.FireWeapon();
        lasers.ForEach(l => l.FireWeapon());
    }

    public override void CancelFireWeapon()
    {
        base.CancelFireWeapon();
        lasers.ForEach(l => l.CancelFireWeapon());
    }

    public override void TurnToFace(Vector3 targetPos)
    {
        lasers.ForEach(l => l.TurnToFace(targetPos));
    }

    public override void ResetOrientation()
    {
        lasers.ForEach(l => l.ResetOrientation());
    }
}
