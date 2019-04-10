using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedDroneMultiBlaster : BlasterWeapon
{
    public override IntRange ProjectileDmgRange
    {
        get => blasters[0].ProjectileDmgRange;
        set => blasters.ForEach(b => b.ProjectileDmgRange = value);
    }

    [SerializeField]
    private List<SingleBlaster> blasters;

    [Header("Ammo")]
    [SerializeField]
    private float reloadInterval = 4;
    [SerializeField]
    private int maxAmmo = 25;

    private int currentAmmo;
    private WeaponRecoil recoiler;
    private AudioSource fireSound;

    private void Awake()
    {
        recoiler = GetComponent<WeaponRecoil>();
        fireSound = GetComponent<AudioSource>();
        currentAmmo = maxAmmo;
    }

    protected override void Start()
    {
        StartCoroutine(RefreshAmmoPeriodically());
    }

    public override void FireWeapon()
    {
        base.FireWeapon();
        recoiler.AttemptRecoil();
        fireSound.PlayOneShot(fireSound.clip);
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

    public override void AttemptFireWeapon()
    {
        if (currentAmmo > 0)
        {
            base.AttemptFireWeapon();
        }
    }

    private IEnumerator RefreshAmmoPeriodically()
    {
        while (true)
        {
            while (currentAmmo == maxAmmo)
            {
                yield return null;
            }
            yield return new WaitForSeconds(reloadInterval);
            currentAmmo = maxAmmo;
        }
    }
}
