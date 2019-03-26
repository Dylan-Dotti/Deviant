using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class VortexSpawner : Enemy
{
    private VortexSpawnerDeathSequence deathSequence;

    protected override void Awake()
    {
        base.Awake();
        deathSequence = GetComponent<VortexSpawnerDeathSequence>();
    }

    public override void Die()
    {
        EnemyDeathEvent?.Invoke(this);
        deathSequence.PlayAnimation();
    }

    protected override IEnumerator SpawnSequence()
    {
        yield return new WaitForSeconds(3);
        GetComponent<NavMeshAgent>().enabled = true;
        GetComponent<IdleWander>().enabled = true;
    }
}
