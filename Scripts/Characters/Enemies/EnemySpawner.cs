using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Spawn
    {
        public GameObject ObjectToSpawn { get { return objectToSpawn; } }
        public float SpawnTime { get { return spawnTime; } }
        public float Cooldown { get { return cooldown; } }

        [SerializeField]
        private GameObject objectToSpawn;
        [SerializeField]
        private float spawnTime;
        [SerializeField]
        private float cooldown;

        public Spawn(GameObject objToSpawn, float spawnTime, float cooldown)
        {
            objectToSpawn = objToSpawn;
            this.spawnTime = spawnTime;
            this.cooldown = cooldown;
        }

        public void UpdateSpawnTime()
        {
            spawnTime += cooldown;
        }
    }

    [SerializeField]
    private List<Spawn> spawnSequence;

    private float spawnStartTime;
    private float timeSinceLastSpawn;


    private void Start()
    {
        //StartCoroutine(SpawnPeriodically());
        StartCoroutine(DemoSpawnSequence());
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        /*List<Spawn> spawnsToRemove = new List<Spawn>();
        float currentTime = Time.time - spawnStartTime;
        foreach (Spawn spawn in spawnSequence)
        {
            if (currentTime > spawn.SpawnTime)
            {
                Vector3 spawnDirection = new Vector3(Random.Range(-1f, 1f),
                    0, Random.Range(-1, 1)).normalized;
                float spawnMagnitude = Random.Range(4f, 4f);

                GameObject spawnCharacter = Instantiate(spawn.ObjectToSpawn);
                Rigidbody spawnCharacterRbody = spawnCharacter
                    .GetComponentInChildren<Rigidbody>();
                spawnCharacterRbody.AddForce(spawnDirection * 
                    spawnMagnitude, ForceMode.VelocityChange);
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
        }*/
    }

    private void SpawnEnemy(Spawn spawn, bool updateTime)
    {
        Vector3 spawnDirection = new Vector3(Random.Range(-1f, 1f),
            0, Random.Range(-1f, 1f)).normalized;
        float spawnMagnitude = Random.Range(4f, 10f);

        GameObject spawnObject = Instantiate(spawn.ObjectToSpawn);
        Rigidbody spawnCharacterRbody = spawnObject
            .GetComponentInChildren<Rigidbody>();
        spawnCharacterRbody.AddForce(spawnDirection *
            spawnMagnitude, ForceMode.VelocityChange);
        if (updateTime)
        {
            spawn.UpdateSpawnTime();
        }
        timeSinceLastSpawn = 0;
    }

    private void Dissipate()
    {
        Destroy(gameObject);
    }

    private Vector3 GenerateIdealSpawnDirection(int numSamples)
    {
        return Vector3.zero;
    }

    private IEnumerator ExecuteSpawnSequence()
    {
        while (true)
        {
            yield return new WaitForSeconds(4);
            float spawnerStartTime = Time.time;
            Vector3 spawnDirection = new Vector3(Random.Range(-1f, 1f),
                0, Random.Range(-1f, 1f)).normalized;
            float spawnMagnitude = Random.Range(4f, 6f);
            GameObject spawnCharacter = Instantiate(spawnSequence[0].ObjectToSpawn);
            Rigidbody spawnCharacterRbody = spawnCharacter
                .GetComponentInChildren<Rigidbody>();
            yield return null;
            spawnCharacterRbody.AddForce(spawnDirection *
                spawnMagnitude, ForceMode.VelocityChange);
        }
    }

    private IEnumerator SpawnPeriodically()
    {
        float startTime = Time.time;
        while (true)
        {
            foreach (Spawn spawn in spawnSequence)
            {
                if (Time.time - startTime >= spawn.SpawnTime && timeSinceLastSpawn >= 0.5f)
                {
                    Debug.Log(spawn.SpawnTime);
                    SpawnEnemy(spawn, true);
                    break;
                }
            }
            yield return null;
        }
    }

    private IEnumerator DemoSpawnSequence()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        for (int i = 0; i < 3; i++)
        {
            SpawnEnemy(spawnSequence[0], false);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(3);
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        for (int i = 0; i < 2; i++)
        {
            SpawnEnemy(spawnSequence[1], false);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(3);
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        for (int i = 0; i < 2; i++)
        {
            SpawnEnemy(spawnSequence[2], false);
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(3);
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        SpawnEnemy(spawnSequence[3], false);
        yield return new WaitForSeconds(3);
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        GameObject.Find("Background Music")
            .GetComponent<BackgroundMusic>().enabled = true;
        StartCoroutine(SpawnPeriodically());
    }
}
