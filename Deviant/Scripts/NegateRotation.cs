using UnityEngine;

public class NegateRotation : MonoBehaviour
{
    private Quaternion originalRotation;

    private void Start()
    {
        RecalculateOriginalRotation();
    }

    private void FixedUpdate()
    {
        transform.rotation = originalRotation;
    }

    private void LateUpdate()
    {
        transform.rotation = originalRotation;
    }

    public void RecalculateOriginalRotation()
    {
        originalRotation = transform.rotation;
    }
}
