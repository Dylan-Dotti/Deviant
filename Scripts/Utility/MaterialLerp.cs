using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialLerp : Lerper
{
    [SerializeField]
    private Material lerpMaterial;

    private Renderer rend;
    private Material originalMaterial;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        originalMaterial = new Material(rend.material);
        //originalMaterial = rend.material;
    }

    protected override IEnumerator LerpCR(bool forward, float duration)
    {
        Debug.Log(duration);
        Material startMaterial = rend.material;
        Material endMaterial = forward ? lerpMaterial : originalMaterial;
        float startTime = Time.time;
        for (float elapsed = 0; elapsed < duration;
            elapsed = Time.time - startTime)
        {
            rend.material.Lerp(startMaterial, endMaterial, elapsed / duration);
            yield return null;
        }
        rend.material = endMaterial;
        yield return StartCoroutine(base.LerpCR(forward, duration));
    }
}
