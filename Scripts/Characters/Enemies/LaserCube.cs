using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LaserCube : Enemy
{
    private enum CombatMode { Idle, SingleLaserSequence, QuadLaserSequence }

    [SerializeField]
    private float attackRange = 10;
    [SerializeField]
    private float attackCooldown = 15;
    [SerializeField]
    private FloatRange quadLaserSequenceCooldown =
        new FloatRange(35, 40);
    [SerializeField]
    private LaserCubeQuadLaser mainQuadLaser;
    [SerializeField]
    private LaserCubeQuadLaser warningQuadLaser;

    private CombatMode mode = CombatMode.Idle;

    private IdleRotate rotateBehavior;
    private IdleWander wanderBehavior;
    private Rigidbody rbody;
    private Transform playerTransform;
    private float timeSinceLastAttack;

    protected override void Awake()
    {
        base.Awake();
        rotateBehavior = GetComponentInChildren<IdleRotate>();
        wanderBehavior = GetComponent<IdleWander>();
        rbody = GetComponent<Rigidbody>();
        playerTransform = PlayerCharacter.Instance.transform;
    }

    private void Update()
    {
        mainQuadLaser.TurnToFace(Vector3.forward);
        warningQuadLaser.TurnToFace(Vector3.forward);

        timeSinceLastAttack += Time.deltaTime;

        float playerDist = Vector3.Distance(transform.position, 
            playerTransform.position);
        if (playerDist <= attackRange && 
            timeSinceLastAttack >= attackCooldown && 
            mode != CombatMode.QuadLaserSequence &&
            mode != CombatMode.SingleLaserSequence)
        {
            Debug.Log("starting single laser attack");
            mode = CombatMode.SingleLaserSequence;
            StartCoroutine(SingleLaserSequenceCR());
            timeSinceLastAttack = 0;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void StartQuadLaserSequence()
    {
        if (mode != CombatMode.QuadLaserSequence)
        {
            mode = CombatMode.QuadLaserSequence;
            StartCoroutine(QuadLaserSequenceCR());
        }
    }

    protected override IEnumerator SpawnSequence()
    {
        yield return new WaitForSeconds(3);
        GetComponent<NavMeshAgent>().enabled = true;
        wanderBehavior.enabled = true;
        timeSinceLastAttack = attackCooldown;
        yield return new WaitForSeconds(3);
        StartCoroutine(PeriodicQuadLaserSequence());
    }

    private IEnumerator SingleLaserSequenceCR()
    {
        rotateBehavior.enabled = false;
        wanderBehavior.enabled = false;
        //wait for slow down
        while (rbody.angularVelocity.magnitude > 0.15f)
        {
            yield return null;
        }
        //get laser to fire
        float angleBetween = Vector3.SignedAngle(transform.forward, 
            playerTransform.position - transform.position, Vector3.up);
        SingleLaser warningLaser, mainLaser;
        if (angleBetween > -45 && angleBetween <= 45)
        {
            warningLaser = warningQuadLaser.ForwardLaser;
            mainLaser = mainQuadLaser.ForwardLaser;
        }
        else if (angleBetween > 45 && angleBetween <= 135)
        {
            warningLaser = warningQuadLaser.RightLaser;
            mainLaser = mainQuadLaser.RightLaser;
        }
        else if (angleBetween <= -45 && angleBetween > -135)
        {
            warningLaser = warningQuadLaser.LeftLaser;
            mainLaser = mainQuadLaser.LeftLaser;
        }
        else
        {
            warningLaser = warningQuadLaser.BackLaser;
            mainLaser = mainQuadLaser.BackLaser;
        }
        //warning lasers
        StartCoroutine(SingleWarningLaserSequence(warningLaser));
        yield return new WaitForSeconds(2.7f);
        //fire main laser
        float widthLerpDuration = 1;
        mainLaser.FireAndLerp(0, 0.2f, widthLerpDuration);
        //track player
        StartCoroutine(TrackPlayerOverDuration(mainLaser.transform, 7.5f, 8));
        yield return new WaitForSeconds(5);
        //disable weapon
        mainLaser.CancelAndLerp(0, 1.5f);
        yield return new WaitForSeconds(3);
        rotateBehavior.enabled = true;
        wanderBehavior.enabled = true;
        mode = CombatMode.Idle;
    }

    private IEnumerator PeriodicQuadLaserSequence()
    {
        while (true)
        {
            while (mode == CombatMode.SingleLaserSequence)
            {
                yield return null;
            }
            Debug.Log("starting quad sequence");
            StartQuadLaserSequence();
            yield return new WaitForSeconds(
                quadLaserSequenceCooldown.RandomRangeValue);
        }
    }

    private IEnumerator QuadLaserSequenceCR()
    {
        rotateBehavior.enabled = false;
        wanderBehavior.enabled = false;
        //wait for rotation to stop
        while (rbody.angularVelocity.magnitude > 0.05f)
        {
            yield return null;
        }
        //spinup sequence
        StartCoroutine(SpinupSequence(5, 5));
        //warning lasers
        StartCoroutine(QuadWarningLaserSequence());
        yield return new WaitForSeconds(5);
        //main lasers
        float lowWidth = 0f;
        float highWidth = 0.2f;
        mainQuadLaser.LaserStartWidth = mainQuadLaser.LaserEndWidth = lowWidth;
        float widthLerpDuration = 1;
        mainQuadLaser.AttemptFireWeapon();
        mainQuadLaser.Lasers.ForEach(l => l.LerpWidthOverAdjustedDuration(
            lowWidth, highWidth, widthLerpDuration));
        yield return new WaitForSeconds(5);
        //disable lasers
        mainQuadLaser.Lasers.ForEach(l => l.LerpWidthOverAdjustedDuration(
            highWidth, lowWidth, widthLerpDuration));
        yield return new WaitForSeconds(widthLerpDuration);
        mainQuadLaser.CancelFireWeapon();
        yield return new WaitForSeconds(2);
        rotateBehavior.enabled = true;
        wanderBehavior.enabled = true;
        timeSinceLastAttack = attackCooldown * 0.75f;
        mode = CombatMode.Idle;
    }

    private IEnumerator SpinupSequence(float spinupDuration, float holdDuration)
    {
        int directionModifier = Random.value > 0.5f ? 1 : -1;
        float maxSpinMagnitude = 0.25f;
        float startTime = Time.time;
        for (float currDuration = 0; currDuration < spinupDuration;
             currDuration = Time.time - startTime)
        {
            float lerpPercentage = currDuration / spinupDuration;
            float lerpVMagnitude = Mathf.Lerp(0.05f, maxSpinMagnitude, 
                lerpPercentage);
            rbody.angularVelocity = new Vector3(0, lerpVMagnitude * 
                directionModifier, 0);
            yield return null;
        }
        startTime = Time.time;
        for (float currDuration = 0; currDuration < holdDuration;
            currDuration = Time.time - startTime)
        {
            rbody.angularVelocity = new Vector3(0, maxSpinMagnitude *
                directionModifier, 0);
            yield return null;
        }
    }

    private IEnumerator SingleWarningLaserSequence(SingleLaser warningLaser)
    {
        mainQuadLaser.gameObject.SetActive(false);
        warningQuadLaser.gameObject.SetActive(true);
        warningLaser.FireWeapon();
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 2; i++)
        {
            warningLaser.CancelFireWeapon();
            yield return new WaitForSeconds(0.25f);
            warningLaser.FireWeapon();
            yield return new WaitForSeconds(0.25f);
        }
        for (int i = 0; i < 2; i++)
        {
            warningLaser.CancelFireWeapon();
            yield return new WaitForSeconds(0.15f);
            warningLaser.FireWeapon();
            yield return new WaitForSeconds(0.15f);
        }
        warningLaser.CancelFireWeapon();
        warningQuadLaser.gameObject.SetActive(false);
        mainQuadLaser.gameObject.SetActive(true);
    }

    private IEnumerator QuadWarningLaserSequence()
    {
        mainQuadLaser.gameObject.SetActive(false);
        warningQuadLaser.gameObject.SetActive(true);
        warningQuadLaser.FireWeapon();
        yield return new WaitForSeconds(2);
        for (int i = 0; i < 3; i++)
        {
            warningQuadLaser.CancelFireWeapon();
            yield return new WaitForSeconds(0.2f);
            warningQuadLaser.FireWeapon();
            yield return new WaitForSeconds(0.2f);
        }
        for (int i = 0; i < 5; i++)
        {
            warningQuadLaser.CancelFireWeapon();
            yield return new WaitForSeconds(0.125f);
            warningQuadLaser.FireWeapon();
            yield return new WaitForSeconds(0.125f);
        }
        warningQuadLaser.CancelFireWeapon();
        warningQuadLaser.gameObject.SetActive(false);
        mainQuadLaser.gameObject.SetActive(true);
    }

    private IEnumerator TrackPlayerOverDuration(Transform forwardTransform,
        float angularSpeedDegrees, float duration)
    {
        float startTime = Time.time;
        for (float currDuration = 0; currDuration < duration;
            currDuration = Time.time - startTime)
        {
            float angleBetween = Vector3.SignedAngle(forwardTransform.forward,
                playerTransform.position - forwardTransform.position, Vector3.up);
            transform.Rotate(new Vector3(0, angularSpeedDegrees * 
                (angleBetween >= 0 ? 1: -1) * Time.deltaTime, 0));
            yield return new WaitForEndOfFrame();
        }
    }
}
