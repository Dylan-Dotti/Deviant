using UnityEngine;

public class AugmentedScatterBarrels : WeaponUpgrade
{
    public override int MaxNumPurchases => 10;

    [SerializeField]
    private DualScatterCannon playerScatterCannon;
    [SerializeField]
    private float percentageIncreaseMultiplier = 0.05f;

    [SerializeField]
    private StatsDisplay rangeStatsDisplay;
    [SerializeField]
    private StatsDisplay spreadStatsDisplay;

    private float baseRange;
    private float baseSpread;

    protected override void Awake()
    {
        baseRange = playerScatterCannon.Range;
        baseSpread = playerScatterCannon.Spread;
        base.Awake();
    }

    private void Start()
    {
        AddStatsDisplay(rangeStatsDisplay);
        AddStatsDisplay(spreadStatsDisplay);
    }

    public override void ApplyUpgrade()
    {
        playerScatterCannon.Range = GetNextRange();
        playerScatterCannon.Spread = GetNextSpread();
        base.ApplyUpgrade();
    }

    public override void UpdateStatsDisplays()
    {
        rangeStatsDisplay.CurrentStats.text = FloatRounder.Round(
            playerScatterCannon.Range, 2).ToString();
        spreadStatsDisplay.CurrentStats.text = FloatRounder.Round(
            playerScatterCannon.Spread, 2).ToString();
        rangeStatsDisplay.NewStats.text = GetNextRange().ToString();
        spreadStatsDisplay.NewStats.text = GetNextSpread().ToString();
    }

    public float GetNextRange()
    {
        return FloatRounder.Round(playerScatterCannon.Range + 
            baseRange * percentageIncreaseMultiplier, 2);
    }

    public float GetNextSpread()
    {
        return FloatRounder.Round(playerScatterCannon.Spread + 
            baseSpread * percentageIncreaseMultiplier, 2);
    }
}
