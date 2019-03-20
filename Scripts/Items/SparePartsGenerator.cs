using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparePartsGenerator : MonoBehaviour
{
    [SerializeField]
    private EnemyType sparePartsPoolType;
    [SerializeField]
    private List<SparePart> sparePartPrefabs;
    [SerializeField]
    private IntRange valuePerPartRange = new IntRange(1, 1);
    [SerializeField]
    private IntRange numPartsRange = new IntRange(1, 1);
    [SerializeField]
    private FloatRange spawnVelMagnitudeRange = new FloatRange(1f, 1f);

    public void GenerateSpareParts()
    {
        int numParts = Random.Range(numPartsRange.Min, 
            numPartsRange.Max + 1);
        for (int i = 0; i < numParts; i++)
        {
            //pick one and instantiate
            int randIndex = Random.Range(0, sparePartPrefabs.Count);
            SparePart part = Instantiate(sparePartPrefabs[randIndex],
                transform.position, Quaternion.identity);
            //assign a value
            part.Value = Random.Range(valuePerPartRange.Min, 
                valuePerPartRange.Max + 1);
            //launch it somewhere
            Vector3 spawnDirection = new Vector3(Random.Range(-1f, 1f),
                0f, Random.Range(-1f, 1f));
            float spawnMagnitude = Random.Range(
                spawnVelMagnitudeRange.Min, spawnVelMagnitudeRange.Max);
            part.GetComponent<Rigidbody>().velocity = 
                spawnDirection * spawnMagnitude;
        }
    }

    private int AssignValue(SparePart part, int valueRemaining, int numPartsRemaining)
    {
        int desiredValue = Mathf.CeilToInt((valueRemaining / (float)numPartsRemaining));
        int assignValue = Mathf.Min(valueRemaining, desiredValue);
        part.Value = assignValue;
        Debug.Log(part.Value);
        return valueRemaining - assignValue;
    }
}
