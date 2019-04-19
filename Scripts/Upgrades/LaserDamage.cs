using UnityEngine;

public class LaserDamage : WeaponUpgrade
{
    [SerializeField]
    private PlayerDualLaser playerLaser;
    [SerializeField]
    private float percentIncreaseScalar;

    [SerializeField]
    private StatsDisplay tickDamageDisplay;
    [SerializeField]
    private StatsDisplay damagePerSecondDisplay;

    private IntRange baseLaserDamage;

    protected override void Awake()
    {
        base.Awake();
        baseLaserDamage = playerLaser.LaserDamagePerTick;
    }

    private void Start()
    {
        AddStatsDisplay(tickDamageDisplay);
        AddStatsDisplay(damagePerSecondDisplay);
    }

    public override void ApplyUpgrade()
    {
        playerLaser.LaserDamagePerTick = GetNextDamageRange();
        base.ApplyUpgrade();
    }

    public override void UpdateStatsDisplays()
    {
        tickDamageDisplay.CurrentStats.text = string.Format(
            "{0} - {1}", playerLaser.LaserDamagePerTick.Min, 
            playerLaser.LaserDamagePerTick.Max);
        FloatRange dps = playerLaser.DamagePerSecond;
        damagePerSecondDisplay.CurrentStats.text = string.Format(
            "{0} - {1}", Mathf.RoundToInt(dps.Min), Mathf.RoundToInt(dps.Max));
        IntRange nextDamageRange = GetNextDamageRange();
        tickDamageDisplay.NewStats.text = string.Format(
            "{0} - {1}", nextDamageRange.Min, nextDamageRange.Max);
        damagePerSecondDisplay.NewStats.text = string.Format(
            "{0} - {1}", nextDamageRange.Min * playerLaser.TicksPerSecond,
            nextDamageRange.Max * playerLaser.TicksPerSecond);
    }

    private IntRange GetNextDamageRange()
    {
        int currentMinDamage = playerLaser.LaserDamagePerTick.Min;
        int currentMaxDamage = playerLaser.LaserDamagePerTick.Max;
        int newMinDamage = Mathf.RoundToInt(currentMinDamage +
            currentMinDamage * percentIncreaseScalar);
        int newMaxDamage = Mathf.RoundToInt(currentMaxDamage +
            currentMaxDamage * percentIncreaseScalar);
        return new IntRange(newMinDamage, newMaxDamage);
    }
}
