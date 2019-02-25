using UnityEngine;

public class LerpRotationToTarget : MonoBehaviour
{
    public Vector3 TargetPosition
    {
        get
        {
            return targetPosition;
        }
        set
        {
            targetPosition = new Vector3(value.x, transform.position.y, value.z);
        }
    }

    [SerializeField]
    private float angularVelocityDegrees = 10f;

    private Vector3 targetPosition;

    private void FixedUpdate()
    {
        if (targetPosition != null)
        {
            float angleBetween = Vector3.SignedAngle(transform.forward,
                TargetPosition - transform.position, Vector3.up);
            if (Mathf.Abs(angleBetween) < angularVelocityDegrees * 0.01f)
            {
                transform.LookAt(TargetPosition);
            }
            else
            {
                transform.Rotate(Vector3.up, angularVelocityDegrees *
                    Mathf.Sign(angleBetween) * Time.fixedDeltaTime);
            }
        }
    }
}
