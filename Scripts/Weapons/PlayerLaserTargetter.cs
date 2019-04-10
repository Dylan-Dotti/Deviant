using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserTargetter : MonoBehaviour
{
    public delegate void TargetterDelegate(Transform target);

    public event TargetterDelegate TargetAcquiredEvent;
    public event TargetterDelegate TargetLostEvent;

    public Transform TargetTransform { get; private set; }
    public bool RetargettingEnabled { get; set; } = true;

    [SerializeField]
    private float maxTargetDist = 5;
    [SerializeField]
    private float baseMoveSpeed = 1;

    private List<Transform> enemyTransforms;
    private ParticleSystem targetParticles;
    private Camera mainCamera;

    private void Awake()
    {
        targetParticles = GetComponent<ParticleSystem>();
        mainCamera = Camera.main;
        enemyTransforms = new List<Transform>();
        Enemy.EnemySpawnedEvent += OnEnemySpawn;
        Enemy.EnemyDeathEvent += OnEnemyDeath;
    }

    private void Start()
    {
        //switching weapons after wave ends?
        if (WaveGenerator.Instance != null)
        {
            WaveGenerator.Instance.WaveEndedEvent += i => enabled = false;
            WaveGenerator.Instance.WaveStartedEvent += i => enabled = true;
        }
    }

    private void OnEnable()
    {
        transform.position = GetAdjustedMousePosition();
        targetParticles.Play();
    }

    private void OnDisable()
    {
        TargetTransform = null;
        targetParticles.Stop();
    }

    private void FixedUpdate()
    {
        Vector3 mousePosition = GetAdjustedMousePosition();
        if (RetargettingEnabled)
        {
            Transform newTransform = GetClosestEnemy(mousePosition);
            if (newTransform != TargetTransform)
            {
                if (TargetTransform != null)
                {
                    TargetLostEvent?.Invoke(TargetTransform);
                }
                TargetTransform = newTransform;
                TargetAcquiredEvent?.Invoke(newTransform);
            }
        }

        if (TargetTransform == null)
        {
            float moveSpeed = Mathf.Lerp(baseMoveSpeed, baseMoveSpeed * 5,
                Vector3.Distance(transform.position, mousePosition) / maxTargetDist);
            transform.position = Vector3.MoveTowards(transform.position, 
                mousePosition, moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            float moveSpeed = Mathf.Lerp(baseMoveSpeed, baseMoveSpeed * 5,
                Vector3.Distance(transform.position, TargetTransform.position) / 10f);
            transform.position = Vector3.MoveTowards(transform.position, 
                GetAdjustedTargetPosition(TargetTransform.position), 
                moveSpeed * Time.fixedDeltaTime);
        }
    }

    private Transform GetClosestEnemy(Vector3 mousePosition)
    {
        float closestDistance = maxTargetDist;
        Transform closestEnemy = null;
        foreach (Transform eTransform in enemyTransforms)
        {
            float enemyDist = Vector3.Distance(mousePosition, eTransform.position);
            if (enemyDist < closestDistance)
            {
                closestEnemy = eTransform;
                closestDistance = enemyDist;
            }
        }
        return closestEnemy;
    }

    private Vector3 GetAdjustedTargetPosition(Vector3 targetPosition)
    {
        return new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
    }

    private Vector3 GetAdjustedMousePosition()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(mousePosition.x, transform.position.y, mousePosition.z);
    }

    private void OnEnemySpawn(Character c)
    {
        enemyTransforms.Add(c.transform);
    }

    private void OnEnemyDeath(Character c)
    {
        if (c.transform == TargetTransform)
        {
            TargetTransform = null;
            TargetLostEvent?.Invoke(c.transform);
        }
        enemyTransforms.Remove(c.transform);
    }
}
