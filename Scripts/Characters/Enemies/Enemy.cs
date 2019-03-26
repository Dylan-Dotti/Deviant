using System.Collections;

public abstract class Enemy : Character
{
    public static CharacterDelegate EnemySpawnedEvent;
    public static CharacterDelegate EnemyDeathEvent;

    protected virtual void Start()
    {
        EnemySpawnedEvent?.Invoke(this);
        PlayerCharacter.PlayerDeathEvent += OnPlayerDeath;
        StartCoroutine(SpawnSequence());
    }

    public override void Die()
    {
        GetComponent<SparePartsGenerator>()?.GenerateSpareParts();
        EnemyDeathEvent?.Invoke(this);
        base.Die();
    }

    protected virtual void OnPlayerDeath(Character c)
    {

    }

    protected abstract IEnumerator SpawnSequence();
}
