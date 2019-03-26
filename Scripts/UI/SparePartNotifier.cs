using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SparePartNotifier : RectTransformWorldMover, 
    IPoolable<SparePartNotifier>
{
    public Text TextComponent { get; private set; }
    private List<Image> childImageComponents;

    public ObjectPool<SparePartNotifier> Pool { get; set; }

    protected override void Awake()
    {
        base.Awake();
        childImageComponents = new List<Image>(
            GetComponentsInChildren<Image>());
        TextComponent = GetComponentInChildren<Text>();
    }

    public override void RemoveFromWorld()
    {
        ReturnToPool();
    }

    public override void SetAlpha(float percentage)
    {
        float newAlpha = Mathf.Lerp(0, 1, 1 - percentage);
        childImageComponents.ForEach(i => i.color = new Color(
            i.color.r, i.color.g, i.color.b, newAlpha));
        TextComponent.color = new Color(TextComponent.color.r,
            TextComponent.color.g, TextComponent.color.b, newAlpha);
    }

    public void ReturnToPool()
    {
        Pool.ReturnToPool(this);
    }
}
