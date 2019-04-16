using UnityEngine;

public class EnhancedBoostDrive : PlayerUpgrade
{
    public override int MaxNumPurchases => 5;

    [SerializeField]
    private float durationPercentageIncrease;
    [SerializeField]
    private float cooldownPercentageDecrease;
    [SerializeField]
    private StatsDisplay durationDisplay;
    [SerializeField]
    private StatsDisplay cooldownDisplay;

    private Boost playerBoost;
    private float baseDuration;
    private float baseCooldown;

    protected override void Awake()
    {
        base.Awake();
        playerBoost = player.Controller.Boost;
        baseDuration = playerBoost.Duration;
        baseCooldown = playerBoost.ChargeCooldown;
    }

    private void Start()
    {
        AddStatsDisplay(durationDisplay);
        AddStatsDisplay(cooldownDisplay);
    }

    public override void ApplyUpgrade()
    {
        playerBoost.Duration = GetNextDuration();
        playerBoost.ChargeCooldown = GetNextCooldown();
        base.ApplyUpgrade();
    }

    public override void UpdateStatsDisplays()
    {
        //duration
        durationDisplay.CurrentStats.text = FloatRounder.Round(
            playerBoost.Duration, 2).ToString();
        durationDisplay.NewStats.text = FloatRounder.Round(
            GetNextDuration(), 2).ToString();
        //cooldown
        cooldownDisplay.CurrentStats.text = FloatRounder.Round(
            playerBoost.ChargeCooldown, 2).ToString();
        cooldownDisplay.NewStats.text = FloatRounder.Round(
            GetNextCooldown(), 2).ToString();
    }

    private float GetNextDuration()
    {
        return baseDuration * (1 + (NumTimesPurchased + 1) *
            durationPercentageIncrease);
    }

    private float GetNextCooldown()
    {
        return baseCooldown * (1 - (NumTimesPurchased + 1) *
            cooldownPercentageDecrease);
    }
}
