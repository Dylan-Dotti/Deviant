using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public static UpgradeMenu Instance { get; private set; }

    [SerializeField]
    private GameObject partialRepairPanel;

    private PlayerController pController;
    private List<GameObject> menus;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            pController = PlayerCharacter.Instance.Controller;

            menus = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                menus.Add(transform.GetChild(i).gameObject);
            }
        }
    }

    private void OnEnable()
    {
        pController.WeaponEnabled = false;
        pController.MouseRotateEnabled = false;
        Camera.main.GetComponent<CameraZoom>().enabled = false;
        foreach (GameObject childMenu in menus)
        {
            childMenu.SetActive(true);
        }
    }

    private void OnDisable()
    {
        pController.WeaponEnabled = true;
        pController.MouseRotateEnabled = true;
        Camera.main.GetComponent<CameraZoom>().enabled = true;
        foreach (GameObject childMenu in menus)
        {
            childMenu.SetActive(false);
        }
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.LeftShift) || 
            Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.R))
        {
            enabled = false;
        }
    }
}
