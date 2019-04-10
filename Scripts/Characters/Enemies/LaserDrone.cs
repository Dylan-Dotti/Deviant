using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class LaserDrone : Enemy
{
    public override EnemyType EType => EnemyType.LaserDrone;

    [SerializeField]
    private float attackRange = 3;
    [SerializeField]
    private MultiLaser laserWeapon;
    [SerializeField]
    private List<ParticleSystem> trailParticles;

    private DroneDeathAnimation deathAnimation;
    private NavMeshAgent navAgent;
    private LerpRotationToTarget rotator;
    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        deathAnimation = GetComponent<DroneDeathAnimation>();
        navAgent = GetComponent<NavMeshAgent>();
        rotator = GetComponentInChildren<LerpRotationToTarget>();
        playerTransform = PlayerCharacter.Instance.transform;
    }

    protected override void Start()
    {
        base.Start();
        enabled = false;
    }

    private void Update()
    {
        rotator.TargetPosition = playerTransform.position;
        UpdateTrailParticles();
        if (Vector3.Angle(rotator.transform.forward,
            playerTransform.position - transform.position) <= 20)
        {
            laserWeapon.TurnToFace(playerTransform.position);
            if (Vector3.Distance(transform.position,
                playerTransform.position) <= attackRange)
            {
                laserWeapon.AttemptFireWeapon();
            }
            else if (laserWeapon.IsFiring)
            {
                laserWeapon.CancelFireWeapon();
            }
        }
        else if (laserWeapon.IsFiring)
        {
            laserWeapon.CancelFireWeapon();
        }
    }

    protected override void ApplyScalars()
    {
        base.ApplyScalars();
        int oldMin = laserWeapon.LaserDamagePerTick.Min;
        int oldMax = laserWeapon.LaserDamagePerTick.Max;
        float dmgScalar = EnemyStrengthScalars.GetDamageScalar(EType);
        laserWeapon.LaserDamagePerTick = new IntRange(Mathf.RoundToInt(
            oldMin * dmgScalar), Mathf.RoundToInt(oldMax * dmgScalar));
    }

    public override void Die()
    {
        base.Die();
        StopAllCoroutines();
        enabled = false;
        rotator.enabled = false;
        laserWeapon.CancelFireWeapon();
        navAgent.acceleration = .5f;
        GetComponent<Rigidbody>().drag = 0;
        deathAnimation.PlayAnimation();
    }

    protected override void OnPlayerDeath(Character c)
    {
        base.OnPlayerDeath(c);
        laserWeapon.CancelFireWeapon();
        //navAgent.ResetPath();
        StopAllCoroutines();
    }

    private void UpdateTrailParticles()
    {
        foreach (ParticleSystem trail in trailParticles)
        {
            Vector3 transformRight = rotator.transform.right;
            ParticleSystem.NoiseModule noiseMod = trail.noise;
            noiseMod.strengthXMultiplier = Mathf.Clamp(
                Mathf.Abs(transformRight.x), 0.2f, 0.8f);
            noiseMod.strengthZMultiplier = Mathf.Clamp(
                Mathf.Abs(transformRight.z), 0.2f, 0.8f);
        }
    }

    protected override IEnumerator SpawnSequence()
    {
        yield return new WaitForSeconds(1.5f);
        navAgent.enabled = true;
        rotator.enabled = true;
        StartCoroutine(SetDestinationPeriodic());
        enabled = PlayerCharacter.Instance.IsActiveInWorld;
    }

    private IEnumerator SetDestinationPeriodic()
    {
        while (true)
        {
            navAgent.destination = playerTransform.position;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
