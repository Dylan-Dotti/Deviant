using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedDroneMultiBlaster : Weapon
{
    [SerializeField]
    private List<SingleBlaster> blasters;
    [Header("Ammo")]
    [SerializeField]
    private float reloadInterval = 4;
    [SerializeField]
    private int maxAmmo = 25;
    private int currentAmmo;
    private WeaponRecoil recoiler;

    private void Awake()
    {
        recoiler = GetComponent<WeaponRecoil>();
        currentAmmo = maxAmmo;
    }

    private void Start()
    {
        StartCoroutine(RefreshAmmoPeriodically());
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void FireWeapon()
    {
        if (currentAmmo > 0)
        {
            base.FireWeapon();
            recoiler.AttemptRecoil();
            int randIndex = Random.Range(0, blasters.Count);
            blasters[randIndex].FireWeapon();
            currentAmmo -= 1;
            if (blasters.Count > 1 && currentAmmo > 0)
            {
                while (true)
                {
                    int randIndex2 = Random.Range(0, blasters.Count);
                    if (randIndex2 != randIndex)
                    {
                        blasters[randIndex2].FireWeapon();
                        currentAmmo -= 1;
                        break;
                    }
                }
            }
        }
    }

    private IEnumerator RefreshAmmoPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(reloadInterval);
            currentAmmo = maxAmmo;
        }
    }
}
