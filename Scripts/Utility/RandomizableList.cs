using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomizableList<T> : List<T>
{
    public void Shuffle()
    {
        for (int i = 0; i < Count; i++)
        {
            int randIndex = Random.Range(0, Count);
            T temp = this[i];
            this[i] = this[randIndex];
            this[randIndex] = temp;
        }
    }
}
