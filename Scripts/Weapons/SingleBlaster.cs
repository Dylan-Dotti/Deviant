using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBlaster : Weapon
{
    [SerializeField]
    protected Transform firePoint;
    [SerializeField]
    protected ParticleSystem fireParticles;
    [SerializeField]
    protected PlayerProjectilePool projectilePool;

    private Vector3 localSpawnPos;
    private float recoilDistance = 0.1f;

    protected override void Awake()
    {
        base.Awake();
        localSpawnPos = transform.localPosition;
    }

    private void Start()
    {
        StartCoroutine(RecoilRecover());
    }

    public override void FireWeapon()
    {
        base.FireWeapon();
        Projectile projectile = projectilePool.Get();
        Rigidbody projectileRBody = projectile.GetComponent<Rigidbody>();
        //projectile.transform.parent = null;
        //projectileRBody.position = firePoint.position;
        //projectileRBody.rotation = firePoint.rotation;
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;
        projectile.gameObject.SetActive(true);
        transform.localPosition = new Vector3(localSpawnPos.x - recoilDistance,
            transform.localPosition.y, transform.localPosition.z);
        fireSound.PlayOneShot(fireSound.clip);
        fireParticles.Play();
    }

    private IEnumerator RecoilRecover()
    {
        while (true)
        {
            Debug.Log("recoil recover");
            if (transform.localPosition.x < localSpawnPos.x)
            {
                float newLocalXPos = Mathf.Min(transform.localPosition.x +
                    (recoilDistance / 8f), localSpawnPos.x);
                transform.localPosition = new Vector3(newLocalXPos, transform.localPosition.y,
                    transform.localPosition.z);
            }
            yield return null;
        }
    }

    public override void TurnToFace(Vector3 targetPos)
    {
        throw new NotImplementedException();
    }
}
