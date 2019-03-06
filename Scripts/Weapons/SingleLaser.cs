using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleLaser : LaserWeapon
{
    public int DamagePerSecond
    {
        get { return damagePerSecond; }
        set { damagePerSecond = value; }
    }

    [SerializeField]
    private int damagePerSecond;
    [SerializeField]
    private LineRenderer laser;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private ParticleSystem hitParticles;
    [SerializeField]
    private List<string> collidableTags;

    private Vector3 targetPosition;

    public override void FireWeapon()
    {
        base.FireWeapon();
        StopAllCoroutines();
        StartCoroutine(ApplyDamagePeriodic());
        laser.enabled = true;
    }

    public override void AttemptFireWeapon()
    {
        if (!IsFiring)
        {
            FireWeapon();
        }
    }

    public override void CancelFireWeapon()
    {
        base.CancelFireWeapon();
        StopAllCoroutines();
        laser.enabled = false;
    }

    public override void TurnToFace(Vector3 targetPos)
    {
        base.TurnToFace(targetPos);
        targetPosition = targetPos;
        laser.SetPosition(1, new Vector3(0, 0, GetLaserLength()));
    }

    private float GetLaserLength()
    {
        /*Ray ray = new Ray(firePoint.position, firePoint.forward);
        float targetDist = Vector3.Distance(targetPosition, firePoint.position);
        RaycastHit[] hits = Physics.RaycastAll(ray, targetDist);

        foreach (RaycastHit hit in hits)
        {
            if (collidableTags.Contains(hit.collider.tag))
            {
                //doesn't always return closest hit point
                return (hit.point - firePoint.position).magnitude;
            }
        }*/

        return (targetPosition - firePoint.position).magnitude;
    }

    private IEnumerator ApplyDamagePeriodic()
    {
        yield return new WaitForSeconds(0.66f);
        while (true)
        {
            bool applyDamage = true;
            for (int i = 0; i < 3; i++)
            {
                Ray ray = new Ray(firePoint.position, firePoint.forward);
                float targetDist = Vector3.Distance(targetPosition, firePoint.position);
                RaycastHit[] hits = Physics.RaycastAll(ray, targetDist);

                foreach (RaycastHit hit in hits)
                {
                    if (collidableTags.Contains(hit.collider.tag))
                    {
                        if (applyDamage)
                        {
                            Character hitCharacter = hit.transform
                                .GetComponentInParent<Character>();
                            if (hitCharacter != null)
                            {
                                hitCharacter.CharacterHealth
                                    .CurrentHealth -= damagePerSecond;
                            }
                        }
                        applyDamage = false;
                        hitParticles.transform.position = hit.point;
                        hitParticles.Play();
                        break;
                    }
                }
                yield return new WaitForSeconds(0.333f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(firePoint.position, firePoint.forward * GetLaserLength());
    }
}
