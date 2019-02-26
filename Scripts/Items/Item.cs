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

    protected Rigidbody rbody;
    protected PlayerCharacter player;

    private bool isMovingToPlayer;

    protected virtual void Awake()
    {
        rbody = GetComponent<Rigidbody>();
        player = PlayerCharacter.Instance;
    }

    private void LateUpdate()
    {
        if (Vector3.Distance(transform.position, 
            player.transform.position) <= AutoPickupRadius &&
            rbody.velocity.magnitude <= 1f)
        {
            MoveToPlayer();
            enabled = false;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == PlayerCharacter.PLAYER_BODY_TAG)
        {
            GetComponentInChildren<Collider>().enabled = false;
            MergeWithPlayer();
        }
        else if (other.tag == "Wall")
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
}
