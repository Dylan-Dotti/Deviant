using UnityEngine;

public class DamagePlayerOnContact : MonoBehaviour
{
    [SerializeField]
    private int damageAmount = 5;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == Tags.PLAYER_BODY_TAG)
        {
            AttemptDamagePlayer();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == Tags.PLAYER_BODY_TAG)
        {
            AttemptDamagePlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.PLAYER_BODY_TAG)
        {
            AttemptDamagePlayer();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == Tags.PLAYER_BODY_TAG)
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
