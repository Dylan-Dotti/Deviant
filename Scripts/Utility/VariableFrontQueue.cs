using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VariableFrontQueue<T>
{
    [SerializeField]
    private RandomizableList<T> list;

    public VariableFrontQueue()
    {
        list = new RandomizableList<T>();
    }

    public VariableFrontQueue(IEnumerable<T> collection, bool shuffle)
    {
        list = new RandomizableList<T>();
        foreach (T item in collection)
        {
            list.Add(item);
        }
        if (shuffle)
        {
            list.Shuffle();
        }
    }

    public T Dequeue(int frontSize)
    {
        frontSize = Mathf.Min(frontSize, list.Count);
        int randIndex = Random.Range(0, frontSize);
        T item = list[randIndex];
        list.RemoveAt(randIndex);
        return item;
    }

    public T DequeueAndCycle(int frontSize)
    {
        T item = Dequeue(frontSize);
        Enqueue(item);
        return item;
    }

    public void Enqueue(T item)
    {
        list.Add(item);
    }
}
