using System.Collections.Generic;
using UnityEngine;

public class SingleScatterCannon : Weapon
{
    [SerializeField]
    private IntRange damageRange = new IntRange(4, 6);
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
}
