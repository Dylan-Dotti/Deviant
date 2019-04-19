using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class WeaponUpgrade : PlayerUpgrade
{
    private static GameObject activeWeapStatsGroup;

    [SerializeField]
    private GameObject baseWeapStatsGroup;
    [SerializeField]
    private GameObject weapStatsGroup;

    protected override void Awake()
    {
        activeWeapStatsGroup = baseWeapStatsGroup;
        base.Awake();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (activeWeapStatsGroup != null)
        {
            activeWeapStatsGroup.SetActive(false);
        }
        activeWeapStatsGroup = weapStatsGroup;
        weapStatsGroup.SetActive(true);
        base.OnPointerEnter(eventData);
    }
}
