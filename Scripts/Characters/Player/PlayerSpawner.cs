using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : AnimationSequence
{
    [SerializeField]
    private GameObject playerBody;
    [SerializeField]
    private GameObject playerUI;
    //remove later
    [SerializeField]
    private EnemySpawnerInitSequence enemySpawnerInit;

    [Header("Particle Effects")]
    [SerializeField]
    private ParticleSystem implosionParticles;
    [SerializeField]
    private ParticleSystem centralParticles;
    [SerializeField]
    private ParticleSystem explosionParticles;

    private PlayerController pController;

    private void Awake()
    {
        pController = PlayerCharacter.Instance.Controller;
    }

    private void Start()
    {
        playerBody.SetActive(false);
        playerUI.SetActive(false);
        StartCoroutine(PlayAnimationSequence());
    }

    protected override IEnumerator PlayAnimationSequence()
    {
        pController.PlayerInputEnabled = false;
        implosionParticles.Play();
        yield return new WaitForSeconds(0.75f);
        centralParticles.Play();

        //lerp implosion speed
        ParticleSystem.MainModule implosionMain = implosionParticles.main;
        float startSimSpeed = implosionMain.simulationSpeed;
        float lerpStartTime = Time.time;
        while (Time.time - lerpStartTime < 3f)
        {
            float lerpPercentage = (Time.time - lerpStartTime) / 3f;
            implosionMain.simulationSpeed = Mathf.Lerp(startSimSpeed, 2f, lerpPercentage);
            yield return null;
        }

        implosionParticles.Stop();
        yield return new WaitForSeconds(0.5f);
        centralParticles.Stop();
        yield return new WaitForSeconds(0.95f);
        explosionParticles.Play();
        playerBody.SetActive(true);
        playerUI.SetActive(true);
        pController.PlayerInputEnabled = true;
        //enemySpawnerInit.enabled = true;
        Destroy(this);
    }
}
