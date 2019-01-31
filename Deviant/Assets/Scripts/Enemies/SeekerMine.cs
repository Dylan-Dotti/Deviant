using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeekerMine : Character
{
    public enum SeekerMineState { IDLE, WARN, SEEK }

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
    private float warnRange = 3.5f;
    [SerializeField]
    private float seekRange = 1.5f;
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
        if (playerDistance <= seekRange)
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
        Destroy(gameObject);
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
        Dictionary<Renderer, Material> lerpStartMaterials = new Dictionary<Renderer, Material>();
        foreach (Renderer lightRend in lightRenderers)
        {
            lerpStartMaterials.Add(lightRend, new Material(lightRend.material));
        }
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
        //float lerpDuration = 2.0f;
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
}
