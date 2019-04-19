using UnityEngine;

public class BlasterDamage : WeaponUpgrade
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

    private BlasterWeapon leftPlayerBlaster;
    private BlasterWeapon rightPlayerBlaster;
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
        dmgPerSecondDisplay.CurrentStats.text = string.Format(
            "{0} - {1}", FloatRounder.Round(currentMin / playerBlaster.FireRate, 2),
            FloatRounder.Round(currentMax / playerBlaster.FireRate, 2));
        dmgPerSecondDisplay.NewStats.text = string.Format(
            "{0} - {1}", FloatRounder.Round(nextDamage.Min / playerBlaster.FireRate, 2),
            FloatRounder.Round(nextDamage.Max / playerBlaster.FireRate, 2));
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
