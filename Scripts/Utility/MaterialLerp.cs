using UnityEngine;

public class MaterialLerp : PercentageLerper
{
    public override float CurrentLerpPercentage
    {
        get => Vector4InverseLerp(originalMaterial.color,
            lerpMaterial.color, rend.material.color);
    }

    [SerializeField]
    private Material lerpMaterial;

    private Material originalMaterial;
    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        originalMaterial = new Material(rend.material);
    }

    protected override void UpdateLerpVariables(float lerpPercentage)
    {
        rend.material.Lerp(originalMaterial, lerpMaterial, lerpPercentage);
    }
}
