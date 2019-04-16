using UnityEngine;

public class ImprovedHeatSinks : PlayerUpgrade
{
    public override int MaxNumPurchases => 5;

    [SerializeField]
    private PlayerWeaponHeat weaponHeat;
    [SerializeField]
    private float increaseMultiplier = 0.04f;

    private float baseNormalHeatDrain;
    private float baseOverheatDrain;

    protected override void Awake()
    {
        baseNormalHeatDrain = weaponHeat.HeatDrainPerSec;
        baseOverheatDrain = weaponHeat.OverheatDrainPerSec;
        base.Awake();
    }

    public override void ApplyUpgrade()
    {
        weaponHeat.HeatDrainPerSec += baseNormalHeatDrain * increaseMultiplier;
        weaponHeat.OverheatDrainPerSec += baseOverheatDrain * increaseMultiplier;
        base.ApplyUpgrade();
    }

    public override void UpdateStatsDisplays()
    {

    }
}
