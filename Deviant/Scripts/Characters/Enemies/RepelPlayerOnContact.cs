using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepelPlayerOnContact : MonoBehaviour
{
    [SerializeField]
    private float repulsionMagnitude = 1f;

    private PlayerCharacter player;

    private void Awake()
    {
        player = PlayerCharacter.Instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == PlayerCharacter.PLAYER_BODY_TAG)
        {
            RepelPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == PlayerCharacter.PLAYER_BODY_TAG)
        {
            RepelPlayer();
        }
    }

    private void RepelPlayer()
    {
        PlayerController pController = player.Controller;
        pController.ResetMovement();
        Vector3 repelDirection = (player.transform.position - transform.position).normalized;
        player.GetComponent<Rigidbody>().AddForce(repelDirection * repulsionMagnitude, ForceMode.Impulse);
    }
}
