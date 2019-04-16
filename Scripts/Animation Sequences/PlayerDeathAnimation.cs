using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathAnimation : AnimationSequence
{
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private GameObject playerBody;
    [SerializeField]
    private DamagedEffect damagedEffect;
    [SerializeField]
    private ParticleSystem explosionParticles;
    [SerializeField]
    private AudioSource explosionSource;
    [SerializeField]
    private AudioClip explosionSound;

    protected override IEnumerator AnimationSequenceCR()
    {
        healthBar.gameObject.SetActive(false);
        yield return PlayerCharacter.Instance.LerpScaleOverDuration(
            new List<Transform>(){ playerBody.transform }, 0.9f, 0.2f);
        damagedEffect.enabled = false;
        explosionParticles.Play();
        explosionSource.PlayOneShot(explosionSound);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        yield return null;
        playerBody.SetActive(false);
    }
}
