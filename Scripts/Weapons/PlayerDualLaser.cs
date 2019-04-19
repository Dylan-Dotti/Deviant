using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Player laser class.
 * Uses a LaserTargetter to lock on to enemies
 */
public class PlayerDualLaser : MultiLaser
{
    public float FiringMoveSpeedPercentage
    {
        get => firingMoveSpeedPercentage;
        set => firingMoveSpeedPercentage = Mathf.Clamp01(value);
    }

    public FloatRange DamagePerSecond => new FloatRange(
        LaserDamagePerTick.Min * TicksPerSecond, 
        LaserDamagePerTick.Max * TicksPerSecond);

    public float TicksPerSecond => 4;

    [SerializeField][Range(0, 1)]
    private float firingMoveSpeedPercentage = 0.6f;
    [SerializeField]
    private PlayerLaserTargetter targetter;
    [SerializeField]
    private List<ParticleSystem> chargingParticles;
    [SerializeField]
    private AudioSource chargingSound;
    [SerializeField]
    private AudioSource firingSound;

    private PlayerController pController;
    private Coroutine fireSequenceRoutine;
    private Coroutine fireEventRoutine;

    private void Awake()
    {
        pController = PlayerCharacter.Instance.Controller;
        Lasers.ForEach(l => l.WeaponFiredEvent += () => InvokeWeaponFiredEvent());
    }

    private void OnEnable()
    {
        targetter.enabled = true;
        targetter.TargetLostEvent += OnTargetterTargetLost;
    }

    private void OnDisable()
    {
        targetter.TargetLostEvent -= OnTargetterTargetLost;
        targetter.enabled = false;
        CancelFireWeapon();
    }

    public override void FireWeapon()
    {
        IsFiring = true;
        targetter.RetargettingEnabled = false;
        pController.MouseRotateEnabled = false;
        pController.MaxVelocityMagnitude *= firingMoveSpeedPercentage;
        pController.Rotator.TargetPosition = 
            targetter.TargetTransform.position;
        fireSequenceRoutine = StartCoroutine(FireSequence());
    }

    public override void AttemptFireWeapon()
    {
        if (targetter.TargetTransform != null)
        {
            base.AttemptFireWeapon();
        }
    }

    public override void CancelFireWeapon()
    {
        if (IsFiring)
        {
            if (fireSequenceRoutine != null)
            {
                StopCoroutine(fireSequenceRoutine);
            }
            if (fireEventRoutine != null)
            {
                StopCoroutine(fireEventRoutine);
            }
            chargingParticles.ForEach(p => p.Stop());
            chargingSound.Stop();
            firingSound.Stop();
            targetter.RetargettingEnabled = true;
            pController.MouseRotateEnabled = true;
            pController.MaxVelocityMagnitude /= firingMoveSpeedPercentage;
        }
        ResetOrientation();
        base.CancelFireWeapon();
    }

    public override void TurnToFace(Vector3 targetPos)
    {
        base.TurnToFace(targetPos);
    }

    private void OnTargetterTargetLost(Transform target)
    {
        CancelFireWeapon();
    }

    // Fires at locked-on enemy as long as fire button is held
    private IEnumerator FireSequence()
    {
        chargingParticles.ForEach(p => p.Play());
        chargingSound.Play();
        float chargeStartTime = Time.time;
        while (Time.time - chargeStartTime < 1)
        {
            pController.Rotator.TargetPosition = 
                targetter.TargetTransform.position;
            yield return null;
        }
        chargingParticles.ForEach(p => p.Stop());
        chargingSound.Stop();
        TurnToFace(targetter.TargetTransform.position);
        base.FireWeapon();
        if (IsFiring)
        {
            firingSound.Play();
            fireEventRoutine = StartCoroutine(FireEventCR());
        }
        while (IsFiring)
        {
            TurnToFace(targetter.TargetTransform.position);
            pController.Rotator.TargetPosition = 
                targetter.TargetTransform.position;
            yield return null;
        }
    }

    private IEnumerator FireEventCR()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            InvokeWeaponFiredEvent();
            InvokeWeaponFiredEvent();
        }
    }
}
