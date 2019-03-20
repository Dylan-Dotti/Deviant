using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void HealthChangedDelegate(float prevHealth, float newHealth);

    public HealthChangedDelegate HealthChangedEvent;
    public HealthChangedDelegate HealthReachedZeroEvent;

    public bool CanReceiveDamage { get; set; } = true;

    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = Mathf.Max(0, value);
            CurrentHealth = Mathf.Min(CurrentHealth, maxHealth);
        }
    }
    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if (value > currentHealth || CanReceiveDamage)
            {
                float prevHealth = currentHealth;
                currentHealth = Mathf.Min(Mathf.Max(0, value), maxHealth);
                if (currentHealth != prevHealth)
                {
                    healthBar.SetHealthPercentage(HealthPercentage);
                    if (currentHealth == 0) HealthReachedZeroEvent?.Invoke(prevHealth, currentHealth);
                    else HealthChangedEvent?.Invoke(prevHealth, currentHealth);
                }
            }

        }
    }
    public float HealthPercentage { get { return (float)currentHealth / maxHealth; } }


    [SerializeField]
    HealthBar healthBar;
    [SerializeField]
    private int maxHealth = 1;

    private int currentHealth = 1;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
}
