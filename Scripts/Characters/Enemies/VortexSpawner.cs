using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class VortexSpawner : Enemy
{
    public override EnemyType EType => EnemyType.VortexSpawner;

    [SerializeField]
    private float attackRange = 10;
    [SerializeField]
    private VortexSpawnerBlaster blaster;
    [SerializeField]
    private VortexLauncher vortexLauncher;

    private VortexSpawnerDeathSequence deathSequence;
    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        deathSequence = GetComponent<VortexSpawnerDeathSequence>();
        playerTransform = PlayerCharacter.Instance.transform;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position,
            playerTransform.position) <= attackRange)
        {
            blaster.TurnToFace(playerTransform.position);
            blaster.AttemptFireWeapon();
        }
        vortexLauncher.AttemptFireWeapon();
    }

    protected override void ApplyScalars()
    {
        Debug.Log(EType.ToString());
        base.ApplyScalars();
        int oldMin = blaster.ProjectileDmgRange.Min;
        int oldMax = blaster.ProjectileDmgRange.Max;
        Debug.Log(oldMin + " " + oldMax);
        float dmgScalar = EnemyStrengthScalars.GetDamageScalar(EType);
        blaster.ProjectileDmgRange = new IntRange(Mathf.RoundToInt(
            oldMin * dmgScalar), Mathf.RoundToInt(oldMax * dmgScalar));
        Debug.Log(blaster.ProjectileDmgRange.Min + " " + blaster.ProjectileDmgRange.Max);
    }

    public override void Die()
    {
        EnemyDeathEvent?.Invoke(this);
        enabled = false;
        deathSequence.PlayAnimation();
    }

    protected override IEnumerator SpawnSequence()
    {
        enabled = PlayerCharacter.Instance.IsActiveInWorld;
        yield return new WaitForSeconds(3);
        GetComponent<NavMeshAgent>().enabled = true;
        GetComponent<IdleWander>().enabled = true;
    }
}
