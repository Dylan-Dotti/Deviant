using UnityEngine;

public class BlasterDamage : PlayerUpgrade
{
    public int MinDamage { get => leftPlayerBlaster.ProjectileDmgRange.Min; }
    public int MaxDamage { get => leftPlayerBlaster.ProjectileDmgRange.Max; }

    [SerializeField]
    private DualBlaster playerBlaster;
    [SerializeField]
    private float percentageIncreaseMultiplier = 0.1f;
    [SerializeField]
    private StatsDisplay blasterDamageDisplay;
    [SerializeField]
    private StatsDisplay dmgPerSecondDisplay;

    private SingleBlaster leftPlayerBlaster;
    private SingleBlaster rightPlayerBlaster;
    private IntRange baseDamage;

    protected override void Awake()
    {
        base.Awake();
        leftPlayerBlaster = playerBlaster.LeftBlaster;
        rightPlayerBlaster = playerBlaster.RightBlaster;
        baseDamage = leftPlayerBlaster.ProjectileDmgRange;
    }

    private void Start()
    {
        AddStatsDisplay(blasterDamageDisplay);
        AddStatsDisplay(dmgPerSecondDisplay);
    }

    public override void ApplyUpgrade()
    {
        leftPlayerBlaster.ProjectileDmgRange = getNextDamage();
        rightPlayerBlaster.ProjectileDmgRange = getNextDamage();
        base.ApplyUpgrade();
    }

    public override void UpdateStatsDisplays()
    {
        int currentMin = MinDamage;
        int currentMax = MaxDamage;
        IntRange nextDamage = getNextDamage();
        //blaster damage
        blasterDamageDisplay.CurrentStats.text = currentMin + " - " + currentMax;
        blasterDamageDisplay.NewStats.text = nextDamage.Min + " - " + nextDamage.Max;
        //DPS
        dmgPerSecondDisplay.CurrentStats.text =
            (currentMin / playerBlaster.FireRate) + " - " +
            (currentMax / playerBlaster.FireRate);
        dmgPerSecondDisplay.NewStats.text =
            (nextDamage.Min / playerBlaster.FireRate) + " - " +
            (nextDamage.Max / playerBlaster.FireRate);
    }

    private IntRange getNextDamage()
    {
        int nextMin = Mathf.RoundToInt(baseDamage.Min * 
            (1 + (NumTimesPurchased + 1) * percentageIncreaseMultiplier));
        int nextMax = Mathf.RoundToInt(baseDamage.Max *
            (1 + (NumTimesPurchased + 1) * percentageIncreaseMultiplier));
        return new IntRange(nextMin, nextMax);
    }
}
