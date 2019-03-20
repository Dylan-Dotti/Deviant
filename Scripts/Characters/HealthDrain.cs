using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrain : MonoBehaviour
{
    [SerializeField]
    private Health healthToDrain;
    [SerializeField]
    private int drainAmount;
    [SerializeField]
    private int drainInterval;

    private void OnEnable()
    {
        StartCoroutine(DrainHealth());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator DrainHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(drainInterval);
            healthToDrain.CurrentHealth -= drainAmount;
        }
    }
}
