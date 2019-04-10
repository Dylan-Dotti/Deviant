using System.Collections.Generic;
using UnityEngine;

public static class EnemyStrengthScalars
{
    private static Dictionary<EnemyType, float> healthScalars;
    private static Dictionary<EnemyType, float> damageScalars;

    static EnemyStrengthScalars()
    {
        InitScalars();
        if (WaveGenerator.Instance != null)
        {
            WaveGenerator.Instance.WaveEndedEvent += OnWaveEnded;
        }
        PlayerCharacter.Instance.PlayerDeathEvent += OnPlayerDeath;
    }

    public static float GetHealthScalar(EnemyType eType)
    {
        return healthScalars[eType];
    }

    public static float GetDamageScalar(EnemyType eType)
    {
        return damageScalars[eType];
    }

    private static void InitScalars()
    {
        healthScalars = new Dictionary<EnemyType, float>();
        damageScalars = new Dictionary<EnemyType, float>();
        foreach (EnemyType eType in GetEnemyTypes())
        {
            healthScalars.Add(eType, 1);
            damageScalars.Add(eType, 1);
        }
    }

    private static void OnWaveEnded(int waveNum)
    {
        foreach (EnemyType eType in GetEnemyTypes())
        {
            healthScalars[eType] = 1 + (waveNum * 0.25f);
            damageScalars[eType] = 1 + (waveNum * 0.25f);
        }

    }

    private static void OnPlayerDeath(Character c)
    {
        foreach (EnemyType eType in GetEnemyTypes())
        {
            healthScalars[eType] = 1;
            damageScalars[eType] = 1;
        }
    }

    private static List<EnemyType> GetEnemyTypes()
    {
        return new List<EnemyType> { EnemyType.SeekerMine,
            EnemyType.Duplicator, EnemyType.VortexSpawner, EnemyType.Vortex,
            EnemyType.RangedDrone, EnemyType.LaserDrone, EnemyType.LaserCube };
    }
}
