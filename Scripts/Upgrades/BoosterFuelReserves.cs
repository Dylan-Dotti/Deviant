using UnityEngine;

public class BoosterFuelReserves : PlayerUpgrade
{
    public override int MaxNumPurchases => 2;

    [SerializeField]
    private StatsDisplay maxChargesDisplay;

    private Boost playerBoost;

    protected override void Awake()
    {
        base.Awake();
        playerBoost = player.Controller.Boost;
    }

    private void Start()
    {
        AddStatsDisplay(maxChargesDisplay);
    }

    public override void ApplyUpgrade()
    {
        playerBoost.MaxNumCharges += 1;
        base.ApplyUpgrade();
        if (Purchasable)
        {
            Cost *= 2;
        }
    }

    public override void UpdateStatsDisplays()
    {
        maxChargesDisplay.CurrentStats.text = 
            playerBoost.MaxNumCharges.ToString();
        maxChargesDisplay.NewStats.text =
            (playerBoost.MaxNumCharges + 1).ToString();
    }
}
