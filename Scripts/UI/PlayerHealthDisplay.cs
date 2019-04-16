using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerSliderBar healthBar;

    private Health playerHealth;

    private void Awake()
    {
        playerHealth = PlayerCharacter.Instance.CharacterHealth;
    }

    private void OnEnable()
    {
        playerHealth.HealthChangedEvent += OnPlayerHealthChanged;
    }

    private void OnDisable()
    {
        playerHealth.HealthChangedEvent -= OnPlayerHealthChanged;
    }

    private void OnPlayerHealthChanged(float prevHealth, float newHealth)
    {
        healthBar.SetPercentage(playerHealth.HealthPercentage);
    }
}
