using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserCubeDeathAnimation : AnimationSequence
{
    [SerializeField]
    private GameObject healthBar;
    [SerializeField]
    private List<GameObject> weapons;
    [SerializeField]
    private List<Renderer> launchCubes;
    [SerializeField]
    private List<LineRenderer> lasers;
    [SerializeField]
    private Material cubeLerpEndMaterial;
    [SerializeField]
    private AudioSource destabilizedSound;
    [SerializeField]
    private AudioSource explosionSound;
    [SerializeField]
    private ParticleSystem explosionParticles;

    private LaserCube laserCube;
    private MeshRenderer cubeRenderer;

    private void Awake()
    {
        laserCube = GetComponent<LaserCube>();
        cubeRenderer = GetComponent<MeshRenderer>();
    }

    private void LaunchCubes()
    {
        foreach (Renderer cube in launchCubes)
        {
            Vector3 launchDirection = (cube.transform.position - 
                transform.position).normalized;
            Rigidbody cubeRb = cube.gameObject.AddComponent<Rigidbody>();
            cubeRb.useGravity = false;
            cubeRb.AddForce(launchDirection * Random.Range(1f, 3f), 
                ForceMode.VelocityChange);
            cubeRb.AddTorque(Vector3.up * Random.Range(-1f, 1f), 
                ForceMode.VelocityChange);
        }
    }

    protected override IEnumerator AnimationSequenceCR()
    {
        healthBar.SetActive(false);
        Destroy(GetComponent<KnockbackPlayerOnContact>());
        Destroy(GetComponent<DamagePlayerOnContact>());
        //lerp cube materials
        destabilizedSound.Play();
        launchCubes.ForEach(c => c.gameObject.SetActive(true));
        StartCoroutine(ActivateLasersCR());
        yield return StartCoroutine(LerpCubeMaterials());
        GetComponent<Collider>().enabled = false;
        weapons.ForEach(w => w.SetActive(false));
        cubeRenderer.enabled = false;
        lasers.ForEach(l => l.enabled = false);
        destabilizedSound.Stop();
        explosionSound.Play();
        explosionParticles.Play();
        LaunchCubes();
        GetComponent<SparePartsGenerator>().GenerateSpareParts();
        yield return new WaitForSeconds(2f);
        yield return laserCube.LerpScaleOverDuration(
            launchCubes.Select(c => c.transform), 0, 1);
        Destroy(gameObject);
    }

    private IEnumerator ActivateLasersCR()
    {
        RandomizableList<LineRenderer> lasersRandomized =
            new RandomizableList<LineRenderer>();
        lasersRandomized.AddRange(lasers);
        lasersRandomized.Shuffle();
        for (int i = 0; i < 9; i++)
        {
            LineRenderer laser = lasersRandomized[i];
            StartCoroutine(LerpLaserWidthCR(laser));
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        }
    }

    private IEnumerator LerpLaserWidthCR(LineRenderer laser)
    {
        float endStartWidth = laser.startWidth;
        float endEndWidth = laser.endWidth;
        laser.enabled = true;
        float lerpDuration = Random.Range(0.5f, 1.5f);
        float lerpStartTime = Time.time;
        for (float elapsed = 0; elapsed < lerpDuration;
             elapsed = Time.time - lerpStartTime)
        {
            float lerpPercentage = elapsed / lerpDuration;
            laser.startWidth = Mathf.Lerp(0, endStartWidth, lerpPercentage);
            laser.endWidth = Mathf.Lerp(0, endEndWidth, lerpPercentage);
            yield return null;
        }
        laser.startWidth = endStartWidth;
        laser.endWidth = endEndWidth;
    }

    private IEnumerator LerpCubeMaterials()
    {
        Dictionary<Renderer, Material> originalMaterials =
            launchCubes.ToDictionary(r => r, r => new Material(r.material));
        float lerpDuration = 2.5f;
        float lerpStartTime = Time.time;
        for (float elapsed = 0; elapsed < lerpDuration; 
             elapsed = Time.time - lerpStartTime)
        {
            float lerpPercentage = elapsed / lerpDuration;
            launchCubes.ForEach(rend => rend.material.Lerp(originalMaterials[rend],
                cubeLerpEndMaterial, lerpPercentage));
            yield return null;
        }
    }
}
