using System.Collections;
using UnityEngine;

public class PlayerSpawnAnimation : AnimationSequence
{
    [SerializeField]
    private GameObject playerBody;
    [SerializeField]
    private GameObject playerUI;

    [Header("Particle Effects")]
    [SerializeField]
    private ParticleSystem implosionParticles;
    [SerializeField]
    private ParticleSystem centralParticles;
    [SerializeField]
    private ParticleSystem explosionParticles;

    [Header("Sound Effects")]
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip chargeUpSound;
    [SerializeField]
    private AudioClip chargeMaxSound;
    [SerializeField]
    private AudioClip explosionSound;

    private PlayerController pController;

    private void Awake()
    {
        pController = PlayerCharacter.Instance.Controller;
    }

    private void Start()
    {
        PlayAnimation();
    }

    protected override IEnumerator AnimationSequenceCR()
    {
        playerBody.SetActive(false);
        playerUI.SetActive(false);
        pController.PlayerInputEnabled = false;
        implosionParticles.Play();
        StartCoroutine(SpawnChargeSoundCR());
        yield return new WaitForSeconds(0.75f);
        centralParticles.Play();

        //lerp implosion speed and center speed
        ParticleSystem.MainModule implosionMain = implosionParticles.main;
        ParticleSystem.MainModule centralMain = centralParticles.main;
        float startSimSpeed = implosionMain.simulationSpeed;
        float lerpStartTime = Time.time;
        while (Time.time - lerpStartTime < 2f)
        {
            float lerpPercentage = (Time.time - lerpStartTime) / 2f;
            implosionMain.simulationSpeed = Mathf.Lerp(startSimSpeed, 2f, lerpPercentage);
            centralMain.simulationSpeed = Mathf.Lerp(1f, 3f, lerpPercentage);
            yield return null;
        }

        implosionParticles.Stop();
        yield return new WaitForSeconds(0.4f);
        //yield return new WaitForSeconds(0.5f);
        centralParticles.Stop();
        yield return new WaitForSeconds(0.55f);
        explosionParticles.Play();
        source.Stop();
        source.PlayOneShot(explosionSound);
        playerBody.SetActive(true);
        playerUI.SetActive(true);
        pController.PlayerInputEnabled = true;
        while (source.isPlaying)
        {
            yield return null;
        }
        Destroy(source);
        Destroy(this);
    }

    private IEnumerator SpawnChargeSoundCR()
    {
        source.clip = chargeUpSound;
        source.Play();
        while (source.isPlaying)
        {
            yield return null;
        }
        source.clip = chargeMaxSound;
        source.loop = true;
        source.Play();
    }
}
