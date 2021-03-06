﻿using UnityEngine;

/* Representation of health used by enemies and the player.
 * Fires events when health changes and controls health bar display
 * (should be changed)
 */ 
public class Health : MonoBehaviour
{
    public delegate void HealthChangedDelegate(float prevHealth, float newHealth);

    public HealthChangedDelegate HealthChangedEvent;

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
                    healthBar?.SetHealthPercentage(HealthPercentage);
                    HealthChangedEvent?.Invoke(prevHealth, currentHealth);
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

    public void SetCurrentAndMaxHealth(int value)
    {
        maxHealth = value;
        currentHealth = value;
    }

    public void IncreaseCurrentAndMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;
    }
}
