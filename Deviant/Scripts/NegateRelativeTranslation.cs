using UnityEngine;

public class NegateRelativeTranslation : MonoBehaviour
{
    private Vector3 localOffset;

    private void Start()
    {
        RecalculateTargetPosition();
    }

    private void FixedUpdate()
    {
        transform.position = transform.parent.position + localOffset;
    }

    private void LateUpdate()
    {
        transform.position = transform.parent.position + localOffset;
    }

    public void RecalculateTargetPosition()
    {
        localOffset = transform.position - transform.parent.position;
    }
}
