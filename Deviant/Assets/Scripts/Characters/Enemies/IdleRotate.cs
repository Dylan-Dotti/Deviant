using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IdleRotate : MonoBehaviour
{
    public float MinTorque { get { return torqueRange.min; } set { torqueRange.min = value; } }
    public float MaxTorque { get { return torqueRange.max; } set { torqueRange.max = value; } }

    [SerializeField]
    private RangeFloat torqueRange = new RangeFloat(1f, 3f);

    private Rigidbody rBody;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        StartCoroutine(AddRandomTorquePeriodic());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator AddRandomTorquePeriodic()
    {
        while (true)
        {
            float rotationDuration = Random.Range(1.5f, 3f);
            float rotationStartTime = Time.time;
            float torqueDirectionScalar = Random.value > 0.5f ? 1 : -1;
            while (Time.time - rotationStartTime < rotationDuration)
            {
                Vector3 torque = Random.Range(torqueRange.min, torqueRange.max) *
                    torqueDirectionScalar * Vector3.up;
                rBody.AddTorque(torque, ForceMode.Acceleration);
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }
}
