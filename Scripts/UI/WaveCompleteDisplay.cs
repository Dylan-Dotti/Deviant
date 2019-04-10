using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCompleteDisplay : MonoBehaviour
{
    public static WaveCompleteDisplay Instance { get; private set; }

    [SerializeField]
    private Text waveCompleteText;

    private List<GameObject> childObjects;

    private void Awake()
    {
        if (Instance == null)
        {
            if (WaveGenerator.Instance != null)
            {
                WaveGenerator.Instance.WaveEndedEvent += i => Activate(i);
            }
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

    public void Activate(int waveNum)
    {
        waveCompleteText.text = "Wave " + waveNum + " Complete";
        enabled = true;
    }

    public void Deactivate()
    {
        enabled = false;
    }
}
