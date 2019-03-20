using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerPeriodic : EnemySpawner
{
    [System.Serializable]
    public class PeriodicSpawn
    {
        public EnemyType EType => eType;
        public FloatRange InitialSpawnTime => initialSpawnTime;
        public FloatRange Cooldown => cooldown;
        public int NumSpawns => numSpawns;

        [SerializeField]
        private EnemyType eType;
        [SerializeField]
        private FloatRange initialSpawnTime = new FloatRange(1, 1);
        [SerializeField]
        private FloatRange cooldown = new FloatRange(1, 1);
        [SerializeField]
        private int numSpawns = int.MaxValue;

        public PeriodicSpawn(EnemyType eType, FloatRange initialSpawnTime,
            FloatRange cooldown, int numSpawns = int.MaxValue)
        {
            this.eType = eType;
            this.initialSpawnTime = initialSpawnTime;
            this.cooldown = cooldown;
            this.numSpawns = numSpawns;
        }
    }

    [SerializeField]
    protected List<PeriodicSpawn> spawns;

    protected virtual void Start()
    {
        SpawnPeriodically();
    }

    protected override void Update()
    {
        base.Update();
        if (spawns.Count == 0)
        {
            enabled = false;
            DissipateAfterSeconds(3);
            Debug.Log(name + " dispersing in 3 seconds");
        }
    }

    public void SpawnPeriodically()
    {
        StopAllCoroutines();
        foreach (PeriodicSpawn pSpawn in spawns)
        {
            StartCoroutine(SpawnPeriodically(pSpawn));
        }
    }

    private IEnumerator SpawnPeriodically(PeriodicSpawn spawn)
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
