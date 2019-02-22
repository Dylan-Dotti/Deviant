using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : Component, IPoolable
{
    [SerializeField]
    private T prefab;
    [SerializeField]
    private int initialPoolSize = 10;

    private Queue<T> pool;

    protected virtual void Awake()
    {
        pool = new Queue<T>(initialPoolSize);
        for (int i = 0; i < initialPoolSize; i++)
        {
            ReturnToPool(Instantiate(prefab));
        }
    }

    public T Get()
    {
        if (pool.Count == 0)
        {
            Debug.Log("Pool empty");
            ReturnToPool(Instantiate(prefab));
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
    }
}
