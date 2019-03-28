using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemySpawnerPeriodic))]
public class EnemySpawnerInitSequence : AnimationSequence
{
    [SerializeField]
    private ParticleSystem circleParticles;
    [SerializeField]
    private ParticleSystem implosionParticles;
    [SerializeField]
    private ParticleSystem centralParticles;
    [SerializeField]
    private ParticleSystem radiationParticles;

    [SerializeField]
    private SphereCollider damageCollider;
    [SerializeField]
    private SphereCollider playerKnockbackCollider;
    [SerializeField]
    private SphereCollider enemyRepulsionCollider;
    [SerializeField]
    private PlayerMagnet magnet;
    [SerializeField]
    private FloatRange centerSimSpeedRange;

    private void Start()
    {
        PlayAnimation();
    }

    public override void CancelSequence()
    {
        StopAllCoroutines();
    }

    protected override IEnumerator AnimationSequenceCR()
    {
        yield return new WaitForSeconds(1);
        IsPlaying = true;

        //init radius variables
        float origDmgColliderRadius = damageCollider.radius;
        damageCollider.radius = 0;
        float origKbColliderRadius = playerKnockbackCollider.radius;
        playerKnockbackCollider.radius = 0;
        FloatRange origFalloffRadiusRange = magnet.FalloffRadiusRange;
        magnet.FalloffRadiusRange = new FloatRange(0, 0);

        //1. Circle spawn
        circleParticles.Play();
        yield return new WaitForSeconds(circleParticles.main.startLifetime.constantMax);

        //2. Implosion and Central buildup
        enemyRepulsionCollider.enabled = true;
        float centerExpandStartTime = Time.time;
        implosionParticles.Play();
        yield return new WaitForSeconds(0.75f);
        ParticleSystem.MainModule centralParticlesMain = centralParticles.main;
        centralParticlesMain.simulationSpeed = centerSimSpeedRange.Min;
        centralParticles.Play();

        //expand the center collider and magnet with the center particle effect,
        //lerp simulation speed of center particle effect
        damageCollider.enabled = true;
        playerKnockbackCollider.enabled = true;
        magnet.enabled = true;
        while (implosionParticles.isPlaying)
        {
            float lerpPercentage = (Time.time - centerExpandStartTime) / 3f;
            damageCollider.radius = Mathf.Lerp(0, 1.55f, lerpPercentage);
            playerKnockbackCollider.radius = Mathf.Lerp(0, origKbColliderRadius, lerpPercentage);
            float minRadius = Mathf.Lerp(0, origFalloffRadiusRange.Min, lerpPercentage);
            magnet.FalloffRadiusRange = new FloatRange(minRadius, minRadius);
            centralParticlesMain.simulationSpeed = Mathf.Lerp(centerSimSpeedRange.Min,
                centerSimSpeedRange.Max, lerpPercentage);
            yield return null;
        }
        circleParticles.Stop();
        while (implosionParticles.particleCount > 0)
        {
            yield return null;
        }

        //3. Brief pause
        yield return new WaitForSeconds(1.25f);

        //4. Completion
        GetComponent<NavMeshObstacle>().enabled = true;
        radiationParticles.Play();
        //expand center collider and magnet with emission particles
        centerExpandStartTime = Time.time;
        float mLerpStartRadius = magnet.FalloffRadiusRange.Min;
        while (Time.time - centerExpandStartTime <= 1f)
        {
            float lerpPercentage = (Time.time - centerExpandStartTime) / 1f;
            damageCollider.radius = Mathf.Lerp(1.55f, 3f, lerpPercentage);
            float maxRadius = Mathf.Lerp(mLerpStartRadius,
                origFalloffRadiusRange.Max, lerpPercentage);
            magnet.FalloffRadiusRange = new FloatRange(mLerpStartRadius, maxRadius);
            yield return null;
        }
        IsPlaying = false;
        GetComponent<EnemySpawnerPeriodic>().enabled = true;
        Destroy(this);
    }
}
