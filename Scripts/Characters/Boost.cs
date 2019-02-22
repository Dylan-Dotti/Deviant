using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public bool IsBoosting { get; private set; }

    [SerializeField]
    private float boostSequenceDuration = 1f;
    [SerializeField]
    private VelocityModifier boostVelocityModifier;
    [SerializeField]
    private ParticleSystem boostParticles;

    private PlayerController pController;

    private void Awake()
    {
        IsBoosting = false;
        pController = PlayerCharacter.Instance.Controller;
    }

    public void ActivateBoost(Vector3 boostDirection)
    {
        if (!IsBoosting)
        {
            StartCoroutine(BoostSequence(boostDirection));
        }
    }

    public void CancelBoost()
    {
        StopAllCoroutines();
        IsBoosting = false;
        boostParticles.Stop();
        pController.RemoveVelocityModifier(boostVelocityModifier);
    }

    private IEnumerator BoostSequence(Vector3 boostDirection)
    {
        IsBoosting = true;
        boostParticles.Play();
        pController.AddVelocityModifier(boostVelocityModifier);
        float boostStartTime = Time.time;
        while (Time.time - boostStartTime < boostSequenceDuration)
        {
            float angleRad = 180f * Mathf.Clamp01((Time.time - boostStartTime) / 
                boostSequenceDuration) * Mathf.Deg2Rad;
            boostVelocityModifier.Velocity = boostDirection *
                boostVelocityModifier.MaxMagnitude * Mathf.Sin(angleRad);
            yield return null;
        }
        pController.RemoveVelocityModifier(boostVelocityModifier);
        boostParticles.Stop();
        IsBoosting = false;
    }
}
