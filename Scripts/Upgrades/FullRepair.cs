using UnityEngine;

public class FullRepair : PlayerUpgrade
{
    public override int Cost
    {
        get => playerHealth.MaxHealth - playerHealth.CurrentHealth;
    }

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
        playerHealth.CurrentHealth += Cost;
        base.ApplyUpgrade();
    }

    public override void UpdateStatsDisplays()
    {
        currentHealthDisplay.CurrentStats.text = 
            playerHealth.CurrentHealth.ToString();
        currentHealthDisplay.NewStats.text =
            (playerHealth.CurrentHealth + Cost).ToString();
    }
}
