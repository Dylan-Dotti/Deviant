using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DetectionZone<T> : MonoBehaviour where T : Component
{
    public HashSet<T> DetectedComponents
    {
        get
        {
            CleanupDetectedComponents();
            return detectedComponents;
        }
    }

    [SerializeField]
    private List<string> detectableTags;

    private HashSet<T> detectedComponents;

    private void Awake()
    {
        detectedComponents = new HashSet<T>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (detectableTags.Contains(other.tag))
        {
            DetectedComponents.Add(other.transform.root.
                GetComponentInChildren<T>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DetectedComponents.Remove(other.transform.root.
            GetComponentInChildren<T>());
    }

    private void CleanupDetectedComponents()
    {
        detectedComponents.RemoveWhere(c => c == null);
    }
}
