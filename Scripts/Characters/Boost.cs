using System.Collections;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public bool IsBoosting { get; private set; }

    [SerializeField]
    private float boostSequenceDuration = 1f;
    [SerializeField]
    private VelocityModifier boostVelocityModifier;
    [SerializeField]
    private float boostCooldown = 5;
    [SerializeField]
    private ParticleSystem boostParticles;

    private float timeSinceLastBoost;
    private PlayerController pController;

    private void Awake()
    {
        IsBoosting = false;
        timeSinceLastBoost = boostCooldown;
        pController = PlayerCharacter.Instance.Controller;
    }

    private void Update()
    {
        timeSinceLastBoost += Time.deltaTime;
    }

    public void AttemptBoost(Vector3 boostDirection)
    {
        if (!IsBoosting && timeSinceLastBoost >= boostCooldown)
        {
            ActivateBoost(boostDirection);
        }
    }

    public void ActivateBoost(Vector3 boostDirection)
    {
        StartCoroutine(BoostSequence(boostDirection));
        timeSinceLastBoost = 0;
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
