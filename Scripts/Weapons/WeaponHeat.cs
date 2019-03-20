using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class WeaponHeat : MonoBehaviour
{
    public bool OverHeated { get; private set; }
    public float CurrentHeat
    {
        get => currentHeat;
        set => currentHeat = Mathf.Clamp(value, 0, maxHeat);
    }
    public float MaxHeat
    {
        get => maxHeat;
        set => maxHeat = Mathf.Max(0, value);
    }
    public float HeatPerShot
    {
        get => heatPerShot;
        set => heatPerShot = Mathf.Max(0, value);
    }
    public float HeatDrainPerSec
    {
        get => heatDrainPerSec;
        set => heatDrainPerSec = Mathf.Max(0, value);
    }
    public float OverheatDrainPerSec
    {
        get => overheatDrainPerSec;
        set => overheatDrainPerSec = Mathf.Max(0, value);
    }

    [SerializeField]
    private float maxHeat;
    [SerializeField]
    private float heatPerShot;
    [SerializeField]
    private float heatDrainPerSec;
    [SerializeField]
    private float overheatDrainPerSec;
    [SerializeField]
    private List<ParticleSystem> overheatParticles;

    private float currentHeat;
    private Weapon weapon;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();
    }

    private void OnEnable()
    {
        weapon.WeaponFiredEvent += IncreaseHeat;
    }

    private void OnDisable()
    {
        weapon.WeaponFiredEvent -= IncreaseHeat;
        weapon.enabled = true;
    }

    private void Update()
    {
        CurrentHeat -= (OverHeated ? overheatDrainPerSec :
            heatDrainPerSec) * Time.deltaTime;
        //Debug.Log(CurrentHeat);
    }

    public void IncreaseHeat()
    {
        CurrentHeat += heatPerShot;
        if (CurrentHeat == maxHeat && !OverHeated)
        {
            StartCoroutine(Overheat());
        }
    }

    private IEnumerator Overheat()
    {
        OverHeated = true;
        foreach (ParticleSystem oheatParticles in overheatParticles)
        {
            oheatParticles.Play();
        }
        weapon.enabled = false;
        while (currentHeat > 0)
        {
            yield return null;
        }
        weapon.enabled = true;
        foreach (ParticleSystem oheatParticles in overheatParticles)
        {
            oheatParticles.Stop();
        }
        OverHeated = false;
    }
}
