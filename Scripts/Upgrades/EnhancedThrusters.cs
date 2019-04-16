using UnityEngine;

public class EnhancedThrusters : PlayerUpgrade
{
    public override int MaxNumPurchases => 5;

    [SerializeField]
    private float accelPercentageIncrease = 0.02f;
    [SerializeField]
    private float speedPercentageIncrease = 0.02f;
    [SerializeField]
    private StatsDisplay accelerationDisplay;
    [SerializeField]
    private StatsDisplay maxSpeedDisplay;

    private float baseAcceleration;
    private float baseMaxSpeed;

    protected override void Awake()
    {
        base.Awake();
        baseAcceleration = player.Controller.Acceleration;
        baseMaxSpeed = player.Controller.MaxVelocityMagnitude;
    }

    private void Start()
    {
        AddStatsDisplay(accelerationDisplay);
        AddStatsDisplay(maxSpeedDisplay);
    }

    public override void ApplyUpgrade()
    {
        player.Controller.Acceleration = GetNextAccel();
        player.Controller.MaxVelocityMagnitude = GetNextSpeed();
        base.ApplyUpgrade();
    }

    public override void UpdateStatsDisplays()
    {
        //acceleration
        accelerationDisplay.CurrentStats.text = FloatRounder.Round(
            player.Controller.Acceleration, 2).ToString();
        accelerationDisplay.NewStats.text = FloatRounder.Round(
            GetNextAccel(), 2).ToString();
        //max speed
        maxSpeedDisplay.CurrentStats.text = FloatRounder.Round(
            player.Controller.MaxVelocityMagnitude, 2).ToString();
        maxSpeedDisplay.NewStats.text = FloatRounder.Round(
            GetNextSpeed(), 2).ToString();
    }

    private float GetNextAccel()
    {
        return baseAcceleration * (1 + accelPercentageIncrease *
            (NumTimesPurchased + 1));
    }

    private float GetNextSpeed()
    {
        return baseMaxSpeed * (1 + speedPercentageIncrease *
            (NumTimesPurchased + 1));
    }
}
