using System.Collections.Generic;
using UnityEngine;

/* Class created to nerf the spawn rates and amounts of duplicators.
 * Starts with a single duplicator, and periodiclly triggers their 
 * duplication until a set limit.
 */
public class DuplicatorGroup : MonoBehaviour
{
    /* The DuplicatorGroup maintains a number of charges 
     * so that the spawn rates increase with the number 
     * currently in the group
     */
    public class DuplicationCharge
    {
        public Duplicator DuplicatorObj { get; private set; }
        public float DuplicationTime { get; private set; }

        public DuplicationCharge(Duplicator obj, float duplicationTime)
        {
            DuplicatorObj = obj;
            DuplicationTime = duplicationTime;
        }
    }

    public int CurrentNumDuplicators { get { return duplicators.Count; } }

    [Header("Duplication Control")]
    [SerializeField]
    private int totalNumDuplications = 10;
    [SerializeField]
    private int numDuplicationCharges = 1;
    [SerializeField]
    private FloatRange duplicationChargeCooldown = new FloatRange(5f, 7.5f);

    private List<Duplicator> duplicators;
    private List<DuplicationCharge> duplicationCharges;

    private void Awake()
    {
        duplicators = new List<Duplicator>();
        duplicationCharges = new List<DuplicationCharge>();
        duplicators.Add(transform.GetChild(0).GetComponent<Duplicator>());
    }

    private void Start()
    {
        Duplicator.DuplicatorDeathEvent += OnDuplicatorDeath;
        duplicationCharges.Add(new DuplicationCharge(duplicators[0],
            GetNewDuplicationTime()));
    }

    private void Update()
    {
        List<DuplicationCharge> triggeredCharges = new List<DuplicationCharge>();
        foreach (DuplicationCharge charge in duplicationCharges)
        {
            if (Time.time >= charge.DuplicationTime && totalNumDuplications > 0)
            {
                triggeredCharges.Add(charge);
            }
        }
        foreach (DuplicationCharge charge in triggeredCharges)
        {
            TriggerDuplication(charge);
        }
    }

    private void TriggerDuplication(DuplicationCharge charge)
    {
        duplicationCharges.Remove(charge);
        Duplicator progenitor = duplicators[0];
        Duplicator clone = progenitor.Duplicate();
        duplicators.Add(clone);
        duplicators.Remove(progenitor);
        duplicators.Add(progenitor);
        totalNumDuplications -= 1;
        duplicationCharges.Add(new DuplicationCharge(
            clone, GetNewDuplicationTime()));
        if (duplicationCharges.Count < numDuplicationCharges)
        {
            duplicationCharges.Add(new DuplicationCharge(
                progenitor, GetNewDuplicationTime()));
        }
        enabled = totalNumDuplications > 0;
    }

    private float GetNewDuplicationTime()
    {
        return Time.time + duplicationChargeCooldown.RandomRangeValue;
    }

    private bool ChargesContainsDuplicator(Duplicator dInstance)
    {
        foreach (DuplicationCharge charge in duplicationCharges)
        {
            if (charge.DuplicatorObj == dInstance)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDuplicatorDeath(Character character)
    {
        Duplicator duplicator = (Duplicator)character;
        if (duplicators.Contains(duplicator))
        {
            duplicators.Remove(duplicator);
            duplicator.transform.parent = null;
            if (duplicators.Count == 0)
            {
                Destroy(gameObject);
            }
        }

        duplicationCharges.RemoveAll(c => c.DuplicatorObj == duplicator);
        foreach (Duplicator dInstance in duplicators)
        {
            if (!ChargesContainsDuplicator(dInstance))
            {
                duplicationCharges.Add(new DuplicationCharge(dInstance, 
                    GetNewDuplicationTime()));
                break;
            }
        }
    }
}
