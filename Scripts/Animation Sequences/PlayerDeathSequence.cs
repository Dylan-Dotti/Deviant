using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathSequence : AnimationSequence
{
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private GameObject playerBody;
    [SerializeField]
    private DamagedEffect damagedEffect;
    [SerializeField]
    private ParticleSystem explosionParticles;

    protected override IEnumerator PlayAnimationSequence()
    {
        IsPlaying = true;
        healthBar.gameObject.SetActive(false);
        PlayerCharacter.Instance.LerpScaleOverDuration(new List<Transform>(){ playerBody.transform }, 0.9f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        damagedEffect.enabled = false;
        explosionParticles.Play();
        yield return null;
        playerBody.SetActive(false);
        IsPlaying = false;
    }
}
