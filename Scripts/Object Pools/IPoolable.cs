using UnityEngine;

public interface IPoolable<T> where T : Component, IPoolable<T>
{
    ObjectPool<T> Pool { get; set; }

    void ReturnToPool();
    //void ReturnToPoolAfter(float delay);
    //void CancelReturnToPool();
}
