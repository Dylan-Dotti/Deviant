using UnityEngine;

public class PartialRepair : PlayerUpgrade
{
    public override int Cost
    {
        get
        {
            return Mathf.Min(cost, playerHealth.MaxHealth -
                playerHealth.CurrentHealth);
        }
    }

    [SerializeField]
    private int repairPerCost = 1;

    private Health playerHealth;

    protected override void Awake()
    {
        base.Awake();
        playerHealth = PlayerCharacter.Instance.CharacterHealth;
    }

    protected override void Update()
    {
        base.Update();
        string repairCostStr = (Cost * repairPerCost).ToString();
        descriptionText.text = "Repair " + repairCostStr +
            " damage from your combat frame";
    }

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        playerHealth.CurrentHealth += Cost * repairPerCost;
    }
}
