using System.Collections.Generic;
using UnityEngine;

public class RepulsionZone : MonoBehaviour
{
    [SerializeField]
    private float repulsionMagnitude = 1;
    [SerializeField]
    private List<string> detectableTags;

    private HashSet<Rigidbody> detectedRbodies;

    private void Awake()
    {
        detectedRbodies = new HashSet<Rigidbody>();
    }

    private void Update()
    {
        List<Rigidbody> destroyedRbodies = new List<Rigidbody>();
        foreach (Rigidbody rbody in detectedRbodies)
        {
            if (rbody == null)
            {
                destroyedRbodies.Add(rbody);
            }
            else
            {
                Vector3 forceDirection = (rbody.transform.position -
                    transform.position).normalized;
                forceDirection = new Vector3(forceDirection.x, 0, forceDirection.z);
                rbody.AddForce(forceDirection * repulsionMagnitude *
                    Time.deltaTime, ForceMode.VelocityChange);
            }
        }
        destroyedRbodies.ForEach(rb => detectedRbodies.Remove(rb));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (detectableTags.Contains(other.tag))
        {
            detectedRbodies.Add(other.GetComponentInParent<Rigidbody>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        detectedRbodies.Remove(other.GetComponentInParent<Rigidbody>());
    }
}
