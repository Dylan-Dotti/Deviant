using System;
using UnityEngine;

// issues with assigning global object pools to intended users on spawn;
// here's the solution
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
}
