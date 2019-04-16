using System.Collections;
using UnityEngine;

/* Peroidically spawned by VortexSpawners
 */
[RequireComponent(typeof(PlayerMagnet))]
public class Vortex : Enemy
{
    public override EnemyType EType => EnemyType.Vortex;

    [SerializeField]
    private float moveSpeed = 1f;

    private PlayerMagnet magnet;
    private PlayerCharacter player;
    private VortexDeathSequence deathSequence;

    protected override void Awake()
    {
        base.Awake();
        player = PlayerCharacter.Instance;
        magnet = GetComponent<PlayerMagnet>();
        deathSequence = GetComponent<VortexDeathSequence>();
    }

    public override void Die()
    {
        base.Die();
        deathSequence.PlayAnimation();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        magnet.enabled = true;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        magnet.enabled = false;
    }

    public void MoveToPosition(Vector3 targetPos)
    {
        StartCoroutine(LerpToPosition(targetPos));
    }

    protected override IEnumerator SpawnSequence()
    {
        yield return null;
    }

    // Move to given spawn position
    private IEnumerator LerpToPosition(Vector3 targetPos)
    {
        targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);

        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, 
                targetPos, moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
