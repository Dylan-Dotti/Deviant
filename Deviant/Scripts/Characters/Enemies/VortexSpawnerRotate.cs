using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexSpawnerRotate : MonoBehaviour
{
    public enum RotationAxis { X, Y, Z }

    public float RotationSpeed { get { return rotationSpeed; } set { rotationSpeed = value; } }

    [SerializeField]
    private RotationAxis rotationAxis;
    [SerializeField]
    private float rotationSpeed = 0.2f;

    void Update()
    {
        float rotationMagnitude = 360 * RotationSpeed * Time.deltaTime;
        switch (rotationAxis)
        {
            case RotationAxis.X:
                transform.Rotate(new Vector3(rotationMagnitude, 0, 0));
                break;
            case RotationAxis.Y:
                transform.Rotate(new Vector3(0, rotationMagnitude, 0));
                break;
            case RotationAxis.Z:
                transform.Rotate(new Vector3(0, 0, rotationMagnitude));
                break;
        }
    }
}
