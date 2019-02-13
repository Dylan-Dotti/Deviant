using UnityEngine;

public class LerpRotationToTarget : MonoBehaviour
{
    public Vector3 Target
    {
        get
        {
            return target;
        }
        set
        {
            target = new Vector3(value.x, transform.position.y, value.z);
        }
    }

    [SerializeField]
    private float angularVelocityDegrees = 10f;

    private Vector3 target;

    private void LateUpdate()
    {
        float angleBetween = Vector3.SignedAngle(transform.forward,
            Target - transform.position, Vector3.up);
        if (Mathf.Abs(angleBetween) < angularVelocityDegrees * 0.01f)
        {
            transform.LookAt(Target);
        }
        else
        {
            transform.Rotate(Vector3.up, angularVelocityDegrees *
                Mathf.Sign(angleBetween) * Time.deltaTime);
        }
    }
}
