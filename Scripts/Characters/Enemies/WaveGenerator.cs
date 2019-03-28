using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour
{
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

    public delegate void WaveGeneratorWaveDelegate(int waveNum);

    public event WaveGeneratorWaveDelegate WaveStartedEvent;
    public event WaveGeneratorWaveDelegate WaveEndedEvent;

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

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void StartNextWave()
    {
        StopAllCoroutines();
        currentWaveCount++;
        Debug.Log("Starting wave " + currentWaveCount);
        currentWave = GenerateRandomWave(6, 2);
        StartCoroutine(SpawnWaveCR(currentWave, new FloatRange(15f, 20)));
        WaveStartedEvent?.Invoke(currentWaveCount);
    }

    private void EndCurrentWave()
    {
        StopAllCoroutines();
        currentWave = null;
        WaveEndedEvent?.Invoke(currentWaveCount);
        Debug.Log("Wave " + currentWaveCount + " complete");
    }

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

    private void SpawnPortalRandomLocation(EnemySpawner portalPrefab)
    {

    }

    private IEnumerator SpawnWaveCR(EnemyWave wave, FloatRange intervalRange)
    {
        int playerStartSpareParts = PlayerCharacter.Instance.NumSpareParts;
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
            yield return null;
        }
        EndCurrentWave();
    }

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
