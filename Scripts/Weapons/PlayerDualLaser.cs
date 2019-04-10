using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDualLaser : MultiLaser
{
    public float FiringMoveSpeedPercentage
    {
        get => firingMoveSpeedPercentage;
        set => firingMoveSpeedPercentage = Mathf.Clamp01(value);
    }

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

    private Vector3 targetPosition;
    private PlayerController pController;
    private Coroutine fireSequenceRoutine;

    private void Awake()
    {
        pController = PlayerCharacter.Instance.Controller;
    }

    private void OnEnable()
    {
        //WaveGenerator.Instance.wave
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
        targetPosition = targetPos;
        base.TurnToFace(targetPos);
    }

    private void OnTargetterTargetLost(Transform target)
    {
        CancelFireWeapon();
    }

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
        firingSound.Play();
        while (IsFiring)
        {
            TurnToFace(targetter.TargetTransform.position);
            pController.Rotator.TargetPosition = targetPosition;
            yield return null;
        }
    }
}
