using UnityEngine;

public class PartialRepair : PlayerUpgrade
{
    public override int Cost => Mathf.Min(base.Cost, playerHealth.MaxHealth -
                playerHealth.CurrentHealth);

    public override string Description => "Repair " + Cost.ToString() +
            " damage from your combat frame";

    [SerializeField]
    protected StatsDisplay currentHealthDisplay;

    protected Health playerHealth;

    protected override void Awake()
    {
        base.Awake();
        playerHealth = player.CharacterHealth;
    }

    private void Start()
    {
        AddStatsDisplay(currentHealthDisplay);
    }

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        playerHealth.CurrentHealth += Cost;
    }

    public override void UpdateStatsDisplays()
    {
        currentHealthDisplay.CurrentStats.text =
            playerHealth.CurrentHealth.ToString();
        currentHealthDisplay.NewStats.text =
            (playerHealth.CurrentHealth + Cost).ToString();
    }
}
