using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCompleteDisplay : MonoBehaviour
{
    public static WaveCompleteDisplay Instance { get; private set; }

    private List<GameObject> childObjects;

    private void Awake()
    {
        if (Instance == null)
        {
            WaveGenerator.Instance.WaveEndedEvent += i => Activate();
            childObjects = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                childObjects.Add(transform.GetChild(i).gameObject);
            }
        }
    }

    private void OnEnable()
    {
        childObjects.ForEach(go => go.SetActive(true));
    }

    private void OnDisable()
    {
        childObjects.ForEach(go => go.SetActive(false));
    }

    public void ActivateUpgradeMenu()
    {
        enabled = false;
        UpgradeMenu.Instance.Activate();
    }

    public void StartNextWave()
    {
        enabled = false;
        WaveGenerator.Instance.StartNextWave();
    }

    public void Activate()
    {
        enabled = true;
    }

    public void Deactivate()
    {
        enabled = false;
    }
}
