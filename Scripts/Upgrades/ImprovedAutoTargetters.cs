using UnityEngine;

public class ImprovedAutoTargetters : WeaponUpgrade
{
    public override int MaxNumPurchases => 10;

    [SerializeField]
    private PlayerDualLaser playerLaser;
    [SerializeField]
    private float percentIncreaseScalar;

    [SerializeField]
    private StatsDisplay moveSpeedDisplay;

    private float basePercentage;

    protected override void Awake()
    {
        base.Awake();
        basePercentage = playerLaser.FiringMoveSpeedPercentage;
    }

    private void Start()
    {
        AddStatsDisplay(moveSpeedDisplay);
    }

    public override void ApplyUpgrade()
    {
        playerLaser.FiringMoveSpeedPercentage = GetNextMovePercentage();
        base.ApplyUpgrade();
    }

    public override void UpdateStatsDisplays()
    {
        moveSpeedDisplay.CurrentStats.text = GetPercentString(
            playerLaser.FiringMoveSpeedPercentage);
        moveSpeedDisplay.NewStats.text = GetPercentString(
            GetNextMovePercentage());
    }

    private float GetNextMovePercentage()
    {
        float multiplierIncrease = (1f - basePercentage) * percentIncreaseScalar;
        return playerLaser.FiringMoveSpeedPercentage + multiplierIncrease;
    }

    private string GetPercentString(float percentage)
    {
        int percentInt = Mathf.RoundToInt(percentage * 100);
        return percentInt + "%";
    }
}
