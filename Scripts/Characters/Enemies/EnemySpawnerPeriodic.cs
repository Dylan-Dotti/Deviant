using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerPeriodic : EnemySpawner
{
    [System.Serializable]
    protected class PeriodicSpawn : Spawn
    {
        public FloatRange InitialSpawnTime => initialSpawnTime;
        public FloatRange Cooldown => cooldown;
        public int NumSpawns => numSpawns;

        [SerializeField]
        private FloatRange initialSpawnTime = new FloatRange(1, 1);
        [SerializeField]
        private FloatRange cooldown = new FloatRange(1, 1);
        [SerializeField]
        private int numSpawns = int.MaxValue;

        public PeriodicSpawn(EnemyType eType, FloatRange initialSpawnTime,
            FloatRange cooldown, int numSpawns = int.MaxValue) :
            base(eType)
        {
            this.initialSpawnTime = initialSpawnTime;
            this.cooldown = cooldown;
            this.numSpawns = numSpawns;
        }
    }

    [SerializeField]
    protected List<PeriodicSpawn> spawns;

    protected override void Update()
    {
        base.Update();
        if (spawns.Count == 0)
        {
            enabled = false;
            DissipateAfterSeconds(3);
        }
    }

    public override void StartSpawning()
    {
        SpawnPeriodically();
    }

    public void SpawnPeriodically()
    {
        StopAllCoroutines();
        foreach (PeriodicSpawn pSpawn in spawns)
        {
            StartCoroutine(SpawnPeriodicallyCR(pSpawn));
        }
    }

    private IEnumerator SpawnPeriodicallyCR(PeriodicSpawn spawn)
    {
        if (spawn.NumSpawns != 0)
        {
            yield return new WaitForSeconds(
                spawn.InitialSpawnTime.RandomRangeValue);
            int numTimesSpawned = 0;
            while (true)
            {
                while (!AttemptSpawnAndLaunchEnemy(spawn.EType))
                {
                    yield return null;
                }
                numTimesSpawned++;
                if (numTimesSpawned >= spawn.NumSpawns)
                {
                    spawns.Remove(spawn);
                    break;
                }
                yield return new WaitForSeconds(
                    spawn.Cooldown.RandomRangeValue);
            }
        }
    }
}
