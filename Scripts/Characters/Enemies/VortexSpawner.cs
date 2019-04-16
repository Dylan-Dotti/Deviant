using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/* The purple ones.
 * They wander around, shoot at the player, and 
 * occasionally fire a Vortex somewhere
 */
public class VortexSpawner : Enemy
{
    public override EnemyType EType => EnemyType.VortexSpawner;

    [SerializeField]
    private float attackRange = 10;
    [SerializeField]
    private VortexSpawnerBlaster blaster;
    [SerializeField]
    private VortexLauncher vortexLauncher;

    private VortexSpawnerDeathAnimation deathAnimation;
    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        deathAnimation = GetComponent<VortexSpawnerDeathAnimation>();
        playerTransform = PlayerCharacter.Instance.transform;
    }

    // fire at the player when close, launch a vortex if off cooldown
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
        base.ApplyScalars();
        int oldMin = blaster.ProjectileDmgRange.Min;
        int oldMax = blaster.ProjectileDmgRange.Max;
        float dmgScalar = EnemyStrengthScalars.GetDamageScalar(EType);
        blaster.ProjectileDmgRange = new IntRange(Mathf.RoundToInt(
            oldMin * dmgScalar), Mathf.RoundToInt(oldMax * dmgScalar));
    }

    public override void Die()
    {
        EnemyDeathEvent?.Invoke(this);
        enabled = false;
        deathAnimation.PlayAnimation();
    }

    protected override IEnumerator SpawnSequence()
    {
        enabled = PlayerCharacter.Instance.IsActiveInWorld;
        yield return new WaitForSeconds(3);
        GetComponent<NavMeshAgent>().enabled = true;
        GetComponent<IdleWander>().enabled = true;
    }
}
