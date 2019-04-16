using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Spawns random enemies periodically
 * The number of enemies spawned each time is also probabilistic
 */
public class EnemySpawnerRandom : EnemySpawner
{
    [System.Serializable]
    protected class ProbabilisticSpawn : Spawn
    {
        [System.Serializable]
        private struct SpawnNumberProbability
        {
            public int numToSpawn;
            [Range(0, 1)] public float probability;
        }

        [SerializeField]
        private List<SpawnNumberProbability> spawnNumProbabilities;

        public ProbabilisticSpawn(EnemyType eType)
            : base(eType)
        {

        }

        public int GetNumToSpawn()
        {
            float randomVal = Random.value;
            float totalChecked = 0;
            foreach (SpawnNumberProbability spawnProb in spawnNumProbabilities)
            {
                if (randomVal <= spawnProb.probability + totalChecked)
                {
                    return spawnProb.numToSpawn;
                }
                totalChecked += spawnProb.probability;
            }
            return 0;
        }
    }

    [SerializeField]
    private List<ProbabilisticSpawn> spawns;

    public override void StartSpawning()
    {
        DissipateAfterSeconds(35);
        StartCoroutine(SpawnSequenceCR());
    }

    private IEnumerator SpawnSequenceCR()
    {
        yield return new WaitForSeconds(Random.Range(2, 4));
        while (true)
        {
            int randIndex = Random.Range(0, spawns.Count);
            ProbabilisticSpawn spawn = spawns[randIndex];
            int numSpawns = spawn.GetNumToSpawn();
            for (int i = 0; i < numSpawns; i++)
            {
                while (!AttemptSpawnAndLaunchEnemy(spawn.EType))
                {
                    yield return null;
                }
            }
            yield return new WaitForSeconds(Random.Range(8.5f, 10f));
        }
    }
}
