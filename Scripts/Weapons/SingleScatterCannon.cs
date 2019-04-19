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

    public float Range
    {
        get => damageCone.transform.localScale.y;
        set
        {
            Vector3 scale = damageCone.transform.localScale;
            damageCone.transform.localScale = new Vector3(
              scale.x, value, scale.z);
            Vector3 position = damageCone.transform.localPosition;
            damageCone.transform.localPosition = new Vector3(
                position.x, position.y, value - 0.15f);
        }
    }

    public float Spread
    {
        get => damageCone.transform.localScale.x;
        set
        {
            Vector3 scale = damageCone.transform.localScale;
            damageCone.transform.localScale = new Vector3(
                value, scale.y, scale.z);
        }
    }

    [SerializeField]
    private IntRange damageRange = new IntRange(4, 6);
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private HealthDetectionZone damageCone;
    [SerializeField]
    private List<ParticleSystem> fireParticles;

    private AudioSource fireSound;
    private DamageNumberPool damageNumPool;

    protected virtual void Awake()
    {
        fireSound = GetComponent<AudioSource>();
        damageNumPool = ObjectPoolManager.Instance.GetDamageNumberPool(
            ObjectPoolManager.DamagerType.Player);
    }

    public override void FireWeapon()
    {
        ApplyAreaDamage();
        base.FireWeapon();
        foreach (ParticleSystem particles in fireParticles)
        {
            particles.Play();
        }
        fireSound.PlayOneShot(fireSound.clip);
    }

    private void ApplyAreaDamage()
    {
        foreach (Health targetHealth in damageCone.DetectedComponents)
        {
            if (targetHealth != null && targetHealth.CurrentHealth != 0)
            {
                Vector3 hitPosition = GetHitPosition(
                    targetHealth.transform.root);
                int damage = Mathf.Min(damageRange.RandomRangeValue, 
                    targetHealth.CurrentHealth);
                DamageNumber dmgNum = damageNumPool.Get();
                dmgNum.DamageText.text = damage.ToString();
                if (hitPosition == null)
                {
                    dmgNum.SpawnAtPos(hitPosition + Vector3.up);
                }
                else
                {
                    dmgNum.SpawnAtPos(targetHealth.transform.position + Vector3.up);
                }
                targetHealth.CurrentHealth -= damage;
            }
        }
    }

    private Vector3 GetHitPosition(Transform target)
    {
        Ray ray = new Ray(firePoint.position, target.position - firePoint.position );
        float targetDist = Vector3.Distance(firePoint.position, target.position);
        RaycastHit[] rayHits = Physics.RaycastAll(ray, targetDist);
        IEnumerable<RaycastHit> hits = rayHits.Where(hit => damageCone.DetectableTags.
        Contains(hit.transform.tag) && hit.transform.root == target.root);
        return GetClosestHit(hits).point;
    }

    private RaycastHit GetClosestHit(IEnumerable<RaycastHit> hits)
    {
        RaycastHit closestHit = new RaycastHit();
        float closestDist = 1000;
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
