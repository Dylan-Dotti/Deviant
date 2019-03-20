using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparePart : Item, IPoolable<SparePart>
{
    public int Value
    {
        get { return value; }
        set { this.value = Mathf.Max(0, value); }
    }

    public ObjectPool<SparePart> Pool { get; set; }

    [SerializeField]
    private int value = 1;

    public override void MergeWithPlayer()
    {
        player.NumSpareParts += value;
        transform.parent = player.transform;
        StartCoroutine(ShrinkAndReturnToPool());
    }

    private IEnumerator ShrinkAndReturnToPool()
    {
        Vector3 originalScale = transform.localScale;
        float duration = 1f;
        float startTime = Time.time;
        for (float elapsed = 0; elapsed < duration; 
            elapsed = Time.time - startTime)
        {
            float lerpPercentage = elapsed / duration;
            float scalar = Mathf.Lerp(1, 0, lerpPercentage);
            transform.localScale *= scalar;
            yield return null;
        }
        Destroy(gameObject);

    }

    public void ReturnToPool()
    {
        throw new System.NotImplementedException();
    }
}
