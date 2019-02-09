using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

[RequireComponent(typeof(VisualEffect))]
public class TentacleTrail : MonoBehaviour
{
    [SerializeField]
    private Transform sourceTransform;

    VisualEffect trail;

    private const string SCALE_PROPERTY_NAME = "Scale";
    private const string POSITION_PROPERTY_NAME = "Position";

    private void Awake()
    {
        trail = GetComponent<VisualEffect>();
    }

    private void Start()
    {
        Debug.Log("Tentacle Start");
        StartCoroutine(LerpXScaleRandom());
        StartCoroutine(LerpZScaleRandom());
    }

    private void Update()
    {
        trail.SetVector3(POSITION_PROPERTY_NAME, sourceTransform.position);
    }

    private IEnumerator LerpXScaleRandom()
    {
        while (true)
        {
            Vector3 startScale = trail.GetVector3(SCALE_PROPERTY_NAME);
            float newXScale = startScale.x < 0 ? Random.Range(1f, 2f) : Random.Range(-2f, -1f);
            Vector3 scaleLerpTarget = new Vector3(newXScale, 0f, 0f);
            float lerpDuration = Random.Range(1f, 2f);
            float lerpStartTime = Time.time;
            while (Time.time - lerpStartTime < lerpDuration)
            {
                Vector3 lerpScale = Vector3.Lerp(startScale, scaleLerpTarget,
                    (Time.time - lerpStartTime) / lerpDuration);
                trail.SetVector3(SCALE_PROPERTY_NAME, lerpScale);
                yield return null;
            }
        }
    }

    private IEnumerator LerpZScaleRandom()
    {
        while (true)
        {
            Vector3 startScale = trail.GetVector3(SCALE_PROPERTY_NAME);
            Vector3 scaleLerpTarget = new Vector3(0f, 0f, Random.Range(-1.5f, 1.5f));
            float lerpDuration = Random.Range(1.5f, 3f);
            float lerpStartTime = Time.time;
            while (Time.time - lerpStartTime < lerpDuration)
            {
                Vector3 lerpScale = Vector3.Lerp(startScale, scaleLerpTarget,
                    (Time.time - lerpStartTime) / lerpDuration);
                trail.SetVector3(SCALE_PROPERTY_NAME, lerpScale);
                yield return null;
            }
        }
    }
}
