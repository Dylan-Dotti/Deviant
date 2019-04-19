using UnityEngine;

public class ImprovedHeatSinks : WeaponUpgrade
{
    public override int MaxNumPurchases => 5;

    [SerializeField]
    private PlayerWeaponHeat weaponHeat;
    [SerializeField]
    private float increaseMultiplier = 0.04f;

    [SerializeField]
    private StatsDisplay heatDrainDisplay;
    [SerializeField]
    private StatsDisplay overheatDrainDisplay;

    private float baseNormalHeatDrain;
    private float baseOverheatDrain;

    protected override void Awake()
    {
        baseNormalHeatDrain = weaponHeat.HeatDrainPerSec;
        baseOverheatDrain = weaponHeat.OverheatDrainPerSec;
        base.Awake();
    }

    private void Start()
    {
        AddStatsDisplay(heatDrainDisplay);
        AddStatsDisplay(overheatDrainDisplay);
    }

    public override void ApplyUpgrade()
    {
        weaponHeat.HeatDrainPerSec += baseNormalHeatDrain * increaseMultiplier;
        weaponHeat.OverheatDrainPerSec += baseOverheatDrain * increaseMultiplier;
        base.ApplyUpgrade();
    }

    public override void UpdateStatsDisplays()
    {
        heatDrainDisplay.CurrentStats.text = weaponHeat.HeatDrainPerSec.ToString();
        heatDrainDisplay.NewStats.text = GetNewHeatDrainValue().ToString();
        overheatDrainDisplay.CurrentStats.text = weaponHeat.OverheatDrainPerSec.ToString();
        overheatDrainDisplay.NewStats.text = GetNewOverheatDrainValue().ToString();
            
    }

    private float GetNewHeatDrainValue()
    {
        return weaponHeat.HeatDrainPerSec + baseNormalHeatDrain * increaseMultiplier;
    }

    private float GetNewOverheatDrainValue()
    {
        return weaponHeat.OverheatDrainPerSec + baseOverheatDrain * increaseMultiplier;
    }
}
