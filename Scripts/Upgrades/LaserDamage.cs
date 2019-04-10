using UnityEngine;

public class LaserDamage : PlayerUpgrade
{
    [SerializeField]
    private PlayerDualLaser playerLaser;
    [SerializeField]
    private float percentIncreaseScalar;

    private IntRange baseLaserDamage;

    protected override void Awake()
    {
        base.Awake();
        baseLaserDamage = playerLaser.LaserDamagePerTick;
    }

    public override void ApplyUpgrade()
    {
        float scalar = 1 + percentIncreaseScalar * (NumTimesPurchased + 1);
        playerLaser.LaserDamagePerTick = new IntRange(Mathf.RoundToInt(
            baseLaserDamage.Min * scalar), Mathf.RoundToInt(
                baseLaserDamage.Max * scalar));
        base.ApplyUpgrade();
    }

    public override void UpdateStatsDisplays()
    {

    }
}
