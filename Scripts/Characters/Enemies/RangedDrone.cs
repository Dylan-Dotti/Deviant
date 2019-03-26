using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RangedDrone : Enemy
{
    private enum CombatMode { Chase, Retreat, Strafe };

    [SerializeField]
    private Weapon weapon;

    [Header("Ranges")]
    [SerializeField]
    private float chaseRange = 8;
    [SerializeField]
    private float retreatRange = 3;
    [SerializeField]
    private float AttackRange = 10;

    private CombatMode cMode = CombatMode.Chase;
    private NavMeshAgent navAgent;
    private LerpRotationToTarget rotator;
    private float rotatorStartSpeed;
    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
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

        //set rotation and fire weapon
        rotator.TargetPosition = playerTransform.position;
        float angleBetween = Vector3.Angle(rotator.transform.forward,
            playerTransform.position - transform.position);

        if (angleBetween <= 22.5f && playerDist <= AttackRange)
        {
            weapon.AttemptFireWeapon();
            rotator.AngularVelocityDegrees = rotatorStartSpeed;
        }
        else
        {
            rotator.AngularVelocityDegrees = Mathf.Lerp(rotatorStartSpeed,
                rotatorStartSpeed * 5, angleBetween / 180);
        }
    }

    protected override void OnPlayerDeath(Character c)
    {
        StopAllCoroutines();
        navAgent.ResetPath();
        enabled = false;
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
        enabled = true;
    }

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

    /*private void OnDrawGizmos()
    {
        foreach (Vector3 direction in GetRandomMoveDirections(
            -rotator.transform.forward, rotator.transform.right, 4, 3))
        {
            Gizmos.DrawRay(transform.position, direction * 4);
        }
        foreach (Vector3 direction in GetRandomMoveDirections(
            -rotator.transform.right, rotator.transform.forward, 4, 3))
        {
            Gizmos.DrawRay(transform.position, direction * 4);
        }
        foreach (Vector3 direction in GetRandomMoveDirections(
            rotator.transform.right, rotator.transform.forward, 4, 3))
        {
            Gizmos.DrawRay(transform.position, direction * 4);
        }
    }*/
}
