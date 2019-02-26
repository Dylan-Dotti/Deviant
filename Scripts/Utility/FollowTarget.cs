using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform targetTransform;

    private NavMeshAgent navAgent;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(AcquireTarget());
    }

    private IEnumerator AcquireTarget()
    {
        while (true)
        {
            if (targetTransform != null)
            {
                navAgent.SetDestination(targetTransform.position);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
