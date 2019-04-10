using UnityEngine;

//bugged if fresnel colors are the same
public class FresnelMaterialLerp : PercentageLerper
{
    public override float CurrentLerpPercentage
    {
        get => Vector4InverseLerp(GetFresnelColor(originalMaterial),
            GetFresnelColor(lerpMaterial), GetFresnelColor(rend.material));
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

    private readonly string fresnelColorPropertyName = "Color_AF7F0660";

    protected override void UpdateLerpVariables(float lerpPercentage)
    {
        rend.material.Lerp(originalMaterial, lerpMaterial, lerpPercentage);
    }

    private Color GetFresnelColor(Material m)
    {
        return m.GetColor(fresnelColorPropertyName);
    }
}
