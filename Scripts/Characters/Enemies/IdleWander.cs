using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class IdleWander : MonoBehaviour
{
    public Vector3 WanderCenter { get; set; }
    public float WanderRadius { get { return wanderRadius; } set { wanderRadius = value; } }

    public bool IsWandering { get; private set; }

    [SerializeField]
    private float wanderSpeed = 1.0f;
    [SerializeField]
    private float wanderAccel = 2.0f;
    [SerializeField]
    private float wanderRadius = 1.0f;
    [SerializeField]
    private Vector2 wanderIntervalRange;

    private NavMeshAgent navAgent;
    private float navAgentOriginalSpeed;
    private float navAgentOriginalAccel;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        WanderCenter = transform.position;
        StartCoroutine(WanderPeriodic());
    }

    private void OnDisable()
    {
        if (IsWandering)
        {
            navAgent.speed = navAgentOriginalSpeed;
            navAgent.acceleration = navAgentOriginalAccel;
            IsWandering = false;
        }
        StopAllCoroutines();
        if (navAgent.isOnNavMesh)
        {
            navAgent.ResetPath();
        }
    }

    public void EnableWander()
    {
        enabled = true;
    }

    public void DisableWander()
    {
        enabled = false;
    }

    private Vector3 GetRandomWanderPosition()
    {
        float radius = Random.Range(0f, wanderRadius);
        float angleRad = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float xPos = radius * Mathf.Cos(angleRad);
        float zPos = radius * Mathf.Sin(angleRad);
        return new Vector3(WanderCenter.x + xPos, 
            transform.position.y, WanderCenter.z + zPos);
    }

    //Changing speeds while wander is enabled is not recommended
    private IEnumerator WanderPeriodic()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(
                wanderIntervalRange.x, wanderIntervalRange.y));
            //calculate destination
            Vector3 wanderTargetPos = GetRandomWanderPosition();
            NavMeshPath path = new NavMeshPath();
            for (int i = 0; i < 500; i++)
            {
                if (navAgent.CalculatePath(wanderTargetPos, path))
                {
                    navAgent.path = path;
                    break;
                }
            }

            //move to destination
            IsWandering = true;
            navAgentOriginalSpeed = navAgent.speed;
            navAgentOriginalAccel = navAgent.acceleration;
            navAgent.speed = wanderSpeed;
            navAgent.acceleration = wanderAccel;
            while (Vector3.Distance(transform.position, wanderTargetPos) > 0.33f)
            {
                yield return null;
            }
            navAgent.speed = navAgentOriginalSpeed;
            navAgent.acceleration = navAgentOriginalAccel;
            IsWandering = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, WanderRadius);
    }
}
