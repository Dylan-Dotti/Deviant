using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class Character : MonoBehaviour
{
    public delegate void CharacterDelegate(Character character);

    public Health CharacterHealth { get; protected set; }

    protected virtual void Awake()
    {
        CharacterHealth = GetComponent<Health>();
        CharacterHealth.HealthReachedZeroEvent += OnHealthReachedZero;
    }

    protected virtual void OnHealthReachedZero()
    {
        Die();
    }

    public abstract void Die();
}
