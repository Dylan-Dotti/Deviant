using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class DroneDeathAnimation : AnimationSequence
{
    [SerializeField]
    private GameObject body;
    [SerializeField]
    private GameObject leftArm;
    [SerializeField]
    private GameObject rightArm;
    [SerializeField]
    private GameObject leftWeapon;
    [SerializeField]
    private GameObject rightWeapon;
    [SerializeField]
    private GameObject healthBar;

    [Header("Particle Effects")]
    [SerializeField]
    private ParticleSystem bodyExplosionParticles;
    [SerializeField]
    private List<ParticleSystem> armExplosionParticles;
    [SerializeField]
    private List<ParticleSystem> trails;

    private Enemy drone;
    private NavMeshAgent navAgent;

    private void Awake()
    {
        drone = GetComponent<Enemy>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    protected override IEnumerator AnimationSequenceCR()
    {
        leftWeapon.GetComponent<WeaponRecoil>()?.StopRecoil();
        rightWeapon.GetComponent<WeaponRecoil>()?.StopRecoil();
        healthBar.SetActive(false);
        if (trails.Count > 0)
        {
            StartCoroutine(LerpTrailLifetimes(2));
        }
        yield return StartCoroutine(JettisonWeapons());
        yield return StartCoroutine(ShrinkAndExplode());
        List<Transform> armsAndWeapons = new List<Transform> {
            leftArm.transform, rightArm.transform, leftWeapon.transform,
            rightWeapon.transform };
        yield return drone.LerpScaleOverDuration(armsAndWeapons, 0, 1);
        armsAndWeapons.ForEach(x => Destroy(x.gameObject));
        Destroy(gameObject);
    }

    private IEnumerator JettisonWeapons()
    {
        //setup components
        Rigidbody bodyRb = body.AddComponent<Rigidbody>();
        bodyRb.useGravity = false;
        leftArm.GetComponent<Collider>().enabled = false;
        Rigidbody leftArmRb = leftArm.AddComponent<Rigidbody>();
        leftArmRb.useGravity = false;
        leftWeapon.GetComponent<Collider>().enabled = false;
        Rigidbody leftWeapRb = leftWeapon.AddComponent<Rigidbody>();
        leftWeapRb.useGravity = false;
        rightArm.GetComponent<Collider>().enabled = false;
        Rigidbody rightArmRb = rightArm.AddComponent<Rigidbody>();
        rightArmRb.useGravity = false;
        rightWeapon.GetComponent<Collider>().enabled = false;
        Rigidbody rightWeapRb = rightWeapon.AddComponent<Rigidbody>();
        rightWeapRb.useGravity = false;

        //launch arms and weapons
        Vector3 firstBodySpin = Vector3.up * Random.Range(1f, 2.5f) * 
            (Random.value >= 0.5f ? 1 : -1);
        Rigidbody launchWeapRb = Random.value >= 0.5f ? leftWeapRb : rightWeapRb;
        Rigidbody launchArmRb = launchWeapRb == leftWeapRb ? leftArmRb : rightArmRb;
        for (int i = 0; i < 2; i++)
        {
            Vector3 launchDirection = (launchWeapRb.transform.position -
                body.transform.position).normalized;
            launchWeapRb.transform.parent = null;
            launchArmRb.transform.parent = null;
            launchWeapRb.AddForce(launchDirection * Random.Range(.5f, 1f) + navAgent.velocity / 1.5f,
                //body.transform.forward * Random.Range(-.5f, .5f) + navAgent.velocity,
                ForceMode.VelocityChange);
            launchWeapRb.AddTorque(Vector3.up * Random.Range(-2f, 2f), ForceMode.VelocityChange);
            launchArmRb.AddForce(launchDirection * Random.Range(.5f, 1f) + navAgent.velocity / 1.5f,
                //body.transform.forward * Random.Range(-.5f, .5f) + navAgent.velocity,
                ForceMode.VelocityChange);
            launchArmRb.AddTorque(Vector3.up * Random.Range(-2f, 2f), ForceMode.VelocityChange);
            bodyRb.angularVelocity = i == 0 ? firstBodySpin : -firstBodySpin *
                Random.Range(0.75f, 1.5f);
            armExplosionParticles[launchWeapRb == leftWeapRb ? 0 : 1].Play();
            yield return new WaitForSeconds(Random.Range(0.4f, 0.6f));
            launchWeapRb = launchWeapRb == leftWeapRb ? rightWeapRb : leftWeapRb;
            launchArmRb = launchWeapRb == leftWeapRb ? leftArmRb : rightArmRb;
        }
    }

    private IEnumerator LerpTrailLifetimes(float duration)
    {
        Dictionary<ParticleSystem, ParticleSystem.MainModule> trailmodules =
            trails.ToDictionary(t => t, t => t.main);
        float startMinLife = trailmodules[trails[0]].startLifetime.constantMin;
        float startMaxLife = trailmodules[trails[0]].startLifetime.constantMax;
        float lerpStartTime = Time.time;
        for (float elapsed = 0; elapsed < duration; 
            elapsed = Time.time - lerpStartTime)
        {
            float lerpPercent = elapsed / duration;
            float lerpMin = Mathf.Lerp(startMinLife, 0, lerpPercent);
            float lerpMax = Mathf.Lerp(startMaxLife, 0, lerpPercent);
            foreach (ParticleSystem trail in trails)
            {
                ParticleSystem.MainModule main = trail.main;
                main.startLifetime = new ParticleSystem.MinMaxCurve(
                    lerpMin, lerpMax);
            }
            yield return null;
        }
    }

    private IEnumerator ShrinkAndExplode()
    {
        yield return drone.LerpScaleOverDuration(new List<Transform> {
            body.transform }, 0.8f, 0.15f);

        //explosionSound.Play();
        //CameraShake.Instance.ShakeByDistance(10f, playerDist, 15f);
        bodyExplosionParticles.Play();
        body.SetActive(false);
        yield return null;
        GetComponent<SparePartsGenerator>().GenerateSpareParts();
        while (bodyExplosionParticles.particleCount > 0)
        {
            yield return null;
        }
    }
}
