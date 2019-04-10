using System.Collections;
using UnityEngine;

public abstract class Enemy : Character
{
    public static CharacterDelegate EnemySpawnedEvent;
    public static CharacterDelegate EnemyDeathEvent;

    public abstract EnemyType EType { get; }

    protected virtual void Start()
    {
        EnemySpawnedEvent?.Invoke(this);
        ApplyScalars();
        StartCoroutine(SpawnSequence());
    }

    protected virtual void OnEnable()
    {
        PlayerCharacter.Instance.PlayerDeathEvent += OnPlayerDeath;
    }

    protected virtual void OnDisable()
    {
        PlayerCharacter.Instance.PlayerDeathEvent -= OnPlayerDeath;
    }

    public override void Die()
    {
        //GetComponent<SparePartsGenerator>()?.GenerateSpareParts();
        EnemyDeathEvent?.Invoke(this);
    }

    protected virtual void OnPlayerDeath(Character c)
    {
        enabled = false;
    }

    protected virtual void ApplyScalars()
    {
        CharacterHealth.SetCurrentAndMaxHealth(Mathf.RoundToInt(
            EnemyStrengthScalars.GetHealthScalar(EType) * 
            CharacterHealth.MaxHealth));
        DamagePlayerOnContact contactDamager = GetComponent<DamagePlayerOnContact>();
        if (contactDamager != null)
        {
            contactDamager.DamageAmount = Mathf.RoundToInt(contactDamager.
                DamageAmount * EnemyStrengthScalars.GetDamageScalar(EType));
        }
    }

    protected abstract IEnumerator SpawnSequence();
}
