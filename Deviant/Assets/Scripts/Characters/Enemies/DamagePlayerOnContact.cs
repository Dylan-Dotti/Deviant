using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerOnContact : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 0.5f;
    [SerializeField]
    private float damageInterval = 1f;

    private Health playerHealth;
    private float timeSinceDamagingPlayer;

    private void Awake()
    {
        playerHealth = PlayerCharacter.Instance.CharacterHealth;
        timeSinceDamagingPlayer = damageInterval;
    }

    private void Update()
    {
        timeSinceDamagingPlayer += Time.deltaTime;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == PlayerCharacter.PLAYER_BODY_TAG)
        {
            AttemptDamagePlayer();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == PlayerCharacter.PLAYER_BODY_TAG)
        {
            AttemptDamagePlayer();
        }
    }

    private void AttemptDamagePlayer()
    {
        if (timeSinceDamagingPlayer > damageInterval)
        {
            playerHealth.CurrentHealth -= damageAmount;
            timeSinceDamagingPlayer = 0f;
        }
    }
}
