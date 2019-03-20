using System;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public enum ProjectileType { Player, RangedDrone }
    public enum DamagerType { Player, RangedDrone}

    public static ObjectPoolManager Instance { get; private set; }

    [Header("Projectile Pools")]
    [SerializeField]
    private ProjectilePool playerProjectilePool;
    [SerializeField]
    private ProjectilePool rangedDroneProjectilePool;

    [Header("Damage Number Pools")]
    [SerializeField]
    private DamageNumberPool playerDamageNumberPool;
    [SerializeField]
    private DamageNumberPool rangedDroneDamageNumberPool;

    [Header("Spare Part Pools")]
    [SerializeField]
    private SparePartPool seekerMinePartsPool;
    [SerializeField]
    private SparePartPool duplicatorPartsPool;
    [SerializeField]
    private SparePartPool vortexSpawnerPartsPool;
    [SerializeField]
    private SparePartPool rangedDronePartsPool;
    [SerializeField]
    private SparePartPool laserDronePartsPool;
    [SerializeField]
    private SparePartPool laserCubePartsPool;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public ProjectilePool GetProjectilePool(ProjectileType projectileType)
    {
        switch (projectileType)
        {
            case ProjectileType.Player:
                return playerProjectilePool;
            case ProjectileType.RangedDrone:
                return rangedDroneProjectilePool;
            default:
                return null;
        }
    }

    public DamageNumberPool GetDamageNumberPool(DamagerType damagerType)
    {
        switch (damagerType)
        {
            case DamagerType.Player:
                return playerDamageNumberPool;
            case DamagerType.RangedDrone:
                return rangedDroneDamageNumberPool;
            default:
                return null;
        }
    }

    public SparePartPool GetSparePartsPool(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.SeekerMine:
                return seekerMinePartsPool;
            case EnemyType.Duplicator:
                return duplicatorPartsPool;
            case EnemyType.VortexSpawner:
                return vortexSpawnerPartsPool;
            case EnemyType.RangedDrone:
                return rangedDronePartsPool;
            case EnemyType.LaserDrone:
                return laserDronePartsPool;
            case EnemyType.LaserCube:
                return laserCubePartsPool;
            default:
                return null;
        }
    }

    public SparePartPool GetSparePartPool()
    {
        return null;
    }
}
