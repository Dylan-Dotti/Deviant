using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Spawn
    {
        public Rigidbody CharacterToSpawn { get { return characterToSpawn; } }
        public float SpawnTime { get { return spawnTime; } }

        [SerializeField]
        private Rigidbody characterToSpawn;
        [SerializeField]
        private float spawnTime;

        public Spawn(Rigidbody chToSpawn, float spawnTime)
        {
            characterToSpawn = chToSpawn;
            this.spawnTime = spawnTime;
        }
    }

    [SerializeField]
    private List<Spawn> spawnSequence;

    private float spawnStartTime;

    private void Start()
    {
        StartCoroutine(ExecuteSpawnSequence());
        spawnStartTime = 1000000;
    }

    private void FixedUpdate()
    {
        List<Spawn> spawnsToRemove = new List<Spawn>();
        float currentTime = Time.time - spawnStartTime;
        foreach (Spawn spawn in spawnSequence)
        {
            if (currentTime > spawn.SpawnTime)
            {
                Vector3 spawnDirection = new Vector3(Random.Range(-1f, 1f),
                    0, Random.Range(-1, 1)).normalized;
                float spawnMagnitude = Random.Range(4f, 4f);

                Rigidbody spawnCharacter = Instantiate(spawn.CharacterToSpawn);
                spawnCharacter.AddForce(spawnDirection * spawnMagnitude, ForceMode.VelocityChange);
                spawnsToRemove.Add(spawn);
            }
        }
        foreach (Spawn spawn in spawnsToRemove)
        {
            spawnSequence.Remove(spawn);
        }
        if (spawnSequence.Count == 0)
        {
            Dissipate();
        }
    }

    private void Dissipate()
    {
        Destroy(gameObject);
    }

    private IEnumerator ExecuteSpawnSequence()
    {
        while (true)
        {
            yield return new WaitForSeconds(4);
            float spawnerStartTime = Time.time;
            Vector3 spawnDirection = new Vector3(Random.Range(-1f, 1f),
                0, Random.Range(-1f, 1f)).normalized;
            float spawnMagnitude = Random.Range(5f, 9f);
            Rigidbody spawnCharacter = Instantiate(spawnSequence[0].CharacterToSpawn);
            yield return null;
            spawnCharacter.AddForce(spawnDirection * spawnMagnitude, ForceMode.VelocityChange);
        }
    }
}
