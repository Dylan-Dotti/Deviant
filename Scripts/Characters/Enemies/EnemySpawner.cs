using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* Sometimes called a portal
 * Abstract superclass for all EnemySpawner types
 * Spawns enemies at specified times
 * Spawning is handled by the EnemyFactory
 */
public abstract class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    protected class Spawn
    {
        public EnemyType EType => eType;

        [SerializeField]
        private EnemyType eType;

        public Spawn(EnemyType eType)
        {
            this.eType = eType;
        }
    }

    public event UnityAction EnemySpawnerDissipateEvent;

    public float TimeSinceLastSpawn { get; private set; }

    private EnemyFactory eFactory;
    private EnemySpawnerDissipate dissipateSequence;

    protected virtual void Awake()
    {
        eFactory = EnemyFactory.Instance;
        dissipateSequence = GetComponent<EnemySpawnerDissipate>();
    }

    protected virtual void Start()
    {
        StartSpawning();
    }

    private void OnEnable()
    {
        PlayerCharacter.Instance.PlayerDeathEvent += OnPlayerDeath;
    }

    private void OnDisable()
    {
        PlayerCharacter.Instance.PlayerDeathEvent -= OnPlayerDeath;
    }

    protected virtual void Update()
    {
        TimeSinceLastSpawn += Time.deltaTime;
    }

    public abstract void StartSpawning();

    public virtual void StopSpawning()
    {
        StopAllCoroutines();
    }

    public void Dissipate()
    {
        StopSpawning();
        EnemySpawnerDissipateEvent?.Invoke();
        dissipateSequence.PlayAnimation();
    }

    public void DissipateAfterSeconds(float seconds)
    {
        StartCoroutine(DissipateAfterSecondsCR(seconds));
    }

    public bool AttemptSpawnAndLaunchEnemy(EnemyType eType)
    {
        if (TimeSinceLastSpawn < 0.4f)
        {
            return false;
        }
        SpawnAndLaunchEnemy(eType);
        return true;
    }

    /* Spawn the given enemy type and launch it in a mostly random direction */
    public void SpawnAndLaunchEnemy(EnemyType eType)
    {
        Vector3 spawnDirection = GetIdealRandomLaunchDirection(20, 20);
        float spawnMagnitude = Random.Range(7f, 12f);

        Enemy spawnedEnemy = eFactory.InstantiateEnemy(eType, transform.position);
        LaunchEnemy(spawnedEnemy, spawnDirection * spawnMagnitude);
        TimeSinceLastSpawn = 0;
    }

    private void LaunchEnemy(Enemy enemy, Vector3 launchVelocity)
    {
        Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
        enemyRb.AddForce(launchVelocity, ForceMode.VelocityChange);
    }

    private void OnPlayerDeath()
    {
        StopAllCoroutines();
        DissipateAfterSeconds(Random.Range(10, 20));
    }

    /* Chooses an ideal random spawn direction from a selection 
     * based on the number of raycast collisions
     */ 
    protected Vector3 GetIdealRandomLaunchDirection(int numRandomSamples, int collisionCheckRange)
    {
        Dictionary<Ray, int> rayCollisionCounts = new Dictionary<Ray, int>();
        int playerLayerMask = ~(1 >> LayerMask.NameToLayer("PlayerBody"));

        //cast a ray in the 8 cardinal directions
        for (int i = 0; i < 8; i++)
        {
            float angleRads = 45 * i * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(angleRads), 
                0, Mathf.Sin(angleRads));
            Ray collCheckRay = new Ray(transform.position, direction);
            RaycastHit[] rayHits = Physics.RaycastAll(collCheckRay,
                collisionCheckRange, playerLayerMask);
            rayCollisionCounts.Add(collCheckRay, rayHits.Length);
            //add bias for walls
            foreach (RaycastHit rayhit in rayHits)
            {
                if (rayhit.collider.tag == Tags.WALL_TAG)
                {
                    rayCollisionCounts[collCheckRay] += 9;
                    break;
                }
            }
        }
        //cast a random ray numRandomSamples times
        for (int i = 0; i < numRandomSamples; i++)
        {
            Vector3 direction = GetRandomLaunchDirection();
            Ray collCheckRay = new Ray(transform.position, direction);
            RaycastHit[] rayHits = Physics.RaycastAll(collCheckRay, 
                collisionCheckRange, playerLayerMask);
            rayCollisionCounts.Add(collCheckRay, rayHits.Length);
            //add bias for walls
            foreach (RaycastHit rayhit in rayHits)
            {
                if (rayhit.collider.tag == Tags.WALL_TAG)
                {
                    rayCollisionCounts[collCheckRay] += 9;
                    break;
                }
            }
        }
        //select rays with lowest number of collisions
        List<KeyValuePair<Ray, int>> bestRayCounts = new List<KeyValuePair<Ray, int>>();
        foreach (KeyValuePair<Ray, int> rayCollCount in rayCollisionCounts)
        {
            if (bestRayCounts.Count == 0 || 
                rayCollCount.Value == bestRayCounts[0].Value)
            {
                bestRayCounts.Add(rayCollCount);
            }
            else if (rayCollCount.Value < bestRayCounts[0].Value)
            {
                bestRayCounts.Clear();
                bestRayCounts.Add(rayCollCount);
            }
        }
        //pick random direction from selection
        Vector3 launchDirection = bestRayCounts[Random.Range(
            0, bestRayCounts.Count)].Key.direction;
        //direction should have y of 0
        return new Vector3(launchDirection.x, 0, launchDirection.z);
    }

    protected Vector3 GetRandomLaunchDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), 0,
            Random.Range(-1f, 1f)).normalized;
    }

    // Despawn after the specified number of seconds
    private IEnumerator DissipateAfterSecondsCR(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Dissipate();
    }
}
