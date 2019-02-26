using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ObjectPool<T> : MonoBehaviour where T : Component, IPoolable<T>
{
    [SerializeField]
    private T prefab;
    [SerializeField]
    private int initialPoolSize = 15;

    private Queue<T> pool;

    protected virtual void Awake()
    {
        pool = new Queue<T>(initialPoolSize);
        for (int i = 0; i < initialPoolSize; i++)
        {
            ReturnToPool(Instantiate(prefab));
        }
    }

    public virtual T Get()
    {
        if (pool.Count == 0)
        {
            ReturnToPool(Instantiate(prefab));
        }
        T poolObject = pool.Dequeue();
        poolObject.transform.parent = null;
        poolObject.gameObject.SetActive(true);
        poolObject.Pool = this;
        return poolObject;
    }

    public void ReturnToPool(T objectToReturn)
    {
        objectToReturn.transform.parent = transform;
        objectToReturn.gameObject.SetActive(false);
        pool.Enqueue(objectToReturn);
    }
}
