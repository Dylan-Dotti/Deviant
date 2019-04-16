using UnityEngine;

public class ReinforcedFrame : PlayerUpgrade
{
    public override string Description => "Increase the base maximum " + 
        "health of your combat " + "frame by 5%";

    [SerializeField]
    private float increaseMultiplier = 0.5f;

    [SerializeField]
    private StatsDisplay currentHealthStats;
    [SerializeField]
    private StatsDisplay maxHealthStats;

    private Health playerHealth;
    private int baseHealth;

    protected override void Awake()
    {
        base.Awake();
        playerHealth = player.CharacterHealth;
        baseHealth = playerHealth.MaxHealth;
        AddStatsDisplay(currentHealthStats);
        AddStatsDisplay(maxHealthStats);
    }

    public override void ApplyUpgrade()
    {
        playerHealth.MaxHealth = GetNextHealth(playerHealth.MaxHealth);
        playerHealth.CurrentHealth = GetNextHealth(playerHealth.CurrentHealth);
        base.ApplyUpgrade();
    }

    public override void UpdateStatsDisplays()
    {
        currentHealthStats.CurrentStats.text = 
            playerHealth.CurrentHealth.ToString();
        currentHealthStats.NewStats.text = GetNextHealth(
            playerHealth.CurrentHealth).ToString();
        maxHealthStats.CurrentStats.text = 
            playerHealth.MaxHealth.ToString();
        maxHealthStats.NewStats.text = GetNextHealth(
            playerHealth.MaxHealth).ToString();
    }

    private int GetNextHealth(int healthValue)
    {
        return Mathf.RoundToInt(healthValue + baseHealth * increaseMultiplier);
    }
}
