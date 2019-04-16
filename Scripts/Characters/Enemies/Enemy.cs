using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/* Enemy is the superclass of all the game's enemies.
 * Mostly handles the firing of enemy-based events and 
 * applying health scalars from EnemyStrengthScalars.
 * 
 * All enemies damage the player on contact, and most 
 * knock the player away as well.
 */
public abstract class Enemy : Character
{
    public static UnityAction<Enemy> EnemySpawnedEvent;
    public static UnityAction<Enemy> EnemyDeathEvent;

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
        EnemyDeathEvent?.Invoke(this);
    }

    protected virtual void OnPlayerDeath()
    {
        enabled = false;
    }

    // Apply health and contact damage scalars. called after each wave completion
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

    /* In the enemy subclasses, this function is mostly used to handle issues 
     * with the NavMeshAgent component when spawning, and to decide on 
     * behavior based on whether the player is dead or not */
    protected abstract IEnumerator SpawnSequence();
}
