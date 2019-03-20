using UnityEngine;

public class BlasterFireRate : PlayerUpgrade
{
    private float ShotsPerSecond
    {
        get => 1 / playerBlaster.FireRate;
    }
    private float ShotsPerMinute
    {
        get => 60 * ShotsPerSecond;
    }

    [SerializeField]
    private DualBlaster playerBlaster;
    [SerializeField]
    private float percentageIncreaseMultiplier = 0.1f;
    [SerializeField]
    private StatsDisplay fireRateDisplay;
    [SerializeField]
    private StatsDisplay dmgPerSecondDisplay;

    private float baseFireRate;

    protected override void Awake()
    {
        base.Awake();
        baseFireRate = playerBlaster.FireRate;
    }

    private void Start()
    {
        AddStatsDisplay(fireRateDisplay);
        AddStatsDisplay(dmgPerSecondDisplay);
    }

    public override void ApplyUpgrade()
    {
        playerBlaster.FireRate = GetNextFireRate();
        base.ApplyUpgrade();
    }

    public override void UpdateStatsDisplays()
    {
        //fire rate
        fireRateDisplay.CurrentStats.text =
            FloatRounder.Round(playerBlaster.FireRate, 3) + "s";
        fireRateDisplay.NewStats.text =
            FloatRounder.Round(GetNextFireRate(), 3) + "s";
        //DPS
        FloatRange dpsRange = GetDPS(playerBlaster.FireRate);
        dmgPerSecondDisplay.CurrentStats.text =
            FloatRounder.Round(dpsRange.Min, 1) + " - " +
            FloatRounder.Round(dpsRange.Max, 1);
        dpsRange = GetDPS(GetNextFireRate());
        dmgPerSecondDisplay.NewStats.text =
            FloatRounder.Round(dpsRange.Min, 1) + " - " +
            FloatRounder.Round(dpsRange.Max, 1);
    }

    private float GetNextFireRate()
    {
        return baseFireRate / (1 + (NumTimesPurchased + 1) *
            percentageIncreaseMultiplier);
    }

    private FloatRange GetDPS(float fireRate)
    {
        IntRange dmgRange = playerBlaster.LeftBlaster.ProjectileDmgRange;
        float minDps = dmgRange.Min / fireRate;
        float maxDps = dmgRange.Max / fireRate;
        return new FloatRange(minDps, maxDps);
    }
}
