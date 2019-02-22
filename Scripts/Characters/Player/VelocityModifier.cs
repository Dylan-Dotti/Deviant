using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VelocityModifier
{
    public Vector3 Velocity
    {
        get
        {
            return velocity;
        }
        set
        {
            if (value.magnitude > MaxMagnitude)
            {
                value = value.normalized * MaxMagnitude;
            }
            velocity = new Vector3(value.x, value.y, value.z);
        }
    }
    public float MaxMagnitude
    {
        get
        {
            return maxMagnitude;
        }
        set
        {
            maxMagnitude = Mathf.Max(value, 0f);
        }
    }

    private Vector3 velocity;
    [SerializeField]
    private float maxMagnitude = 1f;

    public VelocityModifier(float maxMagnitude)
    {
        MaxMagnitude = maxMagnitude;
        velocity = new Vector3();
    }

    public VelocityModifier(float maxMagnitude, Vector3 initVelocity)
    {
        MaxMagnitude = maxMagnitude;
        velocity = initVelocity;
    }
}
