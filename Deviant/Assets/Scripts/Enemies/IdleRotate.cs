using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IdleRotate : MonoBehaviour
{
    public bool RotationEnabled { get; set; }
    public float MinTorque { get { return minTorque; } set { minTorque = value; } }
    public float MaxTorque { get { return maxTorque; } set { maxTorque = value; } }

    [SerializeField]
    private float minTorque = 1f;
    [SerializeField]
    private float maxTorque = 3f;

    private Rigidbody rBody;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();
        RotationEnabled = true;
    }

    private void Start()
    {
        StartCoroutine(AddRandomTorquePeriodic());
    }

    private IEnumerator AddRandomTorquePeriodic()
    {
        while (true)
        {
            if (RotationEnabled)
            {
                float rotationDuration = Random.Range(1.5f, 3f);
                float currentDuration = 0f;
                float torqueDirectionScalar = Random.value > 0.5f ? 1 : -1;
                while (currentDuration < rotationDuration)
                {
                    if (!RotationEnabled)
                    {
                        break;
                    }
                    Vector3 torque = Random.Range(minTorque, maxTorque) *
                        torqueDirectionScalar * Vector3.up;
                    //torque = torque * (Random.value > 0.5f ? 1 : -1);
                    rBody.AddTorque(torque, ForceMode.Acceleration);
                    currentDuration += Time.fixedDeltaTime;
                    yield return new WaitForFixedUpdate();
                }
            }
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }
}
