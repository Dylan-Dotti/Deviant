using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparePart : Item
{
    public int Value
    {
        get { return value; }
        set { this.value = Mathf.Max(0, value); }
    }

    [SerializeField]
    private int value = 1;

    public override void MergeWithPlayer()
    {
        player.NumSpareParts += value;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return null;
        Destroy(gameObject);
    }
}
