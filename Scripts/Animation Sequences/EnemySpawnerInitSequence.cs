using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemySpawner))]
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

    private void Start()
    {
        PlayAnimation();
    }

    public override void CancelSequence()
    {
        StopAllCoroutines();
    }

    protected override IEnumerator PlayAnimationSequence()
    {
        yield return new WaitForSeconds(1);
        IsPlaying = true;
        //1. Circle spawn
        circleParticles.Play();
        yield return new WaitForSeconds(circleParticles.main.startLifetime.constantMin *
            circleParticles.main.simulationSpeed);
        //2. Implosion and Central buildup
        SphereCollider centerCollider = gameObject.AddComponent<SphereCollider>();
        centerCollider.radius = 0.001f;
        centerCollider.isTrigger = true;
        float centerExpandStartTime = Time.time;
        implosionParticles.Play();
        yield return new WaitForSeconds(0.75f);
        centralParticles.Play();
        yield return null;
        //expand the center collider with the center particle effect
        while (implosionParticles.isPlaying)
        {
            centerCollider.radius = Mathf.Lerp(0.001f, 1.55f, (Time.time - centerExpandStartTime) /
                implosionParticles.main.duration * implosionParticles.main.simulationSpeed);
            yield return null;
        }
        circleParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        while (implosionParticles.particleCount > 0)
        {
            yield return null;
        }
        //3. Brief pause
        yield return new WaitForSeconds(1.25f);
        //4. Completion
        radiationParticles.Play();
        centerExpandStartTime = Time.time;
        //expand center collider with emission particles
        while (Time.time - centerExpandStartTime <= 1f)
        {
            centerCollider.radius = Mathf.Lerp(1.55f, 3f,
                (Time.time - centerExpandStartTime) / 1f);
            yield return null;
        }
        IsPlaying = false;
        GetComponent<EnemySpawner>().enabled = true;
    }
}
