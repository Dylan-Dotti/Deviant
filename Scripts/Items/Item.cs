using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Collider itemCollider;
    private bool isMovingToPlayer;

    protected virtual void Awake()
    {
        rbody = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();
        player = PlayerCharacter.Instance;
    }

    private void OnEnable()
    {
        itemCollider.enabled = true;
        StartCoroutine(AttemptMoveToPlayerCR());
    }

    private void OnDisable()
    {
        isMovingToPlayer = false;
        StopAllCoroutines();
    }

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

    public void MoveToPlayer()
    {
        StopAllCoroutines();
        StartCoroutine(MoveToPlayerCR());
    }

    public abstract void MergeWithPlayer();

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
            rbody.velocity.magnitude > 1f)
        {
            yield return null;
        }
        MoveToPlayer();
    }
}
