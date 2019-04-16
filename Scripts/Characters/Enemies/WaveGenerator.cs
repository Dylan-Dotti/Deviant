using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* Generates enemy waves by sampling from a preset selection 
 * of EnemySpawner. Spawns EnemySpawner objects (sometimes 
 * called portals) at one of 7 preset locations on the map 
 * (though I'd like to add more variation). 
 * Fires events on wave start and end.
 */ 
public class WaveGenerator : MonoBehaviour
{
    // class representing an enemy wave composed of EnemySpawners
    private class EnemyWave
    {
        public readonly Queue<EnemySpawner> spawnerQueue;

        public EnemyWave(ICollection<EnemySpawner> spawners)
        {
            spawnerQueue = new Queue<EnemySpawner>();
            foreach (EnemySpawner spawner in spawners)
            {
                spawnerQueue.Enqueue(spawner);
            }
        }
    }

    private class PortalSpawnLocation
    {
        public readonly Transform transform;
        public bool isOccupied = false;

        public PortalSpawnLocation(Transform transform)
        {
            this.transform = transform;
        }
    }

    public static WaveGenerator Instance { get; private set; }

    public event UnityAction<int> WaveStartedEvent;
    public event UnityAction<int> WaveEndedEvent;
    public event UnityAction AllSpawnsCompletedEvent;

    [SerializeField]
    private List<EnemySpawner> spawnerPrefabs;

    private List<PortalSpawnLocation> spawnLocations;
    private EnemyWave currentWave;
    private int currentWaveCount = 0;
    private int numActiveEnemies;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Enemy.EnemySpawnedEvent += c => numActiveEnemies++;
            Enemy.EnemyDeathEvent += c => numActiveEnemies--;
            spawnLocations = new List<PortalSpawnLocation>();
            Transform portalSpawnPoints = transform.Find("Portal Spawn Points");
            for (int i = 0; i < portalSpawnPoints.childCount; i++)
            {
                spawnLocations.Add(new PortalSpawnLocation(
                    portalSpawnPoints.GetChild(i)));
            }
        }
    }

    private void OnEnable()
    {
        PlayerCharacter.Instance.PlayerDeathEvent += OnPlayerDeath;
    }

    private void OnDisable()
    {
        PlayerCharacter.Instance.PlayerDeathEvent -= OnPlayerDeath;
        StopAllCoroutines();
    }

    public void StartNextWave()
    {
        StopAllCoroutines();
        currentWaveCount++;
        currentWave = GenerateRandomWave(5, 2);
        StartCoroutine(SpawnWaveCR(currentWave, new FloatRange(17.5f, 20f)));
        WaveStartedEvent?.Invoke(currentWaveCount);
    }

    private void EndCurrentWave()
    {
        StopAllCoroutines();
        currentWave = null;
        WaveEndedEvent?.Invoke(currentWaveCount);
    }

    // Generates a mostly random wave from the list of EnemySpawner prefabs
    private EnemyWave GenerateRandomWave(int numPortals, int queueFrontSize)
    {
        VariableFrontQueue<EnemySpawner> spawnerQueue =
            new VariableFrontQueue<EnemySpawner>(spawnerPrefabs, true);
        List<EnemySpawner> waveSpawners = new List<EnemySpawner>();
        for (int i = 0; i < numPortals; i++)
        {
            waveSpawners.Add(spawnerQueue.DequeueAndCycle(queueFrontSize));
        }
        return new EnemyWave(waveSpawners);
    }

    /* Spawns a portal at one of the available preset locations (or freezes 
     * the game if they're all occupied. Not ideal)
     */
    private EnemySpawner SpawnPortalPresetLocation(EnemySpawner portalPrefab)
    {
        PortalSpawnLocation spawnLocation;
        while (true)
        {
            int randIndex = Random.Range(0, spawnLocations.Count);
            spawnLocation = spawnLocations[randIndex];
            if (!spawnLocation.isOccupied)
            {
                spawnLocation.isOccupied = true;
                break;
            }
        }
        EnemySpawner portal = Instantiate(portalPrefab, spawnLocation.
            transform.position, Quaternion.identity);
        portal.EnemySpawnerDissipateEvent += () => spawnLocation.isOccupied = false;
        return portal;
    }

    // NYI
    private void SpawnPortalRandomLocation(EnemySpawner portalPrefab)
    {

    }

    private void OnPlayerDeath()
    {
        enabled = false;
    }


    /* Spawns the portals of a given wave, 
     * with a random intervalRange between spawn times
     */
    private IEnumerator SpawnWaveCR(EnemyWave wave, FloatRange intervalRange)
    {
        Queue<EnemySpawner> spawnerQueue = wave.spawnerQueue;
        int numSpawners = spawnerQueue.Count;
        EnemySpawner nextSpawner = SpawnPortalPresetLocation(spawnerQueue.Dequeue());
        nextSpawner.EnemySpawnerDissipateEvent += () => numSpawners--;
        yield return new WaitForSeconds(5);
        while (spawnerQueue.Count > 0)
        {
            nextSpawner = SpawnPortalPresetLocation(spawnerQueue.Dequeue());
            nextSpawner.EnemySpawnerDissipateEvent += () => numSpawners--;
            yield return new WaitForSeconds(intervalRange.RandomRangeValue);
        }
        while (numSpawners > 0 || numActiveEnemies > 0)
        {
            if (numSpawners == 0)
            {
                AllSpawnsCompletedEvent?.Invoke();
            }
            yield return null;
        }
        EndCurrentWave();
    }

    // Spawns random portals. used for testing.
    private IEnumerator SpawnRandomPortalsPeriodic()
    {
        VariableFrontQueue<EnemySpawner> spawnerQueue =
            new VariableFrontQueue<EnemySpawner>(spawnerPrefabs, true);
        SpawnPortalPresetLocation(spawnerQueue.DequeueAndCycle(2));
        yield return new WaitForSeconds(5);
        while (true)
        {
            SpawnPortalPresetLocation(spawnerQueue.DequeueAndCycle(2));
            yield return new WaitForSeconds(20f);
        }
    }
}
