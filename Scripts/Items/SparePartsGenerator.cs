using UnityEngine;
using UnityEngine.AI;

/* Used by enemies to generate spare parts (upgrade currency) after death
 */ 
[RequireComponent(typeof(Enemy))]
public class SparePartsGenerator : MonoBehaviour
{
    public IntRange ValuePerPartRange
    {
        get => valuePerPartRange;
        set => valuePerPartRange = value;
    }

    [SerializeField]
    private IntRange valuePerPartRange = new IntRange(1, 1);
    [SerializeField]
    private IntRange numPartsRange = new IntRange(2, 3);
    [SerializeField]
    private FloatRange spawnVelMagnitudeRange = new FloatRange(2f, 5f);

    private SparePartPool partPool;

    private void Awake()
    {
        partPool = ObjectPoolManager.Instance.
            GetSparePartPool(GetComponent<Enemy>().EType);
    }

    public void GenerateSpareParts()
    {
        int numParts = numPartsRange.RandomRangeValue;
        for (int i = 0; i < numParts; i++)
        {
            //instantiate
            SparePart part = partPool.Get();
            part.transform.position = transform.position;
            //assign a value
            part.Value = valuePerPartRange.RandomRangeValue;
            //launch it somewhere
            Vector3 spawnDirection = new Vector3(Random.Range(-1f, 1f),
                0f, Random.Range(-1f, 1f));
            float spawnMagnitude = spawnVelMagnitudeRange.RandomRangeValue;
            NavMeshAgent navAgent = GetComponent<NavMeshAgent>();
            part.GetComponent<Rigidbody>().velocity = (navAgent == null ?
                GetComponent<Rigidbody>().velocity : navAgent.velocity) +
                spawnDirection * spawnMagnitude;
        }
    }
}
