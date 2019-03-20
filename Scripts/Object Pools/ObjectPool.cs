using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ObjectPool<T> : MonoBehaviour where T : Component, IPoolable<T>
{
    [SerializeField]
    private List<T> prefabs;
    [SerializeField]
    private int initialObjectsPerPrefab = 15;

    private Queue<T> pool;

    protected virtual void Awake()
    {
        //shuffle prefabs before returning to pool
        RandomizableList<T> tempPool = new RandomizableList<T>();
        for (int i = 0; i < prefabs.Count; i++)
        {
            for (int j = 0; j < initialObjectsPerPrefab; j++)
            {
                tempPool.Add(Instantiate(prefabs[Random.Range(0, prefabs.Count)]));
            }
        }
        tempPool.Shuffle();

        pool = new Queue<T>(tempPool.Count);
        foreach (T item in tempPool)
        {
            ReturnToPool(item);
        }
    }

    public virtual T Get()
    {
        if (pool.Count == 0)
        {
            ReturnToPool(Instantiate(prefabs[Random.Range(0, prefabs.Count)]));
        }
        T poolObject = pool.Dequeue();
        poolObject.gameObject.SetActive(true);
        return poolObject;
    }

    public void ReturnToPool(T objectToReturn)
    {
        objectToReturn.transform.parent = transform;
        objectToReturn.gameObject.SetActive(false);
        pool.Enqueue(objectToReturn);
        objectToReturn.Pool = this;
    }
}
