using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingleScatterCannon : Weapon
{
    public IntRange DamageRange
    {
        get => damageRange;
        set => damageRange = value;
    }

    [SerializeField]
    private IntRange damageRange = new IntRange(4, 6);
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private HealthDetectionZone damageCone;
    [SerializeField]
    private List<ParticleSystem> fireParticles;

    private AudioSource source;

    protected virtual void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public override void FireWeapon()
    {
        base.FireWeapon();
        ApplyAreaDamage();
        foreach (ParticleSystem particles in fireParticles)
        {
            particles.Play();
        }
        source.PlayOneShot(source.clip);
    }

    private void ApplyAreaDamage()
    {
        foreach (Health targetHealth in damageCone.DetectedComponents)
        {
            if (targetHealth != null)
            {
                targetHealth.CurrentHealth -= Random.Range(
                    damageRange.Min, damageRange.Max + 1);
            }
        }
    }

    private Vector3 GetHitPosition(Vector3 targetPosition)
    {
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        float targetDist = Vector3.Distance(firePoint.position, targetPosition);
        RaycastHit[] rayHits = Physics.RaycastAll(ray, targetDist);
        IEnumerable<RaycastHit> hits = rayHits.Where(hit => damageCone.DetectableTags.
        Contains(hit.transform.tag));
        return GetClosestHit(hits).point;
    }

    private RaycastHit GetClosestHit(IEnumerable<RaycastHit> hits)
    {
        RaycastHit closestHit = new RaycastHit();
        float closestDist = Mathf.Infinity;
        foreach (RaycastHit rayHit in hits)
        {
            float hitDist = Vector3.Distance(damageCone.transform.position, rayHit.point);
            if (hitDist < closestDist)
            {
                closestDist = hitDist;
                closestHit = rayHit;
            }
        }
        return closestHit;
    }
}
