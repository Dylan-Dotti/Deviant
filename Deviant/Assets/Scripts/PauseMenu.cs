using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    private PlayerController pController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            pController = GameObject.FindGameObjectWithTag("Player")
                .GetComponent<PlayerController>();
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            pController.enabled = true;
        }
    }
}
