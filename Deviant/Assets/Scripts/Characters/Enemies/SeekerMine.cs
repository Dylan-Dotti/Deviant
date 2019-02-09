using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SeekerMine : Character
{
    public enum SeekerMineState { IDLE, WARN, SEEK, EXPLODE }

    [SerializeField]
    private float lightLerpDuration = 3f;
    private bool isLerping = false;

    [SerializeField]
    private List<Renderer> lightRenderers;

    [Header("Light Materials")]
    [SerializeField]
    private Material greenMaterial;
    [SerializeField]
    private Material orangeMaterial;
    [SerializeField]
    private Material redMaterial;

    [Header("State Ranges")]
    [SerializeField]
    private float warnRange = 8f;
    [SerializeField]
    private float seekRange = 4f;
    [SerializeField]
    private float explodeRange = 1.5f;

    [Header("Explosion Effect")]
    [SerializeField]
    private float maxExplosionDamage = 20f;
    [SerializeField]
    private RangeFloat damageFalloffInterval = new RangeFloat(1.5f, 5);
    [SerializeField]
    private GameObject bodyObject;
    [SerializeField]
    private GameObject healthBar;
    [SerializeField]
    private GameObject explosionParticles;

    private SeekerMineState combatState = SeekerMineState.IDLE;
    private NavMeshAgent navAgent;
    private IdleWander wanderBehavior;
    private PlayerCharacter player;

    protected override void Awake()
    {
        base.Awake();
        navAgent = GetComponent<NavMeshAgent>();
        wanderBehavior = GetComponent<IdleWander>();
    }

    private void Start()
    {
        player = PlayerCharacter.Instance;
        StartCoroutine(LerpLightsToStatePeriodic());
        StartCoroutine(SetDestinationPeriodic());
    }

    private void Update()
    {
        float playerDistance = Vector3.Distance(transform.position,
            player.transform.position);
        if (playerDistance <= explodeRange)
        {
            if (combatState != SeekerMineState.EXPLODE)
            {
                combatState = SeekerMineState.EXPLODE;
                Die();
            }
        }
        else if (playerDistance <= seekRange)
        {
            combatState = SeekerMineState.SEEK;
        }
        else if (playerDistance <= warnRange)
        {
            combatState = SeekerMineState.WARN;
        }
        else
        {
            combatState = SeekerMineState.IDLE;
        }
    }

    public override void Die()
    {
        StopAllCoroutines();
        navAgent.ResetPath();
        StartCoroutine(ShrinkAndExplode());
    }

    private IEnumerator SetDestinationPeriodic()
    {
        while (true)
        {
            if (combatState == SeekerMineState.SEEK)
            {
                if (wanderBehavior.enabled)
                {
                    wanderBehavior.DisableWander();
                }
                navAgent.SetDestination(player.transform.position);
            }
            else
            {
                if (!wanderBehavior.enabled)
                {
                    //navAgent.acceleration = 10f;
                    navAgent.ResetPath();
                    wanderBehavior.EnableWander();
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator LerpLightsToStatePeriodic()
    {
        SeekerMineState prevState = combatState;
        while (true)
        {
            if (combatState != prevState && !isLerping)
            {
                StartCoroutine(LerpLights());
                prevState = combatState;
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator LerpLights()
    {
        isLerping = true;

        //set up materials
        Dictionary<Renderer, Material> lerpStartMaterials = lightRenderers.
            ToDictionary(r => r, r => new Material(r.material));
        Material lerpEndMaterial = null;
        switch (combatState)
        {
            case SeekerMineState.IDLE:
                lerpEndMaterial = greenMaterial;
                break;
            case SeekerMineState.WARN:
                lerpEndMaterial = orangeMaterial;
                break;
            case SeekerMineState.SEEK:
                lerpEndMaterial = redMaterial;
                break;
        }

        //lerp materials
        float lerpStartTime = Time.time;
        while (Time.time - lerpStartTime < lightLerpDuration)
        {
            float lerpPercentage = Mathf.Min(Time.time - lerpStartTime, lightLerpDuration) / lightLerpDuration;
            foreach (Renderer lightRend in lerpStartMaterials.Keys)
            {
                lightRend.material.Lerp(lerpStartMaterials[lightRend], lerpEndMaterial, lerpPercentage);
            }
            yield return null;
        }
        foreach (Renderer lightRend in lightRenderers)
        {
            lightRend.material = lerpEndMaterial;
        }

        isLerping = false;
    }

    private IEnumerator ShrinkAndExplode()
    {
        //shrink
        float shrinkDuration = 0.15f;
        LerpScaleOverDuration(new List<Transform>(1) { bodyObject.transform }, 0.85f, shrinkDuration);
        yield return new WaitForSeconds(shrinkDuration - 0.05f);

        //explode
        float playerDist = Vector3.Distance(transform.position, player.transform.position);
        if (playerDist < damageFalloffInterval.min)
        {
            player.CharacterHealth.CurrentHealth -= maxExplosionDamage;
        }
        else if (playerDist < damageFalloffInterval.max)
        {
            player.CharacterHealth.CurrentHealth -= Mathf.Round(
                Mathf.Lerp(maxExplosionDamage, 0, (playerDist - damageFalloffInterval.min) /
                (damageFalloffInterval.max - damageFalloffInterval.min)));
        }
        CameraShake.Instance.ShakeByDistance(10f, playerDist, 15f);
        explosionParticles.SetActive(true);
        yield return null;
        bodyObject.SetActive(false);
        healthBar.SetActive(false);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
