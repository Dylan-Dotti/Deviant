using System.Collections;

public abstract class Enemy : Character
{
    protected virtual void Start()
    {
        StartCoroutine(SpawnSequence());
    }

    protected abstract IEnumerator SpawnSequence();
}
