using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMagnet))]
public class Vortex : Enemy
{
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
        EnemyDeathEvent?.Invoke(this);
        deathSequence.PlayAnimation();
    }

    private void OnEnable()
    {
        magnet.enabled = true;
    }

    private void OnDisable()
    {
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
