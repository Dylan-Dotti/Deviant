using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialLerp : MonoBehaviour
{
    public float LerpDuration
    {
        get { return lerpDuration; }
        set { lerpDuration = value; }
    }

    private Material startMaterial;
    [SerializeField]
    private Material lerpMaterial;

    [SerializeField]
    private float lerpDuration = 3.0f;
    private Renderer rend;
    private float startTime;

    private void Start()
    {
        //LerpDuration = 3.0f;
        startTime = Time.time;
        rend = GetComponent<Renderer>();
        startMaterial = new Material(rend.material);
    }

    private void Update()
    {
        float lerp = Mathf.PingPong(Time.time - startTime, LerpDuration) / LerpDuration;
        //float lerp = Mathf.Sin(Time.time / lerpDuration);
        rend.material.Lerp(startMaterial, lerpMaterial, lerp);
    }
}
