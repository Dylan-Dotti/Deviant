using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackPlayerOnContact : MonoBehaviour
{
    [SerializeField]
    private VelocityModifier knockbackModifier;
    [SerializeField]
    private float knockbackDuration;

    private PlayerController pController;

    private void Awake()
    {
        pController = PlayerCharacter.Instance.Controller;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        pController.RemoveVelocityModifier(knockbackModifier);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == Tags.PLAYER_BODY_TAG)
        {
            StopAllCoroutines();
            StartCoroutine(KnockbackPlayer());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.PLAYER_BODY_TAG)
        {
            StopAllCoroutines();
            StartCoroutine(KnockbackPlayer());
        }
    }

    private IEnumerator KnockbackPlayer()
    {
        pController.ResetMovement();
        pController.AddVelocityModifier(knockbackModifier);
        Vector3 knockbackDirection = (pController.transform.position -
            transform.position).normalized;
        Vector3 lerpStartVelocity = knockbackDirection * 
            knockbackModifier.MaxMagnitude;
        float knockbackStartTime = Time.time;
        while (Time.time - knockbackStartTime < knockbackDuration)
        {
            float lerpPercentage = Time.time - knockbackStartTime;
            knockbackModifier.Velocity = Vector3.Lerp(lerpStartVelocity,
                Vector3.zero, lerpPercentage);
            yield return null;
        }
        pController.RemoveVelocityModifier(knockbackModifier);
        //player.GetComponent<Rigidbody>().AddForce(knockbackDirection * knockbackModifier., ForceMode.Impulse);
    }
}
