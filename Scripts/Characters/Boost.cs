using System.Collections;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public float Duration
    {
        get => boostSequenceDuration;
        set => boostSequenceDuration = value;
    }
    public float ChargeCooldown
    {
        get => chargeCooldown;
        set => chargeCooldown = value;
    }

    public int MaxNumCharges
    {
        get => maxNumCharges;
        set
        {
            if (value > maxNumCharges && CurrentChargeFillPercent == 1)
            {
                timeSinceLastBoostRecharge = 0;
            }
            maxNumCharges = value;
        }
    }

    public int CurrentNumCharges { get; private set; }
    public float CurrentChargeFillPercent => Mathf.Clamp01(
        timeSinceLastBoostRecharge / chargeCooldown);

    public bool IsBoosting { get; private set; }

    [SerializeField]
    private float boostSequenceDuration = 1f;
    [SerializeField]
    private VelocityModifier boostVelocityModifier;
    [SerializeField]
    private float chargeCooldown = 5;
    [SerializeField]
    private ParticleSystem boostParticles;
    [SerializeField]
    private AudioSource boostSound;

    private int maxNumCharges = 1;
    private float timeSinceLastBoostRecharge;
    private PlayerController pController;

    private void Awake()
    {
        IsBoosting = false;
        CurrentNumCharges = MaxNumCharges;
        timeSinceLastBoostRecharge = chargeCooldown;
        pController = PlayerCharacter.Instance.Controller;
    }

    private void Update()
    {
        timeSinceLastBoostRecharge += Time.deltaTime;
        if (timeSinceLastBoostRecharge >= chargeCooldown && 
            CurrentNumCharges < MaxNumCharges)
        {
            CurrentNumCharges += 1;
            timeSinceLastBoostRecharge = 0;
        }
    }

    public void AttemptBoost(Vector3 boostDirection)
    {
        if (!IsBoosting && CurrentNumCharges > 0)
        {
            ActivateBoost(boostDirection);
        }
    }

    public void ActivateBoost(Vector3 boostDirection)
    {
        StartCoroutine(BoostSequence(boostDirection));
        if (CurrentNumCharges == MaxNumCharges)
        {
            timeSinceLastBoostRecharge = 0;
        }
        CurrentNumCharges = Mathf.Max(CurrentNumCharges - 1, 0);
        boostSound.Play();
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
