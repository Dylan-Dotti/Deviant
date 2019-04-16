using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* The reddish ones that shoot.
 * They try to stay within a specified range of the player, 
 * and will advance or retreat if outside those bounds.
 * If within the specified range, will strafe left and right 
 * randomly.
 * 
 * Raycasting "whiskers" are used to avoid strafing and retreating 
 * into other enemies and walls
 */
[RequireComponent(typeof(NavMeshAgent))]
public class RangedDrone : Enemy
{
    public override EnemyType EType => EnemyType.RangedDrone;

    private enum CombatMode { Chase, Retreat, Strafe };

    [SerializeField]
    private DualBlaster blaster;

    [Header("Ranges")]
    [SerializeField]
    private float chaseRange = 8;
    [SerializeField]
    private float retreatRange = 3;
    [SerializeField]
    private float AttackRange = 10;

    private CombatMode cMode = CombatMode.Chase;
    private DroneDeathAnimation deathAnimation;
    private NavMeshAgent navAgent;
    private LerpRotationToTarget rotator;
    private float rotatorStartSpeed;
    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        deathAnimation = GetComponent<DroneDeathAnimation>();
        navAgent = GetComponent<NavMeshAgent>();
        rotator = GetComponentInChildren<LerpRotationToTarget>();
        rotatorStartSpeed = rotator.AngularVelocityDegrees;
        playerTransform = PlayerCharacter.Instance.transform;
    }

    protected override void Start()
    {
        base.Start();
        enabled = false;
    }

    // Update combat state based on distance from player; fire weapon
    private void Update()
    {
        float playerDist = Vector3.Distance(transform.position,
            playerTransform.position);

        //set state
        if (playerDist >= chaseRange)
        {
            cMode = CombatMode.Chase;
        }
        else if (playerDist <= retreatRange)
        {
            cMode = CombatMode.Retreat;
        }
        else
        {
            cMode = CombatMode.Strafe;
        }

        //set rotation and fire weapon if in range
        rotator.TargetPosition = playerTransform.position;
        float angleBetween = Vector3.Angle(rotator.transform.forward,
            playerTransform.position - transform.position);

        if (angleBetween <= 22.5f && playerDist <= AttackRange)
        {
            blaster.AttemptFireWeapon();
            rotator.AngularVelocityDegrees = rotatorStartSpeed;
        }
        else
        {
            rotator.AngularVelocityDegrees = Mathf.Lerp(rotatorStartSpeed,
                rotatorStartSpeed * 5, angleBetween / 180);
        }
    }

    public override void Die()
    {
        base.Die();
        StopAllCoroutines();
        enabled = false;
        rotator.enabled = false;
        deathAnimation.PlayAnimation();
    }

    protected override void OnPlayerDeath()
    {
        base.OnPlayerDeath();
        cMode = CombatMode.Strafe;
    }

    protected override void ApplyScalars()
    {
        base.ApplyScalars();
        int oldMin = blaster.ProjectileDmgRange.Min;
        int oldMax = blaster.ProjectileDmgRange.Max;
        float dmgScalar = EnemyStrengthScalars.GetDamageScalar(EType);
        blaster.ProjectileDmgRange = new IntRange(Mathf.RoundToInt(
            oldMin * dmgScalar), Mathf.RoundToInt(oldMax * dmgScalar));
    }

    private Vector3 GetChasePosition()
    {
        return playerTransform.position;
    }

    private Vector3 GetStrafePosition(float range)
    {
        List<Vector3> sideMoveDirections = new List<Vector3>();
        //add left
        sideMoveDirections.AddRange(GetRandomMoveDirections(
            -rotator.transform.right, rotator.transform.forward, range + 1, 5));
        //add right
        sideMoveDirections.AddRange(GetRandomMoveDirections(
            rotator.transform.right, rotator.transform.forward, range + 1, 5));
        if (sideMoveDirections.Count == 0)
        {
            return transform.position;
        }
        return transform.position + sideMoveDirections[
            Random.Range(0, sideMoveDirections.Count)] * range;
    }

    private Vector3 GetRetreatPosition(float range)
    {
        List<Vector3> backMoveDirections = GetRandomMoveDirections(
            -rotator.transform.forward, rotator.transform.right, range + 1, 5);
        if (backMoveDirections.Count == 0)
        {
            return GetStrafePosition(range);
        }
        return transform.position + backMoveDirections[
            Random.Range(0, backMoveDirections.Count)] * range;
    }

    /* Generates a number of possible move directions in the general 
     * direction of baseDirection, omitting directions that hit 
     * another enemy or a wall
     */
    private List<Vector3> GetRandomMoveDirections(Vector3 baseDirection,
        Vector3 variationDirection, float maxRange, int numSamples)
    {
        List<Vector3> possibleMoveDirections = new List<Vector3>();
        for (int i = 0; i < numSamples; i++)
        {
            possibleMoveDirections.Add((baseDirection + variationDirection * 
                Random.Range(-1f, 1f)).normalized);
        }

        List<Vector3> validMoveDirections = new List<Vector3>();
        validMoveDirections.AddRange(possibleMoveDirections);

        //check collisions
        foreach (Vector3 direction in possibleMoveDirections)
        {
            Ray ray = new Ray(transform.position, direction);
            foreach (RaycastHit hit in Physics.RaycastAll(ray, maxRange))
            {
                if (hit.transform.tag == Tags.WALL_TAG || 
                    hit.transform.tag == Tags.ENEMY_TAG)
                {
                    validMoveDirections.Remove(direction);
                    break;
                }

            }
        }
        return validMoveDirections;
    }

    protected override IEnumerator SpawnSequence()
    {
        yield return new WaitForSeconds(1.5f);
        navAgent.enabled = true;
        StartCoroutine(SetMoveTargetPeriodic());
        enabled = PlayerCharacter.Instance.IsActiveInWorld;
    }

    // Periodically sets the move target for the NavMeshAgent based on state
    private IEnumerator SetMoveTargetPeriodic()
    {
        while (true)
        {
            switch (cMode)
            {
                case CombatMode.Chase:
                    navAgent.stoppingDistance = chaseRange - 1;
                    navAgent.destination = GetChasePosition();
                    break;
                case CombatMode.Retreat:
                    navAgent.stoppingDistance = 0;
                    Vector3 retreatPos = GetRetreatPosition(3);
                    if (retreatPos != transform.position)
                    {
                        navAgent.destination = retreatPos;
                    }
                    else
                    {
                        navAgent.ResetPath();
                    }
                    break;
                case CombatMode.Strafe:
                    if (Random.Range(0, 2) == 1)
                    {
                        navAgent.stoppingDistance = 0;
                        navAgent.destination = GetStrafePosition(3);
                    }
                    break;
            }
            yield return new WaitForSeconds(0.66f);
        }
    }
}
