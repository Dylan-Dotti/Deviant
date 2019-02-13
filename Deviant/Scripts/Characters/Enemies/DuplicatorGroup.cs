using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicatorGroup : MonoBehaviour
{
    public int CurrentNumDuplicators { get { return duplicators.Count; } }

    private List<Duplicator> duplicators;

    [Header("Duplication Control")]
    [SerializeField]
    private int maxConcurrentDuplicators = 2;
    [SerializeField]
    private int totalNumDuplications = 10;
    [SerializeField]
    private int maxNumDuplicationCharges = 1;
    [SerializeField]
    private RangeFloat duplicationChargeCooldownRange = new RangeFloat(1f, 2f);

    [SerializeField]
    private float baseTimeBetweenDuplicates = 2f;
    private float timeSinceLastDuplicate = 0f;

    private void Awake()
    {
        duplicators = new List<Duplicator>();
        duplicators.Add(transform.GetChild(0).GetComponent<Duplicator>());
    }

    private void Start()
    {
        Duplicator.DuplicatorDeathEvent += OnDuplicatorDeath;
    }

    private void Update()
    {
        float timeBetweenDuplicates = Mathf.Clamp(baseTimeBetweenDuplicates / 
            CurrentNumDuplicators, 0.5f, baseTimeBetweenDuplicates);
        if (timeSinceLastDuplicate > timeBetweenDuplicates &&
            CurrentNumDuplicators < maxConcurrentDuplicators &&
            totalNumDuplications > 0)
        {
            TriggerDuplication();
        }
        timeSinceLastDuplicate += Time.deltaTime;
    }

    private void TriggerDuplication()
    {
        Duplicator progenitor = duplicators[0];
        Duplicator clone = progenitor.Duplicate();
        clone.transform.parent = transform;
        duplicators.Add(clone);
        duplicators.Remove(progenitor);
        duplicators.Add(progenitor);
        timeSinceLastDuplicate = 0f;
        totalNumDuplications -= 1;

    }

    private void OnDuplicatorDeath(Character character)
    {
        Duplicator duplicator = (Duplicator)character;
        if (duplicators.Contains(duplicator))
        {
            duplicators.Remove(duplicator);
            duplicator.transform.parent = null;
            if (transform.childCount == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
