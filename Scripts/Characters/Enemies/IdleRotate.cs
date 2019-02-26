using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IdleRotate : MonoBehaviour
{
    [SerializeField]
    private FloatRange torqueRange = new FloatRange(1f, 3f);

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
                Vector3 torque = Random.Range(torqueRange.Min, torqueRange.Max) *
                    torqueDirectionScalar * Vector3.up;
                rBody.AddTorque(torque, ForceMode.Acceleration);
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }
}
