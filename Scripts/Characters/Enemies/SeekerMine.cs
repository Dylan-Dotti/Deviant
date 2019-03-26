using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeekerMine : Enemy
{
    public enum SeekerMineState { IDLE, WARN, SEEK }

    [SerializeField]
    private float colorLerpDuration = 3f;
    private bool isLerping = false;

    [SerializeField]
    private List<Renderer> lightRenderers;
    [SerializeField]
    private List<Renderer> bodyRenderers;

    [Header("Light Materials")]
    [SerializeField]
    private Material greenLightMaterial;
    [SerializeField]
    private Material orangeLightMaterial;
    [SerializeField]
    private Material redLightMaterial;

    [Header("Body Materials")]
    [SerializeField]
    private Material greenBodyMaterial;
    [SerializeField]
    private Material orangeBodyMaterial;
    [SerializeField]
    private Material redBodyMaterial;

    [Header("State Ranges")]
    [SerializeField]
    private float warnRange = 8f;
    [SerializeField]
    private float seekRange = 4f;
    [SerializeField]
    private float explodeRange = 1.5f;

    [Header("Explosion Effect")]
    [SerializeField]
    private int maxExplosionDamage = 20;
    [SerializeField]
    private FloatRange damageFalloffInterval = new FloatRange(1.5f, 5);
    [SerializeField]
    private GameObject bodyObject;
    [SerializeField]
    private GameObject healthBar;
    [SerializeField]
    private GameObject explosionParticles;

    private SeekerMineState combatState = SeekerMineState.IDLE;
    private AudioSource explosionSound;
    private NavMeshAgent navAgent;
    private IdleWander wanderBehavior;
    private PlayerCharacter player;

    protected override void Awake()
    {
        base.Awake();
        explosionSound = GetComponent<AudioSource>();
        navAgent = GetComponent<NavMeshAgent>();
        wanderBehavior = GetComponent<IdleWander>();
    }

    protected override void Start()
    {
        base.Start();
        player = PlayerCharacter.Instance;
        StartCoroutine(LerpLightsToStatePeriodic());
        //StartCoroutine(SetDestinationPeriodic());
    }

    private void Update()
    {
        float playerDistance = Vector3.Distance(transform.position,
            player.transform.position);
        if (playerDistance <= explodeRange)
        {
            Die();
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
        enabled = false;
        StopAllCoroutines();
        navAgent.ResetPath();
        EnemyDeathEvent?.Invoke(this);
        StartCoroutine(ShrinkAndExplode());
    }

    protected override void OnPlayerDeath(Character c)
    {
        base.OnPlayerDeath(c);
        combatState = SeekerMineState.IDLE;
        navAgent.ResetPath();
        enabled = false;
    }

    protected override IEnumerator SpawnSequence()
    {
        yield return new WaitForSeconds(1.25f);
        navAgent.enabled = true;
        StartCoroutine(SetDestinationPeriodic());
        yield return new WaitForSeconds(1.75f);
        wanderBehavior.enabled = true;
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
                StartCoroutine(LerpColors());
                prevState = combatState;
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator LerpColors()
    {
        isLerping = true;

        //set up materials
        Material lightLerpStartMaterial = new Material(lightRenderers[0].material);
        //Material bodyLerpStartMaterial = new Material(bodyRenderers[0].material);
        Material lightLerpEndMaterial = null;
        Material bodyLerpEndMaterial = null;
        switch (combatState)
        {
            case SeekerMineState.IDLE:
                lightLerpEndMaterial = greenLightMaterial;
                bodyLerpEndMaterial = greenBodyMaterial;
                break;
            case SeekerMineState.WARN:
                lightLerpEndMaterial = orangeLightMaterial;
                bodyLerpEndMaterial = orangeBodyMaterial;
                break;
            case SeekerMineState.SEEK:
                lightLerpEndMaterial = redLightMaterial;
                bodyLerpEndMaterial = redBodyMaterial;
                break;
        }

        //lerp materials
        float lerpStartTime = Time.time;
        for (float elapsed = 0; elapsed <= colorLerpDuration;
             elapsed = Time.time - lerpStartTime)
        {
            float lerpPercentage = elapsed / colorLerpDuration;
            lightRenderers.ForEach(rend => rend.material.Lerp(
                lightLerpStartMaterial, lightLerpEndMaterial, lerpPercentage));
            //bodyRenderers.ForEach(rend => rend.material.Lerp(
                //bodyLerpStartMaterial, bodyLerpEndMaterial, lerpPercentage));
            yield return null;
        }
        lightRenderers.ForEach(rend => rend.material = lightLerpEndMaterial);
        //bodyRenderers.ForEach(rend => rend.material = bodyLerpEndMaterial);

        isLerping = false;
    }

    //place in animation sequence class
    private IEnumerator ShrinkAndExplode()
    {
        //shrink
        float shrinkDuration = 0.15f;
        yield return LerpScaleOverDuration(new List<Transform> {
            bodyObject.transform }, 0.80f, shrinkDuration);

        //explode
        float playerDist = Vector3.Distance(transform.position, player.transform.position);
        if (playerDist < damageFalloffInterval.Min)
        {
            player.CharacterHealth.CurrentHealth -= maxExplosionDamage;
        }
        else if (playerDist < damageFalloffInterval.Max)
        {
            player.CharacterHealth.CurrentHealth -= Mathf.RoundToInt(
                Mathf.Lerp(maxExplosionDamage, 0, (playerDist - damageFalloffInterval.Min) /
                (damageFalloffInterval.Max - damageFalloffInterval.Min)));
        }
        explosionSound.Play();
        CameraShake.Instance.ShakeByDistance(10f, playerDist, 15f);
        explosionParticles.SetActive(true);
        yield return null;
        bodyObject.SetActive(false);
        healthBar.SetActive(false);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
