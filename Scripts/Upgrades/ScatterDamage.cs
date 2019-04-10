using UnityEngine;

public class ScatterDamage : PlayerUpgrade
{
    [SerializeField]
    private DualScatterCannon playerScatterCannon;
    [SerializeField]
    private float percentIncreaseScalar;

    private IntRange baseScatterDamage;

    protected override void Awake()
    {
        base.Awake();
        baseScatterDamage = playerScatterCannon.DamageRange;
    }

    public override void ApplyUpgrade()
    {
        Debug.Log("Old damage: <" + playerScatterCannon.DamageRange.Min +
            ", " + playerScatterCannon.DamageRange.Max + ">");
        float scalar = 1 + percentIncreaseScalar * (NumTimesPurchased + 1);
        playerScatterCannon.DamageRange = new IntRange(Mathf.RoundToInt(
            baseScatterDamage.Min * scalar), Mathf.RoundToInt(
                baseScatterDamage.Max * scalar));
        Debug.Log("New damage: <" + playerScatterCannon.DamageRange.Min +
    ", " + playerScatterCannon.DamageRange.Max + ">");
        base.ApplyUpgrade();
    }

    public override void UpdateStatsDisplays()
    {

    }
}
