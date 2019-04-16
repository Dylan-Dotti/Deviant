using UnityEngine;

public class WeaponHeatDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerWeaponHeat monitoredWeapHeat;
    private PlayerSliderBar heatBar;

    private void Awake()
    {
        heatBar = GetComponent<PlayerSliderBar>();
    }

    private void Update()
    {
        if (monitoredWeapHeat != null)
        {
            heatBar.SetPercentage(monitoredWeapHeat.HeatPercentage);
        }
    }
}
