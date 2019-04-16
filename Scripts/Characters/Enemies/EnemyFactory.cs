using UnityEngine;

/* Spawns enemies for given EnemyTypes
 */
public sealed class EnemyFactory : MonoBehaviour
{
    public static EnemyFactory Instance { get; private set; }

    [Header("Enemy Prefabs")]
    [SerializeField]
    private SeekerMine seekerMinePrefab;
    [SerializeField]
    private DuplicatorGroup duplicatorGroupPrefab;
    [SerializeField]
    private VortexSpawner vortexSpawnerPrefab;
    [SerializeField]
    private RangedDrone rangedDronePrefab;
    [SerializeField]
    private LaserDrone laserDronePrefab;
    [SerializeField]
    private LaserCube laserCubePrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public Enemy InstantiateEnemy(EnemyType eType)
    {
        switch (eType)
        {
            case EnemyType.SeekerMine:
                return Instantiate(seekerMinePrefab);
            case EnemyType.Duplicator:
                DuplicatorGroup group = Instantiate(duplicatorGroupPrefab);
                return group.GetComponentInChildren<Duplicator>();
            case EnemyType.VortexSpawner:
                return Instantiate(vortexSpawnerPrefab);
            case EnemyType.RangedDrone:
                return Instantiate(rangedDronePrefab);
            case EnemyType.LaserDrone:
                return Instantiate(laserDronePrefab);
            case EnemyType.LaserCube:
                return Instantiate(laserCubePrefab);
            default:
                return null;
        }
    }

    public Enemy InstantiateEnemy(EnemyType eType, Vector3 spawnPos)
    {
        Enemy spawnedEnemy = InstantiateEnemy(eType);
        spawnedEnemy.transform.position = spawnPos;
        return spawnedEnemy;
    }

    public Enemy InstantiateEnemy(EnemyType eType, Vector3 spawnPos, Quaternion spawnRot)
    {
        Enemy spawnedEnemy = InstantiateEnemy(eType);
        spawnedEnemy.transform.position = spawnPos;
        spawnedEnemy.transform.rotation = spawnRot;
        return spawnedEnemy;
    }
}
