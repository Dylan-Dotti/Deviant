
public class FullRepair : PartialRepair
{
    public override int Cost
    {
        get => playerHealth.MaxHealth - playerHealth.CurrentHealth;
    }

    public override string Description => "Fully repair your combat frame";
}
