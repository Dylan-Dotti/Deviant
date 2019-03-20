using UnityEngine;

public class ReinforcedFrame : PlayerUpgrade
{
    [SerializeField]
    private int increaseAmount;

    [SerializeField]
    private StatsDisplay currentHealthStats;
    [SerializeField]
    private StatsDisplay maxHealthStats;

    private Health playerHealth;

    protected override void Awake()
    {
        base.Awake();
        playerHealth = player.CharacterHealth;
        AddStatsDisplay(currentHealthStats);
        AddStatsDisplay(maxHealthStats);
        UpdateStatsDisplays();
        description = "Increase the maximum health of your combat " +
            "frame by " + increaseAmount;
    }

    public override void ApplyUpgrade()
    {
        playerHealth.MaxHealth += increaseAmount;
        playerHealth.CurrentHealth += increaseAmount;
        base.ApplyUpgrade();
    }

    public override void UpdateStatsDisplays()
    {
        currentHealthStats.CurrentStats.text = 
            playerHealth.CurrentHealth.ToString();
        currentHealthStats.NewStats.text = 
            (playerHealth.CurrentHealth + increaseAmount).ToString();
        maxHealthStats.CurrentStats.text = 
            playerHealth.MaxHealth.ToString();
        maxHealthStats.NewStats.text =
            (playerHealth.MaxHealth + increaseAmount).ToString();
    }
}
