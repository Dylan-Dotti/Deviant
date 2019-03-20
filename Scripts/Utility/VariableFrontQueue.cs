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

    public T Pop(int frontSize)
    {
        frontSize = Mathf.Min(frontSize, list.Count);
        int randIndex = Random.Range(0, frontSize);
        T item = list[randIndex];
        list.RemoveAt(randIndex);
        return item;
    }

    public T PopAndCycle(int frontSize)
    {
        T item = Pop(frontSize);
        Push(item);
        return item;
    }

    public void Push(T item)
    {
        list.Add(item);
    }
}
