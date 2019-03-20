using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ShakeCamera(float magnitude)
    {
        CameraShaker.Instance.ShakeOnce(magnitude, 0.33f, 0.5f, 0.5f);
    }

    public void ShakeByDistance(float magnitude, float currentDist, float maxDist)
    {
        float distModifier = 1 - Mathf.Clamp01(currentDist / maxDist);
        ShakeCamera(magnitude * distModifier);
    }
}
