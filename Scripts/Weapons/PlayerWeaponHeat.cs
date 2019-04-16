using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Weapon))]
public class PlayerWeaponHeat : MonoBehaviour
{
    [System.Serializable]
    private class HeatBasedWeapon
    {
        public Weapon weapon;
        public float heatPerShot;
        public List<ParticleSystem> heatParticles;
    }

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
    public float HeatPercentage => currentHeat / maxHeat;
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
    private List<HeatBasedWeapon> weaponHeatPerShotList;
    [SerializeField]
    private float maxHeat;
    //[SerializeField]
    //private float heatPerShot;
    [SerializeField]
    private float heatDrainPerSec;
    [SerializeField]
    private float overheatDrainPerSec;

    private Weapon currentWeapon;
    private float currentHeat;

    private void Awake()
    {
        weaponHeatPerShotList.ForEach(wh => wh.weapon.WeaponFiredEvent +=
            () => IncreaseHeat(wh.heatPerShot));
        PlayerController pController = PlayerCharacter.Instance.Controller;
        pController.WeaponEquippedEvent += OnPlayerSwitchedWeapon;
        currentWeapon = pController.EquippedWeapon;
    }

    private void Update()
    {
        CurrentHeat -= (OverHeated ? overheatDrainPerSec :
            heatDrainPerSec) * Time.deltaTime;
    }

    public void IncreaseHeat(float heatAmount)
    {
        CurrentHeat += heatAmount;
        if (CurrentHeat == maxHeat && !OverHeated)
        {
            StartCoroutine(Overheat());
        }
    }

    private void OnPlayerSwitchedWeapon(Weapon newWeapon)
    {
        if (OverHeated)
        {
            weaponHeatPerShotList.ForEach(wh =>
            {
                if (wh.weapon == currentWeapon)
                {
                    wh.heatParticles.ForEach(p => p.Stop());
                }
                else if (wh.weapon == newWeapon)
                {
                    wh.heatParticles.ForEach(p => p.Play());
                }
            });
        }
        currentWeapon = newWeapon;
    }

    private IEnumerator Overheat()
    {
        OverHeated = true;
        weaponHeatPerShotList.ForEach(wh => {
            wh.weapon.enabled = false;
            if (wh.weapon.gameObject.activeInHierarchy)
            {
                wh.heatParticles.ForEach(p => p.Play());
            }
        });
        while (currentHeat > 0)
        {
            yield return null;
        }
        weaponHeatPerShotList.ForEach(wh => {
            wh.weapon.enabled = true;
            if (wh.weapon.gameObject.activeInHierarchy)
            {
                wh.heatParticles.ForEach(p => p.Stop());
            }
        });
        OverHeated = false;
    }
}
