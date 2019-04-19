using UnityEngine;

public class ScatterDamage : WeaponUpgrade
{
    [SerializeField]
    private DualScatterCannon playerScatterCannon;
    [SerializeField]
    private float percentIncreaseScalar;

    [SerializeField]
    private StatsDisplay damageStatsDisplay;
    [SerializeField]
    private StatsDisplay dpsStatsDisplay;

    private IntRange baseScatterDamage;

    protected override void Awake()
    {
        baseScatterDamage = playerScatterCannon.DamageRange;
        base.Awake();
    }

    private void Start()
    {
        AddStatsDisplay(damageStatsDisplay);
        AddStatsDisplay(dpsStatsDisplay);
    }

    public override void ApplyUpgrade()
    {
        playerScatterCannon.DamageRange = GetNextDamageRange();
        base.ApplyUpgrade();
    }

    public override void UpdateStatsDisplays()
    {
        damageStatsDisplay.CurrentStats.text = string.Format(
            "{0} - {1}", playerScatterCannon.DamageRange.Min,
            playerScatterCannon.DamageRange.Max);
        FloatRange dps = playerScatterCannon.DamagePerSecond;
        dpsStatsDisplay.CurrentStats.text = string.Format(
            "{0} - {1}", FloatRounder.Round(dps.Min, 2), 
            FloatRounder.Round(dps.Max, 2));
        IntRange nextDamageRange = GetNextDamageRange();
        damageStatsDisplay.NewStats.text = string.Format(
            "{0} - {1}", nextDamageRange.Min, nextDamageRange.Max);
        dpsStatsDisplay.NewStats.text = string.Format(
            "{0} - {1}", FloatRounder.Round(nextDamageRange.Min / 
            playerScatterCannon.FireRate, 2), FloatRounder.Round(
            nextDamageRange.Max / playerScatterCannon.FireRate, 2));
    }

    private IntRange GetNextDamageRange()
    {
        float scalar = 1 + percentIncreaseScalar * (NumTimesPurchased + 1);
        return new IntRange(Mathf.RoundToInt(baseScatterDamage.Min * scalar),
            Mathf.RoundToInt(baseScatterDamage.Max * scalar));
    }
}
