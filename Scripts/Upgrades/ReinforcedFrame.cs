using UnityEngine;

public class ReinforcedFrame : PlayerUpgrade
{
    [SerializeField]
    private int increaseAmount;

    private Health playerHealth;

    protected override void Awake()
    {
        base.Awake();
        playerHealth = PlayerCharacter.Instance.CharacterHealth;
    }

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        playerHealth.MaxHealth += increaseAmount;
        playerHealth.CurrentHealth += increaseAmount;
    }
}
