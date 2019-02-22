﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexSpawnerDeathSequence : AnimationSequence
{
    [SerializeField]
    private List<Weapon> weapons;
    [SerializeField]
    private List<ParticleSystem> centralParticles;
    [SerializeField]
    private List<ParticleSystem> damagedParticles;
    [SerializeField]
    private List<ParticleSystem> explosionParticles;
    [SerializeField]
    private GameObject healthBar;
    [SerializeField]
    private VortexSpawnerRotate outerRing;
    [SerializeField]
    private VortexSpawnerRotate innerRing;

    private void Awake()
    {
    }

    protected override IEnumerator PlayAnimationSequence()
    {
        //disable stuff
        healthBar.SetActive(false);
        GetComponent<IdleWander>().enabled = false;
        Destroy(GetComponentInChildren<DamagePlayerOnContact>());
        Destroy(GetComponentInChildren<KnockbackPlayerOnContact>());
        foreach (Weapon weapon in weapons)
        {
            weapon.enabled = false;
        }

        //wind up
        outerRing.RotationSpeed *= 4;
        innerRing.RotationSpeed *= 4;
        foreach (ParticleSystem particles in damagedParticles)
        {
            particles.Play();
        }
        yield return new WaitForSeconds(2f);

        //blast rings outward
        GetComponent<Collider>().enabled = false;
        Explode();

        //wind down
        outerRing.RotationSpeed /= 5;
        innerRing.RotationSpeed /= 5;
        yield return new WaitForSeconds(2);

        //shrink
        GetComponent<VortexSpawner>().LerpScaleOverDuration(new List<Transform>() {
            outerRing.transform, innerRing.transform }, 0f, 1f);
        yield return new WaitForSeconds(1.05f);

        Destroy(gameObject);
    }

    private void Explode()
    {
        foreach (ParticleSystem particles in centralParticles)
        {
            particles.Stop();
        }

        Vector3 forceDirection = (transform.forward + transform.right *
            Random.Range(-0.5f, 0.5f)).normalized;
        if (Random.Range(0, 2) == 1)
        {
            forceDirection = new Vector3(-forceDirection.x, -forceDirection.y,
                -forceDirection.z);
        }
        Rigidbody ringRbody = outerRing.GetComponent<Rigidbody>();
        ringRbody.isKinematic = false;
        ringRbody.AddForce(forceDirection * Random.Range(3f, 4.5f),
            ForceMode.VelocityChange);

        forceDirection = (-transform.right + transform.forward *
            Random.Range(-0.5f, 0.5f)).normalized;
        if (Random.Range(0, 2) == 1)
        {
            forceDirection = new Vector3(-forceDirection.x, -forceDirection.y,
                -forceDirection.z);
        }
        ringRbody = innerRing.GetComponent<Rigidbody>();
        ringRbody.isKinematic = false;
        ringRbody.AddForce(forceDirection * Random.Range(4f, 6f),
            ForceMode.VelocityChange);

        foreach (ParticleSystem particles in explosionParticles)
        {
            particles.Play();
        }

        GetComponent<SparePartsGenerator>()?.GenerateSpareParts();
    }
}