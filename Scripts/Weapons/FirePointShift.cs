using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePointShift : MonoBehaviour
{
    [SerializeField]
    private float maxShiftMagnitude = 1f;

    private Vector3 startLocalPos;
    private PlayerController pController;

    private void Awake()
    {
        pController = PlayerCharacter.Instance.Controller;
        startLocalPos = transform.localPosition;
    }

    private void FixedUpdate()
    {
        Vector3 shiftVector = new Vector2(pController.
            TotalLocalVelocity.x, pController.TotalLocalVelocity.z);
        shiftVector = Vector3.ClampMagnitude(shiftVector * 0.5f, maxShiftMagnitude);
        transform.localPosition = -shiftVector;
    }
}
