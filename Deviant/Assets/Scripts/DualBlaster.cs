using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualBlaster : Weapon
{
    [SerializeField]
    protected Transform leftBlaster;
    [SerializeField]
    protected Transform leftFirePoint;
    [SerializeField]
    protected ParticleSystem leftFireParticles;
    [SerializeField]
    protected Transform rightBlaster;
    [SerializeField]
    protected Transform rightFirePoint;
    [SerializeField]
    protected ParticleSystem rightFireParticles;

    private bool firedLeftLast = false;

    public override void FireWeapon()
    {
        Transform firePoint = firedLeftLast ? rightFirePoint : leftFirePoint;
        Projectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        if (firedLeftLast) { rightFireParticles.Play(); }
        else { leftFireParticles.Play(); }
        fireSound.PlayOneShot(fireSound.clip);

        TimeSinceLastFire = 0.0f;
        firedLeftLast = !firedLeftLast;
    }

    public override void TurnToFace(Vector3 targetPos)
    {
        Vector3 direction = targetPos - transform.position;
        //Debug.Log(direction);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Debug.Log(angle);
        //leftBlaster.transform.rotation = Quaternion.AngleAxis(angle - 270, Vector3.forward);
        //rightBlaster.transform.rotation = Quaternion.AngleAxis(angle - 270, Vector3.forward);
        leftBlaster.transform.localRotation = Quaternion.LookRotation((Vector2)targetPos);
        rightBlaster.transform.localRotation = Quaternion.LookRotation((Vector2)targetPos);
    }
}
