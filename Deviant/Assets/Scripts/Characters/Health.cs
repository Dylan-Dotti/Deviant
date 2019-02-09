using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void HealthChangedDelegate(float prevHealth, float newHealth);

    public HealthChangedDelegate HealthChangedEvent;
    public HealthChangedDelegate HealthReachedZeroEvent;

    public Character ParentCharacter { get; private set; }
    public bool CanReceiveDamage { get; set; }

    public float MaxHealth
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
    public float CurrentHealth
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
                healthBar?.SetHealthPercentage(currentHealth / maxHealth);
                if (HealthReachedZeroEvent != null && currentHealth == 0 && prevHealth != 0)
                {
                    HealthReachedZeroEvent(prevHealth, currentHealth);
                }
            }

        }
    }

    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float currentHealth;

    [SerializeField]
    HealthBar healthBar;

    private void Awake()
    {
        currentHealth = maxHealth;
        CanReceiveDamage = true;
    }
}
