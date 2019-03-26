using UnityEngine;
using TMPro;

public class DamageNumber : RectTransformWorldMover, IPoolable<DamageNumber>
{
    public TextMeshPro DamageText { get; private set; }

    public ObjectPool<DamageNumber> Pool { get; set; }

    protected override void Awake()
    {
        base.Awake();
        DamageText = GetComponent<TextMeshPro>();
    }

    public void ReturnToPool()
    {
        Pool.ReturnToPool(this);
    }

    public override void SetAlpha(float percentage)
    {
        DamageText.alpha = Mathf.Lerp(0, 1, 1 - percentage);
    }

    public override void RemoveFromWorld()
    {
        ReturnToPool();
    }
}
