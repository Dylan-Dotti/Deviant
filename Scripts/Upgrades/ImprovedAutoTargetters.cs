using UnityEngine;

public class ImprovedAutoTargetters : PlayerUpgrade
{
    public override int MaxNumPurchases => 5;

    [SerializeField]
    private PlayerDualLaser playerLaser;
    [SerializeField]
    private float percentIncreaseScalar;

    private float basePercentage;

    protected override void Awake()
    {
        base.Awake();
        basePercentage = playerLaser.FiringMoveSpeedPercentage;
    }

    public override void ApplyUpgrade()
    {
        float multiplierIncrease = (1f - basePercentage) * percentIncreaseScalar;
        playerLaser.FiringMoveSpeedPercentage += multiplierIncrease;
        base.ApplyUpgrade();
    }

    public override void UpdateStatsDisplays()
    {

    }
}
