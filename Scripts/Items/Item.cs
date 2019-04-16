using System.Collections;
using UnityEngine;

/* Superclass for all items.
 * Currently, the only items are SpareParts.
 * Items will move to merge with the player after the player moves within range
 */
public abstract class Item : MonoBehaviour
{
    [Header("Auto Pickup")]
    [SerializeField]
    protected float maxMoveSpeed = 1f;
    [SerializeField]
    protected float acceleration = 1f;
    [SerializeField]
    protected float AutoPickupRadius = 3f;

    protected PlayerCharacter player;

    private Rigidbody rbody;
    private Vector3 originalScale;
    private Collider itemCollider;
    private bool isMovingToPlayer;

    private Coroutine moveToPlayerCR;
    private Coroutine despawnAfterCR;

    protected virtual void Awake()
    {
        rbody = GetComponent<Rigidbody>();
        originalScale = transform.localScale;
        itemCollider = GetComponent<Collider>();
        player = PlayerCharacter.Instance;
    }

    private void OnEnable()
    {
        player.PlayerDeathEvent += OnPlayerDeath;
        transform.localScale = originalScale;
        itemCollider.enabled = true;
        despawnAfterCR = StartCoroutine(DespawnAfterCR(30));
        moveToPlayerCR = StartCoroutine(AttemptMoveToPlayerCR());
    }

    private void OnDisable()
    {
        player.PlayerDeathEvent -= OnPlayerDeath;
        isMovingToPlayer = false;
        itemCollider.enabled = false;
        StopAllCoroutines();
    }

    // merge with player on collision
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.PLAYER_BODY_TAG)
        {
            itemCollider.enabled = false;
            MergeWithPlayer();
        }
        else if (other.tag == Tags.WALL_TAG)
        {
            rbody.velocity = Vector3.zero;
            if (isMovingToPlayer)
            {
                MoveToPlayer();
            }
        }
    }

    protected abstract void Despawn();

    protected void MoveToPlayer()
    {
        StopCoroutine(despawnAfterCR);
        StartCoroutine(MoveToPlayerCR());
    }

    protected abstract void MergeWithPlayer();

    protected virtual void OnPlayerDeath()
    {
        enabled = false;
    }

    // Move to merge with player
    private IEnumerator MoveToPlayerCR()
    {
        isMovingToPlayer = true;
        float moveStartTime = Time.time;
        while (true)
        {
            Vector3 moveDirection = (player.transform.position - 
                transform.position).normalized;
            float moveSpeed = Mathf.Clamp((Time.time - moveStartTime) *
                acceleration, 0, maxMoveSpeed);
            rbody.velocity = moveDirection * moveSpeed;
            yield return null;
        }
    }

    private IEnumerator AttemptMoveToPlayerCR()
    {
        yield return new WaitForSeconds(0.25f);
        while (Vector3.Distance(transform.position,
            player.transform.position) > AutoPickupRadius ||
            rbody.velocity.magnitude > 1.75f)
        {
            yield return null;
        }
        MoveToPlayer();
    }

    // Shrink and despawn
    private IEnumerator DespawnCR()
    {
        GetComponent<Collider>().enabled = false;
        float duration = 2;
        float lerpStartTime = Time.time;
        for (float elapsed = 0; elapsed < duration;
             elapsed = Time.time - lerpStartTime)
        {
            float lerpPercent = elapsed / duration;
            transform.localScale = originalScale * (1f - lerpPercent);
            yield return null;
        }
        Despawn();
    }

    //Despawn after the specified number of seconds
    private IEnumerator DespawnAfterCR(float delay)
    {
        yield return new WaitForSeconds(delay);
        StopCoroutine(moveToPlayerCR);
        StartCoroutine(DespawnCR());
    }
}
