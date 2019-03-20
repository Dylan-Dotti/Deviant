using UnityEngine;

public class PartialRepair : FullRepair
{
    public override int Cost
    {
        get => Mathf.Min(base.Cost, playerHealth.MaxHealth -
                playerHealth.CurrentHealth);
    }

    protected override void Update()
    {
        description = "Repair " + Cost.ToString() +
            " damage from your combat frame";
        base.Update();
    }
}
