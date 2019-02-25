using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedDroneMultiBlaster : Weapon
{
    [SerializeField]
    private List<SingleBlaster> blasters;
    private WeaponRecoil recoiler;

    private void Awake()
    {
        recoiler = GetComponent<WeaponRecoil>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void FireWeapon()
    {
        base.FireWeapon();
        recoiler.AttemptRecoil();
        int randIndex = Random.Range(0, blasters.Count);
        blasters[randIndex].FireWeapon();
    }
}
