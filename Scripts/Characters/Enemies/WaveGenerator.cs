using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour
{
    private class PortalSpawnLocation
    {
        public readonly Transform transform;
        public bool isOccupied = false;

        public PortalSpawnLocation(Transform transform)
        {
            this.transform = transform;
        }
    }

    [SerializeField]
    private List<EnemySpawner> spawnerPrefabs;

    private List<PortalSpawnLocation> spawnLocations;

    private void Awake()
    {
        spawnLocations = new List<PortalSpawnLocation>();
        Transform portalSpawnPoints = transform.Find("Portal Spawn Points");
        for (int i = 0; i < portalSpawnPoints.childCount; i++)
        {
            spawnLocations.Add(new PortalSpawnLocation(
                portalSpawnPoints.GetChild(i)));
        }
    }

    private void OnEnable()
    {
        StartCoroutine(SpawnPortalsPeriodic());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void SpawnPortalPresetLocation(EnemySpawner portalPrefab)
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
    }

    private void SpawnPortalRandomLocation(EnemySpawner portalPrefab)
    {

    }

    private IEnumerator SpawnPortalsPeriodic()
    {
        VariableFrontQueue<EnemySpawner> spawnerQueue =
            new VariableFrontQueue<EnemySpawner>(spawnerPrefabs, true);
        while (true)
        {
            SpawnPortalPresetLocation(spawnerQueue.PopAndCycle(2));
            yield return new WaitForSeconds(20f);
        }
    }
}
