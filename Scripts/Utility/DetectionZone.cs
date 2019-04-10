using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DetectionZone<T> : MonoBehaviour where T : Component
{
    public IEnumerable<T> DetectedComponents
    {
        get
        {
            CleanupDetectedComponents();
            return detectedComponents;
        }
    }

    public ICollection<string> DetectableTags => detectableTags;

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
            detectedComponents.Add(other.transform.root.
                GetComponentInChildren<T>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        detectedComponents.Remove(other.transform.root.
            GetComponentInChildren<T>());
    }

    private void CleanupDetectedComponents()
    {
        detectedComponents.RemoveWhere(c => c == null);
    }
}
