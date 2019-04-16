using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

/* The green ones.
 * They copy themselves and chase the player 
 * around if they get close enough. Duplication 
 * is managed by a DuplicatorGroup
 */
public class Duplicator : Enemy
{
    public static UnityAction<Duplicator> DuplicatorDeathEvent;

    public override EnemyType EType => EnemyType.Duplicator;

    [SerializeField]
    private Duplicator duplicatorPrefab;

    private DuplicatorDeathAnimation deathAnimation;
    private SparePartsGenerator partsGenerator;
    private Rigidbody rBody;
    private NavMeshAgent navAgent;
    private NavMeshObstacle navObstacle;
    private Coroutine setDestinationRoutine;
    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        deathAnimation = GetComponent<DuplicatorDeathAnimation>();
        partsGenerator = GetComponent<SparePartsGenerator>();
        rBody = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
        navObstacle = GetComponentInChildren<NavMeshObstacle>();
        playerTransform = PlayerCharacter.Instance.transform;
    }

    public override void Die()
    {
        base.Die();
        if (navAgent.enabled)
        {
            navAgent.ResetPath();
            rBody.velocity = navAgent.velocity;
            navAgent.enabled = false;
        }
        navObstacle.enabled = false;
        DuplicatorDeathEvent?.Invoke(this);
        StopAllCoroutines();
        GetComponent<SparePartsGenerator>().GenerateSpareParts();
        deathAnimation.PlayAnimation();
    }

    protected override void OnPlayerDeath()
    {
        base.OnPlayerDeath();
        if (setDestinationRoutine != null)
        {
            StopCoroutine(setDestinationRoutine);
        }
        if (navAgent.enabled && navAgent.isOnNavMesh)
        {
            navAgent.ResetPath();
        }
    }

    /* Generate a clone and separate from it, also splitting 
     * the value of generated player currency in the process.
     * Called by the DuplicatorGroup of a given Duplicator
     */
    public Duplicator Duplicate()
    {
        navAgent.radius = 0;
        Duplicator clone = Instantiate(duplicatorPrefab, 
            transform.position, transform.rotation);
        clone.UndoScalars();
        //split value of currency drops
        FloatRange halfPartsValue = new FloatRange(partsGenerator.
            ValuePerPartRange.Min / 2f, partsGenerator.ValuePerPartRange.Max / 2f);
        partsGenerator.ValuePerPartRange = new IntRange(Mathf.CeilToInt(
            halfPartsValue.Min), Mathf.CeilToInt(halfPartsValue.Max));
        clone.partsGenerator.ValuePerPartRange = new IntRange(Mathf.FloorToInt(
            halfPartsValue.Min), Mathf.FloorToInt(halfPartsValue.Max));

        StartCoroutine(SeparateFromClone(clone));
        return clone;
    }

    /* The clones were having the scalars applied to them again with each 
     * new one spawned. Needed to undo when spawning them
     */
    private void UndoScalars()
    {
        CharacterHealth.SetCurrentAndMaxHealth(Mathf.RoundToInt(CharacterHealth.
            MaxHealth / EnemyStrengthScalars.GetHealthScalar(EType)));
        DamagePlayerOnContact contactDamager = GetComponent<DamagePlayerOnContact>();
        contactDamager.DamageAmount = Mathf.RoundToInt(contactDamager.
            DamageAmount / EnemyStrengthScalars.GetDamageScalar(EType));

    }

    protected override IEnumerator SpawnSequence()
    {
        yield return new WaitForSeconds(1);
        if (PlayerCharacter.Instance.IsActiveInWorld)
        {
            setDestinationRoutine = StartCoroutine(SetDestinationPeriodic());
        }
    }

    // Chase player if close enough
    private IEnumerator SetDestinationPeriodic()
    {
        bool isChasingPlayer = false;
        while (true)
        {
            if (Vector3.Distance(transform.position, 
                playerTransform.position) < 5)
            {
                if (!isChasingPlayer)
                {
                    navObstacle.enabled = false;
                    yield return null;
                    navAgent.enabled = true;
                    isChasingPlayer = true;
                }
                navAgent.SetDestination(playerTransform.position);
            }
            else if (isChasingPlayer)
            {
                navAgent.ResetPath();
                rBody.velocity = navAgent.velocity;
                navAgent.enabled = false;
                yield return new WaitForSeconds(0.05f);
                navObstacle.enabled = true;
                isChasingPlayer = false;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Launches this duplicator and its new clone in mostly opposite directions
    private IEnumerator SeparateFromClone(Duplicator clone)
    {
        Vector3 separationVelocity = new Vector3(
            Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f))
            .normalized * Random.Range(0.75f, 1.25f);
        rBody.velocity = separationVelocity;
        clone.rBody.velocity = -separationVelocity;

        List<Collider> colliders = new List<Collider>(
            GetComponentsInChildren<Collider>());
        foreach (Collider c in colliders) c.isTrigger = true;
        yield return new WaitForSeconds(0.3f);
        foreach (Collider c in colliders) c.isTrigger = false;
        navAgent.radius = 0.9f;
        clone.navAgent.radius = 0.9f;
    }

    //not currently called by anything
    private IEnumerator DuplicatePeriodically()
    {
        List<Collider> colliders = new List<Collider>();
        colliders.AddRange(GetComponentsInChildren<Collider>());
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 5f));
            Duplicator clone = Instantiate(duplicatorPrefab, 
                transform.position, transform.rotation);
            StartCoroutine(SeparateFromClone(clone));
        }
    }
}
