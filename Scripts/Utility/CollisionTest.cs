using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Detected");
    }
}
