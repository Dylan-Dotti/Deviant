using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void HealthReachedZeroDelegate();

    public HealthReachedZeroDelegate HealthReachedZeroEvent;

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
                currentHealth = Mathf.Min(Mathf.Max(0, value), MaxHealth);
                if (currentHealth == 0 && HealthReachedZeroEvent != null)
                {
                    HealthReachedZeroEvent();
                }
            }

        }
    }

    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        CanReceiveDamage = true;
    }
}
