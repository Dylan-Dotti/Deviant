using UnityEngine;

public class FullRepair : PartialRepair
{
    public override int Cost
    {
        get
        {
            int baseCost = playerHealth.MaxHealth - playerHealth.CurrentHealth;
            int discountedCost = Mathf.RoundToInt(baseCost * 0.9f);
            return discountedCost > base.Cost * 2 ? discountedCost : baseCost;
        }
    }

    public override string Description //=> "Fully repair your combat frame ";
    {
        get
        {
            int discountedCost = Mathf.RoundToInt((playerHealth.MaxHealth -
                playerHealth.CurrentHealth) * 0.9f);
            if (discountedCost > base.Cost * 2)
            {
                Debug.Log("Full repair should be discounted");
            }
            return "Fully repair your combat frame" +
                (discountedCost > base.Cost * 2 ? " at a 10% discount" : "");
        }
    }

    private int GetBaseCost()
    {
        return playerHealth.MaxHealth - playerHealth.CurrentHealth;
    }

    private int GetDiscountedCost()
    {
        return Mathf.RoundToInt((playerHealth.MaxHealth -
            playerHealth.CurrentHealth) * 0.9f);
    }
}
